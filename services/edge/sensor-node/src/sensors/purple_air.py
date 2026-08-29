'''
File for reading the purple air sensors

'''

# -*- coding: utf-8 -*-
try:
    import serial
except ImportError:
    print("***\nFailed to load module pyserial.  \nPlease install it using \n> apt-get install python3-serial\n***\n")
    exit(-1)

import time
import argparse
import configparser
import os
import csv
import re
from glob import glob
from datetime import datetime

try:
    from .parsers import extract_purple_air_line, is_purple_air_minute_data
    from .transport import LineSource, ReplayLineSource, SerialLineSource
except ImportError:
    from parsers import extract_purple_air_line, is_purple_air_minute_data
    from transport import LineSource, ReplayLineSource, SerialLineSource

class PurpleAir(object):
    def __init__(self, device_name=None, line_source: LineSource | None = None):
        if line_source is None:
            if not device_name:
                raise ValueError("device_name is required when line_source is not supplied")
            device_path = os.path.join('/dev/', device_name)
            print("Opening serial connection to {}".format(device_path))
            line_source = SerialLineSource(device_path, 115200)
        self.line_source = line_source
        self.latitude = 0.0
        self.longitude = 0.0
        self.elevation = 0.0
        # self.key1 = None

    def load_settings(self, latitude, longitude, elevation, config_file_path):
        ''' '''
        self.latitude = latitude
        self.longitude = longitude
        self.elevation = elevation
        if config_file_path:
            config_file = configparser.ConfigParser(default_section='ModuleDefaults', interpolation=None)
            config_file.read(config_file_path)
            section = config_file["Controller"]
            elev = float(section.get('Elevation', None))
            if elev:
                self.elevation = elev

    def read(self):
        ''' blocking.  Reads the next complete line from the serial port device. '''
        encoded_line = self.line_source.readline()
        self.at_eof = encoded_line == b''
        # Decode bytes into a string
        try:
            line = encoded_line.decode("utf-8")
        except UnicodeDecodeError as ex:
            print('Failed to convert line to utf-8 because {}.\n Line: {}'.format(ex, encoded_line))
            return ''
        return extract_purple_air_line(line)

    def close(self):
        if self.line_source:
            print("Closing serial connection")
            self.line_source.close()

    @staticmethod
    def dataline_is_minute_data(dataline):
        ''' Checks if dataline has the right format for a line of minute data '''
        return is_purple_air_minute_data(dataline)

    @staticmethod
    def find_purpleairs():
        ''' Looks for devices that use the same driver as the purple air serial to usb chip. '''
        usb_module_path = '/sys/bus/usb/drivers/ch341/'
        #/sys/bus/usb/drivers/ch341/1-1.4:1.0/ttyUSB0
        wildcard_path = '1-*/tty*'
        paths = glob(usb_module_path + wildcard_path)
        # if we found something, print it out
        if paths:
            print("Found Purple Air devices at paths:")
            for path in paths:
                print("\t" + path)
        devices = [os.path.basename(path) for path in paths]
        # make sure devices are in alphabetical order
        return sorted(devices)


def load_device(args_device, device_list):
    ''' Load either default or specified device '''
    device = args_device
    if not device:
        device = device_list[0]
        print("Defaulting to first device: {}".format(device))
    return PurpleAir(device)

def loop(purpleair, output_folder_path, stop_on_eof=False): #, upload_data):
    ''' loop that reads data from device. '''
    if not os.path.exists(output_folder_path):
        os.makedirs(output_folder_path)
    while True:
        dataline = purpleair.read()
        if stop_on_eof and purpleair.at_eof:
            break
        # now = datetime.now()
        now = int(time.time())
        if PurpleAir.dataline_is_minute_data(dataline):
            print('+ {}, {}'.format((datetime.now()).strftime('%Y-%m-%d'), dataline))
            # write data out to file
            filename = 'purpleair_{}.csv'.format((datetime.now()).strftime('%Y-%m-%d'))
            fullpath = os.path.join(output_folder_path, filename)
            with open(fullpath, "a") as fh:
            # print('trying to push to db')
                fh.write('{},{}\n'.format(now, dataline))
            # database.write_to_db(now, dataline, database_config)
            # print() # add extra space
        # else:
            # print('- ' + dataline)
            # print() # add extra blankspace, makes things easier to read

if __name__ == "__main__":
    PARSER = argparse.ArgumentParser(description='Log readings from a purple air sensor')
    PARSER.add_argument('-d', '--device', action='store', default="", help="Device to read, if not set, defaults to first found device.")
    PARSER.add_argument('-p', '--path', action='store', default='./logs', help="Path for saving the log file.  Defaults to local directory.")
    PARSER.add_argument('-l', '--listonly', action='store_true', default=False, help="List the available Purple Air sensors")
    PARSER.add_argument('-x', '--latitude', action='store', default=38.54, help="Default set to Davis")
    PARSER.add_argument('-y', '--longitude', action='store', default=-121.75, help="Default set to Davis")
    PARSER.add_argument('-e', '--elevation', action='store', default=1.01, help="Default set to Davis")
    PARSER.add_argument('-c', '--configpath', action='store', default=None, help="Not tested")
    PARSER.add_argument('-u', '--uploaddata', action='store_true', default=False, help="Not implemented")
    PARSER.add_argument('--input-file', default=None, help="Replay raw sensor lines from a local file")
    ARGS = PARSER.parse_args()
    # Find the available devices
    DEVICES = [] if ARGS.input_file else PurpleAir.find_purpleairs()
    if not DEVICES and not ARGS.device and not ARGS.input_file:
        print("Failed to find any Purple Air devices.")
        exit()
    # if only going to list the devices, exit here
    if ARGS.listonly:
        print("Devices: {}".format(DEVICES))
        exit()
    line_source = ReplayLineSource(ARGS.input_file) if ARGS.input_file else None
    if line_source:
        print("Replaying Purple Air input from {}".format(ARGS.input_file))
        PURPLEAIR = PurpleAir(line_source=line_source)
    else:
        PURPLEAIR = load_device(ARGS.device, DEVICES)
    PURPLEAIR.load_settings(ARGS.latitude, ARGS.longitude, ARGS.elevation, ARGS.configpath)
    try:
        loop(PURPLEAIR, ARGS.path, stop_on_eof=bool(ARGS.input_file)) #, ARGS.uploaddata)
    except KeyboardInterrupt:
        print("Program killed")
    finally:
        PURPLEAIR.close()
