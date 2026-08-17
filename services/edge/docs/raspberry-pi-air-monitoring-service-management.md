# Raspberry Pi Air Monitoring Service Management

This guide documents the `systemd` commands and service-management procedures used for CAMN Sensor Network air monitoring services running on Raspberry Pi edge collector devices.

The edge collector services acquire measurements from attached or network-connected air monitoring sensors and transmit collected data to the configured endpoint.

> **Scope:** These procedures currently support Raspberry Pi edge collector development. Production deployment and fleet-wide service management may introduce additional automation later.

## Service Naming Convention

CAMN-owned `systemd` services use the `camn-` prefix to provide a consistent namespace and distinguish CAMN applications from operating-system and third-party services.

Current air monitoring services:

| Service     | systemd Unit               |
| ----------- | -------------------------- |
| AggieAir    | `camn-aggie-air.service`   |
| PurpleAir   | `camn-purple-air.service`  |
| Wind Sensor | `camn-wind-sensor.service` |

Service unit files maintained by the CAMN project should use the same naming convention.

```text
camn-<service-name>.service
```

This allows CAMN services to be identified and managed independently from other services installed on the Raspberry Pi.

## Common Service Commands

### List Running Services

List all services currently in the active/running state:

```bash
systemctl list-units --type=service --state=running
```

### List CAMN Services

List loaded CAMN service units:

```bash
systemctl list-units 'camn-*' --type=service
```

Include installed CAMN units that may not currently be loaded:

```bash
systemctl list-unit-files 'camn-*'
```

### Check CAMN Service Status

Check multiple sensor services:

```bash
systemctl status \
    camn-aggie-air.service \
    camn-purple-air.service \
    camn-wind-sensor.service
```

Check an individual service:

```bash
systemctl status camn-aggie-air.service
```

### Start a Service

```bash
sudo systemctl start camn-aggie-air.service
```

### Stop a Service

```bash
sudo systemctl stop camn-aggie-air.service
```

### Restart a Service

Restart a service after application or configuration changes:

```bash
sudo systemctl restart camn-aggie-air.service
```

### Enable a Service at Boot

```bash
sudo systemctl enable camn-aggie-air.service
```

### Disable a Service at Boot

```bash
sudo systemctl disable camn-aggie-air.service
```

Disabling a service does not stop a currently running instance.

To disable the service and stop it immediately:

```bash
sudo systemctl disable --now camn-aggie-air.service
```

### Enable and Start a Service

For a newly installed service, enable it at boot and start it immediately:

```bash
sudo systemctl enable --now camn-aggie-air.service
```

## View Service Logs

CAMN services write application output through `systemd`, allowing logs to be inspected using `journalctl`.

### View Service Logs

```bash
journalctl -u camn-aggie-air.service
```

### Follow Logs in Real Time

Useful during sensor and edge collector development:

```bash
journalctl -fu camn-aggie-air.service
```

Press `Ctrl+C` to stop following the log.

### View Logs From the Current Boot

```bash
journalctl -u camn-aggie-air.service -b
```

### View Recent Log Entries

Display the most recent 100 entries:

```bash
journalctl -u camn-aggie-air.service -n 100
```

### View Recent Logs and Continue Following

```bash
journalctl -fu camn-aggie-air.service -n 100
```

## Install a CAMN Service

CAMN service unit files should be maintained in the Git repository as part of the edge collector source code.

The repository version is the **authoritative source** for the service configuration. The installed file under `/etc/systemd/system/` is a deployment copy.

### 1. Copy the Unit File

For example:

```bash
sudo cp camn-wind-sensor.service /etc/systemd/system/
```

CAMN-owned service files should be installed under:

```text
/etc/systemd/system/
```

### 2. Reload systemd

After adding or modifying a unit file:

```bash
sudo systemctl daemon-reload
```

This causes `systemd` to reload its unit configuration.

### 3. Enable and Start the Service

```bash
sudo systemctl enable --now camn-wind-sensor.service
```

### 4. Verify the Service

```bash
systemctl status camn-wind-sensor.service
```

If necessary, inspect its logs:

```bash
journalctl -u camn-wind-sensor.service -n 100
```

## Modify an Existing CAMN Service

Because CAMN service files are maintained in source control, modify the **repository copy** rather than treating `/etc/systemd/system/` as the authoritative configuration.

The normal development workflow is:

1. Modify the `.service` file in the repository.
2. Copy the updated unit into `/etc/systemd/system/`.
3. Reload the `systemd` configuration.
4. Restart the service.
5. Verify its status and logs.
6. Commit the repository change to Git.

For example:

```bash
sudo cp camn-wind-sensor.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl restart camn-wind-sensor.service
systemctl status camn-wind-sensor.service
```

Then inspect its runtime output if necessary:

```bash
journalctl -fu camn-wind-sensor.service
```

### Editing an Installed Unit Directly

`systemctl` also supports editing an installed unit:

```bash
sudo systemctl edit --full camn-wind-sensor.service
```

However, this should generally be reserved for troubleshooting or temporary development changes.

> **Important:** Editing the installed service does **not** update the service definition maintained in the Git repository. Any intentional configuration change must also be made to the repository copy so it can be reviewed, committed, and deployed consistently to other devices.

## Remove a CAMN Service

Permanently removing a CAMN service requires stopping it, disabling it, removing its installed unit file, and reloading `systemd`.

### 1. Identify the Service

Verify the service before removing it:

```bash
systemctl status camn-wind-sensor.service
```

Determine where its unit file is loaded from:

```bash
systemctl cat camn-wind-sensor.service
```

For CAMN-owned services, the installed unit should normally be located under:

```text
/etc/systemd/system/
```

### 2. Stop and Disable the Service

Stop and disable the service in one operation:

```bash
sudo systemctl disable --now camn-wind-sensor.service
```

Alternatively:

```bash
sudo systemctl stop camn-wind-sensor.service
sudo systemctl disable camn-wind-sensor.service
```

> **Caution:** `disable --now` immediately stops the specified service. Be particularly careful when running this command against infrastructure services such as `ssh.service` while connected remotely. Stopping SSH can terminate the active SSH connection.

### 3. Remove the Installed Unit File

For a CAMN-owned service:

```bash
sudo rm /etc/systemd/system/camn-wind-sensor.service
```

Do not generally delete service files directly from:

```text
/usr/lib/systemd/system/
```

Units in that location are typically managed by installed Linux packages. Package-managed services should normally be removed through the appropriate package manager.

### 4. Reload systemd

After removing the unit:

```bash
sudo systemctl daemon-reload
```

### 5. Reset Failed State

If the removed service previously entered a failed state:

```bash
sudo systemctl reset-failed
```

### 6. Verify Removal

```bash
systemctl status camn-wind-sensor.service
```

The unit should no longer be found.

## Renaming Existing Development Services

The initial development services previously used the following names:

| Previous Name             | CAMN Service Name          |
| ------------------------- | -------------------------- |
| `run_aggie_air.service`   | `camn-aggie-air.service`   |
| `run_purple_air.service`  | `camn-purple-air.service`  |
| `run_wind_sensor.service` | `camn-wind-sensor.service` |

When migrating an existing Raspberry Pi, remove the old unit before installing the renamed unit.

For example:

```bash
sudo systemctl disable --now run_wind_sensor.service
sudo rm /etc/systemd/system/run_wind_sensor.service
sudo systemctl daemon-reload
```

Install the renamed service:

```bash
sudo cp camn-wind-sensor.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable --now camn-wind-sensor.service
```

Verify the new service:

```bash
systemctl status camn-wind-sensor.service
```

## systemd Responsibilities

`systemd` provides process and lifecycle management for CAMN edge collector services.

Its responsibilities include:

* Starting collector services
* Stopping collector services
* Starting services automatically during device boot
* Restarting processes according to their unit configuration
* Tracking service state
* Capturing application output for `journalctl`
* Providing a consistent operational interface for collector processes

Application behavior should remain within the CAMN edge collector software rather than being implemented through `systemd`.

This includes:

* Sensor communication and data acquisition
* Measurement validation
* Data buffering
* Retry behavior for sensor operations
* Endpoint communication
* Telemetry transmission
* Application-level error handling

Keeping these responsibilities separate allows `systemd` to remain the process supervisor while the edge collector remains responsible for its application and data-collection behavior.

## Quick Reference

| Operation         | Command                                               |
| ----------------- | ----------------------------------------------------- |
| Status            | `systemctl status camn-aggie-air.service`             |
| Start             | `sudo systemctl start camn-aggie-air.service`         |
| Stop              | `sudo systemctl stop camn-aggie-air.service`          |
| Restart           | `sudo systemctl restart camn-aggie-air.service`       |
| Enable            | `sudo systemctl enable camn-aggie-air.service`        |
| Disable           | `sudo systemctl disable camn-aggie-air.service`       |
| Enable + Start    | `sudo systemctl enable --now camn-aggie-air.service`  |
| Disable + Stop    | `sudo systemctl disable --now camn-aggie-air.service` |
| Follow Logs       | `journalctl -fu camn-aggie-air.service`               |
| Current Boot Logs | `journalctl -u camn-aggie-air.service -b`             |
| Reload Units      | `sudo systemctl daemon-reload`                        |
| Reset Failures    | `sudo systemctl reset-failed`                         |
