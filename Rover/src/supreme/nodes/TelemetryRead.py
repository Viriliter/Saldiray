#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import serial
import time
import rospy
from std_msgs.msg import String
import sys
from serial.tools import list_ports
last_lat = 0.0
last_lon = 0.0
def read_serial():
    global last_lat
    global last_lon
    #send_output = Float32MultiArray()
    
    while 1:
        r_byte = ser.readline()
        #print r_byte.encode('hex')
       # send_output.data = []
        if r_byte.encode('hex')[:2] == 'fe':
            if r_byte.encode('hex')[2:4] == '00':
                print r_byte.encode('hex')
                vehicle_state = bin(int(r_byte.encode('hex')[4:6], 16))[2:]

                left_x = int(r_byte.encode('hex')[6:8], 16)
     
                left_y = int(r_byte.encode('hex')[8:10], 16)
            
                right_x = int(r_byte.encode('hex')[10:12], 16)
        
                right_y = int(r_byte.encode('hex')[12:14], 16)
          
                other_state = bin(int(r_byte.encode('hex')[14:16], 16))[2:]
                output = str(vehicle_state).zfill(8)+","+str(left_x)+","+str(left_y)+","+str(right_x)+","+str(right_y)+","+str(other_state).zfill(8)
                #pub.publish(output)
                print output
                vehicle_com = ""
            if r_byte.encode('hex')[2:4] == '01':
                weapon_com = ""
            if r_byte.encode('hex')[2:4] == '02':
                drive_com =""
            if r_byte.encode('hex')[2:4] == '03':
                latitude = ""
                longitude = ""
            #print lat_int
            #print lon_int
        #time.sleep(0.02) 



if __name__=="__main__":
    VID = int("10c4", 16) #6790
    PID = int("ea60", 16) #29987
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
    pub = rospy.Publisher('sup/guicommand', String ,queue_size=10) #
    rospy.init_node('telemetry1', anonymous=True)
    rate = rospy.Rate(10)
    print("waiting...")
    time.sleep(0.5)
    print ("begin...")
    read_serial()
    ser.close() # Only executes once the loop exits
