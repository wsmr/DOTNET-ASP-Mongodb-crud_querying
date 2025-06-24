using DiyawannaSupBackend.Api.Exceptions;
using DiyawannaSupBackend.Api.Security;
using DiyawannaSupBackend.Api.Services;
using DiyawannaSupBackend.Api.Repositories;
using DiyawannaSupBackend.Api.Models.Entities;
using DiyawannaSupBackend.Api.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization; // For [Authorize] attribute

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Configuration for MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddScoped(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    return client.GetDatabase(settings.DatabaseName);
});

// 2. Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Add other repositories here (e.g., IUniversityRepository, IFacultyRepository, etc.)

// 3. Add Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
// Add other services here (e.g., IUniversityService, IFacultyService, etc.)

// 4. Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
{
    throw new InvalidOperationException("JWT Secret is not configured. Please set it in appsettings.json or environment variables.");
}
builder.Services.AddSingleton(jwtSettings); // Make JwtSettings available via DI
builder.Services.AddScoped<JwtTokenGenerator>(); // Register JwtTokenGenerator

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Set to true in production if you have an issuer
        ValidateAudience = false, // Set to true in production if you have an audience
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero // No clock skew for token expiration
    };
});

builder.Services.AddAuthorization();

// 5. Add Caching
builder.Services.AddMemoryCache(); // In-memory cache

// 6. Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:3000", "http://localhost:4200" ) // Replace with your frontend URLs
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()); // If you need to send cookies/auth headers
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer( );
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Diyawanna Sup Backend API", Version = "v1" });

    // Configure Swagger to use JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Handling Middleware
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var errorResponse = GlobalExceptionHandler.HandleException(exception);
        context.Response.StatusCode = errorResponse.StatusCode;
        await context.Response.WriteAsJsonAsync(errorResponse.Value);
    });
});

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin"); // Use the CORS policy

app.UseAuthentication(); // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

// MongoDB Indexing (Optional: Run on startup or as a separate migration)
using (var scope = app.Services.CreateScope())
{
    var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    await MongoDbIndexCreator.CreateIndexesAsync(mongoDatabase);
}

app.Run();
