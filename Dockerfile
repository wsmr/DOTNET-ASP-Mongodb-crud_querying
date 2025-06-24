# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY . .
RUN dotnet restore "src/DiyawannaSupBackend.Api/DiyawannaSupBackend.Api.csproj"

# Build the application
WORKDIR /app/src/DiyawannaSupBackend.Api
RUN dotnet publish "DiyawannaSupBackend.Api.csproj" -c Release -o /app/publish

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080 # Or 5000/5001 depending on your appsettings.json
ENTRYPOINT ["dotnet", "DiyawannaSupBackend.Api.dll"]
