'''
File for reading the AggieAir

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
# from time import sleep
import os
import serial

class AggieAir(object):
    ''' Class for reading wind sensor data from a serial port. '''
    def __init__(self, device_name):
        device_path = os.path.join('/dev/', device_name)
        print(f"Opening serial connection to {device_path}")
        self.serial_device = serial.Serial(device_path, 115200, timeout=10)
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
        if not line or not line[0].isdigit():
            return ''
        return line
    
    def close(self):
        ''' Close the serial connection '''
        if self.serial_device:
            print("Closing serial connection")
            self.serial_device.close()


    @staticmethod
    def find_aggieair_sensors():
        ''' Looks for devices that use the same driver as the wind sensor serial to usb chip. '''
        usb_module_path = '/sys/bus/usb/drivers/cdc_acm/'
        #/sys/bus/usb/drivers/cdc_acm/1-1.2:1.1/tty/ttyACM0
        wildcard_path = '1-*/tty/tty*'
        paths = glob(usb_module_path + wildcard_path)
        # if we found something, print it out
        if paths:
            print("Found AggieAir devices at paths:")
            for path in paths:
                print("\t" + path)
        devices = [os.path.basename(path) for path in paths]
        # make sure devices are in alphabetical order
        return sorted(devices)

def loop(aggieair_sensor, output_folder_path):
    ''' Main loop for reading data from the wind sensor and writing to a file. '''
    if not os.path.exists(output_folder_path):
        os.makedirs(output_folder_path)
    try:
        while True:
            # Read line: deviceId, u, wd, v
            line = aggieair_sensor.read()
            if line:
                print(f"Received: {line}")
                timestamp = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
                filename = 'voc_{}.csv'.format((datetime.now()).strftime('%Y-%m-%d'))
                dated_filename = os.path.join(output_folder_path, filename)
                with open(dated_filename, 'a', encoding='utf-8') as f:
                    # Record = RPi timestamp, deviceID, AggieAir timestamp, WE, AE, VOCppb
                    f.write(f"{timestamp},{line}")
    except KeyboardInterrupt:
        print("Stopped by user.")
    finally:
        aggieair_sensor.close()

if __name__ == '__main__':
    # Find the available AggieAir sensors
    DEVICES = AggieAir.find_aggieair_sensors()
    if not DEVICES:
        print("No AggieAir sensors found.")
        exit(1)
    device = DEVICES[0]
    print(f"Using AggieAir device: {device}")
    VOCSENSOR = AggieAir(device)
    OUTPUT_PATH = './sensor_logs'
    try:
        loop(VOCSENSOR, OUTPUT_PATH)
    except KeyboardInterrupt:
        print("Exiting program.")
    finally:
        VOCSENSOR.close()
