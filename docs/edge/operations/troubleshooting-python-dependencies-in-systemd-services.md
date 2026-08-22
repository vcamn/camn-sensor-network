# Troubleshooting Python Dependencies in systemd Services

This runbook covers Python import failures in CAMN edge services that run from a `uv`-managed virtual environment. It was created after troubleshooting `camn-aggie-air.service` on a Raspberry Pi Zero.

## Symptoms

The service fails immediately and enters a restart loop:

```text
Failed to load module pyserial.
Please install it using apt-get install python3-serial
Main process exited, code=exited, status=255/EXCEPTION
Failed with result 'exit-code'.
```

## Likely Cause

The systemd unit uses the project's virtual-environment interpreter, but the required package is not installed in that environment:

```text
/home/zero/camn-sensor-network/services/edge/sensor-node/.venv/bin/python
```

A package installed for the operating system's Python interpreter is not automatically available to `.venv`. For a `uv`-managed application, dependencies should be declared in `pyproject.toml`, recorded in `uv.lock`, and installed with `uv sync`.

## Troubleshooting Procedure

### 1. Stop the restart loop

```bash
sudo systemctl stop camn-aggie-air.service
sudo systemctl reset-failed camn-aggie-air.service
```

### 2. Inspect the effective service configuration

```bash
sudo systemctl cat camn-aggie-air.service

sudo systemctl show camn-aggie-air.service \
  -p User -p Group -p WorkingDirectory -p ExecStart -p Environment
```

Confirm the following:

- `ExecStart` uses the intended `.venv/bin/python` interpreter.
- The script path is correct after any rename or move.
- `WorkingDirectory` is correct.
- `User` and `Group` match the intended runtime account.
- Any required `Environment` or `EnvironmentFile` settings are present.

An empty `User=` means the system service runs as `root`. CAMN services should normally use the dedicated device account unless root access is explicitly required.

### 3. Test the import with the service's interpreter

```bash
cd /home/zero/camn-sensor-network/services/edge/sensor-node

.venv/bin/python -c \
  "import sys, serial; print(sys.executable); print(serial.__file__)"
```

This test is more reliable than running `python3` because it uses the exact environment configured in `ExecStart`.

### 4. Declare the missing dependency

On the development machine:

```bash
cd services/edge/sensor-node
uv add pyserial
git add pyproject.toml uv.lock
git commit -m "Add pyserial dependency"
git push
```

Do not treat the application's suggestion to install `python3-serial` with `apt` as authoritative. That installs a system Python package and does not correct a missing dependency in the project's virtual environment.

### 5. Synchronize the Pi environment

On the Raspberry Pi:

```bash
cd /home/zero/camn-sensor-network/services/edge/sensor-node
git pull
uv sync --frozen
```

Confirm installation:

```bash
uv pip show pyserial

.venv/bin/python -c \
  "import serial; print(f'pyserial loaded from: {serial.__file__}')"
```

### 6. Restart and verify the service

```bash
sudo systemctl restart camn-aggie-air.service
sudo systemctl status camn-aggie-air.service --no-pager --full
```

Inspect only recent logs so an earlier failure is not mistaken for a current one:

```bash
journalctl -u camn-aggie-air.service \
  --since "2 minutes ago" \
  --no-pager
```

For live monitoring:

```bash
sudo journalctl -u camn-aggie-air.service -f
```

## Successful Result

The service should report:

```text
Active: active (running)
```

For the AggieAir collector, the logs should also show that the sensor was discovered and its serial connection opened:

```text
Using AggieAir device: ttyACM0
Opening serial connection to /dev/ttyACM0
```

## Recommended Unit Configuration

Use explicit paths and a non-root runtime account:

```ini
[Service]
User=zero
Group=zero
WorkingDirectory=/home/zero/camn-sensor-network/services/edge/sensor-node
ExecStart=/home/zero/camn-sensor-network/services/edge/sensor-node/.venv/bin/python /home/zero/camn-sensor-network/services/edge/sensor-node/src/sensors/aggie_air.py
```

If the device is exposed through a serial port, grant the service account access through the appropriate operating-system group rather than running the service as root:

```bash
sudo usermod -aG dialout zero
```

A logout or reboot may be required before new group membership applies to user sessions. Restart the system service after making the change.

After editing a unit file, reload systemd before restarting:

```bash
sudo systemctl daemon-reload
sudo systemctl restart camn-aggie-air.service
```

## Incident Summary

`camn-aggie-air.service` worked before it was renamed, but the reinstalled unit launched the application through the new project `.venv`. The environment did not contain `pyserial`, so the process exited during startup. Adding `pyserial` to the `uv` project dependencies and synchronizing the Pi environment resolved the failure.

The duplicate `Closing serial connection` log entry observed during shutdown is a separate, non-blocking cleanup item. The legacy script may close the connection from both a signal or exception handler and a `finally` block.
