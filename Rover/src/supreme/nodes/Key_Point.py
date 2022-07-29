#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#


import sys
import rospy
from std_msgs.msg import String
import sys, select, termios, tty
from sys import stdin                                       
def getKey():
    tty.setraw(sys.stdin.fileno())
    select.select([sys.stdin], [], [], 0)
    key = sys.stdin.read(1)
    termios.tcsetattr(sys.stdin, termios.TCSADRAIN, settings)
    return key

if __name__=="__main__":
    settings = termios.tcgetattr(sys.stdin)
    pub = rospy.Publisher('key/zoom', String ,queue_size=10) #
    rospy.init_node('key1', anonymous=True)
    rate = rospy.Rate(10)
    try:
        print("basladi")
        while(1):
            key = stdin.readline()
            
            if (key[0] == "+"):
                print("zoom out")
                pub.publish(key[0])
            if (key[0] == "-"):
                print("zoom in")
                pub.publish(key[0])
            if (key[0] == "w"):
                print("y axis goes up")
                pub.publish(key[0])
            if (key[0] == "s"):
                print("y axis goes down")
                pub.publish(key[0])
            if (key[0] == "d"):
                print("x axis goes right")
                pub.publish(key[0])
            if (key[0] == "a"):
                print("x axis goes left")
                pub.publish(key[0])
            if (key[0] == "["):
                print("taken waypoint : " + str(key))
                pub.publish(key[0:-1])
            if (key[0] == "x"):
                print("deleted")
                pub.publish("del")
            if (key[0] == "e"):
                    break

    except Exception as e:
        print(e)
