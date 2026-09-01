# Raspberry Pi field deployment checklist

This checklist is for the Raspberry Pi field node running the CAMN edge sensor services. It is intended to prevent serial-port conflicts, confirm stable hardware mapping, and document the expected sensor-to-port relationship before the node is put into service.

> Operational note: for this project, live service logs are not always a reliable indicator of sensor health. In the field, a service may appear quiet in `journalctl` while the sensor CSV continues to be written. The most reliable health indicators are actual file growth, device presence, and kernel USB/serial events.

## 1. Confirm sensor hardware and cable mapping

Before starting services, label each USB serial adapter and the sensor it serves.

- AggieAir sensor
- PurpleAir sensor
- Wind sensor
- Raspberry Pi USB port used
- USB serial adapter vendor/product
- Physical cable ID and installation location

Record the mapping in the site runbook or field inventory.

## 2. Identify actual serial devices

On the Raspberry Pi, inspect the current serial ports:

```bash
ls -l /dev/ttyUSB*
ls -l /dev/ttyACM*
ls -l /dev/serial/by-id 2>/dev/null || true
```

Then confirm which device is associated with each sensor by checking the kernel log:

```bash
sudo dmesg | tail -n 50
```

Look specifically for:

- `cp210x converter now attached to ttyUSB0`
- `ch341-uart converter now attached to ttyUSB1`
- `cdc_acm ... ttyACM0`
- repeated `failed set request`, `failed to set baud rate`, or `urb stopped` messages

If needed, use `udevadm` for the specific device path:

```bash
udevadm info /dev/ttyUSB0
```

## 3. Verify the service-to-device mapping

Each service must have a unique and intentional device mapping.

Current configuration pattern:

- AggieAir service starts with a configured device path such as `--device ttyUSB0`
- PurpleAir service should be checked separately
- Wind sensor service should be checked separately

The mapping must be validated before startup.

## 4. Validate that no two services share the same device

Check whether multiple service unit files point to the same path.

Example review items:

- `camn-aggie-air.service`
- `camn-purple-air.service`
- `camn-wind-sensor.service`

Each service should map to a different device path or a stable udev alias.

## 5. Prefer stable device names

Prefer stable names over transient Linux numbering such as `ttyUSB0`.

Recommended pattern:

```text
/dev/camn/aggieair
/dev/camn/purpleair
/dev/camn/wind
```

These should be created with udev rules or equivalent stable mapping if the environment requires persistent hardware identity.

## 6. Start services one at a time

Do not start all sensors simultaneously during initial validation.

Sequence:

1. Bring up the first sensor service
2. Confirm it reads normally
3. Record the port and sensor output behavior
4. Start the next service
5. Repeat until all sensors are validated

This helps isolate accidental shared-port behavior.

## 7. Treat file output as the primary health signal

For each service, confirm that the expected CSV/log file is being written to and is growing.

Example checks:

```bash
ls -l /home/zero/camn-sensor-network/services/edge/logs/
ls -lh /home/zero/camn-sensor-network/services/edge/logs/*.csv
```

A file that continues to grow is a stronger health signal than a quiet `journalctl` stream.

## 8. Use service logs only as secondary validation

Use `journalctl` as a secondary check, not the primary health indicator.

```bash
systemctl status camn-wind-sensor.service --no-pager
journalctl -u camn-wind-sensor.service -n 50 --no-pager
```

Do not conclude a sensor is unhealthy solely because the log stream appears silent.

## 9. Check the kernel for serial and USB failures

When the sensor output is intermittent, the kernel log is the key source of truth for transport health.

Use:

```bash
sudo dmesg -w
```

Check for:

- low-level CP210x/CH341 failures
- baud-rate configuration failures
- USB resets or disconnects
- repeated `urb stopped` or `failed set request` messages
- kernel logs showing a device is disappearing or re-enumerating

## 10. Validate output quality

Ensure each service writes real sensor data to the expected log or CSV file and not noisy or fragmented payloads.

Check:

- log directory exists
- file is growing as expected
- timestamps are valid
- field values match the expected sensor semantics
- the output is not partial, duplicated, or interleaved

## 11. Recheck after reboot

Because USB serial names can change after reboot or replugging, verify again after a node restart.

```bash
sudo reboot
```

After the restart:

```bash
ls -l /dev/ttyUSB*
ls -l /dev/ttyACM*
ls -l /dev/serial/by-id 2>/dev/null || true
```

Confirm the device mapping still matches the field documentation.

## 12. Record the final mapping in the deployment log

Capture the final verified configuration in the site record:

- sensor name
- service name
- device path
- USB alias or stable mapping
- timestamp of validation
- operator name
- whether file output was observed to grow
- whether kernel log showed any serial/USB errors

## 13. Operational rule

Any time a USB cable is moved, swapped, or replaced, re-run the serial validation checklist before considering the node healthy.

## 14. Escalation trigger

Escalate to engineering if any of the following occurs:

- two services appear to read the same port
- sensor output is partial or interleaved
- a port path changes unexpectedly after a reboot
- the CSV stops growing even though the service remains started
- `dmesg` shows repeated serial adapter failures or USB resets
- the device is present but reads remain silent or invalid
