<!-- appliesTo: "services/cloud/api/fleet-api/**" -->

# Fleet API Instructions

Technology Stack:

- ASP.NET Core Web API
- .NET 9+
- Entity Framework Core
- Dependency Injection
- PostgreSQL
- Clean Architecture
- Service Layer pattern
- DTO-based API contracts
- API versioning

Project Structure:
- Fleet.Api
- Fleet.Domain
- Fleet.Infrastructure


Dependency direction:

Fleet.Api
  -> Fleet.Infrastructure
  -> Fleet.Domain

Fleet.Infrastructure
  -> Fleet.Domain

Never reference Infrastructure from Domain.

---
## Fleet API Architecture

Controllers -> Services -> FleetDbContext

Controllers never access EF Core.

Services own all persistence logic.

Services return DTOs.

Controllers contain no business logic.

Register every service with DI.

Use constructor injection.

Never expose EF entities through the API.

Use async throughout.

Use ApiVersioning.

Create matching .http files.

Follow the existing project folder structure.

---

## Entity Design

Entities belong in Fleet.Domain.

Use:

- Guid identifiers
- Navigation properties
- Explicit relationships

Avoid:

- Business logic inside controllers
- Anemic service layers
- Repository patterns that simply wrap DbContext

---

## EF Core

Migrations live in:

Fleet.Infrastructure

Use:

- IEntityTypeConfiguration<T>
- Separate configuration classes
- Schema-based organization

Example:

```text
Persistence/
  Configurations/
    Metadata/
    Sites/
    Stations/
    Devices/
    Sensors/
```


Do not place EF configuration inside DbContext.

---

## API Standards

Use:

- RESTful routes
- DTOs
- Validation
- ProblemDetails responses

Avoid exposing EF entities directly.

Always use DTO contracts.

---
## Controllers

- Thin controllers
- No business logic
- No DbContext usage

---
## Services

- Own all persistence logic
- Return DTOs
- Perform validation 

---
## General

- Async everywhere
- Constructor injection
- Existing naming conventions
- Nullable reference types

---

## PostgreSQL Standards

Default schema:

metadata

Migration history schema:

admin

Use UUID columns.

Use snake_case naming.

Prefer explicit indexes for:

- foreign keys
- status lookups
- search fields

---

## Authentication

Current phase:

Single administrative user authentication.

Future direction:

Microsoft Entra ID integration.

Do not tightly couple authentication logic to business logic.