'''
File for reading the wind sensors
NEED TO FORMAT THE AVERAGES TO TWO DECIMAL PLACES

'''

# -*- coding: utf-8 -*-
import argparse
try:
    import serial
except ImportError:
    print("***\nFailed to load module pyserial.")
    print('Please install it using')
    print('apt-get install python3-serial')
    print('***\n')
    exit(-1)


from glob import glob
from datetime import datetime
import os
import serial

try:
    from .parsers import WindReading, parse_wind_line
    from .transport import LineSource, ReplayLineSource, SerialLineSource
except ImportError:
    from parsers import WindReading, parse_wind_line
    from transport import LineSource, ReplayLineSource, SerialLineSource

class WindSensor(object):
    ''' Class for reading wind sensor data from a serial port. '''
    def __init__(self, device_name=None, line_source: LineSource | None = None):
        if line_source is None:
            if not device_name:
                raise ValueError("device_name is required when line_source is not supplied")
            device_path = os.path.join('/dev/', device_name)
            print(f"Opening serial connection to {device_path}")
            line_source = SerialLineSource(device_path, 9600)
        self.line_source = line_source
        
    def read(self):
        ''' blocking.  Reads the next complete line from the serial port device. '''
        encoded_line = self.line_source.readline()
        self.at_eof = encoded_line == b''
        # Decode bytes into a string
        try:
            line = encoded_line.decode("utf-8", errors='replace')
        except UnicodeDecodeError as ex:
            print(f'Failed to convert line to utf-8 because {ex}.\n Line: {encoded_line}')
            return ''
        return parse_wind_line(line)
    
    def close(self):
        ''' Close the serial connection '''
        if self.line_source:
            print("Closing serial connection")
            self.line_source.close()


    @staticmethod
    def find_wind_sensors():
        ''' Looks for devices that use the same driver as the wind sensor serial to usb chip. '''
        usb_module_path = '/sys/bus/usb/drivers/cp210x/'
        #/sys/bus/usb/drivers/cp210x/1-1.4:1.0/ttyUSB0
        wildcard_path = '1-*/tty*'
        paths = glob(usb_module_path + wildcard_path)
        # if we found something, print it out
        if paths:
            print("Found Wind Sensor devices at paths:")
            for path in paths:
                print("\t" + path)
        devices = [os.path.basename(path) for path in paths]
        # make sure devices are in alphabetical order
        return sorted(devices)

def loop(wind_sensor, output_folder_path, stop_on_eof=False):
    ''' Main loop for reading data from the wind sensor and writing to a file. '''
    # Initiate sensor reading arrays
    u = []
    v = []
    wd = []
    last_write = datetime(1970, 1, 1, 0, 0)
    if not os.path.exists(output_folder_path):
        os.makedirs(output_folder_path)
    try:
        while True:
            # Read line: deviceId, u, wd, v
            reading: WindReading | None = wind_sensor.read()
            if stop_on_eof and wind_sensor.at_eof:
                break
            if reading:
                print(f"Received: {reading}")
                
                timestamp = datetime.now()
                time_difference = (timestamp - last_write).seconds
                
                # If just starting up, reset last_write
                if time_difference > 70:
                    last_write = timestamp
                
                elif time_difference > 60:
                    print("Writing to file")
                    u_mean = sum(u)/len(u) if u else 0
                    v_mean = sum(v)/len(v) if v else 0
                    wd_mean = sum(wd)/len(wd) if wd else 0
                    print(f"u_mean: {u_mean}, v_mean: {v_mean}, wd_mean: {wd_mean}")
                    u = []
                    v = []
                    wd = []
                    last_write = timestamp
                    timestamp_formatted = timestamp.strftime('%Y-%m-%d %H:%M:%S')
                    filename = 'LTwind_{}.csv'.format((datetime.now()).strftime('%Y-%m-%d'))
                    dated_filename = os.path.join(output_folder_path, filename)
                    with open(dated_filename, 'a', encoding='utf-8') as f:
                        f.write(f"{timestamp_formatted},{wd_mean},{u_mean},{v_mean}\n")
                else:
                    try:
                        u.append(reading.u)
                        wd.append(reading.wd)
                        v.append(reading.v)
                    except ValueError as ve:
                        print(f"Value error: {ve}. Line: {reading}")
    except KeyboardInterrupt:
        print("Stopped by user.")
    finally:
        wind_sensor.close()

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Log readings from a wind sensor')
    parser.add_argument('-d', '--device', default='', help='Device name, if not replaying input')
    parser.add_argument('-p', '--path', default='./logs', help='Path for saving the log file')
    parser.add_argument('--input-file', help='Replay raw sensor lines from a local file')
    args = parser.parse_args()

    # Find the available wind sensors
    DEVICES = [] if args.input_file else WindSensor.find_wind_sensors()
    if not DEVICES and not args.input_file and not args.device:
        print("No wind sensors found.")
        exit(1)
    device = args.device or (DEVICES[0] if DEVICES else None)
    line_source = ReplayLineSource(args.input_file) if args.input_file else None
    if line_source:
        print(f"Replaying wind sensor input from {args.input_file}")
    else:
        print(f"Using wind sensor device: {device}")
    WINDSENSOR = WindSensor(device, line_source=line_source)
    try:
        loop(WINDSENSOR, args.path, stop_on_eof=bool(args.input_file))
    except KeyboardInterrupt:
        print("Exiting program.")
    finally:
        WINDSENSOR.close()
