# CAMN Sensor Network Documentation Strategy
This document describes the documentation strategy for adding and managing documentation throughout the monorepo.

## Hybrid documentation strategy:
Given the CAMN monorepo architecture, documentation lives as close as possible to what it describes, while a small root `docs/` directory handles cross-cutting architecture and project-wide concerns. That fits the existing module-oriented repository design and avoids turning `docs/` into an unstructured dumping ground.

A structure like this should scale well (general example):
```
camn-sensor-network/
├── README.md
├── docs/
│   ├── README.md
│   ├── architecture/
│   │   ├── system-overview.md
│   │   ├── control-plane.md
│   │   ├── data-plane.md
│   │   └── decisions/
│   │       ├── 0001-postgresql.md
│   │       └── 0002-control-data-plane-separation.md
│   ├── development/
│   │   ├── local-development.md
│   │   └── repository-structure.md
│   └── operations/
│       └── ...
│
├── services/
│   ├── edge/
│   │   ├── README.md
│   │   ├── docs/
│   │   │   ├── raspberry-pi-development-setup.md
│   │   │   ├── networking.md
│   │   │   └── troubleshooting.md
│   │   └── collector/
│   │
│   └── cloud/
│       ├── api/
│       │   └── fleet-api/
│       │       ├── README.md
│       │       └── docs/
│       └── ingestion/
│           ├── README.md
│           └── docs/
│
├── apps/
│   └── fleet-control-center/
│       ├── README.md
│       └── docs/
│
├── database/
│   ├── README.md
│   └── docs/
│
└── infrastructure/
    ├── README.md
    └── docs/
```

## Placement Rule:
**Put documentation at the lowest repository level where its scope is still fully correct**. System architecture spanning edge, cloud, database, and applications goes in `/docs/architecture`. Fleet API design belongs near `services/cloud/api/fleet-api`. Pi setup belongs near `services/edge`. PostgreSQL-specific development procedures belong near `/database`. Cross-repo developer onboarding belongs in `/docs/development`. The root `README.md` should remain the front door—project purpose, high-level architecture, repository map, basic getting-started information, and links into deeper documentation.

## Documentation Content Guidelines:
Distinguish **README**, **guide**, and **decision documentation** rather than putting everything into README files. `README.md` explains *what this module is and how to get started*. Files under `docs/` explain *how something works or how to perform a task*. Architecture Decision Records under `docs/architecture/decisions/` explain *why an important architectural choice was made*. That becomes particularly valuable for CAMN because decisions such as PostgreSQL as the system of record and keeping control-plane metadata separate from telemetry are architectural constraints future development should understand, not merely implementation details.