---
appliesTo:
  - "services/cloud/api/fleet-api/**"
---

# Fleet API Instructions

Technology Stack:

- ASP.NET Core Web API
- .NET 9+
- Entity Framework Core
- PostgreSQL
- Clean Architecture

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