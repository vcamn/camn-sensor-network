<!-- appliesTo: "services/cloud/functions/**" -->

# Ingestion Function Instructions

Technology Stack:

- Azure Functions
- .NET Isolated Worker
- Azure IoT Hub
- PostgreSQL

Purpose:

Telemetry ingestion only.

Responsibilities:

- Receive telemetry
- Validate payloads
- Transform messages
- Persist telemetry

Do not implement fleet-management functionality.

---

## Architecture

Function Apps must remain stateless.

Prefer:

- Dependency injection
- Small focused functions
- Idempotent processing

Avoid:

- Long-running workflows
- Business orchestration
- Complex state management

---

## Telemetry Contracts

Use shared contracts from:

services/shared/contracts

Never duplicate telemetry models.

---

## Reliability

Always consider:

- retries
- duplicate messages
- poison message handling
- observability

Design for at-least-once delivery semantics.