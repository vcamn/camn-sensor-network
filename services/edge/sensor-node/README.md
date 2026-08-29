# CAMN Sensor Node Edge Collector

The `sensor-node` module contains the Python edge collector software
that runs on CAMN Raspberry Pi sensor nodes.

This README documents the initial Raspberry Pi development/runtime
setup, Python dependency management with `uv`, and the
hardware-integration testing boundary. It is a living document and
should evolve with the collector implementation.

## Current Runtime Model

The current edge workflow is intentionally simple:

``` text
Developer workstation (Windows 11 + WSL 2)
        |
        | Git push
        v
      GitHub
        |
        | git pull
        v
Raspberry Pi Zero 2 W (Debian)
        |
        +-- uv-managed Python environment
        +-- sensor-node/.venv
        +-- physical sensor/serial integration
        +-- systemd service runtime
```

The Raspberry Pi maintains its own checkout and Python virtual
environment. The `.venv` directory must not be copied between WSL and
the Raspberry Pi because the development workstation and Pi use
different CPU architectures.

## Project Structure

``` text
sensor-node/
├── .python-version       # Python version used by uv
├── pyproject.toml        # Project metadata and dependency declarations
├── uv.lock               # Locked dependency graph
├── config/               # Device/deployment configuration
├── scripts/              # Deployment and runtime-management scripts
├── src/
│   ├── main.py           # Expected collector entry point
│   ├── config/           # Configuration loading/validation
│   ├── sensors/          # Sensor acquisition/integration
│   └── telemetry/        # Readings and telemetry transmission
├── systemd/              # Source-controlled systemd unit files
└── tests/
```

The project was initialized with:

``` bash
uv init --name edge-collector --no-package
```

`--no-package` allows `uv` to manage the project environment and
dependencies without requiring the collector itself to be installed as a
Python package.

## Raspberry Pi Prerequisites

The current development target is a Raspberry Pi Zero 2 W running 64-bit
Debian.

Before configuring the Python project, the Pi should have:

-   Network connectivity
-   SSH access
-   Git installed and configured
-   The CAMN monorepo checked out with `services/edge/sensor-node`
    available
-   Access to the attached sensor hardware required for integration
    testing

The repository working tree remains the runtime source during the
current development phase. A separate application installation under
`/opt` is not required at this stage.

## Install uv on the Raspberry Pi

Install `uv` as the Raspberry Pi user that will manage and run the
collector environment.

Use the official standalone installer:

``` bash
curl -LsSf https://astral.sh/uv/install.sh | sh
```

Restart the shell or load the updated shell environment if required,
then verify:

``` bash
uv --version
```

Do not install collector dependencies into Debian's system Python. `uv`
should create and manage the project-specific virtual environment.

## Python Version

The project should declare its intended Python version in:

``` text
.python-version
```

Verify the Python version selected by `uv`:

``` bash
uv python find
```

If the required Python version is not available, `uv` can install a
managed Python interpreter:

``` bash
uv python install
```

Hardware-facing dependencies must be validated on the Raspberry Pi
before changing the project's supported Python version.

## Create the Raspberry Pi Project Environment

From the Raspberry Pi repository checkout:

``` bash
cd ~/camn-sensor-network/services/edge/sensor-node
```

Synchronize the environment from the committed project metadata and
lockfile:

``` bash
uv sync --locked
```

This creates the local project virtual environment:

``` text
sensor-node/.venv/
```

The `.venv/` directory is generated per machine and must remain
Git-ignored.

The source-controlled dependency authority is:

``` text
pyproject.toml
        |
        v
     uv.lock
        |
        v
uv sync --locked
        |
        v
     .venv/
```

`pyproject.toml`, `uv.lock`, and `.python-version` should be committed
to Git.

## Dependency Management

Dependency changes should normally be made during development and
committed with the resulting lockfile.

Add a runtime dependency:

``` bash
uv add <package>
```

Remove a dependency:

``` bash
uv remove <package>
```

Add a development dependency:

``` bash
uv add --dev <package>
```

Synchronize the local environment:

``` bash
uv sync
```

On the Raspberry Pi deployment/testing environment, prefer:

``` bash
uv sync --locked
```

This prevents the Pi from silently resolving a dependency graph that
differs from the committed lockfile.

Avoid manually maintaining a parallel `requirements.txt` unless a future
external integration specifically requires one.

## Running the Collector During Development

The expected application entry point is currently:

``` text
src/main.py
```

Once implemented, run it through the project environment with:

``` bash
uv run python src/main.py
```

For interactive development and testing, `uv run` is appropriate because
it executes within the project environment.

The production-style `systemd` runtime should eventually execute the
prepared `.venv` interpreter directly rather than using `uv run` during
service startup.

For example:

``` ini
WorkingDirectory=/home/<user>/camn-sensor-network/services/edge/sensor-node
ExecStart=/home/<user>/camn-sensor-network/services/edge/sensor-node/.venv/bin/python /home/<user>/camn-sensor-network/services/edge/sensor-node/src/main.py
```

Exact paths and service definitions will be finalized as the collector
entry-point/runtime design is completed.

### Hardware-Free Replay

The sensor scripts can replay raw line data from a local file. Replay
does not perform USB discovery, open `/dev` devices, or require sensor
hardware. It stops automatically at end-of-file.

``` bash
uv run python src/sensors/aggie_air.py --input-file tests/fixtures/aggie_air_sample.log --path ./logs
uv run python src/sensors/wind_sensor.py --input-file tests/fixtures/wind_sample.log --path ./logs
uv run python src/sensors/purple_air.py --input-file tests/fixtures/purple_air_sample.log --path ./logs
```

Replay files contain raw sensor bytes, so preserve the line endings
used by the sensor. Use sanitized or synthetic data for committed
fixtures; do not commit credentials or identifying device captures.

The replay transport and reader tests can be run with:

``` bash
uv run pytest tests/test_transports.py tests/test_sensor_replay.py -q
```

## Raspberry Pi Hardware Integration Testing

WSL 2 is the primary local development environment, but the Raspberry Pi
remains the authoritative environment for physical sensor integration.

The following areas require Raspberry Pi testing.

### Serial Device Discovery

Identify attached serial devices before testing collector communication:

``` bash
ls -l /dev/ttyUSB*
ls -l /dev/ttyACM*
```

Not every sensor will necessarily use these device names. Confirm the
actual device path used by each sensor integration.

Useful device information can also be inspected with:

``` bash
dmesg | tail
```

or, when appropriate:

``` bash
udevadm info <device-path>
```

### Device Permissions

The Linux user running the collector must have permission to access the
required serial device.

Inspect the device ownership:

``` bash
ls -l <device-path>
```

On Debian systems, serial devices are commonly associated with the
`dialout` group. Check the current user's groups:

``` bash
groups
```

If the device is owned by `dialout` and the collector user is not a
member, add the user:

``` bash
sudo usermod -aG dialout <username>
```

Log out and back in before retesting so the new group membership takes
effect.

Do not solve persistent device-access problems by broadly changing
device permissions with commands such as `chmod 777`.

### Sensor Communication

Hardware integration testing must verify, as applicable:

-   The expected sensor is discovered by Linux.
-   The configured device path is correct.
-   The collector user can open the device.
-   Serial parameters match the physical sensor protocol.
-   The collector can receive complete sensor messages.
-   Malformed or incomplete input is handled safely.
-   Disconnect/reconnect behavior is understood.
-   Sensor failures do not cause uncontrolled collector failure.
-   Measurements are parsed into the expected internal representation.
-   Resulting telemetry contains the expected sensor identity and
    measurement data.

### Stable Device Identification

Device paths such as `/dev/ttyUSB0` can change when USB devices are
disconnected, reconnected, or enumerated in a different order.

As the collector integration matures, evaluate stable Linux device
identification using `/dev/serial/by-id/` and/or project-owned `udev`
rules where necessary.

Do not assume that a particular sensor will permanently remain
`/dev/ttyUSB0`.

## WSL vs. Raspberry Pi Testing Boundary

Most collector behavior should be testable in WSL without physical
sensor hardware.

### WSL 2

Use WSL for:

-   Python development
-   `uv` dependency management
-   Unit tests
-   Configuration parsing/validation
-   Sensor message parsing
-   Telemetry construction
-   Error-handling logic
-   Fake/mocked serial input

### Raspberry Pi

Use the Raspberry Pi for:

-   Physical serial-device discovery
-   Linux device permissions
-   Actual sensor communication
-   USB disconnect/reconnect behavior
-   Device-path stability
-   ARM64 dependency compatibility
-   End-to-end hardware integration
-   `systemd` runtime verification
-   Runtime logs through `journalctl`

Application imports should not require a physical serial device to
exist. Opening hardware devices should occur at the sensor-integration
boundary so that the rest of the collector remains testable without
hardware.

## Updating the Raspberry Pi

The current single-device update workflow is:

``` bash
cd ~/camn-sensor-network/services/edge/sensor-node

git pull
uv sync --locked
```

After application changes are synchronized, perform the required
Raspberry Pi hardware/integration tests.

When the collector is running through `systemd`, restart the affected
CAMN service and inspect its status and logs:

``` bash
sudo systemctl restart camn-<service>.service
systemctl status camn-<service>.service
journalctl -u camn-<service>.service -n 100
```

If a source-controlled unit file changes, copy the updated unit into
`/etc/systemd/system/` and reload `systemd` before restarting the
service.

## Source Control

Commit:

-   `pyproject.toml`
-   `uv.lock`
-   `.python-version`
-   Collector source code
-   Tests
-   Configuration templates
-   Deployment/runtime scripts
-   `systemd` unit definitions

Do not commit:

-   `.venv/`
-   Python cache files
-   Local secrets or credentials
-   Device-specific temporary files
-   Runtime logs

## Current Design Principles

-   `uv` owns Python environment and dependency management.
-   Git owns application and deployment source.
-   `systemd` owns process supervision on the Raspberry Pi.
-   WSL provides the primary Linux-compatible development environment.
-   The Raspberry Pi provides authoritative hardware integration
    testing.
-   Physical sensor access remains behind a small application boundary.
-   Avoid adding deployment or packaging complexity until the
    operational need exists.

## Related Documentation

Broader edge development, deployment, SSH, Git, and Raspberry Pi
service-management documentation is maintained under the repository's
`docs/edge/` documentation tree.

This README should remain focused on the `sensor-node` software module
and the minimum setup required to develop, synchronize, and
hardware-test it on a Raspberry Pi.
