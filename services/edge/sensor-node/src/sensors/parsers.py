"""Pure parsing helpers for sensor serial payloads."""

from __future__ import annotations

import csv
import re
from dataclasses import dataclass


@dataclass(frozen=True)
class WindReading:
    """A validated wind record in the sensor's wire-field order."""

    device_id: str
    u: float
    wd: float
    v: float


_MAC_ADDRESS_PATTERN = re.compile(
    r"^(?:[0-9a-fA-F]{1,2}:){5}[0-9a-fA-F]{1,2}$"
)


def parse_aggie_air_line(line: str) -> str | None:
    """Return a digit-leading AggieAir payload without inventing its schema."""
    if not line or not line[0].isdigit():
        return None
    return line


def parse_wind_line(line: str) -> WindReading | None:
    """Parse a wind record formatted as ``device_id,u,wd,v``."""
    normalized_line = "".join(line.split())
    if not normalized_line or not normalized_line[0].isdigit():
        return None

    parts = normalized_line.split(",")
    if len(parts) < 4:
        return None

    try:
        return WindReading(
            device_id=parts[0],
            u=float(parts[1]),
            wd=float(parts[2]),
            v=float(parts[3]),
        )
    except ValueError:
        return None


def extract_purple_air_line(raw_line: str) -> str:
    """Extract the final payload from Purple Air carriage-return framing."""
    parts = raw_line.split("\r")
    if parts and parts[-1] == "\n":
        parts = parts[:-1]
    return parts[-1] if parts else ""


def is_purple_air_minute_data(dataline: str) -> bool:
    """Check the minimum shape of a Purple Air minute-data CSV record."""
    try:
        parsed_line = next(csv.reader([dataline]))
    except csv.Error:
        return False

    if len(parsed_line) <= 35:
        return False

    return bool(_MAC_ADDRESS_PATTERN.fullmatch(parsed_line[1]))
