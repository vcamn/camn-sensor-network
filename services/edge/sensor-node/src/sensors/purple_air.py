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

class PurpleAir(object):
    def __init__(self, device_name):
        device_path = os.path.join('/dev/', device_name)
        print("Opening serial connection to {}".format(device_path))
        self.serial_device = serial.Serial(device_path, 115200)
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
        encoded_line = self.serial_device.readline()
        # Decode bytes into a string
        try:
            line = encoded_line.decode("utf-8")
        except UnicodeDecodeError as ex:
            print('Failed to convert line to utf-8 because {}.\n Line: {}'.format(ex, encoded_line))
            return ''
        # break the line into pieces
        parts = line.split('\r')
        # many lines have an extra '\n' as the last item because they are ended with \r\n so strip it
        if parts[-1] == '\n':
            parts = parts[:-1]
        # since we are splitting on '\r' (reset cursor), we only need to keep the last text section
        if parts:
            return parts[-1]
        # else:
        return ''

    def close(self):
        if self.serial_device:
            print("Closing serial connection")
            self.serial_device.close()

    @staticmethod
    def dataline_is_minute_data(dataline):
        ''' Checks if dataline has the right format for a line of minute data '''
        reader = csv.reader([dataline])
        parsed_line = reader.__next__()
        if not len(parsed_line) > 35:
            # print(len(parsed_line))
            return False
        # for a spot check, check that the second column is a (possibly badly formatted) MAC address
        #  note: the device ID is the MAC address but missing the 0's
        mac_address = parsed_line[1]
        if re.match(r'([0-9a-fA-F]?[0-9a-fA-F]?:){5}([0-9a-fA-F]?[0-9a-fA-F]?)', mac_address):
            return True
        return False

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

def loop(purpleair, output_folder_path): #, upload_data):
    ''' loop that reads data from device. '''
    if not os.path.exists(output_folder_path):
        os.makedirs(output_folder_path)
    while True:
        dataline = purpleair.read()
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
    ARGS = PARSER.parse_args()
    # Find the available devices
    DEVICES = PurpleAir.find_purpleairs()
    if not DEVICES and not ARGS.device:
        print("Failed to find any Purple Air devices.")
        exit()
    # if only going to list the devices, exit here
    if ARGS.listonly:
        print("Devices: {}".format(DEVICES))
        exit()
    PURPLEAIR = load_device(ARGS.device, DEVICES)
    PURPLEAIR.load_settings(ARGS.latitude, ARGS.longitude, ARGS.elevation, ARGS.configpath)
    try:
        loop(PURPLEAIR, ARGS.path) #, ARGS.uploaddata)
    except KeyboardInterrupt:
        print("Program killed")
    finally:
        PURPLEAIR.close()
