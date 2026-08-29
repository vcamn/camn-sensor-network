from sensors.aggie_air import AggieAir, loop as aggie_air_loop
from sensors.parsers import WindReading
from sensors.purple_air import PurpleAir, loop as purple_air_loop
from sensors.transport import ReplayLineSource
from sensors.wind_sensor import WindSensor


def test_aggie_air_reads_replayed_bytes_without_hardware():
    sensor = AggieAir(line_source=ReplayLineSource([b"123,payload\r\n", b"status\n"]))

    assert sensor.read() == "123,payload\r\n"
    assert sensor.read() == ""
    assert sensor.read() == ""
    sensor.close()


def test_wind_sensor_reads_typed_replayed_records_without_hardware():
    sensor = WindSensor(line_source=ReplayLineSource([b"123,1.5,270,-2\r\n", b"bad\n"]))

    assert sensor.read() == WindReading("123", 1.5, 270.0, -2.0)
    assert sensor.read() is None
    assert sensor.read() is None
    sensor.close()


def test_purple_air_reads_replayed_carriage_return_framing_without_hardware():
    sensor = PurpleAir(
        line_source=ReplayLineSource([b"discarded\rminute-data\r\n"])
    )

    assert sensor.read() == "minute-data"
    assert sensor.read() == ""
    sensor.close()


def test_aggie_air_loop_writes_replay_data_and_stops_at_eof(tmp_path):
    sensor = AggieAir(line_source=ReplayLineSource([b"123,payload\n"]))

    aggie_air_loop(sensor, tmp_path, stop_on_eof=True)

    output_files = list(tmp_path.glob("aggieair_*.csv"))
    assert len(output_files) == 1
    assert output_files[0].read_text(encoding="utf-8").endswith(",123,payload\n")


def test_purple_air_loop_writes_valid_replay_data_and_stops_at_eof(tmp_path):
    fields = ["timestamp", "AA:B:C:D:E:F", *(["0"] * 34)]
    sensor = PurpleAir(
        line_source=ReplayLineSource([(",".join(fields) + "\r\n").encode()])
    )

    purple_air_loop(sensor, tmp_path, stop_on_eof=True)

    output_files = list(tmp_path.glob("purpleair_*.csv"))
    assert len(output_files) == 1
    assert ",timestamp,AA:B:C:D:E:F," in output_files[0].read_text(encoding="utf-8")
