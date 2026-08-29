"""Line-source transports for physical and replayed sensor input."""

from __future__ import annotations

from collections.abc import Callable, Iterable
from pathlib import Path
from typing import BinaryIO, Protocol

import serial


class LineSource(Protocol):
    """Source of raw bytes separated into sensor input lines."""

    def readline(self) -> bytes:
        """Return the next raw line, or b"" when the source reaches EOF."""
        ...

    def close(self) -> None:
        """Release the source resources."""
        ...


class SerialDevice(Protocol):
    """Subset of pyserial required by the serial line source."""

    port: str | None
    rts: bool
    dtr: bool

    def reset_input_buffer(self) -> None: ...

    def reset_output_buffer(self) -> None: ...

    def readline(self) -> bytes: ...

    def close(self) -> None: ...


class SerialLineSource:
    """Line source backed by a pyserial connection."""

    def __init__(
        self,
        device_path: str,
        baudrate: int,
        timeout: float | None = None,
        serial_factory: Callable[..., SerialDevice] = serial.Serial,
    ) -> None:
        self._serial_device = serial_factory(
            port=None,
            baudrate=baudrate,
            timeout=timeout,
        )
        self._serial_device.rts = False
        self._serial_device.dtr = False
        self._serial_device.port = device_path
        self._serial_device.reset_input_buffer()
        self._serial_device.reset_output_buffer()

    def readline(self) -> bytes:
        return self._serial_device.readline()

    def close(self) -> None:
        self._serial_device.close()


class ReplayLineSource:
    """Line source that replays raw bytes from a file or iterable."""

    def __init__(
        self,
        source: str | Path | BinaryIO | Iterable[bytes],
        *,
        close_source: bool | None = None,
    ) -> None:
        self._closed = False
        self._iterator: Iterable[bytes]
        self._source_file: BinaryIO | None = None

        if isinstance(source, (str, Path)):
            self._source_file = Path(source).open("rb")
            self._iterator = self._source_file
            self._close_source = True
        elif hasattr(source, "readline"):
            self._source_file = source
            self._iterator = source
            self._close_source = close_source is True
        else:
            self._iterator = source
            self._close_source = False if close_source is None else close_source

        self._source_iterator = iter(self._iterator)

    def readline(self) -> bytes:
        if self._closed:
            return b""
        try:
            line = next(self._source_iterator)
        except StopIteration:
            return b""
        if not isinstance(line, bytes):
            raise TypeError("ReplayLineSource input must yield bytes")
        return line

    def close(self) -> None:
        if self._closed:
            return
        self._closed = True
        if self._close_source and self._source_file is not None:
            self._source_file.close()
