# CAMN Sensor Network Documentation Strategy
This document describes the documentation strategy for adding and managing documentation throughout the monorepo.

## Primary documentation strategy:
Organize `docs/` primarily by system/module, and then by concern within each module:


`docs`/`<system-or-module>`/`<concern>/`


Only genuinely cross-cutting material—overall architecture, repository conventions, system-wide ADRs—should sit outside those subsystem directories.

A structure like this should scale well (general example):
```
camn-sensor-network/
├── README.md
├── docs/
│   ├── README.md
│   │
│   ├── architecture/
│   │   ├── system-overview.md
│   │   └── decisions/
│   │
│   ├── edge/
│   │   ├── README.md
│   │   ├── development/
│   │   │   ├── raspberry-pi-development-setup.md
│   │   │   ├── raspberry-pi-git-development-setup.md
│   │   │   └── raspberry-pi-vscode-remote-development.md
│   │   ├── deployment/
│   │   │   └── raspberry-pi-deployment.md
│   │   └── operations/
│   │       └── raspberry-pi-service-management.md
│   │
│   ├── cloud/
│   │   └── ...
│   ├── database/
│   │   └── ...
│   └── infrastructure/
│       └── ...
│
└── services/
    └── edge/
        └── sensor-node/
            ├── README.md
            ├── pyproject.toml
            ├── uv.lock
            ├── config/
            ├── scripts/
            ├── src/
            ├── systemd/
            └── tests/
```

## Placement Rule:
**root /docs is the authoritative documentation library**, organized by concern, while module READMEs remain local navigation/getting-started documents.

## Documentation Content Guidelines:
Distinguish **README**, **guide**, and **decision documentation** rather than putting everything into README files. `README.md` explains *what this module is and how to get started*. Files under `docs/` explain *how something works or how to perform a task*. Architecture Decision Records under `docs/architecture/decisions/` explain *why an important architectural choice was made*. That becomes particularly valuable for CAMN because decisions such as PostgreSQL as the system of record and keeping control-plane metadata separate from telemetry are architectural constraints future development should understand, not merely implementation details.