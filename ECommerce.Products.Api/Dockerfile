FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ECommerce.Products.Api/ECommerce.Products.Api.csproj ECommerce.Products.Api/
COPY ECommerce.Infrastructure/ECommerce.Infrastructure.csproj ECommerce.Infrastructure/
COPY src/BuildingBlocks/BuildingBlocks.csproj src/BuildingBlocks/
COPY src/ECommerce/ECommerce.csproj src/ECommerce/

COPY Directory.Build.props ./

RUN dotnet nuget locals all --clear
RUN dotnet restore ECommerce.Products.Api/ECommerce.Products.Api.csproj

COPY ECommerce.Products.Api/ ECommerce.Products.Api/
COPY ECommerce.Infrastructure/ ECommerce.Infrastructure/
COPY src/ECommerce/ src/ECommerce/
COPY src/BuildingBlocks/ src/BuildingBlocks/

WORKDIR /app/ECommerce.Products.Api
RUN dotnet nuget locals all --clear
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y postgresql-client curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

RUN echo '#!/bin/bash\n\
echo "Waiting for PostgreSQL schema and Inventory seed data..."\n\
export PGPASSWORD=postgres\n\
until psql -h postgres -U postgres -d ecommerce -c "SELECT 1 FROM \"Inventory\" LIMIT 1;" &> /dev/null; do\n\
  echo "PostgreSQL Inventory data is not ready - sleeping 2s"\n\
  sleep 2\n\
done\n\
echo "PostgreSQL data is ready - Starting app"\n\
exec "$@"' > /app/wait-for-postgres.sh \
&& chmod +x /app/wait-for-postgres.sh

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["/app/wait-for-postgres.sh", "dotnet", "ECommerce.Products.Api.dll"] 