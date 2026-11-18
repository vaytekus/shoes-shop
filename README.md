# Shoes Shop — Backend

.NET microservices backend for a footwear e-commerce store. Educational project.

## Tech Stack

- **.NET 10** — Minimal APIs
- **MediatR** — CQRS (Commands / Queries / Handlers)
- **FluentValidation** — validation pipeline behavior
- **EF Core + Npgsql** — ORM + PostgreSQL (separate DB per service)
- **Keycloak** — Identity Provider (OpenID Connect / OAuth2), roles via realm_access
- **Redis** — shopping basket (Basket.Api)
- **RabbitMQ** — async messaging between services via MassTransit
- **MinIO** — media file storage (avatars, product images)
- **Elasticsearch** — full-text product search with facets
- **.NET Aspire** — local orchestration (service discovery, dashboard)

## Services

| Service | Responsibility |
|---|---|
| **Catalog.Api** | Products, categories, brands, sizes, reviews, ratings |
| **Basket.Api** | Shopping cart (Redis), guest basket merge on login |
| **Order.Api** | Orders, statuses, Nova Poshta tracking |
| **Wishlist.Api** | User wishlist |
| **AppHost** | .NET Aspire orchestrator |
| **ServiceDefaults** | Shared: health checks, service discovery, telemetry |

## Implemented

- Product catalog with pagination, filters, Elasticsearch full-text search with facets
- Basket for authenticated users and guests (guestId in cookie)
- Guest basket merge on login
- Checkout for both authenticated and guest users
- Order statuses: Pending → Processing → Shipped → Delivered / Cancelled / Returned
- Nova Poshta auto-tracking — `BackgroundService` polls TTN status every 30 min, auto-updates to `Delivered` (StatusCode 9) or `Returned` (StatusCode 7)
- Product reviews with moderation (Pending / Approved / Rejected)
- Product ratings — denormalized into Elasticsearch via `ReviewStatusChangedEvent`
- Wishlist
- Avatar upload (MinIO)
- Vertical slice architecture (feature folders)

## Architecture

**Vertical Slices** — each feature contains Command/Query, Handler, Validator, and Endpoint in one folder.

**CQRS** — reads and writes separated via MediatR.

**Async messaging** — events between services via RabbitMQ + MassTransit.

```
[Frontend / Admin] ──→ [Catalog.Api]  ──→ [PostgreSQL]
                   ──→ [Basket.Api]   ──→ [Redis]
                   ──→ [Order.Api]    ──→ [PostgreSQL]
                   ──→ [Wishlist.Api] ──→ [PostgreSQL]

[Catalog.Api] ──→ [RabbitMQ] ──→ [Catalog.Api consumer] (ReviewStatusChangedEvent)

[Order.Api BackgroundService] ──→ [Nova Poshta API] (TTN tracking)

[Keycloak] ← all services validate JWT
```

## Project Structure

```
shoes_shop/src/
├── AppHost/                    # .NET Aspire orchestrator
├── ServiceDefaults/            # health checks, service discovery
├── Shared/ShoesShop.Shared/    # BaseEntity, Pagination, Events
├── Catalog.Api/
│   ├── Domain/                 # Product, Category, ProductReview, ReviewStatus
│   ├── DTOs/
│   ├── Features/               # vertical slices
│   │   ├── Products/
│   │   ├── Categories/
│   │   ├── Reviews/
│   │   └── Search/
│   ├── Infrastructure/         # CatalogDbContext, MinioService, ElasticsearchService
│   ├── Configurations/         # EF entity configs
│   └── Migrations/
├── Basket.Api/
│   └── Features/Basket/
├── Order.Api/
│   ├── Domain/                 # CustomerOrder, OrderItem, OrderStatus, DeliveryType
│   ├── Features/Orders/        # CreateOrder, UpdateOrderStatus, SetTrackingNumber
│   └── Infrastructure/         # NovaPoshtaTrackingClient, OrderTrackingBackgroundService
└── Wishlist.Api/
```

## Getting Started

Requirements: .NET 10 SDK, Docker Desktop

```bash
dotnet run --project src/AppHost
```

Aspire dashboard at `http://localhost:15888` — logs, traces, health status for all services.

## Backlog

### Backend
- **Polly** — resilience between services (retry, circuit breaker, timeout)
- **Catalog.Api caching** — Redis cache for product list (TTL 5 min)
- **Optimistic Concurrency** — RowVersion on CustomerOrder
- **Redis Sorted Sets** — top popular products by sales
- **Social login** — Google, GitHub via Keycloak Identity Providers
- **Fix JWT authorization** — proper `Authority` + `Audience` validation instead of manual token parsing
- **Stock check on checkout** — validate stockQuantity in CreateOrder, return 409 if insufficient
- **Backend guestId generation** — `POST /api/basket/guest`
- **SizeChart** — multiple size systems (EU/US/UK)
- **OpenIddict migration** — replace Keycloak with OpenIddict (new Identity.Api)

### Email Notifications
- **Notification Worker Service** — listens to RabbitMQ events (`OrderCreatedEvent`, `ReviewStatusChangedEvent`), sends via Resend or Brevo
- Emails: order confirmation, status change, review approved/rejected

### Payments
- **WayForPay** — HMAC MD5 signed request, POST callback (UA market)
- **Stripe** — PaymentIntent flow, webhook for payment confirmation

### Tests
- **Unit tests** — xUnit + EF SQLite in-memory + NSubstitute for Catalog.Api handlers
- **Integration tests** — xUnit + Testcontainers.PostgreSql + WebApplicationFactory
