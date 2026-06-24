# CAMN Sensor Network - Global Copilot Instructions

## Repository Overview

This repository contains the CAMN Sensor Network platform.

Purpose:

- Environmental sensor network management
- Fleet metadata management
- IoT telemetry ingestion
- Device lifecycle management
- OTA update support
- Environmental data collection

Repository layout:

```text
camn-sensor-network/

docs/
infra/

services/
├── cloud/
│   ├── api/
│   ├── functions/
│   └── shared/
│
├── edge/
│
└── web/
```

---

## Architecture Principles

Always follow these principles:

- Prefer simple solutions over premature abstraction.
- Favor maintainability over cleverness.
- Keep infrastructure concerns separate from business logic.
- Keep telemetry ingestion separate from fleet management.
- Keep control plane and data plane responsibilities separate.
- Design for future expansion to 34+ deployment sites.

---

## Current Project Phase

Current focus:

Phase 0.5 – Fleet Registry

The primary objective is implementing:

- Sites
- Stations
- Devices
- Sensors
- Sensor Integrations
- Sensor Types
- Lookup Tables
- Status Management

Do not introduce telemetry processing functionality unless explicitly requested.

---

## Database Standards

Database:

- PostgreSQL
- Schema-first organization
- EF Core Code-First migrations

Primary keys:

- Use Guid/UUID identifiers.
- Never use integer identity keys for domain entities.

Lookup tables:

- Prefer database lookup tables over enums.
- Use migration-based seeding.
- Use stable GUID constants for system-controlled reference data.

Naming conventions:

- C#:
  - PascalCase

- PostgreSQL:
  - snake_case

Use EFCore.NamingConventions.

---

## Environment Strategy

Supported environments:

- Development
- Staging
- Production

Never assume a single environment deployment.

All configuration must come from:

- appsettings
- environment variables
- Azure configuration sources

Never hardcode secrets.

---

## Azure Strategy

Preferred Azure services:

- Azure IoT Hub
- Azure Functions
- Azure Database for PostgreSQL Flexible Server
- Azure Blob Storage
- Azure App Service

Favor managed Azure services over self-hosted alternatives.

---

## Documentation Standards

When generating documentation:

- Use Markdown.
- Include architecture rationale.
- Include deployment considerations.
- Include operational considerations.

Prefer ADR-style explanations when discussing architectural decisions.

---

## Commit Message Guidelines

Generate commit messages using the Conventional Commits format.

### Format

```text
<type>(<scope>): <short summary>

- <key change>
- <key change>
- <key change>
```

### Requirements

* Use a concise summary line (preferably under 50 characters).
* Choose the scope based on the primary feature, service, domain, or API area affected.
* Focus the summary on the most important user-facing or architectural change.
* Use bullet points to describe significant supporting changes.
* Do not list every modified file.
* Group related changes into logical themes.
* Mention validation, error handling, DTO, entity, or documentation updates only when they are meaningful to understanding the change.
* Prefer feature-oriented descriptions over implementation details.

### Common Types

* `feat` - New functionality
* `fix` - Bug fixes
* `refactor` - Code restructuring without behavior changes
* `perf` - Performance improvements
* `test` - Test additions or updates
* `docs` - Documentation changes
* `build` - Build or dependency changes
* `chore` - Maintenance tasks

### Example

```text
feat(site): add site station lookup endpoint

- Add endpoint to retrieve all stations associated with a site
- Implement site station queries in the service layer
- Reuse station DTO mapping across services
- Strengthen site DTO and entity validation with required properties
- Improve API error responses for missing sites
- Update API documentation examples
```
