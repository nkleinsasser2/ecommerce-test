# 🛒 ECommerce-Microservices

> **The primary objective of this project is transition a codebase with a monolith architecture to a microservice architecture to understand the required workflow and maintenance.** 🚀

> 💡 **This project is education-oriented and technical aspects mainly relate to changing startup processes and defining communication networks.**

## Technologies - Libraries

- ✔️ **[`.NET 9`](https://github.com/dotnet/aspnetcore)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core.
- ✔️ **[`MVC Versioning API`](https://github.com/microsoft/aspnet-api-versioning)** - Set of libraries which add service API versioning to ASP.NET Web API, OData with ASP.NET Web API, and ASP.NET Core.
- ✔️ **[`EF Core`](https://github.com/dotnet/efcore)** - Modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations.
- ✔️ **[`AspNetCore OpenApi`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi?view=aspnetcore-9.0&tabs=visual-studio#configure-openapi-document-generation)** - Provides built-in support for OpenAPI document generation in ASP.NET Core.
- ✔️ **[`MediatR`](https://github.com/jbogard/MediatR)** - Simple, unambitious mediator implementation in .NET.
- ✔️ **[`FluentValidation`](https://github.com/FluentValidation/FluentValidation)** - Popular .NET validation library for building strongly-typed validation rules.
- ✔️ **[`Swagger & Swagger UI`](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Swagger tools for documenting API's built on ASP.NET Core.
- ✔️ **[`Serilog`](https://github.com/serilog/serilog)** - Simple .NET logging with fully-structured events.
- ✔️ **[`Scrutor`](https://github.com/khellang/Scrutor)** - Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection.
- ✔️ **[`AutoMapper`](https://github.com/AutoMapper/AutoMapper)** - Convention-based object-object mapper in .NET.
- ✔️ **[`NewId`](https://github.com/phatboyg/NewId)** - NewId can be used as an embedded unique ID generator that produces 128 bit (16 bytes) sequential IDs.
- ✔️ **[`Sieve`](https://github.com/Biarity/Sieve)** - Sieve is a framework for .NET Core that adds sorting, filtering, and pagination functionality out of the box.
- ✔️ **[`xUnit.net`](https://github.com/xunit/xunit)** - A free, open source, community-focused unit testing tool for the .NET Framework.
- ✔️ **[`Respawn`](https://github.com/jbogard/Respawn)** - Respawn is a small utility to help in resetting test databases to a clean state.
- ✔️ **[`Testcontainers`](https://github.com/testcontainers/testcontainers-dotnet)** - Testcontainers for .NET is a library to support tests with throwaway instances of Docker containers.
- ✔️ **[`Bogus`](https://github.com/bchavez/Bogus)** - Bogus is a simple fake data generator for .NET.

## Development Setup

### Dotnet Tools Packages
For installing our requirement package with .NET cli tools, we need to install `dotnet tool manifest`.
```bash
dotnet new tool-manifest

```
And after that we can restore our dotnet tools packages with .NET cli tools from `.config` folder and `dotnet-tools.json` file.
```bash
dotnet tool restore
```

## How to Run

You can run this app by defining the startup projects to include all of the relevant services upon startup.

1. 

## References

Thank you to the contributors from the [original codebase](https://github.com/meysamhadeli/ecommerce-monolith) from which we forked the project and performed our changes. 