from __future__ import annotations

from io import BytesIO
from pathlib import Path

import pytest

from sensors.transport import ReplayLineSource, SerialLineSource


class FakeSerial:
    def __init__(self, *, port: str | None, baudrate: int, timeout: float | None):
        self._port = port
        self.baudrate = baudrate
        self.timeout = timeout
        self.rts = True
        self.dtr = True
        self.is_open = False
        self.opened_with_lines_low = False
        self.reset_input_buffer_calls = 0
        self.reset_output_buffer_calls = 0
        self.close_calls = 0
        self.lines = iter([b"one\r\n", b"two\n"])

    @property
    def port(self) -> str | None:
        return self._port

    @port.setter
    def port(self, value: str) -> None:
        if self.rts or self.dtr:
            raise AssertionError("serial port opened before RTS/DTR were lowered")
        self._port = value

    def open(self) -> None:
        if self._port is None:
            raise AssertionError("serial port opened without a configured path")
        if self.rts or self.dtr:
            raise AssertionError("serial port opened before RTS/DTR were lowered")
        self.is_open = True
        self.opened_with_lines_low = True

    def reset_input_buffer(self) -> None:
        if not self.is_open:
            raise AssertionError("input buffer reset before serial port opened")
        self.reset_input_buffer_calls += 1

    def reset_output_buffer(self) -> None:
        if not self.is_open:
            raise AssertionError("output buffer reset before serial port opened")
        self.reset_output_buffer_calls += 1

    def readline(self) -> bytes:
        return next(self.lines, b"")

    def close(self) -> None:
        self.is_open = False
        self.close_calls += 1


def test_serial_line_source_configures_and_delegates_to_serial():
    devices: list[FakeSerial] = []

    def factory(*, port: str | None, baudrate: int, timeout: float | None) -> FakeSerial:
        device = FakeSerial(port=port, baudrate=baudrate, timeout=timeout)
        devices.append(device)
        return device

    source = SerialLineSource("/dev/ttyUSB0", 9600, timeout=2, serial_factory=factory)

    assert source.readline() == b"one\r\n"
    assert devices[0].port == "/dev/ttyUSB0"
    assert devices[0].baudrate == 9600
    assert devices[0].timeout == 2
    assert devices[0].opened_with_lines_low is True
    assert devices[0].reset_input_buffer_calls == 1
    assert devices[0].reset_output_buffer_calls == 1

    source.close()
    assert devices[0].close_calls == 1


def test_replay_line_source_preserves_bytes_and_repeats_eof():
    source = ReplayLineSource([b"first\r\n", b"second\n"])

    assert source.readline() == b"first\r\n"
    assert source.readline() == b"second\n"
    assert source.readline() == b""
    assert source.readline() == b""


def test_replay_line_source_opens_and_closes_path(tmp_path: Path):
    fixture = tmp_path / "sensor.log"
    fixture.write_bytes(b"first\r\nsecond\n")

    source = ReplayLineSource(fixture)

    assert source.readline() == b"first\r\n"
    assert source.readline() == b"second\n"
    source.close()
    assert source.readline() == b""


def test_replay_line_source_does_not_close_injected_stream_by_default():
    stream = BytesIO(b"first\n")
    source = ReplayLineSource(stream)

    source.close()

    assert stream.closed is False
    assert source.readline() == b""


def test_replay_line_source_can_close_injected_stream():
    stream = BytesIO(b"first\n")
    source = ReplayLineSource(stream, close_source=True)

    source.close()

    assert stream.closed is True


def test_replay_line_source_rejects_text_lines():
    source = ReplayLineSource(["not bytes"])  # type: ignore[list-item]

    with pytest.raises(TypeError):
        source.readline()
