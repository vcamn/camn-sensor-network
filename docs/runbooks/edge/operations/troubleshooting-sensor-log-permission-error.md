# Troubleshoot PurpleAir Log Permission Errors

## Symptoms

The `camn-<sensor>.service` starts and connects to the sensor, but no CSV output is written.

The service journal reports (example):

```text
PermissionError: [Errno 13] Permission denied:
'./logs/purpleair_YYYY-MM-DD.csv'
```

The service then exits and is restarted by systemd.

## Cause

The service runs as the `zero` user:

```ini
User=zero
Group=zero
```

The relative output path `./logs` is resolved from the service's working directory:

```ini
WorkingDirectory=/home/zero/camn-sensor-network/services/edge/
```

Therefore, the effective log directory is:

```text
/home/zero/camn-sensor-network/services/edge/logs
```

The error occurred because this directory was owned by `root`, so the `zero` service account could not create or append to the CSV file.

## Diagnosis

Check the service log (example):

```bash
sudo journalctl -u camn-purple-air.service -n 100 --no-pager
```

Check the log directory ownership and permissions:

```bash
ls -ld /home/zero/camn-sensor-network/services/edge/logs
ls -l /home/zero/camn-sensor-network/services/edge/logs
```

If the directory or CSV files are owned by `root`, correct their ownership.

## Resolution

Assign ownership of the log directory and its contents to the service account:

```bash
sudo chown -R zero:zero /home/zero/camn-sensor-network/services/edge/logs
sudo chmod 755 /home/zero/camn-sensor-network/services/edge/logs
```

Restart the service (example):

```bash
sudo systemctl restart camn-purple-air.service
```

## Verification

Confirm that the service is running (example):

```bash
sudo systemctl status camn-purple-air.service --no-pager
```

Check for new errors (example):

```bash
sudo journalctl -u camn-purple-air.service -n 50 --no-pager
```

Confirm that the daily CSV file exists:

```bash
ls -l /home/zero/camn-sensor-network/services/edge/logs
```

Inspect the latest readings (example):

```bash
tail -n 5 /home/zero/camn-sensor-network/services/edge/logs/purpleair_$(date +%F).csv
```

## Prevention

Do not run `<sensor>.py` with `sudo`. Doing so can create root-owned directories or CSV files that the systemd service cannot subsequently update.

Keep runtime output owned by the same account configured in the service:

```text
zero:zero
```

For clarity, the service can also pass an explicit output path instead of relying on the script's relative default (example):

```ini
ExecStart=/home/zero/camn-sensor-network/services/edge/sensor-node/.venv/bin/python /home/zero/camn-sensor-network/services/edge/sensor-node/src/sensors/purple_air.py --path /home/zero/camn-sensor-network/services/edge/logs
```

After changing the unit file (example):

```bash
sudo systemctl daemon-reload
sudo systemctl restart camn-purple-air.service
```
