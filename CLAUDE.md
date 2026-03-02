# CLAUDE.md — MicroservicesApo

## Project Overview

This is a .NET 8 microservices solution (`MicroservicesApo.sln`) implementing an e-commerce backend with an ASP.NET MVC frontend. The solution demonstrates two architectural generations side by side: simple repository-pattern services (V1) and Clean Architecture with CQRS/MediatR (V2).

---

## Solution Structure

```
MicroservicesApo.sln
├── FrontEnd/
│   └── Apo.Web/                          # ASP.NET MVC frontend (port 7000 via gateway)
├── GateWay/
│   └── Apo.Gateway/                      # Ocelot API Gateway (port 7000)
├── Integration/
│   ├── Apo.Messaging/                    # RabbitMQ messaging library
│   └── Apo.Messaging.Tests/              # xUnit + Testcontainers integration tests
├── Services/
│   ├── AuthAPI/
│   │   └── Apo.Service.AuthAPI/          # Authentication & JWT (port 7002)
│   ├── CouponAPI/
│   │   ├── Apo.Services.CouponAPI/       # Coupon V1 — simple controller + EF Core
│   │   ├── Apo.Services.CouponAPI_V2/    # Coupon V2 — Clean Arch host (port 7001)
│   │   ├── Apo.Services.CouponAPI_V2.Domain/
│   │   ├── Apo.Services.CouponAPI_V2.Application/
│   │   └── Apo.Services.CouponAPI_V2.Infrastructure/
│   ├── ProductAPI/
│   │   ├── Apo.Service.ProductAPI/       # Product V1 — simple controller + EF Core (port 7003)
│   │   ├── Apo.Service.ProductAPI_V2/    # Product V2 — Clean Arch host
│   │   ├── Apo.Services.ProductAPI_V2.Domain/
│   │   ├── Apo.Services.ProductAPI_V2.Application/
│   │   └── Apo.Services.ProdictAPI_V2.Infrastructure/  # Note: typo in folder name ("Prodict")
│   └── ShoppingCartAPI/
│       └── Apo.Service.ShoppingCartAPI/  # Shopping Cart (port 7004)
```

---

## Technology Stack

| Concern | Technology |
|---|---|
| Framework | .NET 8, ASP.NET Core |
| ORM | Entity Framework Core 9.x (SQL Server) |
| Object Mapping | AutoMapper 12.x |
| Input Validation | FluentValidation 12.x |
| CQRS / Mediator | MediatR 11.x |
| API Gateway | Ocelot 24.x |
| Messaging | RabbitMQ.Client 7.x |
| Auth | ASP.NET Identity + JWT (ECDSA ES256 asymmetric) |
| Crypto | BouncyCastle.Cryptography 2.x |
| API Docs | Swashbuckle / Swagger |
| Testing | xUnit + Testcontainers (RabbitMQ) |
| Frontend Auth | Cookie-based JWT forwarding |

---

## Service Port Map

| Service | Port |
|---|---|
| Apo.Gateway | 7000 |
| Apo.Services.CouponAPI (V1 or V2) | 7001 |
| Apo.Service.AuthAPI | 7002 |
| Apo.Service.ProductAPI (V1 or V2) | 7003 |
| Apo.Service.ShoppingCartAPI | 7004 |

All services are proxied through the Ocelot gateway at `https://localhost:7000`. Routes are configured in `Apo.Gateway/ocelot.json`.

---

## Database Configuration

Each service uses its own SQL Server LocalDB database (code-first migrations):

| Service | Database |
|---|---|
| AuthAPI | `Apo_Auth` |
| CouponAPI | `Apo_Coupon` |
| ProductAPI | `Apo_Product` |
| ShoppingCartAPI | `Apo_ShoppingCart` |

Connection string pattern (in each `appsettings.json`):
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Apo_<Name>;Trusted_Connection=True"
}
```

**Migrations are applied automatically on startup** via `ApplyMigrations()` called at the end of each `Program.cs`.

---

## Build & Run

```bash
# Build entire solution
dotnet build MicroservicesApo.sln

# Run a specific service
dotnet run --project Apo.Gateway/Apo.Gateway.csproj
dotnet run --project Apo.Service.AuthAPI/Apo.Service.AuthAPI.csproj
dotnet run --project Apo.Services.CouponAPI_V2/Apo.Services.CouponAPI_V2.csproj
dotnet run --project Apo.Service.ProductAPI_V2/Apo.Service.ProductAPI_V2.csproj
dotnet run --project Apo.Service.ShoppingCartAPI/Apo.Service.ShoppingCartAPI.csproj
dotnet run --project Apo.Web/Apo.Web.csproj

# Run tests (requires Docker for Testcontainers)
dotnet test Apo.Messaging.Tests/Apo.Messaging.Tests.csproj

# Add EF Core migration (example for CouponAPI V1)
dotnet ef migrations add <MigrationName> --project Apo.Services.CouponAPI
dotnet ef database update --project Apo.Services.CouponAPI
```

---

## Architectural Patterns

### V1 Services — Simple Architecture

`Apo.Services.CouponAPI`, `Apo.Service.ProductAPI`, `Apo.Service.ShoppingCartAPI` follow a flat structure:

```
Service/
├── Controllers/      # API endpoints (inherit ControllerBase)
├── Data/             # AppDbContext
├── Models/           # Domain entities + DTOs
├── Migrations/       # EF Core migrations
└── MappingConfig.cs  # AutoMapper profiles
```

Controllers directly use `AppDbContext` and `IMapper` — no repository layer.

### V2 Services — Clean Architecture + CQRS

`Apo.Services.CouponAPI_V2` and `Apo.Service.ProductAPI_V2` split across four projects:

**Domain** (`*.Domain`) — pure entities, no dependencies:
```csharp
public class Coupon
{
    [Key] public int CouponId { get; set; }
    public string CouponCode { get; set; }
    public double DiscountAmount { get; set; }
    public int MinAmount { get; set; }
}
```

**Application** (`*.Application`) — use cases, interfaces, DTOs, CQRS:
- `IRepository` interface (defined here, implemented in Infrastructure)
- `Features/<Entity>/<Operation>/` — one folder per use case:
  - `CreateXxxCommand.cs` → `IRequest<XxxDto>` (record)
  - `CreateXxxHandler.cs` → `IRequestHandler<Command, Result>`
  - `GetAllXxxQuery.cs` → `IRequest<IEnumerable<XxxDto>>` (record)
  - `GetXxxByIdQuery.cs`, `UpdateXxxCommand.cs`, `DeleteXxxCommand.cs`
- `Common/Exceptions/NotFoundException.cs`, `ValidationException.cs`
- `DependencyInjection.cs` — `AddApplication()` extension registers MediatR

**Infrastructure** (`*.Infrastructure`) — EF Core implementation:
- `AppDbContext.cs` — includes seed data
- `XxxRepository.cs` — implements `IXxxRepository`

**Host** (`Apo.Services.CouponAPI_V2`, `Apo.Service.ProductAPI_V2`):
- `Program.cs` — wires up DI, registers middleware
- `Controllers/` — thin controllers that dispatch to MediatR
- `Middleware/ExceptionMiddleware.cs` — global exception handling
- `Mapping/MappingConfig.cs` — AutoMapper setup

#### CQRS Example Pattern

```csharp
// Query (record)
public record GetCouponByIdQuery(int Id) : IRequest<CouponDto>;

// Handler
public class GetCouponByIdHandler : IRequestHandler<GetCouponByIdQuery, CouponDto>
{
    private readonly ICouponRepository _repo;
    private readonly IMapper _mapper;

    public async Task<CouponDto> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _repo.GetByIdAsync(request.Id);
        return _mapper.Map<CouponDto>(coupon);
    }
}
```

---

## Shared Conventions

### ResponseDto

Every API response is wrapped in a consistent envelope (duplicated per service, not shared as a library):

```csharp
public class ResponseDto
{
    public bool IsSuccess { get; set; } = true;
    public object? Result { get; set; }
    public string? Message { get; set; } = "";
}
```

`AuthAPI` extends this with `List<string> Errors`.

### MappingConfig

AutoMapper profiles are configured via a static `MappingConfig.RegisterMappings()` method, then registered both as a singleton and via `AddAutoMapper()`:

```csharp
IMapper mapper = MappingConfig.RegisterMappings().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
```

### Repository Interface

V2 repositories define a standard async CRUD interface in the Application layer:

```csharp
public interface IXxxRepository
{
    Task<IEnumerable<Xxx>> GetAllAsync();
    Task<Xxx?> GetByIdAsync(int id);
    Task<Xxx> CreateAsync(Xxx entity);
    Task<Xxx> UpdateAsync(Xxx entity);
    Task<bool> DeleteAsync(int id);
}
```

### Exception Middleware (V2 services)

`ExceptionMiddleware` maps domain exceptions to HTTP status codes:
- `NotFoundException` → 404
- `ValidationException` → 400
- Any other `Exception` → 500 with generic message

---

## Authentication

### JWT Strategy: ECDSA ES256 (Asymmetric)

The active authentication scheme uses ECDSA with ES256. Key files are PEM-encoded and stored at configurable paths:

```json
"ApiSettings": {
    "JwtOptionsForAssymetricES256": {
        "Issuer": "your-issuer",
        "Audience": "your-audience",
        "PrivateKeyPath": "./keys/private.pem",   // AuthAPI only
        "PublicKeyPath": "./keys/public.pem"       // all services
    }
}
```

- `AuthAPI` uses both private (to sign) and public (to validate) keys
- Gateway and all other services use only the public key to validate tokens
- A commented-out HMAC SHA256 (symmetric) implementation exists as an alternative

### JWT in Web Frontend

`JwtCookieToHeaderMiddleware` reads the JWT from a cookie and injects it as the `Authorization: Bearer <token>` header for downstream API calls.

### ASP.NET Identity

`AuthAPI` uses `IdentityDbContext<ApplicationUser>` for user management with role support.

---

## API Gateway (Ocelot)

`Apo.Gateway/ocelot.json` configures all upstream routes. Key structure:

```json
{
    "UpstreamPathTemplate": "/api/coupon/{id}",
    "UpstreamHttpMethod": ["GET"],
    "DownstreamPathTemplate": "/api/coupon/{id}",
    "DownstreamScheme": "https",
    "DownstreamHostAndPorts": [{ "Host": "localhost", "Port": 7001 }],
    "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer",
        "AllowedScopes": []
    }
}
```

Auth/register and auth/login endpoints are **unauthenticated**. All other routes require a valid Bearer token.

---

## Messaging (RabbitMQ)

`Apo.Messaging` is a standalone class library providing:

- `IMessagePublisher` — abstraction for publishing messages
- `RabbitMqMessagePublisher` — implementation using `RabbitMQ.Client`
- `RabbitMqOptions` — connection string, default exchange, default routing key

The publisher uses **Topic Exchange** by default. Queue name is derived from routing key (dots replaced with underscores). Messages are serialized with `Newtonsoft.Json`.

```csharp
await publisher.PublishAsync(
    message: new { Name = "Test" },
    exchange: "apo.exchange",
    routingKey: "order.created"
);
// → queue name: "order_created"
```

### Messaging Tests

`Apo.Messaging.Tests` uses xUnit + Testcontainers for real RabbitMQ integration tests. Three test classes:

- `RabbitMqPublisherIntegrationTests` — direct/topic exchange publish + consume
- `FanoutExchangeIntegrationTests` — fanout broadcast to multiple queues
- `TopicRoutingIntegrationTests` — topic routing with wildcard patterns

Tests share a `RabbitMqFixture` (via `IClassFixture<RabbitMqFixture>`) that spins up a RabbitMQ container.

**Requires Docker** to run the messaging tests.

---

## FluentValidation

Validators are registered via `AddValidatorsFromAssemblyContaining<Program>()` and injected directly into controller action parameters with `[FromServices] IValidator<T>`:

```csharp
[HttpPost("register")]
public async Task<ActionResult<ResponseDTO>> Register(
    [FromServices] IValidator<RegistrationRequestDTO> validator,
    [FromBody] RegistrationRequestDTO model)
{
    var validationResult = await ValidationHelper.ValidateAsync(model, validator);
    if (validationResult != null)
        return validationResult;
    // ...
}
```

`ValidationHelper.ValidateAsync` returns a `BadRequestObjectResult` with `ResponseDTO { IsSuccess = false, Message = "Validation failed", Errors = [...] }` on failure, or `null` on success.

---

## Naming Conventions

- Namespaces follow project names: `Apo.Service.ProductAPI`, `Apo.Services.CouponAPI_V2`, etc.
- Controllers are named `XxxAPIController` or `XxxController`
- Routes use lowercase: `[Route("api/coupon")]`, `[Route("api/product")]`, `[Route("api/auth")]`
- DTOs are named `XxxDto` (services) or `XxxDTO` (AuthAPI uses uppercase)
- CQRS records use `XxxCommand` / `XxxQuery` suffixes; handlers use `XxxHandler`
- `AppDbContext` is the EF context name in every service
- **Known typo**: Infrastructure project for ProductAPI V2 is `Apo.Services.ProdictAPI_V2.Infrastructure` ("Prodict" instead of "Product") — do not rename without updating the `.sln` and all `ProjectReference` entries

---

## Key Files by Service

| Service | Key Files |
|---|---|
| Gateway | `ocelot.json`, `Program.cs`, `Extensions/ServiceCollectionExtensions.cs` |
| AuthAPI | `Program.cs`, `Controllers/AuthAPIController.cs`, `Service/JwtTokenGeneratorUsingES256.cs`, `Extensions/ServiceCollectionExtensions.cs` |
| CouponAPI V1 | `Program.cs`, `Controllers/CouponAPIController.cs`, `Data/AppDbContext.cs`, `MappingConfig.cs` |
| CouponAPI V2 | Host `Program.cs`, `Middleware/ExceptionMiddleware.cs`; Application `Features/Coupons/**`, `ICouponRepository.cs`; Infrastructure `CouponRepository.cs` |
| ProductAPI V1 | `Program.cs`, `Controllers/ProductAPIController.cs`, `Data/AppDbContext.cs` |
| ProductAPI V2 | Same layout as CouponAPI V2 |
| ShoppingCartAPI | `Program.cs`, `Controllers/ShoppingCartAPIController.cs`, `Data/AppDbContext.cs` |
| Web | `Program.cs`, `Controllers/`, `Service/` (typed HttpClients), `Utility/SD.cs` (static config), `Extensions/MiddlewareExtenstions.cs` |
| Messaging | `Abstractions/IMessagePublisher.cs`, `RabbitMq/RabbitMqMessagePublisher.cs`, `RabbitMq/RabbitMqOptions.cs` |

---

## Development Notes for AI Assistants

1. **Adding a new V2 service**: Follow the Clean Architecture structure — Domain → Application (interface + CQRS handlers) → Infrastructure (EF repository) → Host (Program.cs + controllers + middleware).

2. **Adding a new CQRS operation**: Create a `Features/<Entity>/<Operation>/` folder with a `*Command.cs`/`*Query.cs` (record implementing `IRequest<T>`) and a `*Handler.cs` (implementing `IRequestHandler<TRequest, TResponse>`). MediatR auto-discovers handlers from the Assembly via `AddApplication()`.

3. **Adding a new Ocelot route**: Edit `Apo.Gateway/ocelot.json`, following the existing route structure. Add `"AuthenticationOptions"` if the route should be protected.

4. **Migrations**: Each service manages its own migrations. Run `dotnet ef` commands from the service project directory. The `ApplyMigrations()` function in `Program.cs` will apply pending migrations automatically at startup.

5. **ResponseDto is duplicated**: Each service has its own copy of `ResponseDto`. Do not create a shared library unless specifically requested — this is an intentional per-service isolation pattern.

6. **JWT keys**: The `./keys/private.pem` and `./keys/public.pem` files are not in source control. They must be generated and placed in the respective `Keys/` directories of `AuthAPI`, `Gateway`, and other services before running.

7. **The `SD` static class in `Apo.Web`** (`Utility/SD.cs`) holds service URL base addresses and cookie name constants. Update it when adding new service integrations.

8. **`appsettings.json` ServiceUrls in Apo.Web**: Note the key naming inconsistency — config keys use `CouponApi`/`AuthApi`/`ProductApi` while the `Program.cs` reads them as `CouponAPI`/`AuthApi`/`ProductApi`/`ShoppingCartApi`. Verify the exact key names when adding new service URLs.
