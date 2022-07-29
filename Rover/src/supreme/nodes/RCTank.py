#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import sys
import serial
import time
import rospy
from std_msgs.msg import String
from std_msgs.msg import Float64,Int64MultiArray
import sys
from serial.tools import list_ports
import struct
import threading
s1 = "00"
s2 = "00"
state = 0
def DecToHex(decvalue):
    out = hex(decvalue).split('x')[-1]
    if len(out) == 1:
        out = "0" + out
    return out.upper()
def callback(data):
    global s1
    global s2
    global state
    print str(data.data[1]) + str("***") + str(data.data[0])
    res = "00" + DecToHex(data.data[1]) + DecToHex(data.data[0]) + "FF"
    stopcode = "00" + DecToHex(127) + DecToHex(127) + "FF"
    ser.write(res.decode('hex'))
    time.sleep(0.01)
    
def listener():
    rospy.Subscriber("/sup/tank", Int64MultiArray, callback)
    rospy.spin()
def shutdown_tank():
    stopcode = "00" + DecToHex(127) + DecToHex(127) + "FF"
    ser.write(stopcode.decode('hex'))
    time.sleep(0.01)
    ser.close()
if __name__=="__main__":
    time.sleep(6.5)
    VID = int("1a86", 16) #6790
    PID = int("7523", 16) #29987
    #print(VID)
    device_list = list_ports.comports()
    for device in device_list:
       
        if (device.vid != None or device.pid != None):
            if (device.vid == VID and
                device.pid == PID):
                port = device.device
                break
            port = None
    print(port)
    ser = serial.Serial("/dev/ttyUSB0", 9600,write_timeout=1)
    pub = rospy.Publisher('sup/tankmessage', String ,queue_size=100) #
    rospy.init_node('tank', anonymous=True)
    rate = rospy.Rate(10)
    rospy.on_shutdown(shutdown_tank)
    print("waiting...")
    
    print ("begin...")

    stop = False

    
    listener()
    
    stop = True
    
    
    ser.close() # Only executes once the loop exits
