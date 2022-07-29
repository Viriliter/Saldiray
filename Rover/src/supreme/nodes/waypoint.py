#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import rospy
from std_msgs.msg import String
from visualization_msgs.msg import Marker
from std_msgs.msg import Header, ColorRGBA, String
import math
import numpy as np
marker_id = -1
lat1 = 39.8714500
lon1 = 32.7504900
marker_mainpoints = np.empty(list((0,)) + [2], dtype=np.float)
def appender2d(arr1,arr2):
    x = np.append(arr1[:,0],[arr2[0]])
    new_arr = np.zeros(list(x.shape) + [2], dtype=np.float)
    new_arr[:,0] = x
    new_arr[:,1] = np.append(arr1[:,1],[arr2[1]])
    return new_arr
def show_marker(x,y,z):
    global marker_id
    marker = Marker()
    marker.header.frame_id = "map"
    marker.type = marker.SPHERE
    marker.action = marker.ADD
    marker.scale.x = 0.05
    marker.scale.y = 0.05
    marker.scale.z = 0.05
    marker.color = ColorRGBA(0.0, 2.0, 0.0, 0.8)
    marker.pose.orientation.w = 1.0
    marker.pose.position.x = x
    marker.pose.position.y = y
    marker.pose.position.z = 0
    marker.id = marker_id+1
    path_pub.publish(marker)
def command_callback(ddd):
    if str(ddd.data)[:3] == "FE03":
        print ddd.data
    global lat1
    global lon1
    global marker_mainpoints
    lat2 = 39.8718500
    lon2 = 32.7508900
    dx = (lon2-lon1)*40000*math.cos((lat1+lat2)*math.pi/360)/360
    dx = dx * 1000
    dy = (lat1-lat2)*40000/360
    dy = dy * 1000
    indx  = np.where(abs(marker_mainpoints - (dx,dy))<0.5)
    finding_arr = marker_mainpoints[indx]
    if len(finding_arr) == 0:
        show_marker(dx,dy,0)
        marker_mainpoints = appender2d(marker_mainpoints, [dx,dy])
def listener():
    rospy.Subscriber('sup/guicommand', String, command_callback)
    rospy.spin()
if __name__ == "__main__"   :
    rospy.init_node('waypoint_test', anonymous=True)
    path_pub = rospy.Publisher('sup/path', Marker,queue_size=100)
    listener()
