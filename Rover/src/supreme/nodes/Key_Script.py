#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#

from getkey import getkey, keys
import sys
import rospy
from std_msgs.msg import String
import sys, select, termios, tty
from sys import stdin                                       
if __name__=="__main__":
    settings = termios.tcgetattr(sys.stdin)
    pub = rospy.Publisher('key/zoom', String ,queue_size=10) #
    rospy.init_node('key1', anonymous=True)
    pub2 = rospy.Publisher('key/arac', String,queue_size=100) #
    pub3 = rospy.Publisher('key/rccar', String,queue_size=100) #
    rate = rospy.Rate(10)
    try:
        print("basladi")
        while(1):
            key = getkey()
            if (key[0] == "t"):
                print("upp")
                pub3.publish(key[0])
            if (key[0] == "g"):
                print("stop")
                pub3.publish(key[0])
            if (key[0] == "1"):
                print("ON")
                pub2.publish(key[0])
            if (key[0] == "2"):
                print("Spark-plug heating")
                pub2.publish(key[0])   
            if (key[0] == "3"):
                print("Vehicle Start")
                pub2.publish(key[0])  
            if (key[0] == "4"):
                print("Vehicle Act")
                pub2.publish(key[0]) 
            if (key[0] == "5"):
                print("Vehicle pass")
                pub2.publish(key[0])
            if (key[0] == "6"):
                print("motion stop")
                pub2.publish(key[0])      
            if (key[0] == "+"):
                print("engine +")
                pub2.publish(key[0])
            if (key[0] == "-"):
                print("engine -")
                pub2.publish(key[0])
            if (key[0] == "w"):
                print("go forward")
                pub2.publish(key[0])
            if (key[0] == "s"):
                print("go backward")
                pub2.publish(key[0])
            if (key[0] == "d"):
                print("turn right")
                pub2.publish(key[0])
            if (key[0] == "a"):
                print("turn left")
                pub2.publish(key[0])
            if (key[0] == "0"):
                print("Vehicle OFF")
                pub2.publish(key[0])

            if (key[0] == "x"):
                print("delete")
                pub.publish("del")
            if (key[0] == "["):
                print("taken waypoint : " + str(key))
                pub.publish(key[0:-1])
            if (key[0] == "u"):
                    break

    except Exception as e:
        print(e)
