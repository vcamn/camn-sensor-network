from pathlib import Path

from sensors.aggie_air import AggieAir, loop as aggie_air_loop
from sensors.parsers import WindReading
from sensors.purple_air import PurpleAir, loop as purple_air_loop
from sensors.transport import ReplayLineSource
from sensors.wind_sensor import WindSensor

FIXTURES = Path(__file__).parent / "fixtures"


def test_aggie_air_reads_replayed_bytes_without_hardware():
    sensor = AggieAir(
        line_source=ReplayLineSource(FIXTURES / "aggie_air_sample.log")
    )

    assert sensor.read() == "123,0.2,45.0,0.3\n"
    assert sensor.read() == ""
    assert sensor.read() == "456,0.4,90.0,0.5\n"
    assert sensor.read() == ""
    assert sensor.read() == ""
    sensor.close()


def test_wind_sensor_reads_typed_replayed_records_without_hardware():
    sensor = WindSensor(line_source=ReplayLineSource(FIXTURES / "wind_sample.log"))

    assert sensor.read() == WindReading("123", 1.5, 270.0, -2.0)
    assert sensor.read() == WindReading("123", 2.0, 275.0, -1.5)
    assert sensor.read() is None
    assert sensor.read() is None
    sensor.close()


def test_purple_air_reads_replayed_carriage_return_framing_without_hardware():
    sensor = PurpleAir(line_source=ReplayLineSource(FIXTURES / "purple_air_sample.log"))

    assert sensor.read().startswith("timestamp,AA:BB:CC:DD:EE:FF,")
    assert sensor.read() == ""
    sensor.close()


def test_aggie_air_loop_writes_replay_data_and_stops_at_eof(tmp_path):
    sensor = AggieAir(line_source=ReplayLineSource([b"123,payload\n"]))

    aggie_air_loop(sensor, tmp_path, stop_on_eof=True)

    output_files = list(tmp_path.glob("aggieair_*.csv"))
    assert len(output_files) == 1
    assert output_files[0].read_text(encoding="utf-8").endswith(",123,payload\n")


def test_aggie_air_find_sensors_falls_back_to_ttyusb_when_cdc_acm_missing(monkeypatch):
    matches = {
        "/sys/bus/usb/drivers/cdc_acm/1-*/tty/tty*": [],
        "/sys/bus/usb/drivers/cp210x/1-*/tty*": [
            "/sys/bus/usb/drivers/cp210x/1-1.2:1.0/ttyUSB0"
        ],
        "/sys/bus/usb/drivers/ch341/1-*/tty*": [],
    }

    monkeypatch.setattr("sensors.aggie_air.glob", lambda pattern: matches.get(pattern, []))

    assert AggieAir.find_aggieair_sensors() == ["ttyUSB0"]


def test_purple_air_loop_writes_valid_replay_data_and_stops_at_eof(tmp_path):
    fields = ["timestamp", "AA:B:C:D:E:F", *(["0"] * 34)]
    sensor = PurpleAir(
        line_source=ReplayLineSource([(",".join(fields) + "\r\n").encode()])
    )

    purple_air_loop(sensor, tmp_path, stop_on_eof=True)

    output_files = list(tmp_path.glob("purpleair_*.csv"))
    assert len(output_files) == 1
    assert ",timestamp,AA:B:C:D:E:F," in output_files[0].read_text(encoding="utf-8")
