#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import serial
import time
import serial.tools.list_ports
import nmeaParser
import rospy
from std_msgs.msg import String
import sys, select, termios, tty
from sys import stdin  
class UbloxGPS(object):
    
    def __init__(self,portName="/dev/ttyUSB1",baudrate=9600,bytesize=8,parity='N',stopbits=1,timeout=None,write_timeout=None):
        """
        Initializes class
        """
        self.msg = None
        self.portName = portName
        self.baudrate = baudrate
        self.bytesize = bytesize
        self.parity = parity
        self.stopbits = stopbits
        self.timeout = timeout
        self.write_timeout = write_timeout
        self.isDataSended = True
        self.serialCom = serial.Serial(self.portName,self.baudrate,self.bytesize,self.parity,self.stopbits,self.timeout,write_timeout=self.write_timeout)
        self.parser = nmeaParser.nmeaParser()
        
        
    #Accessors
    #region
    def getPortName(self):
        return self.portName

    def getBaudrate(self):
        return self.baudrate

    def getByteSize(self):
        return self.bytesize

    def getParity(self):
        return self.parity

    def getStopBits(self):
        return self.stopbits

    def getTimeout(self):
        return self.timeout

    def getWTimeout(self):
        return self.write_timeout

    #endregion

    #Mutators
    #region
        
    def setPortName(self,portName):
        self.portName = portName

    def setBaudrate(self,baudrate):
        self.baudrate = baudrate

    def setByteSize(self,bytesize):
        self.bytesize = bytesize

    def setParity(self,parity):
        self.parity = parity

    def setStopBits(self,stopbits):
        self.stopbits = stopbits

    def setTimeout(self,timeout):
        self.timeout = timeout

    def setWTimeout(self,write_timeout):
        self.write_timeout = write_timeout
    
    #endregion


    def openPort(self):
        '''
        Opens serial port
        '''
        if(self.serialCom.isOpen()==False):
            self.serialCom.open()
            return True
        return False
        

    def closePort(self):
        '''
        Closes serial port
        '''
        if(self.serialCom.isOpen()):
            self.serialCom.close()
            return True
        return False

    def restartPort(self):
        '''
        Closes, redefines and opens serial port again
        '''
        self.serialCom.close()
        self.serialCom = serial.Serial(self.portName,self.baudrate,self.bytesize,self.parity,self.stopbits,self.timeout,write_timeout=self.write_timeout)
        UbloxGPS.openPort(self)
        return True



    def listenPort(self):
        '''
        Reads messages line by line and parses it
        '''
        message = self.serialCom.readline()
        #Parse message
        self.parser.NMEAparser(message)
        #Print location
        print(self.parser.getLocation())
        #print "lat :" + str(self.parser.getLocation()[0])
        #print "long:" + str(self.parser.getLocation()[1])
        #Print GPS status
        print(self.parser.getStatus())
        #Print Time and Date
        print (self.parser.getTime())
        #Print Altitude
        print (self.parser.getAltitude())
        #Get Speed
        print (self.parser.getSpeed())
        #Get Orientation
        print (self.parser.getOrientation())
        return str(self.parser.getLocation()[0]),str(self.parser.getLocation()[1]),str(self.parser.getAltitude()),str(self.parser.getSpeed())

class GNSSDeviceManager(object):
    def __init__(self):
        self.USBManufName = "Prolific Technology Inc."
        
    def detectPort(self):
        '''
        Detects port name where GPS module is connected. 
        '''
        ports = list(serial.tools.list_ports.comports())
        for p in ports:
            if(p.manufacturer==self.USBManufName):
                return p.device
def DecToHex(decvalue):
    out = hex(decvalue).split('x')[-1]
    if len(out) == 1:
        out = "0" + out
    return out.upper()    
def createmsg(lat,lng,alt,spd):
    #39.9250180
    #lat = 39.92501800
    #lng = 032.8369560
    #alt = 1100.33
    new_lat = str(format(round(lat,7),'.7f')).zfill(11)
    new_lat = new_lat.replace(".","")
    last_lat = str(DecToHex(int(new_lat[0:2])))+ str(DecToHex(int(new_lat[2:4]))) + str(DecToHex(int(new_lat[4:6]))) + \
        str(DecToHex(int(new_lat[6:8]))) + str(DecToHex(int(new_lat[8:10])))

    new_long = str(format(round(lng,7),'.7f')).zfill(11)
    new_long = new_long.replace(".","")
    last_long = str(DecToHex(int(new_long[0:2])))+ str(DecToHex(int(new_long[2:4]))) + str(DecToHex(int(new_long[4:6]))) + \
        str(DecToHex(int(new_long[6:8]))) + str(DecToHex(int(new_long[8:10])))
    
    new_alt = str(format(round(alt,5),'.7f')).zfill(7)
    new_alt = new_alt.replace(".","")
    last_alt = str(DecToHex(int(new_alt[0:2])))+ str(DecToHex(int(new_alt[2:4]))) + str(DecToHex(int(new_alt[4:6])))
    

    new_spd = str(format(round(spd,2),'.2f')).zfill(5)
    new_spd = new_spd.replace(".","")
    last_spd = str(DecToHex(int(new_spd[0:2])))+ str(DecToHex(int(new_spd[2:4])))
    pub.publish("FE03" + last_lat + last_long + last_alt + last_spd + "0000")
    print "FE03" + last_lat + last_long + last_alt + last_spd + "0000"

    #print "speed :::" + str (str(format(round(spd,2),'.2f')).zfill(5))
if __name__ == "__main__"   :
    settings = termios.tcgetattr(sys.stdin)
    pub = rospy.Publisher('sup/telemetry/TX', String ,queue_size=10) #
    rospy.init_node('gpsnode2', anonymous=True)
    rate = rospy.Rate(10)
    #Detect GNSS device
    dm = GNSSDeviceManager()
    portName = dm.detectPort()
    
    #Create UbloxGPS object
    gps = UbloxGPS(portName)
    #Open serial port
    gps.openPort()
    #Update serial port
    isOpened = gps.restartPort()

    print "listening port:"
    print gps.getPortName()

    while(isOpened):
        #Listen the port in every 10ms
        values = gps.listenPort()
        try:
            createmsg(float(values[0]),float(values[1]),float(values[2]),float(values[3]))
        except:
            pass
        time.sleep(0.01)
