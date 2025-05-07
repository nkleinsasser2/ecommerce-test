FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ECommerce.Inventories.Api/ECommerce.Inventories.Api.csproj ECommerce.Inventories.Api/
COPY ECommerce.Infrastructure/ECommerce.Infrastructure.csproj ECommerce.Infrastructure/
COPY src/BuildingBlocks/BuildingBlocks.csproj src/BuildingBlocks/
COPY src/ECommerce/ECommerce.csproj src/ECommerce/

COPY Directory.Build.props ./

RUN dotnet nuget locals all --clear
RUN dotnet restore ECommerce.Inventories.Api/ECommerce.Inventories.Api.csproj

COPY ECommerce.Inventories.Api/ ECommerce.Inventories.Api/
COPY ECommerce.Infrastructure/ ECommerce.Infrastructure/
COPY src/ECommerce/ src/ECommerce/
COPY src/BuildingBlocks/Core/ src/BuildingBlocks/Core/
COPY src/BuildingBlocks/EFCore/ src/BuildingBlocks/EFCore/
COPY src/BuildingBlocks/Exception/ src/BuildingBlocks/Exception/
COPY src/BuildingBlocks/Logging/ src/BuildingBlocks/Logging/
COPY src/BuildingBlocks/OpenApi/ src/BuildingBlocks/OpenApi/
COPY src/BuildingBlocks/Validation/ src/BuildingBlocks/Validation/
COPY src/BuildingBlocks/Web/ src/BuildingBlocks/Web/
COPY src/BuildingBlocks/BuildingBlocks.csproj src/BuildingBlocks/

RUN mkdir -p src/BuildingBlocks/TestBase
RUN touch src/BuildingBlocks/TestBase/Placeholder.cs
RUN echo "namespace BuildingBlocks.TestBase { public class Placeholder {} }" > src/BuildingBlocks/TestBase/Placeholder.cs

WORKDIR /app/ECommerce.Inventories.Api

RUN dotnet nuget locals all --clear
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y postgresql-client curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

RUN echo '#!/bin/bash\necho "Waiting for PostgreSQL schema and Inventory seed data..."\nexport PGPASSWORD=postgres\nuntil psql -h postgres -U postgres -d ecommerce -c "SELECT 1 FROM \"Inventory\" LIMIT 1;" &> /dev/null; do\n  echo "PostgreSQL Inventory data is not ready - sleeping 2s"\n  sleep 2\ndone\necho "PostgreSQL data is ready - Starting the application"\nexec dotnet ECommerce.Inventories.Api.dll\n' > /app/wait-for-postgres.sh \
    && chmod +x /app/wait-for-postgres.sh

EXPOSE 5004
ENV ASPNETCORE_URLS=http://+:5004

ENTRYPOINT ["/app/wait-for-postgres.sh"] 