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
from tf.transformations import euler_from_quaternion, quaternion_from_euler
from nav_msgs.msg import Odometry

class Teleoperation(object):
    
    def __init__(self,portName="/dev/ttyUSB2",baudrate=57600,bytesize=8,parity='N',stopbits=1,timeout=1,write_timeout=1):
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
        self.serialCom = None
        self.parser = teleParser.teleParser()
    
    

        #Telemetry Device Properties
       # self.VID = int("1a86", 16) #6790
        #self.PID = int("7523", 16) #29987
        
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
        if(self.serialCom == None):
            print "serial opening..."
            self.serialCom = serial.Serial(self.portName,self.baudrate,self.bytesize,self.parity,self.stopbits,self.timeout,write_timeout=self.write_timeout)
            self.serialCom.close()
        if(self.serialCom.isOpen()==False):
            self.serialCom.open()
            return True
        else:
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
        
    def writeMessage(self,message):
        '''
        Writes messages to the serial port
        '''
        if len(message)>1:
            self.serialCom.write(message.decode('hex'))#+ '\r\n')
            print ">>>>>>>" + str (message)

    def listenPort(self):
        '''
        Reads messages line by line and parses it
        '''
        message = self.serialCom.read_until('oo')
        '''
        b_message = bytearray()
        b_message.extend(message)
        for item in b_message:
            print item
        '''
        output = self.parser.messageParser(message)
        
        if len(output)>0 :
            print output
            pub.publish(output)
        #else:
            #pub.publish("S")

    def findtelemetryport(self):
        '''
        Detects port name where telemetry device is connected. (OLD VERSION)
        '''
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

    def detectPort(self):
        '''
        Detects port name where telemetry device is connected. 
        '''
        device_list = list_ports.comports()
        for device in device_list:
            if (device.device != None):
                if (device.product == "CP2102 USB to UART Bridge Controller"):
                    #Get port name of the device
                    port = device.device
                    print "Telemetry is found"
                    #Set port name
                    self.setPortName(port)
                    print "Port is setted"
                    break
                port = None
                  
def shutdown_fornox(self):
    '''
    Sends shutdown signal to the ROS in the case of interruption in communication
    '''
    rospy.signal_shutdown("ss")
    pub.publish("S")
    #print "stop"  
tx_message = ""

def telemetry_tx_callback(data):
    global tx_message
    tx_message = str(data.data)
    #d = tx_message.split(',')
    #print tx_message
if __name__ == "__main__"   :
    global tx_message
    #Create Telemetry object
    teleoperator = Teleoperation()
    #Detect Telemetry device
    teleoperator.detectPort()
    #Open serial port
    isOpened = teleoperator.openPort()

    print "listening port:"
    print teleoperator.getPortName()
    #ROS Settings
    pub = rospy.Publisher('sup/guicommand', String, queue_size=100)
    rospy.init_node('TelemetryOutput', anonymous=True,disable_signals=True)
    rospy.Subscriber('sup/telemetry/TX', String, telemetry_tx_callback)
    rospy.on_shutdown(shutdown_fornox)
    #Define update frequency of the node
    rate = rospy.Rate(10)

    #Main telemetry loop
    while(isOpened):
        #Listen the port
        teleoperator.listenPort()
        #Writes the message to the port
        teleoperator.writeMessage(tx_message)
        #Reset last message
        tx_message = ""
        #Thread sleep in every 10ms
        time.sleep(0.01)