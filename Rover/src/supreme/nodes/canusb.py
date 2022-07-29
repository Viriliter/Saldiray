#!/usr/bin/env python
# -*- coding: utf-8 -*-

'''
Created on 2015/01/30

@author: spiralray
'''

import sys
import yaml
import roslib
roslib.load_manifest("canusb");

import rospy
from _CAN import CAN

import serial
import struct
import time
import threading
from serial.tools import list_ports
class CanUSB(serial.Serial):
    """
    Wrapper for Serial
    """
    try:
        import io
    except ImportError:
        # serial.Serial inherits serial.FileLike
        pass
    else:
        
        def readline(self):
            """
            Overrides io.RawIOBase.readline which cannot handle with '\r' delimiters
            """
            ret = ''
            while True:
                c = self.read(1)
                if c == '':
                    return ret
                elif c == '\r':
                    if ret == 'z' or ret == 'Z':
                        self.status = 0
                    return ret + c
                elif c == chr(7):
                    self.status = -1
                    return c
                else:
                    ret += c
                
            	
                    
        def init(self):
            self.write("\r\r")
            while 1:
                line = self.readline()
                if line == "":
                    break
            self.write("C\r")   # Close port
            ret = ord( self.read() )
            self.status = 0
        
        def version(self):  #Check version
            can.write("V\r")
            return can.readline()
        
        def serial(self):
            can.write("N\r")
            return can.readline()
        
        def setTimestamp(self, enable=0):
            if enable == True:
                can.write("Z1\r")
            else:
                can.write("Z0\r")
            if can.read() == chr(13):
                return 0
            else:
                return -1
        
        def setBaud(self, baud):
            can.write(baud+"\r")
            if can.read() == chr(13):
                return 0
            else:
                return -1
            
        def start(self):
            can.write("O\r")   # Open port
            if can.read() == chr(13):
                return 0
            else:
                return -1
        
        def close(self):
            #Close port
            self.write("C\r")   # Close port
            if can.read() == chr(13):
                print "Close port: Success"
            else:
                print "Close port: Fail"
            serial.Serial.close(self)   #Call method of super class
            
        def send(self, msg):
            if self.status == 1:
                return 1
            self.d = ''
            for i in range(0, len(msg.data)):
                self.d += "{0:02X}".format(ord(msg.data[i]))
            self.status = 1
            
            if msg.extId < 0: #Standard format
                self.write( "t{0:03X}{1:d}{2:s}\r".format( msg.stdId, len(msg.data), self.d) )
                #print "t{0:03X}{1:d}{2:s}\r".format( msg.stdId, len(msg.data), self.d)
            else:   #Extended format
                self.write( "T{0:08X}{1:d}{2:s}\r".format( msg.stdId + (msg.extId*0x18FE1234), len(msg.data), self.d) )
                #print "T{0:08X}{1:d}{2:s}\r".format( msg.stdId + (msg.extId*0x400), len(msg.data), self.d)
            return 0
                   
        def analyze(self, str):
            command = str[:1]
            if command == 't':
                self.msg = CAN()
                self.dlc = int(str[4:5],16)
                self.msg.timestamp = rospy.get_rostime()
                self.msg.stdId = int(str[1:4],16)
                self.msg.extId = -1
                for i in range(0, self.dlc):
                    self.msg.data +=  struct.pack('B', int(str[5+i*2:7+i*2],16))
                return self.msg
            elif command == 'T':
                self.msg = CAN()
                self.dlc = int(str[9:10],16)
                self.msg.timestamp = rospy.get_rostime()
                self.msg.stdId = int(str[1:9],16)%0x400
                self.msg.extId = int(str[1:9],16)/0x400
                for i in range(0, self.dlc):
                    self.msg.data +=  struct.pack('B', int(str[10+i*2:12+i*2],16))
                return self.msg
            else:
                return 0
            
def talker():
    pub = rospy.Publisher('canrx', CAN, queue_size=100)
    
    while stop == False:
        
        
        line = can.readline()
        #print line
        #if line == "z\r" or line == "Z\r":
        #print "Transmit Success"
        if line == chr(7):
            print "Transmit Failed"
        elif line != "" and line != "z\r" and line != "Z\r":
            #print line
            #print can.analyze(line)
            try:
                msg = can.analyze(line)
                pub.publish( msg )
                #print msg
            except:
                rospy.logerr('Invalid command %s', line )

def callback(msg):
    while can.status == 1:
        time.sleep (0.0001);
    res = can.send(msg)
    if res  == 1:
        rospy.logerr( "Device busy")
    elif res != 0:
        rospy.logerr( "Transmit Failed")
    
def listener():
    rospy.Subscriber("cantx", CAN, callback)
    rospy.spin()
def findcanport():
    VID = int("0403", 16)
    PID = int("6001", 16)
    device_list = list_ports.comports()
    for device in device_list:
        if (device.vid != None or device.pid != None):
            if (device.vid == VID and
                device.pid == PID):
                port = device.device
                break
            port = None
    return port          
if __name__ == '__main__':
    argv = rospy.myargv(sys.argv)
    rospy.init_node('canusb')
    

        #port = rospy.get_param('~port')
    port = findcanport()
	#port = '/dev/ttyUSB0'
    print port
    time.sleep(2)

    
    can = CanUSB(port, 2000000, timeout=0.1)
    
    can.init()
    print "Version: " + can.version()
    print "Serial Number: " + can.serial()
    
    #Disable timestamp
    if can.setTimestamp(False) != 0:
        rospy.logerr( "Disable timestamp: Failed!" )
    
    #Set baud rate
    try:
        baud = rospy.get_param('~baud')
        rospy.loginfo('Parameter %s has value %s', rospy.resolve_name('~baud'), baud)
    except:
        baud = "250k"
        rospy.loginfo("%s not defined.", rospy.resolve_name('~port'))
        rospy.loginfo("Default baudrate: 250kbps")
    
    if baud == "10k":
        res = can.setBaud("S0")
    elif baud == "20k":
        res = can.setBaud("S1")
    elif baud == "50k":
        res = can.setBaud("S2")
    elif baud == "100k":
        res = can.setBaud("S3")
    elif baud == "125k":
        res = can.setBaud("S4")
    elif baud == "250k":
        res = can.setBaud("S5")
    elif baud == "500k":
        res = can.setBaud("S6")
    elif baud == "800k":
        res = can.setBaud("S7")
    elif baud == "1m":
        res = can.setBaud("S8")
    else:
        rospy.logerr( "Set correct baudrate. See README")
        can.close()
        exit()
    
    if res != 0:
        rospy.logerr( "Disable baudrate: Failed!")
        can.close()
        exit()
    
    #Open port
    if can.start() != 0:
        rospy.logerr( "Open port: Failed" )
        exit()
    
    stop = False
    t = threading.Thread(name='talker', target=talker)
    t.start()
    
    listener()
    
    stop = True
    t.join()
    can.close()
