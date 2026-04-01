# Local Development Setup Runbook

### 1. Overview
This runbook provides step-by-step instructions for setting up a *local development environment* for the CAMN Sensor Network.

It is designed to:

- Onboard new developers quickly
- Provide a consistent, repeatable setup
- Reduce environment-related issues
- Standardize the local workflow using dev.sh

### 2. Prerequisites

Ensure the following are installed:

### Required Tools
- Git
- Docker Desktop (or Docker Engine + Docker Compose)
- .NET SDK (v8 or later)
- Node.js (only required for future web UI work)
- WSL (Windows users)

### Verify Installations
```
docker --version
dotnet --version
git --version
```
### 3. Repository Setup
#### 3.1 Clone the Repository
```
git clone <repo-url>
cd camn-sensor-network
```
#### 3.2 Configure Environment Variables

Navigate to:

```infra/docker/postgres/```

Copy the example file:

```cp .env.example .env```

Update values if needed:
```
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres

CAMN_DB_NAME=camn_sensor_network
CAMN_APP_USER=camn_app
CAMN_APP_PASSWORD=<local dev password>
```

### 4. One-Command Dev Workflow

All local setup and operations are managed through:

```infra/scripts/dev/dev.sh```

### 5. Initial Setup (First-Time Only)

Run:

```./infra/scripts/dev/dev.sh up```

This will:

1. Start PostgreSQL via Docker
2. Initialize database, roles, and schema
2. Apply EF Core migrations

### Database Requirements

The following extensions must be installed before applying EF migrations:

- uuid-ossp
- citext

And should be located in the init SQL script(s)

### 6. Daily Development Commands

#### Start Environment
```./infra/scripts/dev/dev.sh up```

#### Stop Environment
```./infra/scripts/dev/dev.sh down```

#### Reset Database (Clean Rebuild)
```./infra/scripts/dev/dev.sh reset```

This will:

- Drop database and roles
- Recreate schema
- Re-run migrations

#### Apply Migrations Only
```./infra/scripts/dev/dev.sh migrate```

### 7. Running the Fleet API

Navigate to the API:

```cd services/cloud/api/fleet-api/Fleet.Api```

Run:

```dotnet run```

API will start (default):

```https://localhost:5001```

### 8. Database Access

#### Connect via CLI

```docker exec -it camn-postgres psql -U postgres```

Then:

```\c camn_sensor_network```

#### Verify Schema

```
\dn
\dt metadata.*
```
#### Note:
You can also use a PostgreSQL IDE extention or admin too (e.g. pgAdmin)

### 9. Development Workflow

Typical workflow:
```
# Start environment
dev.sh up

# Make code changes
# Update EF models

# Create migration
dotnet ef migrations add <MigrationName> \
  --project Fleet.Infrastructure \
  --startup-project Fleet.Api

# Apply migration
dev.sh migrate

# Run API
dotnet run
```
---
## 10. Troubleshooting

### ❌ Docker container not starting
#### Check:
```
docker container ls
docker logs camn-postgres (or container name you specified in .env)
```

#### Fix:
```
dev.sh down
dev.sh up
```
---
### ❌ Database connection fails

#### Check:
- .env values
- Port 5432 availability
- Container is running

```docker container ls```

---
### ❌ EF Core migration issues

#### Common fix:
```
dotnet clean
dotnet build
```
Ensure:
- Fleet.Api has ```Microsoft.EntityFrameworkCore.Design```
- Migrations exist in ```Fleet.Infrastructure```

---
### ❌ First migration shows “relation does not exist”

This is expected behavior on initial run.

EF Core attempts to read:
```
__EFMigrationsHistory
```
before it exists.

✔ Safe to ignore if migration succeeds afterward.

---
### ❌ Port conflict on 5432

Find process:
```
lsof -i :5432
```
Stop conflicting service or update Docker port mapping.

---
### 1. Common Pitfalls
- ❌ Committing .env files
- ❌ Running API before database is initialized
- ❌ Forgetting to apply migrations
- ❌ Using incorrect connection string
- ❌ Not running through dev.sh (bypassing workflow)

---
### 12. Architecture Notes (Context)

Local development mirrors the production architecture:

- PostgreSQL database (```camn_sensor_network```)
- metadata schema (```Phase 0.5 fleet registry```)
- EF Core migrations as source of truth
- Clean Architecture (```Api / Domain / Infrastructure```)

This ensures:

- Consistent schema evolution
- Alignment with CI/CD pipelines
- Smooth transition to Azure environments

---
### 13. Quick Start (TL;DR)

```
git clone <repo>
cd camn-sensor-network

cp infra/docker/postgres/.env.example infra/docker/postgres/.env

./infra/scripts/dev/dev.sh up

cd services/cloud/api/fleet-api/Fleet.Api
dotnet run
```

---
### 14. Support

If issues persist:

- Check this runbook
- Review logs (docker logs, API output)
- Ask team or open a GitHub issue