# Edge serial port assignment and uniqueness

## Status

Proposed

## Context

The Raspberry Pi edge node runs multiple sensor collectors as separate Python processes and separate `systemd` services. Each collector opens a serial device path and reads a continuous stream of data. The current implementation assumes that each service is mapped to a distinct device path rather than all services sharing the same port.

The code discovers candidate devices by USB driver family and then selects a device name per sensor class:

- AggieAir checks `cp210x`, `ch341`, and `cdc_acm` device paths
- PurpleAir checks `ch341` device paths
- Wind sensor checks `cp210x` device paths

At runtime, the service configuration may also pass an explicit device path such as `--device ttyUSB0`.

This design expresses the intended behavior: one service should own one serial device. However, the code does not currently enforce uniqueness across services. There is no lock, registry, or startup validation that prevents multiple services from being configured for the same physical serial port.

## Decision

We will document the intended architecture as follows:

- Each sensor service owns exactly one serial device path.
- Device assignment is explicit and verified during deployment.
- USB serial names such as `ttyUSB0` and `ttyACM0` are not guaranteed to remain stable across cable reconnections or reboot order.
- The system must use stable mapping rules or udev names rather than relying solely on transient Linux port numbering.

## Consequences

### Positive

- Clear ownership of each sensor device
- Easier debugging when a single service fails
- Lower risk of interleaved serial traffic
- Better operational clarity for field deployment

### Negative / Trade-offs

- Device path naming remains sensitive to USB reordering unless additional mapping is implemented
- Manual deployment checks are still required until stronger validation is added
- The current code structure does not enforce exclusivity without additional guardrails

## Follow-up work

- Add startup validation to confirm the configured serial path exists and is not already opened by another service
- Introduce stable device names via `udev` rules or explicit config mapping
- Add logs that record the resolved device path and expected sensor identity
- Add duplicate-port detection to protect against accidental shared assignments
