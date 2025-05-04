# recipes-manager-api

A comprehensive backend API for managing recipes, ingredients, menus, shopping lists, and user subscriptions focused on scalable and extensible functionality with C# and .NET 7.

## Table of Contents
- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Configuration](#configuration)

## Features
- User authentication & authorization with JWT tokens
- Recipe CRUD operations with support for categories, ingredients, and images
- Ingredient parsing and calorie estimation streamed through server-sent events (SSE)
- Management of menus, shopping lists, contacts, and subscriptions
- OpenAI integration for chat completions with logging support
- Roles and user management with role assignments
- Health checks integrated with MongoDB
- GraphQL API support
- Background service for health monitoring
- Image upload with cloud storage support
- Search capabilities for recipes, categories, and saved/shared items
- Azure App Configuration integration
- Automated CI/CD workflows for deployment to Azure Web Apps

## Stack
- Language: C#
- Framework: .NET 7 (ASP.NET Core)
- Database: MongoDB
- Authentication: JWT
- API: REST Controllers + GraphQL (HotChocolate)
- Cloud: Azure App Configuration, Azure Web Apps
- Dev Tools: Visual Studio Code, Docker Dev Containers

## Installation

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [MongoDB](https://www.mongodb.com/try/download/community) instance running
- Docker (optional, for dev container)

### Setup Instructions

1. Clone the repository:
```bash
git clone https://github.com/Shchoholiev/recipes-manager-api.git
cd recipes-manager-api
```

2. Restore and build the API project:
```bash
cd RecipesManagerApi.Api
dotnet restore
dotnet build
```

3. Run the API locally:
```bash
dotnet run
```

4. Optional: Run the DbInitializer to populate initial data
```bash
cd ../RecipesManagerApi.DbInitializer
dotnet run
```

5. Access Swagger UI for API testing at:
```
https://localhost:<port>/swagger
```

6. To develop inside a dev container (VSCode recommended), open the workspace and let the `.devcontainer` config set up the environment.

7. CI/CD Deployment:
- GitHub Actions workflows are configured for deployments on `develop` (dev environment) and `master` (production) branches to Azure Web Apps.

## Configuration

Configure the following environment variables or use the provided `appsettings.json` and Azure App Configuration:

- `ConnectionStrings:MongoDb` - Connection string for MongoDB database
- `ConnectionStrings:MongoDatabaseName` - MongoDB database name
- `APP_CONFIG` (optional) - Azure App Configuration connection string or resource identifier
- JWT Authentication settings (configured internally via `.AddJWTTokenAuthentication`)
- Logging configurations for API behavior

You can modify the `appsettings.json` file in `RecipesManagerApi.Api` for local development settings.

To enable Azure App Configuration integration, ensure that the `APP_CONFIG` environment variable is set or update the connection string in configuration files.
