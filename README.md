# Diyawanna Sup Backend (.NET Core)

A comprehensive ASP.NET Core backend service with MongoDB Atlas integration, JWT authentication, CRUD operations, performance optimizations, and dynamic query processing, re-implemented from a Java Spring Boot project.

## Table of Contents

- [Overview](#overview)
- [Technology Stack](#technology-stack)
- [Project Architecture](#project-architecture)
- [Features](#features)
- [Setup and Installation](#setup-and-installation)
- [Configuration](#configuration)
- [API Documentation](#api-documentation)
- [Database Schema](#database-schema)
- [Security](#security)
- [Performance Optimizations](#performance-optimizations)
- [Testing](#testing)
- [Error Handling](#error-handling)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Diyawanna Sup Backend is a robust, scalable ASP.NET Core application designed to provide comprehensive backend services for educational institutions. It features user management, university and faculty administration, shopping cart functionality, and dynamic query processing capabilities. This project is a re-implementation of an existing Java Spring Boot application, translating its architecture and features into the .NET ecosystem.

### Key Highlights

- **Modern Architecture**: Built with .NET 8 (or later) and ASP.NET Core.
- **Secure Authentication**: JWT-based authentication with ASP.NET Core Authentication.
- **High Performance**: MongoDB Atlas integration with connection pooling and caching.
- **Dynamic Queries**: Configurable query system with parameter substitution (to be implemented).
- **Comprehensive Testing**: Unit and integration tests (to be expanded).
- **Production Ready**: Performance monitoring, error handling, and deployment configurations.

## Technology Stack

### Core Technologies
- **C#**: Latest version
- **.NET SDK**: 8.0 or higher
- **ASP.NET Core**: 8.0
- **MongoDB .NET Driver**: For interacting with MongoDB
- **MongoDB Server**: 7.0 (Atlas M0 free tier recommended)

### Security & Authentication
- **ASP.NET Core Authentication**: JWT Bearer authentication
- **BCrypt.Net-Next**: Password hashing

### Testing & Quality
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework
- **ASP.NET Core Test Host**: Integration testing
- **Swagger/OpenAPI**: API documentation and testing

### Development Tools
- **Visual Studio / VS Code**: IDE
- **.NET CLI**: Command-line interface for building and running projects
- **Git**: Version control

## Project Architecture

The application follows a layered architecture pattern with clear separation of concerns:

```
src/DiyawannaSupBackend.Api/
├── Controllers/      # REST API controllers
├── Models/
│   ├── DTOs/         # Data Transfer Objects
│   ├── Entities/     # MongoDB entity models
│   └── Settings/     # Configuration settings classes
├── Exceptions/       # Custom exception classes and global handler
├── Repositories/     # Data access layer interfaces and implementations
├── Security/         # Security configurations and JWT utilities
├── Services/         # Business logic layer interfaces and implementations
├── Utils/            # Utility classes (e.g., Password Hashing)
├── appsettings.json  # Application configuration
├── Program.cs        # Application startup and DI configuration
└── DiyawannaSupBackend.Api.csproj # Project file
```

### Architecture Layers

1.  **Presentation Layer** (`Controllers/`)
    -   REST API endpoints
    -   Request/response handling
    -   Input validation

2.  **Business Logic Layer** (`Services/`)
    -   Core business logic
    -   Data processing
    -   Business rule enforcement

3.  **Data Access Layer** (`Repositories/`)
    -   MongoDB operations
    -   Query execution
    -   Data persistence

4.  **Security Layer** (`Security/`)
    -   Authentication filters
    -   Authorization rules
    -   JWT token processing

## Features

### 🔐 Authentication & Authorization
-   JWT-based stateless authentication
-   User registration and login
-   Token validation (refresh token to be implemented)
-   Password encryption with BCrypt

### 👥 User Management
-   Complete CRUD operations for users
-   User profile management
-   Search and filtering capabilities
-   Soft delete functionality

### 🏛️ University & Faculty Management (To be implemented)
-   University registration and management
-   Faculty administration
-   Location-based university search

### 🛒 Shopping Cart System (To be implemented)
-   User-specific cart management
-   Item addition and removal

### 🔍 Dynamic Query System (To be implemented)
-   Configurable query execution
-   Parameter substitution

### 📊 Performance Monitoring (To be implemented)
-   Real-time performance metrics
-   Cache management
-   Health check endpoints

### 🚀 Performance Optimizations
-   MongoDB connection pooling (handled by driver)
-   In-memory caching (can be extended to distributed cache)
-   Database indexing

## Setup and Installation

### Prerequisites

Before running the application, ensure you have the following installed:

-   **[.NET SDK 8.0 or higher](https://dotnet.microsoft.com/download/dotnet/8.0)**
-   **MongoDB Atlas account** (or local MongoDB instance)
-   **Git** (for cloning the repository)

### Installation Steps

1.  **Create Project Structure**
    Create a solution folder, then a `src` folder inside it, and then `DiyawannaSupBackend.Api` inside `src`.
    ```bash
    mkdir DiyawannaSupBackend
    cd DiyawannaSupBackend
    dotnet new sln
    mkdir src
    cd src
    dotnet new webapi -n DiyawannaSupBackend.Api
    cd ..
    dotnet sln add src/DiyawannaSupBackend.Api/DiyawannaSupBackend.Api.csproj
    ```

2.  **Copy File Contents**
    Copy the provided code into the respective files and folders within `src/DiyawannaSupBackend.Api/`. Create new folders as needed (e.g., `Models/DTOs`, `Repositories`, `Services`, `Security`, `Exceptions`, `Utils`, `Models/Settings`).

3.  **Install NuGet Packages**
    Navigate to the `src/DiyawannaSupBackend.Api` directory in your terminal and run:
    ```bash
    dotnet add package MongoDB.Driver
    dotnet add package BCrypt.Net-Next
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    dotnet add package Swashbuckle.AspNetCore
    ```

4.  **Configure MongoDB Atlas**
    -   Create a MongoDB Atlas account at [mongodb.com/atlas](https://www.mongodb.com/atlas)
    -   Create a new cluster (M0 free tier recommended)
    -   Create a database named `diyawanna_sup_main`
    -   Get your connection string

5.  **Update `appsettings.json`**
    Open `src/DiyawannaSupBackend.Api/appsettings.json` and update the `MongoDbSettings:ConnectionString` and `JwtSettings:Secret` with your actual values. The JWT secret should be a long, strong key.

6.  **Run the Application**
    Navigate to the solution root directory (`DiyawannaSupBackend/`) and run:
    ```bash
    dotnet run --project src/DiyawannaSupBackend.Api/DiyawannaSupBackend.Api.csproj
    ```
    The application will start, typically on `http://localhost:5000` or `http://localhost:5001` (HTTPS). Swagger UI will be available at `/swagger`.

### Docker Setup (Optional)

1.  **Create `Dockerfile` in the solution root (`DiyawannaSupBackend/`)**
    ```dockerfile
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
    ```

2.  **Create `docker-compose.yml` in the solution root (`DiyawannaSupBackend/`)**
    ```yaml
    version: '3.8'

    services:
      diyawannasupbackend:
        build:
          context: .
          dockerfile: Dockerfile
        ports:
          - "8080:8080" # Map host port 8080 to container port 8080
        environment:
          # Ensure these match your appsettings.json keys
          - MongoDbSettings__ConnectionString=mongodb+srv://<username>:<password>@<cluster>.mongodb.net/diyawanna_sup_main?retryWrites=true&w=majority
          - MongoDbSettings__DatabaseName=diyawanna_sup_main
          - JwtSettings__Secret=your-production-secret-key-256-bits-or-longer
          - JwtSettings__ExpirationMinutes=60
        # Optional: Link to a MongoDB container if running locally
        # depends_on:
        #   - mongodb
        # networks:
        #   - app-network

      # Optional: Local MongoDB container
      # mongodb:
      #   image: mongo:7.0
      #   ports:
      #     - "27017:27017"
      #   volumes:
      #     - mongodb_data:/data/db
      #   networks:
      #     - app-network

    # networks:
    #   app-network:
    #     driver: bridge

    # volumes:
    #   mongodb_data:
    ```
    **Note**: Remember to replace placeholders in `docker-compose.yml` with your actual MongoDB connection string and JWT secret.

3.  **Build and Run with Docker Compose**
    ```bash
    docker-compose up --build -d
    ```

## Configuration

### `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.Authentication": "Information"
    }
  },
  "AllowedHosts": "*",
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://<username>:<password>@<cluster>.mongodb.net/diyawanna_sup_main?retryWrites=true&w=majority",
    "DatabaseName": "diyawanna_sup_main"
  },
  "JwtSettings": {
    "Secret": "your-256-bit-secret-key-here-that-is-long-enough-and-secure",
    "ExpirationMinutes": 60
  },
  "CacheSettings": {
    "UsersCacheDurationMinutes": 5,
    "UniversitiesCacheDurationMinutes": 10
  }
}
```

### Environment Variables

For production deployment, use environment variables to override `appsettings.json` values:

```bash
export MongoDbSettings__ConnectionString="mongodb+srv://prod-user:password@prod-cluster.mongodb.net/diyawanna_sup_main"
export JwtSettings__Secret="your-production-secret-key-256-bits-or-longer"
export ASPNETCORE_URLS="http://+:8080" # Or your desired port
```
Note the double underscore `__` for nested configuration sections.

## API Documentation

The project uses Swagger/OpenAPI for API documentation. Once the application is running, navigate to `/swagger` in your browser (e.g., `http://localhost:5000/swagger`).

### Base URL
```
http://localhost:5000/api
```
(Or whatever port your application runs on)

### Authentication Endpoints

#### `POST /api/auth/login`
Authenticate user and receive JWT token.

**Request Body:**
```json
{
  "username": "admin",
  "password": "password123"
}
```

**Successful Response (200 OK):**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "expiresIn": 3600000,
  "message": "Login successful"
}
```

#### `POST /api/auth/register`
Register a new user account.

**Request Body:**
```json
{
  "name": "John Doe",
  "username": "johndoe",
  "email": "john@example.com",
  "password": "password123",
  "age": 25,
  "university": "University of Colombo",
  "school": "Faculty of Science",
  "work": "Software Engineer"
}
```

**Successful Response (201 Created):**
```json
{
  "id": "64f8a1b2c3d4e5f6a7b8c9d0",
  "name": "John Doe",
  "username": "johndoe",
  "email": "john@example.com",
  "passwordHash": "...", // Hashed password, not returned in real API
  "age": 25,
  "university": "University of Colombo",
  "school": "Faculty of Science",
  "work": "Software Engineer",
  "active": true,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

#### `POST /api/auth/validate`
Validate JWT token. Requires `Authorization: Bearer <jwt-token>` header.

**Successful Response (200 OK):**
```json
{
  "valid": true,
  "username": "johndoe",
  "expiresAt": "2024-01-15T11:30:00Z"
}
```

### User Management Endpoints

#### `GET /api/users`
Get all active users. Requires `Authorization: Bearer <jwt-token>` header.

#### `GET /api/users/{id}`
Get user by ID. Requires `Authorization: Bearer <jwt-token>` header.

#### `POST /api/users`
Create a new user. Requires `Authorization: Bearer <jwt-token>` header.

#### `PUT /api/users/{id}`
Update user information. Requires `Authorization: Bearer <jwt-token>` header.

#### `DELETE /api/users/{id}`
Soft delete user (sets active = false). Requires `Authorization: Bearer <jwt-token>` header.

#### `GET /api/users/search/name?name={name}`
Search users by name. Requires `Authorization: Bearer <jwt-token>` header.

#### `GET /api/users/university/{university}`
Get users by university. Requires `Authorization: Bearer <jwt-token>` header.

#### `GET /api/users/age-range?minAge={min}&maxAge={max}`
Get users within age range. Requires `Authorization: Bearer <jwt-token>` header.

## Database Schema

The application uses MongoDB with the following collections. The schema is largely the same as the Java version.

#### `users` Collection
```javascript
{
  "_id": ObjectId,
  "name": String,
  "username": String (unique),
  "email": String (unique),
  "passwordHash": String (hashed), // Renamed from 'password'
  "age": Number,
  "university": String,
  "school": String,
  "work": String,
  "active": Boolean,
  "createdAt": Date,
  "updatedAt": Date
}
```
**Indexes:**
- `username` (unique)
- `email` (unique)
- `active`
- `university`
- `age`
- `createdAt`
- Compound: `{active: 1, university: 1}`

(Other collections like `universities`, `faculties`, `carts`, `queries` will have similar structures once implemented.)

## Security

### Authentication Flow

1.  **User Registration/Login**
    -   User provides credentials.
    -   Password is hashed using BCrypt.
    -   JWT token is generated upon successful authentication.

2.  **Token Validation**
    -   Each protected request includes JWT token in `Authorization` header (`Bearer <token>`).
    -   ASP.NET Core's `JwtBearerAuthentication` middleware validates the token.
    -   User claims are set in `HttpContext.User`.

3.  **Authorization**
    -   `[Authorize]` attribute on controllers or actions enforces authentication.
    -   Role-based access control can be implemented using `[Authorize(Roles = "Admin")]`.

### Security Features

-   **Password Encryption**: BCrypt.Net-Next with salt rounds.
-   **JWT Tokens**: Stateless authentication with configurable expiration.
-   **CORS Configuration**: Cross-origin request handling.
-   **Input Validation**: Using Data Annotations (`[Required]`, etc.) and model binding.
-   **NoSQL Injection Prevention**: MongoDB .NET Driver handles parameterization.

## Performance Optimizations

### Database Optimizations

1.  **Connection Pooling**: Handled automatically by the MongoDB .NET Driver.
2.  **Indexing Strategy**: Indexes are created on application startup (see `MongoDbIndexCreator.cs`).
3.  **Query Optimization**: Efficient query patterns using the MongoDB .NET Driver.

### Caching Strategy

1.  **Application-Level Caching**: Uses `IMemoryCache` for in-memory caching. This can be extended to distributed caching (e.g., Redis) for multi-instance deployments.
2.  **Cache Configuration**: Cache durations are configurable in `appsettings.json`.

## Testing (To be expanded)

The project structure includes a `DiyawannaSupBackend.Tests` project for unit and integration tests.

### Testing Strategy

1.  **Unit Tests**: For service layer logic using xUnit and Moq.
2.  **Integration Tests**: For API endpoints and database interactions using `Microsoft.AspNetCore.Mvc.Testing`.

### Running Tests

Navigate to the solution root directory (`DiyawannaSupBackend/`) and run:
```bash
dotnet test
```

## Error Handling

### Global Exception Handling

The application uses a centralized error handling approach configured in `Program.cs` using `app.UseExceptionHandler()`. Custom exceptions are mapped to appropriate HTTP status codes and problem details responses.

### Error Response Format

All error responses follow a consistent Problem Details (RFC 7807) format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Resource Not Found",
  "status": 404,
  "detail": "User with ID 12345 not found",
  "instance": "Your API endpoint path here"
}
```

## Deployment

### Production Deployment

1.  **Build Production DLL**
    Navigate to the solution root and run:
    ```bash
    dotnet publish src/DiyawannaSupBackend.Api/DiyawannaSupBackend.Api.csproj -c Release -o ./publish
    ```

2.  **Run Application**
    ```bash
    dotnet ./publish/DiyawannaSupBackend.Api.dll
    ```
    Ensure environment variables are set for production configuration (e.g., `MongoDbSettings__ConnectionString`, `JwtSettings__Secret`).

### Docker Deployment

Refer to the "Docker Setup" section above for `Dockerfile` and `docker-compose.yml` examples.

### Cloud Deployment

ASP.NET Core applications can be deployed to various cloud platforms:
-   **Azure App Service / Azure Kubernetes Service (AKS)**
-   **AWS Elastic Beanstalk / ECS / EKS**
-   **Google Cloud Run / GKE**
-   **Heroku**

## Contributing

Contributions are welcome! Please follow standard Git workflow:
1.  Fork the repository.
2.  Create a new feature branch (`git checkout -b feature/your-feature`).
3.  Commit your changes (`git commit -m 'feat: add new feature'`).
4.  Push to the branch (`git push origin feature/your-feature`).
5.  Create a Pull Request.

## License

This project is licensed under the MIT License.

## Support

For support and questions, please refer to the project's GitHub issues page (if hosted).


