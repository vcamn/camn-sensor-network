---
appliesTo:
  - "services/shared/contracts/**"
---

# Shared Contracts Instructions

Purpose:

Canonical contract definitions shared across services.

Consumers:

- Fleet API
- Azure Functions
- Edge Node
- Fleet Control UI

---

## Rules

Contracts must be:

- backward compatible
- versioned
- technology agnostic

Avoid:

- business logic
- persistence concerns
- framework dependencies

---

## Versioning

Use explicit contract versions.

Example:

contracts/
  fleet/
    v1/

  telemetry/
    v1/

Do not make breaking changes to existing versions.