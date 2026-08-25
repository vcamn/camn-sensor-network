# Enable Persistent systemd Journal Logging

Raspberry Pi OS may configure `systemd-journald` to store logs in volatile storage under `/run/log/journal`. Volatile journal logs are lost when the Raspberry Pi reboots.

For development and troubleshooting, persistent journaling allows logs from previous boots to be inspected after an unexpected reboot or system failure.

## Check the Current Journal Configuration

Display the effective `systemd-journald` configuration:

```bash
systemd-analyze cat-config systemd/journald.conf
```

On the current Raspberry Pi configuration, Raspberry Pi OS provides the following system drop-in:

```text
/usr/lib/systemd/journald.conf.d/40-rpi-volatile-storage.conf
```

containing:

```ini
[Journal]
Storage=volatile
```

This causes journal data to be stored under:

```text
/run/log/journal/
```

Because `/run` is volatile, these logs do not survive a reboot.

> Do not modify files under `/usr/lib/systemd/`. These files are managed by the operating system and may be replaced during package updates.

## Create a Persistent Storage Override

Create the local journald configuration directory if necessary:

```bash
sudo mkdir -p /etc/systemd/journald.conf.d
```

Create a local override:

```bash
sudo nano /etc/systemd/journald.conf.d/90-persistent-storage.conf
```

Add:

```ini
[Journal]
Storage=persistent
```

Save and close the file.

The `90-` prefix ensures that this local configuration is processed after Raspberry Pi OS's `40-rpi-volatile-storage.conf`.

## Verify the Effective Configuration

Run:

```bash
systemd-analyze cat-config systemd/journald.conf
```

The output should include both settings in this order:

```text
# /usr/lib/systemd/journald.conf.d/40-rpi-volatile-storage.conf

[Journal]
Storage=volatile

# /etc/systemd/journald.conf.d/90-persistent-storage.conf

[Journal]
Storage=persistent
```

Because the local configuration is processed later, `Storage=persistent` becomes the effective setting.

## Restart journald

Apply the configuration:

```bash
sudo systemctl restart systemd-journald
```

Flush existing runtime journal data to persistent storage:

```bash
sudo journalctl --flush
```

## Verify Persistent Journal Files

Persistent journals are stored under:

```text
/var/log/journal/
```

Verify the directory:

```bash
ls -lh /var/log/journal/
```

A directory corresponding to the system's machine ID should appear.

Verify that journal files are being created:

```bash
sudo find /var/log/journal -maxdepth 2 -type f -ls
```

Typical files include:

```text
/var/log/journal/<machine-id>/system.journal
/var/log/journal/<machine-id>/user-1000.journal
```

## Verify Persistence Across Reboots

Perform a controlled reboot:

```bash
sudo reboot
```

After reconnecting to the Raspberry Pi, list recorded boots:

```bash
journalctl --list-boots
```

With persistent journaling working, the output should contain both the previous and current boot:

```text
IDX BOOT ID                          FIRST ENTRY                 LAST ENTRY
 -1 <previous-boot-id>               ...                         ...
  0 <current-boot-id>                ...                         ...
```

Verify that logs from the previous boot can be read:

```bash
sudo journalctl -b -1 -n 20 --no-pager
```

## Inspect Logs After an Unexpected Reboot

After an unexpected reboot, first identify the available boots:

```bash
journalctl --list-boots
```

Display the final 100 messages from the previous boot:

```bash
sudo journalctl -b -1 -n 100 --no-pager
```

Inspect previous-boot kernel messages:

```bash
sudo journalctl -k -b -1 --no-pager | tail -100
```

Search for common system-level failures:

```bash
sudo journalctl -b -1 --no-pager | grep -Ei \
'oom|out of memory|killed process|panic|watchdog|voltage|under.?voltage|thermal|mmc|i/o error|ext4|reset|segfault|failed|error'
```

### Distinguishing a Controlled Reboot From an Abrupt Reset

A normal controlled reboot should contain shutdown messages similar to:

```text
Reached target shutdown.target
Finished systemd-reboot.service
Reached target reboot.target
Shutting down
Syncing filesystems and block devices
Received SIGTERM
Journal stopped
```

If the previous boot's journal instead ends abruptly during ordinary system activity without a shutdown sequence, the system may have experienced an unexpected reset, power loss, or another failure below the normal operating-system shutdown path.

Persistent journaling is particularly useful in this situation because the events immediately preceding the reset remain available after the device starts again.

## Disable Persistent Journaling

If persistent journaling is no longer desired, remove the CAMN override:

```bash
sudo rm /etc/systemd/journald.conf.d/90-persistent-storage.conf
```

Restart journald:

```bash
sudo systemctl restart systemd-journald
```

Verify that the Raspberry Pi OS setting is effective again:

```bash
systemd-analyze cat-config systemd/journald.conf
```

The effective configuration should return to:

```ini
[Journal]
Storage=volatile
```

## Storage Considerations

Persistent journaling introduces additional writes to the Raspberry Pi's storage and consumes disk space over time.

For development devices, the additional diagnostic capability is useful because logs survive unexpected reboots. Production edge-device logging and retention policies should be evaluated separately based on SD-card endurance, available storage, operational troubleshooting requirements, and centralized telemetry/logging capabilities.
