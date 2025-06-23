# 📌 Livemap-web ![Livemap](https://img.shields.io/badge/Livemap-cSharp_App-blue)
---
| CI                | status                                                                                                                                                                                                    |
|-------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| .NET              | [![.NET](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-dotnet.yml/badge.svg)](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-dotnet.yml)                                 |
| .NET Build & Test | [![.NET Build & Test](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-tests_coverage.yml/badge.svg)](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-tests_coverage.yml)    |
| Roslyn Analyzer   | [![Roslyn Analyzers](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-roslyn_analyzers.yml/badge.svg)](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-roslyn_analyzers.yml) |
| Tailwind/Preline  | [![Tailwind CSS Build](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-tailwind.yml/badge.svg)](https://github.com/STU-GroupI/LiveMap-Web/actions/workflows/ci-tailwind.yml)               |


## 🚩 Structure

```markdown
│── Livemap.Api                 # Api controllers and endpoints
│── Livemap.Application         # Application layer, contains all the business logic
│── Livemap.Domain              # Domain layer, contains all the domain entities and value objects
│── Livemap.Infrastructure      # Infrastructure layer, contains all the logic to connect the web app to the API
│── Livemap.Persistence         # Persistence layer, contains all the database related code
│── Livemap.Tests               # Tests layer, contains all the tests
│── Livemap.Web                 # Web layer, contains all the web related code
```

## 📦 Main Packages Used

### NuGet Packages by Project

**LiveMap.Api**
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets
- Microsoft.CodeAnalysis.Analyzers
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore

**LiveMap.Infrastructure**
- Microsoft.Extensions.Http

**LiveMap.Persistence**
- Bogus
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite
- Microsoft.EntityFrameworkCore.Tools
- NetTopologySuite

**LiveMapDashboard.Web**
- Microsoft.CodeAnalysis.Analyzers
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets

### Primary npm Package for Dashboard

**LiveMapDashboard.Web**
- Postcss - `8.5.3`
- Tailwindcss - `4.1.3`
- Preline - `3.0.1`
- Maplibre - `5.4.0`
- Geocoder - `1.5.0`
- Mapbox (draw) - `1.5.0`

## 🧪 Run
Please run the following from the root folder to boot the application in debug mode.
This will start the dashboard, API, and image server projects:

```powershell
cd LiveMapDashboard.Web
npm install
cd ..
dotnet clean
dotnet restore
dotnet build

# Open three terminals and run each of the following commands in a separate terminal:
dotnet run --project LiveMapDashboard.Web
dotnet run --project LiveMap.Api
dotnet run --project LiveMap.ImageServer
```

> For best results, use separate terminal windows for each `dotnet run` command so you can see the output from each service.

## Tests
To run the tests, please make sure that you have docker running on your machine. The tests use a testcontainer to boot up database instances per test group to keep the tests isolated from each other. This, however, does mean that the host machine will be running up to +-7 database instances at ones. Please make sure that your machine is able to handle these kinds of loads as the tests may be very slow on a low power machine. You could alternatively run the tests without parralelism enabled. On low power machines this is actually faster than running the tests in normal mode.