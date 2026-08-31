'''
File for reading the AggieAir

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
# from time import sleep
import os
import serial

try:
    from .parsers import parse_aggie_air_line
    from .transport import LineSource, ReplayLineSource, SerialLineSource
except ImportError:
    from parsers import parse_aggie_air_line
    from transport import LineSource, ReplayLineSource, SerialLineSource

class AggieAir(object):
    ''' Class for reading wind sensor data from a serial port. '''
    def __init__(self, device_name=None, line_source: LineSource | None = None):
        if line_source is None:
            if not device_name:
                raise ValueError("device_name is required when line_source is not supplied")
            device_path = os.path.join('/dev/', device_name)
            print(f"Opening serial connection to {device_path}")
            line_source = SerialLineSource(device_path, 115200, timeout=10)
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
        return parse_aggie_air_line(line) or ''
    
    def close(self):
        ''' Close the serial connection '''
        if self.line_source:
            print("Closing serial connection")
            self.line_source.close()


    @staticmethod
    def find_aggieair_sensors():
        '''Looks for AggieAir serial devices across the common USB-to-serial drivers.'''
        discovery_paths = [
            ('cdc_acm', '/sys/bus/usb/drivers/cdc_acm/', '1-*/tty/tty*'),
            ('cp210x', '/sys/bus/usb/drivers/cp210x/', '1-*/tty*'),
            ('ch341', '/sys/bus/usb/drivers/ch341/', '1-*/tty*'),
        ]

        paths = []
        for _, usb_module_path, wildcard_path in discovery_paths:
            paths.extend(glob(usb_module_path + wildcard_path))

        if paths:
            print("Found AggieAir devices at paths:")
            for path in paths:
                print("\t" + path)

        devices = [os.path.basename(path) for path in paths]
        return sorted(set(devices))

def loop(aggieair_sensor, output_folder_path, stop_on_eof=False):
    ''' Main loop for reading data from the wind sensor and writing to a file. '''
    if not os.path.exists(output_folder_path):
        os.makedirs(output_folder_path)
    try:
        while True:
            # Read line: deviceId, u, wd, v
            line = aggieair_sensor.read()
            if stop_on_eof and aggieair_sensor.at_eof:
                break
            if line:
                print(f"Received: {line}")
                timestamp = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
                filename = 'aggieair_{}.csv'.format((datetime.now()).strftime('%Y-%m-%d'))
                dated_filename = os.path.join(output_folder_path, filename)
                with open(dated_filename, 'a', encoding='utf-8') as f:
                    # Record = RPi timestamp, deviceID, AggieAir timestamp, WE, AE, VOCppb
                    f.write(f"{timestamp},{line}")
    except KeyboardInterrupt:
        print("Stopped by user.")
    finally:
        aggieair_sensor.close()

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Log readings from an AggieAir sensor')
    parser.add_argument('-d', '--device', default='', help='Device name, if not replaying input')
    parser.add_argument('-p', '--path', default='./logs', help='Path for saving the log file')
    parser.add_argument('--input-file', help='Replay raw sensor lines from a local file')
    args = parser.parse_args()

    # Find the available AggieAir sensors
    DEVICES = [] if args.input_file else AggieAir.find_aggieair_sensors()
    if not DEVICES and not args.input_file and not args.device:
        print("No AggieAir sensors found.")
        exit(1)
    device = args.device or (DEVICES[0] if DEVICES else None)
    line_source = ReplayLineSource(args.input_file) if args.input_file else None
    if line_source:
        print(f"Replaying AggieAir input from {args.input_file}")
    else:
        print(f"Using AggieAir device: {device}")
    VOCSENSOR = AggieAir(device, line_source=line_source)
    try:
        loop(VOCSENSOR, args.path, stop_on_eof=bool(args.input_file))
    except KeyboardInterrupt:
        print("Exiting program.")
    finally:
        VOCSENSOR.close()
