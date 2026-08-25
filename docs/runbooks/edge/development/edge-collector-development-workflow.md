# Edge Collector Development Workflow

This guide documents the local development workflow for the CAMN Sensor Network Raspberry Pi edge collector.

The workflow separates **software development on the developer workstation** from **hardware integration and runtime testing on the Raspberry Pi**.

Visual Studio Code Remote - SSH is not required. Source code is developed locally, synchronized between systems through Git, and tested on the Raspberry Pi using ordinary SSH when access to the actual device environment or attached sensors is required.

> **Scope:** This workflow supports development of the Raspberry Pi edge collector. Production deployment and fleet-wide device management may use different deployment mechanisms in the future.

## Development Model

The primary development environment consists of:

* Windows 11 development workstation
* Visual Studio Code
* WSL2 Linux development environment
* Git and GitHub
* Python managed with `uv`
* Raspberry Pi Zero 2 W running Debian
* Ordinary SSH access to the Raspberry Pi
* Git sparse checkout on the Raspberry Pi
* `systemd` for Raspberry Pi runtime process management

The development flow is:

```text
Windows 11 Development Workstation
│
├── Visual Studio Code
│
└── WSL2
    └── CAMN repository
        ├── source development
        ├── uv / Python environment
        ├── unit tests
        └── non-hardware testing
                 │
                 │ git commit + push
                 ▼
              GitHub
                 │
                 │ git pull
                 ▼
Raspberry Pi Zero 2 W
    └── CAMN sparse checkout
        ├── uv sync --locked
        ├── real sensor hardware
        ├── serial/device integration testing
        ├── systemd
        └── journalctl
```

## Development Responsibilities

### Development Workstation

The development workstation is the primary environment for writing and maintaining edge collector source code.

Development activities performed locally include:

* Editing Python source code
* Managing dependencies
* Running unit tests
* Testing configuration loading and validation
* Testing sensor data parsing
* Testing telemetry construction and processing
* Testing application logic that does not require Raspberry Pi hardware
* Git commits and pushes
* Code review and general repository maintenance

Because the workstation primarily runs Windows 11 while the Raspberry Pi runs Linux, edge collector development should use **WSL2** where Linux compatibility is important.

This provides a Linux environment for Python, `uv`, shell commands, filesystem paths, and other Linux-oriented development behavior without requiring the Raspberry Pi to host the development editor.

## Raspberry Pi Responsibilities

The Raspberry Pi is the authoritative environment for testing behavior that depends on the actual edge runtime or hardware.

Pi-specific testing includes:

* Serial-port communication
* Physical sensor communication
* USB device behavior
* Linux device permissions
* Raspberry Pi-specific dependencies
* Runtime configuration
* `systemd` service execution
* Process supervision and restart behavior
* `journalctl` logging
* Final integration testing against the edge device environment

The Raspberry Pi should not be required for every unit-test or source-code change.

## Hardware Boundary

The collector should maintain a clear boundary between hardware communication and application logic.

Importing or testing application modules should not inherently require a physical serial device such as:

```text
/dev/ttyUSB0
```

Where practical, serial communication should be isolated from logic such as:

* Sensor message parsing
* Measurement validation
* Configuration processing
* Telemetry construction
* Retry decision logic
* Data transformation

These components can then be tested locally using controlled or simulated input.

Actual serial-port communication remains a Raspberry Pi integration test.

This boundary improves testability while accurately representing the operational distinction between application logic and physical sensor I/O.

## Local Development Environment

The CAMN repository should be available within the WSL Linux filesystem.

The edge collector project uses:

```text
services/edge/sensor-node/
├── .python-version
├── pyproject.toml
├── uv.lock
├── config/
├── scripts/
├── src/
├── systemd/
└── tests/
```

The generated project virtual environment:

```text
.venv/
```

is local to each environment and is not committed to Git.

Synchronize the local Python environment using:

```bash
uv sync
```

Run tests through the project environment:

```bash
uv run pytest
```

Other Python development commands should likewise use the project environment rather than installing collector dependencies into the operating system's Python installation.

## Source Synchronization

Git is the primary synchronization mechanism between the development workstation and Raspberry Pi.

The normal source workflow is:

```text
Developer Workstation
    │
    ├── edit
    ├── test
    ├── git commit
    └── git push
           │
           ▼
        GitHub
           │
           ▼
Raspberry Pi
    │
    └── git pull
```

This intentionally keeps Git as the authoritative source of application changes rather than introducing a separate filesystem synchronization mechanism.

## Deploy a Development Change to the Raspberry Pi

After a change is ready for Raspberry Pi testing, commit and push it from the development environment.

On the Raspberry Pi, connect using ordinary SSH:

```bash
ssh <username>@<raspberry-pi-ip-address>
```

Navigate to the sparse checkout:

```bash
cd ~/camn-sensor-network
```

Update the working tree:

```bash
git pull
```

Navigate to the sensor node:

```bash
cd services/edge/sensor-node
```

Synchronize the Python environment against the committed lockfile:

```bash
uv sync --locked
```

Using `--locked` ensures deployment does not silently resolve a dependency environment different from the committed `uv.lock`.

## Interactive Raspberry Pi Testing

During development, collector code may be executed interactively from the SSH session when testing hardware behavior.

This is useful for:

* Serial communication development
* Sensor protocol debugging
* Configuration troubleshooting
* Inspecting application output directly
* Testing a new application entry point before installing or restarting a service

The exact execution command depends on the collector entry-point design.

Interactive execution should generally be used before involving `systemd` when debugging application behavior.

## systemd Runtime Testing

Once collector behavior works interactively, verify it through the actual Raspberry Pi runtime model.

If a source-code change does not modify the unit definition, restart the applicable service:

```bash
sudo systemctl restart camn-<service>.service
```

Check its status:

```bash
systemctl status camn-<service>.service
```

Inspect recent logs:

```bash
journalctl -u camn-<service>.service -n 100
```

Follow runtime logs:

```bash
journalctl -fu camn-<service>.service
```

If a source-controlled `systemd` unit itself changed, install the updated unit before restarting the service:

```bash
sudo cp systemd/<service>.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl restart camn-<service>.service
```

The repository copy of the unit remains authoritative.

## Normal Development Cycle

The expected development cycle is:

1. Open the CAMN repository using VS Code and WSL.
2. Modify the edge collector source locally.
3. Run applicable local unit tests.
4. Commit the completed change to Git.
5. Push the commit to GitHub.
6. Connect to the Raspberry Pi using ordinary SSH.
7. Pull the commit into the Raspberry Pi sparse checkout.
8. Run `uv sync --locked` when dependencies may have changed.
9. Test hardware-dependent behavior on the Raspberry Pi.
10. Verify runtime behavior through `systemd` when applicable.
11. Inspect application logs using `journalctl`.

In abbreviated form:

```text
edit
  ↓
local tests
  ↓
commit + push
  ↓
SSH to Raspberry Pi
  ↓
git pull
  ↓
uv sync --locked
  ↓
hardware/integration test
  ↓
systemd runtime test
  ↓
journalctl verification
```

Not every change requires every step. For example, a documentation-only change does not require Raspberry Pi deployment, and a source change with no dependency changes does not necessarily require `uv sync --locked`.

## Development vs. Deployment Responsibilities

The tools in this workflow have intentionally separate responsibilities.

| Tool         | Responsibility                               |
| ------------ | -------------------------------------------- |
| VS Code      | Source editing and developer tooling         |
| WSL2         | Local Linux development environment          |
| Git          | Source control and synchronization           |
| GitHub       | Shared source repository                     |
| uv           | Python environment and dependency management |
| SSH          | Interactive Raspberry Pi access              |
| Raspberry Pi | Hardware and runtime integration environment |
| systemd      | Runtime process supervision                  |
| journalctl   | Runtime log inspection                       |

Keeping these responsibilities separate prevents development tooling from becoming part of the edge collector runtime.

The resulting runtime relationship is:

```text
Git
 ↓
uv
 ↓
Edge Collector
 ↓
systemd
```

VS Code, WSL, and SSH support development and administration but are not runtime dependencies of the collector.

## Why Remote - SSH Is Not the Primary Workflow

VS Code Remote - SSH was previously used to edit and develop source directly on the Raspberry Pi.

The current workflow instead keeps the primary development environment on the workstation.

This has several advantages for the Raspberry Pi Zero 2 W:

* VS Code Server does not need to run on the Raspberry Pi.
* VS Code remote extensions do not consume Raspberry Pi resources.
* Development tooling remains on the more capable workstation.
* The Raspberry Pi remains focused on hardware integration and collector execution.
* Development more closely resembles the independently deployable runtime model.
* Git provides an explicit and reproducible boundary between development and device testing.

Ordinary SSH remains part of the workflow for Raspberry Pi administration, interactive testing, service management, and troubleshooting.

## Docker

Docker is not currently required for local edge collector development.

WSL2 provides the Linux compatibility needed for the initial development workflow with considerably less additional infrastructure.

Docker may be reconsidered if future requirements justify it, such as:

* Native dependencies that are difficult to reproduce between development environments
* A need for stronger environment isolation
* Container-based automated testing
* Containerization becoming part of the edge deployment model

Docker should not be introduced solely to reproduce Linux when WSL2 and `uv` already satisfy the development requirement.

## Faster Synchronization Options

Git remains the initial and authoritative synchronization mechanism.

Additional mechanisms such as `rsync` may be considered later if the Git commit/push/pull cycle creates significant friction during rapid hardware development.

Such mechanisms should be treated as **development conveniences**, not replacements for the Git-based deployment workflow.

The simpler Git workflow should remain in place until actual development experience demonstrates a need for a faster synchronization loop.

## Related Documentation

See the other CAMN edge documentation for detailed procedures covering:

* Raspberry Pi development networking and SSH setup
* Raspberry Pi Git and sparse-checkout configuration
* Python and `uv` environment management
* Raspberry Pi deployment
* `systemd` service management and logging

These documents are maintained under the CAMN edge documentation hierarchy:

```text
docs/edge/
├── development/
├── deployment/
└── operations/
```
