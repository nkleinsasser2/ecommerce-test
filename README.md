# 🛒 E-Commerce Microservices Architecture

> This project demonstrates the transition from a monolithic architecture to a microservices-based architecture, showcasing best practices in service decomposition, API design, and containerization.

## 🚀 Quick Start

### Prerequisites
- Docker Desktop
- Docker Compose
- .NET 8.0 SDK (for local development)

### Running with Docker

1. **Build and start all services**
   ```bash
   docker-compose up --build
   ```

2. **Run tests**
   ```bash
   .\run-tests.ps1
   ```

3. **Access the APIs**
   - Products API: http://localhost:5001/swagger
   - Categories API: http://localhost:5002/swagger
   - Orders API: http://localhost:5003/swagger
   - Inventories API: http://localhost:5004/swagger

## 🏗️ Architecture

The application has been redesigned from a monolithic architecture to a microservices-based architecture with the following services:

1. **Products API** (Port 5001)
   - Product catalog management
   - Product CRUD operations
   - Product search and filtering

2. **Categories API** (Port 5002)
   - Category management
   - Product categorization
   - Category hierarchy

3. **Orders API** (Port 5003)
   - Order processing
   - Order lifecycle management
   - Order status tracking

4. **Inventories API** (Port 5004)
   - Inventory management
   - Stock level tracking
   - Inventory updates

## 🛠️ Technology Stack

- **.NET 8.0** - Core framework
- **PostgreSQL** - Database
- **Docker** - Containerization
- **Docker Compose** - Service orchestration
- **Swagger/OpenAPI** - API documentation
- **MediatR** - CQRS implementation
- **FluentValidation** - Request validation
- **AutoMapper** - Object mapping
- **xUnit** - Testing framework

## 🔄 Development Workflow

1. **Local Development**
   ```bash
   # Start services
   docker-compose up -d

   # Run tests
   .\run-tests.ps1
   ```

2. **Adding New Features**
   - Create feature in appropriate service
   - Implement using CQRS pattern
   - Add tests
   - Update documentation

## 📚 API Documentation

Each service provides Swagger UI for API documentation:
- Products API: http://localhost:5001/swagger
- Categories API: http://localhost:5002/swagger
- Orders API: http://localhost:5003/swagger
- Inventories API: http://localhost:5004/swagger

## 🧪 Testing

The solution includes comprehensive test coverage:
- Unit tests
- Integration tests
- End-to-end tests

Run all tests using:
```bash
.\run-tests.ps1
```

## 🔍 Troubleshooting

1. **Docker Issues**
   - Ensure Docker Desktop is running
   - Check container logs: `docker-compose logs`
   - Verify port availability

2. **Database Issues**
   - Check database container status
   - Verify connection strings
   - Check database logs

3. **API Issues**
   - Verify service health: `http://localhost:<port>/health`
   - Check service logs
   - Verify API documentation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 🙏 Acknowledgments

Thank you to the contributors from the [original codebase](https://github.com/meysamhadeli/ecommerce-monolith) from which we forked the project and performed our changes. 