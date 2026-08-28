from sensors.parsers import (
    WindReading,
    extract_purple_air_line,
    is_purple_air_minute_data,
    parse_aggie_air_line,
    parse_wind_line,
)


def test_parse_aggie_air_line_preserves_accepted_payload():
    line = "123,0.2,45.0,0.3\r\n"

    assert parse_aggie_air_line(line) == line


def test_parse_aggie_air_line_rejects_non_digit_leading_payload():
    assert parse_aggie_air_line("status,not,data") is None
    assert parse_aggie_air_line("") is None


def test_parse_wind_line_normalizes_and_converts_fields():
    assert parse_wind_line(" 123, 1.5, 270, -2.0\r\n") == WindReading(
        device_id="123",
        u=1.5,
        wd=270.0,
        v=-2.0,
    )


def test_parse_wind_line_rejects_invalid_records():
    assert parse_wind_line("123,1,2") is None
    assert parse_wind_line("123,not-a-number,2,3") is None
    assert parse_wind_line("status,1,2,3") is None


def test_extract_purple_air_line_returns_final_carriage_return_segment():
    assert extract_purple_air_line("discarded\rminute-data\r\n") == "minute-data"
    assert extract_purple_air_line("minute-data\n") == "minute-data\n"


def purple_air_minute_line(device_id: str = "AA:B:C:D:E:F") -> str:
    fields = ["timestamp", device_id, *(["0"] * 34)]
    return ",".join(fields)


def test_is_purple_air_minute_data_requires_36_fields_and_valid_mac():
    assert is_purple_air_minute_data(purple_air_minute_line()) is True
    assert is_purple_air_minute_data("timestamp,AA:B:C:D:E:F,0") is False
    assert is_purple_air_minute_data(
        purple_air_minute_line("AA:B:C:D:E")
    ) is False


def test_is_purple_air_minute_data_rejects_malformed_csv():
    malformed_line = 'timestamp,"AA:B:C:D:E:F,0' + ',0' * 34

    assert is_purple_air_minute_data(malformed_line) is False
