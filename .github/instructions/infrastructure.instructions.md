---
appliesTo:
  - "infra/**"
---

# Infrastructure Instructions

Purpose:

Infrastructure as Code, automation, deployment, and operational tooling.

---

## Organization

Executable assets belong in:

```infra/```

Documentation belongs in:

```docs/runbooks/```

Never place deployment scripts inside application projects.

---

## Local Development

Support:

- Docker
- PostgreSQL
- dev.sh workflow

Assume WSL/Linux-compatible execution.

---

## Database Scripts

Use:

```infra/database/postgres/```

Database initialization scripts must:

- be idempotent
- support repeatable execution
- avoid hardcoded secrets

Secrets must come from:

- environment variables
- CI/CD secrets
- Azure Key Vault

---

## CI/CD

Database schema changes are delivered through:

- EF Core migrations

Do not manually modify deployed schemas.

Migration execution should be automated through CI/CD pipelines.

---

## Azure

Prefer Infrastructure as Code.

Target environments:

- Dev
- Staging
- Production

Assume resource-group isolation between environments.