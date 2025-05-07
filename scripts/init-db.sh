#!/bin/bash
set -e

echo "Waiting for PostgreSQL to be ready..."
until PGPASSWORD=postgres psql -h postgres -U postgres -c '\q'; do
  echo "PostgreSQL is unavailable - sleeping"
  sleep 1
done

echo "PostgreSQL is up - creating database if it doesn't exist"
PGPASSWORD=postgres psql -h postgres -U postgres -tc "SELECT 1 FROM pg_database WHERE datname = 'ecommerce'" | grep -q 1 || PGPASSWORD=postgres psql -h postgres -U postgres -c "CREATE DATABASE ecommerce"

echo "Executing schema script"
PGPASSWORD=postgres psql -h postgres -U postgres -d ecommerce -f /app/scripts/schema.sql

echo "Schema created - executing init script"
PGPASSWORD=postgres psql -h postgres -U postgres -d ecommerce -f /app/scripts/init-db.sql

echo "Database initialization completed" 