<!-- appliesTo: "services/edge/**" -->

# Edge Node Instructions

Technology Stack:

- Python 3.11+
- Linux
- Raspberry Pi
- Azure IoT Hub SDK

Purpose:

Field-deployed sensor node software.

---

## Design Principles

Edge software must:

- Survive network outages
- Survive power interruptions
- Support remote updates
- Operate autonomously

Assume intermittent connectivity.

---

## Code Organization

Prefer:

- Typed Python
- Dataclasses
- Structured logging
- Dependency injection where practical

Avoid:

- Global state
- Hidden side effects
- Hardcoded configuration

---

## Configuration

All configuration should come from:

- configuration files
- environment variables
- device provisioning

Never hardcode:

- connection strings
- secrets
- IoT Hub credentials

---

## Telemetry

Batch telemetry when appropriate.

Prioritize:

- reliability
- observability
- recoverability

over maximum throughput.