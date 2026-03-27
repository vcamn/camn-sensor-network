'''
File for reading the wind sensors
NEED TO FORMAT THE AVERAGES TO TWO DECIMAL PLACES

'''

# -*- coding: utf-8 -*-
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

class WindSensor(object):
    ''' Class for reading wind sensor data from a serial port. '''
    def __init__(self, device_name):
        device_path = os.path.join('/dev/', device_name)
        print(f"Opening serial connection to {device_path}")
        self.serial_device = serial.Serial(device_path, 9600)
        self.serial_device.reset_input_buffer()
        self.serial_device.reset_output_buffer()
        
    def read(self):
        ''' blocking.  Reads the next complete line from the serial port device. '''
        encoded_line = self.serial_device.readline()
        # Decode bytes into a string
        try:
            line = encoded_line.decode("utf-8", errors='replace')
        except UnicodeDecodeError as ex:
            print(f'Failed to convert line to utf-8 because {ex}.\n Line: {encoded_line}')
            return ''
        # Remove all whitespace in the line
        line = ''.join(line.split())
        if not line or not line[0].isdigit():
            return ''
        return line
    
    def close(self):
        ''' Close the serial connection '''
        if self.serial_device:
            print("Closing serial connection")
            self.serial_device.close()


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

def loop(wind_sensor, output_folder_path):
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
            line = wind_sensor.read()
            if line:
                print(f"Received: {line}")
                
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
                    filename = 'wind_{}.csv'.format((datetime.now()).strftime('%Y-%m-%d'))
                    dated_filename = os.path.join(output_folder_path, filename)
                    with open(dated_filename, 'a', encoding='utf-8') as f:
                        f.write(f"{timestamp_formatted},{wd_mean},{u_mean},{v_mean}\n")
                else:
                    try:
                        parts = line.split(',')
                        if len(parts) >= 4:
                            u.append(float(parts[1]))
                            wd.append(float(parts[2]))
                            v.append(float(parts[3]))
                    except ValueError as ve:
                        print(f"Value error: {ve}. Line: {line}")
    except KeyboardInterrupt:
        print("Stopped by user.")
    finally:
        wind_sensor.close()

if __name__ == '__main__':
    # Find the available wind sensors
    DEVICES = WindSensor.find_wind_sensors()
    if not DEVICES:
        print("No wind sensors found.")
        exit(1)
    device = DEVICES[0]
    print(f"Using wind sensor device: {device}")
    WINDSENSOR = WindSensor(device)
    OUTPUT_PATH = './sensor_logs'
    try:
        loop(WINDSENSOR, OUTPUT_PATH)
    except KeyboardInterrupt:
        print("Exiting program.")
    finally:
        WINDSENSOR.close()
