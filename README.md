# E-Commerce API Gateway

Central API Gateway for the E-Commerce microservices platform using YARP (Yet Another Reverse Proxy).

## Features

- Request Routing to Microservices
- API Versioning Support
- Authentication/Authorization (Keycloak)
- Multi-Tenancy Support
- Rate Limiting
- Response Caching (Redis)
- Health Check Aggregation
- Swagger UI Aggregation
- Load Balancing
- Circuit Breaker Pattern

## Endpoints

- `/api/v1/customers/**` → Customer Management Service
- `/api/v1/products/**` → Product Catalog Service
- `/api/v1/orders/**` → Order Management Service
- `/health` → Health Check Aggregation
- `/swagger` → API Documentation

## Configuration

Environment variables:
- `KEYCLOAK_AUTHORITY` - Keycloak server URL
- `REDIS_CONNECTION_STRING` - Redis connection string
- `CUSTOMER_SERVICE_URL` - Customer service endpoint
- `PRODUCT_SERVICE_URL` - Product service endpoint
- `ORDER_SERVICE_URL` - Order service endpoint

## Run Locally

```bash
dotnet run
```

Access: https://localhost:7000
