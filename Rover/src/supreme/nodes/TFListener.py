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
import rospy
from tf2_msgs.msg import TFMessage
from sensor_msgs.msg import PointCloud2, PointField
import math
from numpy import *
from math import sqrt
import numpy as np
import sensor_msgs.point_cloud2 as pc2
from nav_msgs.msg import Odometry
from tf.transformations import euler_from_quaternion, quaternion_from_euler
last_points = np.empty(list((0,)) + [3], dtype=np.float)

def odometryCb(msg):
    print msg.pose.pose
    quaternion = (
        msg.pose.pose.orientation.x,
        msg.pose.pose.orientation.y,
        msg.pose.pose.orientation.z,
        msg.pose.pose.orientation.w)
    euler = euler_from_quaternion(quaternion)
    roll = euler[0]
    pitch = euler[1]
    yaw = euler[2]
    print "yaw : ", math.degrees(yaw)
    print "pitch : ", math.degrees(pitch)
    print "roll : ", math.degrees(roll)
if __name__ == "__main__"   :
    rospy.init_node('nodeodom', anonymous=True) #make node 
    rospy.Subscriber('/integrated_to_init',Odometry,odometryCb)
    rospy.spin()





