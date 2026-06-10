---
appliesTo:
  - "services/web/fleet-control/**"
---

# Fleet Control Instructions

Technology Stack:

- Vue 3
- TypeScript
- REST API integration

Purpose:

Fleet management and administrative UI.

Current Phase:

CRUD management of:

- Sites
- Stations
- Devices
- Sensors

---

## UI Architecture

Use:

- Composition API
- Strong TypeScript typing
- Reusable components

Avoid:

- Business logic in components
- Direct API calls from deeply nested components

---

## API Integration

Consume Fleet API DTOs.

Do not duplicate domain rules.

Validation rules belong to:

- API
- Shared validation utilities

---

## Future Expansion

Design for:

- Fleet dashboards
- OTA management
- Device health monitoring

Avoid assumptions that the UI will remain CRUD-only.