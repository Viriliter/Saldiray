#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import serial
import time
from serial.tools import list_ports
import teleParser
import rospy
from std_msgs.msg import String
class Teleoperation(object):
    
    def __init__(self,portName="/dev/ttyUSB2",baudrate=57600,bytesize=8,parity='N',stopbits=1,timeout=None,write_timeout=None):
        """
        Initializes class
        """
        self.msg = None
        self.portName = self.findtelemetryport()
        self.baudrate = baudrate
        self.bytesize = bytesize
        self.parity = parity
        self.stopbits = stopbits
        self.timeout = timeout
        self.write_timeout = write_timeout
        self.isDataSended = True
        self.serialCom = serial.Serial(self.portName,self.baudrate,self.bytesize,self.parity,self.stopbits,self.timeout,write_timeout=self.write_timeout)
        self.parser = teleParser.teleParser()

        #Telemetry Device Properties
        self.VID = int("1a86", 16) #6790
        self.PID = int("7523", 16) #29987


        
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
        Teleoperation.openPort(self)
        return True
        
    def detectPort(self):
        '''
        Detects port name where GPS module is connected. 
        '''
        device_list = list_ports.comports()
        for device in device_list:
       
            if (device.vid != None or device.pid != None):
                if (device.vid == self.VID and
                    device.pid == self.PID):
                    #Get port name of the device
                    port = device.device
                    #Set port name
                    self.setPortName(self,port)
                    break
                port = None

    def listenPort(self):
        '''
        Reads messages lin by line and parses it
        '''
        message = self.serialCom.readline()
        '''
        b_message = bytearray()
        b_message.extend(message)
        for item in b_message:
            print item
        '''
        #print message.encode('hex')
        output = self.parser.messageParser(message)
        
        if len(output)>0 :
            print output
            #pub.publish(output)
            
        else:
            pub.publish("S")
    def findtelemetryport(self):
        VID = int("10c4", 16)
        PID = int("ea60", 16)
        device_list = list_ports.comports()
        for device in device_list:
            if (device.vid != None or device.pid != None):
                if (device.vid == VID and
                    device.pid == PID):
                    port = device.device
                    break
                port = None
        return port  
                  
        #print self.parser.getVehicleState()
tx_message = ""
def shutdown_fornox():
    rospy.signal_shutdown("ss")
    pub.publish("S")
    #print "stop"  
def telemetry_tx_callback(data):
    global tx_message
    tx_message = data.data
if __name__ == "__main__"   :
    global tx_message
    #Create UbloxGPS object
    teleoperator = Teleoperation()
    #Detect GNSS device
    teleoperator.detectPort()
    #Open serial port
    teleoperator.openPort()
    #Update serial port
    isOpened = teleoperator.restartPort()

    print "listening port:"
    print teleoperator.getPortName()
    pub = rospy.Publisher('sup/guicommand', String, queue_size=100)
    rospy.init_node('TelemtryOutput', anonymous=True,disable_signals=True)
    rospy.Subscriber('sup/telemetry/TX', String, telemetry_tx_callback)
    rospy.on_shutdown(shutdown_fornox)
    rate = rospy.Rate(10)
    while(isOpened):
        #Listen the port in every 300ms
        teleoperator.listenPort()
        if len(tx_message)>1:
            teleoperator.writemessage(tx_message)  ## writemessage komutu ekle
            tx_message = ""
        time.sleep(0.01)

