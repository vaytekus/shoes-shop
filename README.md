# shoes-shop

E-commerce platform for footwear built with .NET microservices and React frontend.

## Tech Stack

### Backend
- **.NET 10** — Web API (Minimal APIs)
- **MediatR** — CQRS (Commands / Queries)
- **FluentValidation** — validation pipeline behavior
- **EF Core + Npgsql** — ORM + PostgreSQL per service
- **Keycloak** — Identity Provider (OpenID Connect / OAuth2)
- **Redis** — Basket + output cache
- **RabbitMQ** → **Azure Service Bus** (prod) — async messaging between services
- **MinIO** → **Azure Blob Storage** (prod) — media files
- **Elasticsearch** → **Azure AI Search** (prod) — full-text search
- **Firebase FCM** — push notifications
- **YARP** — API Gateway
- **Serilog** — structured logging
- **OpenTelemetry** — distributed tracing + metrics
- **Polly** — resilience (retry, circuit breaker)

### Frontend
- **React + TypeScript**
- **TanStack Query** — server state
- **Zustand** — client state (basket)
- **React Router v7**

### Infrastructure
- **.NET Aspire** — local orchestration (service discovery, dashboard)
- **Docker + docker-compose** — containerization
- **Kubernetes / AKS** — production orchestration
- **Azure Container Registry** — image registry
- **Azure Key Vault** — secrets management
- **Azure Monitor** — observability (prod)

## Modules

| Module | Responsibility |
|---|---|
| **Identity** | Keycloak — auth, registration, roles |
| **Catalog** | Products, categories, brands, sizes |
| **Basket** | Shopping cart (Redis) |
| **Order** | Order lifecycle, Outbox pattern |
| **Payment** | Payment processing |
| **Wishlist** | User wishlists |
| **Review** | Product reviews and ratings |
| **Inventory** | Stock management |
| **Promotion** | Discounts, coupons, promo codes |
| **Notification** | Firebase push notifications |
| **Analytics** | Events, reports, dashboards |
| **Admin** | Back-office management |
| **Media** | File upload, image processing |
| **Search** | Full-text search via Elasticsearch |

## Architecture

**Microservices** — each service has its own process, database, and Docker container.

**Vertical Slices** — each feature (use case) contains its query/command, handler, validator, and endpoint in one folder.

**CQRS** — read and write operations separated via MediatR.

**Async communication** — events between services via RabbitMQ (local) / Azure Service Bus (prod).

**Outbox pattern** — guaranteed event delivery for Order and Payment services.

```
[React] ──→ [YARP Gateway] ──→ [Catalog.API]
                           ──→ [Basket.API]
                           ──→ [Order.API]
                           ──→ [Payment.API]
                           ──→ ...

[Order.API] ──→ [RabbitMQ] ──→ [Inventory.API]
                           ──→ [Notification.API]
                           ──→ [Analytics.API]

[Keycloak] ← all services validate JWT token
```

## Project Structure

```
shoes-shop/
├── src/
│   ├── AppHost/                        # .NET Aspire orchestrator
│   ├── ServiceDefaults/                # shared: telemetry, health checks, service discovery
│   │
│   ├── Gateway/
│   │   └── ShoesShop.Gateway/          # YARP API Gateway
│   │
│   ├── Services/
│   │   ├── Catalog/
│   │   │   ├── ShoesShop.Catalog.Api/
│   │   │   └── ShoesShop.Catalog.Tests/
│   │   ├── Basket/
│   │   ├── Order/
│   │   ├── Payment/
│   │   ├── Wishlist/
│   │   ├── Review/
│   │   ├── Inventory/
│   │   ├── Promotion/
│   │   ├── Notification/
│   │   ├── Analytics/
│   │   ├── Admin/
│   │   ├── Media/
│   │   └── Search/
│   │
│   └── Shared/
│       └── ShoesShop.Shared/           # BaseEntity, Pagination, Events, errors
│
├── frontend/
│   └── web/                            # React + TypeScript
│
├── k8s/
│   └── helm/                           # Helm charts for AKS
│
└── docker-compose.yml
```

## Local Development

Requires: .NET 10 SDK, Docker Desktop

```bash
dotnet run --project src/AppHost
```

Opens Aspire dashboard at `http://localhost:15888` — logs, traces, and health status for all services.

## Production (Azure)

```bash
azd init
azd up
```

Deploys to AKS with Azure Database for PostgreSQL, Azure Cache for Redis, Azure Service Bus, Azure Blob Storage, Azure AI Search.
