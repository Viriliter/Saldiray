#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import serial
import time
import rospy
from std_msgs.msg import Float64MultiArray,String
import sys
from serial.tools import list_ports
import math
def DecToHex(decvalue):
    out = hex(decvalue).split('x')[-1]
    if len(out) == 1:
        out = "0" + out
    return out.upper()
def createmsg(value):
    #39.9250180
    #lat = 39.92501800
    #lng = 032.8369560
    #alt = 1100.33
    value = value + 360.0
    new_value = str(format(round(value,3),'.3f')).zfill(7)
    #print "new :" + str(new_value)
    new_value = new_value.replace(".","")
    if len(new_value)<>6:
        new_value = "000000"
    last_value = str(DecToHex(int(new_value[0:2])))+ str(DecToHex(int(new_value[2:4])))+ str(DecToHex(int(new_value[4:6])))
    return last_value
def read_serial():
    while 1:
        try:
            text = ""
            r_byte = ser.readline()
            data1 = r_byte.split('=')[1]
            data2 = data1.split(',')
            output_float = Float64MultiArray()
            for i in range (0,len(data2)):
                text = text + str ( createmsg(float(data2[i])) )
                output_float.data.insert(i,float(data2[i])+360.0)
            print output_float.data
            GUIoutput = "FE02" + text + "6F6F"
            print GUIoutput
            pub2.publish(GUIoutput)
            pub.publish(output_float)
        except:
            pass
if __name__=="__main__":
    VID = int("1b4f", 16) #6790
    PID = int("9d0f", 16) #29987
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
    ser = serial.Serial(port, 9600)
    pub = rospy.Publisher('sup/imudata', Float64MultiArray ,queue_size=10) #
    pub2 = rospy.Publisher('sup/telemetry/TX', String ,queue_size=10) #
    rospy.Subscriber("/integrated_to_init",Odometry, odom_calldata)
    rospy.init_node('imu_sup', anonymous=True)
    rate = rospy.Rate(10)
    print("waiting...")
    time.sleep(0.5)
    print ("begin...")
    read_serial()
    ser.close() # Only executes once the loop exits
    print "closed"