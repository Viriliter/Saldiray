#!/usr/bin/env python
# Software License Agreement (BSD License)
#

import sys
import rospy
from std_msgs.msg import String
from std_msgs.msg import Int16MultiArray
import matplotlib.pyplot as plt
import cv2
import numpy as np
import roslib
from cv_bridge import CvBridge, CvBridgeError
from geometry_msgs.msg import Twist
from sensor_msgs.msg import PointCloud2

def depth_callback(data):
	print(data)
	#print(cv_image)   
def listener():
    
    rospy.init_node('Sweep1_2', anonymous=True)
    rospy.Subscriber("pc2",PointCloud2, depth_callback)
    
    
    #rospy.Subscriber("/zed/left/image_rect_color",Image,camera_left_callback)
    #ax.relim()
    #ax.autoscale_view()
    
    #
    #fig.canvas.flush_events()
    rospy.spin()
   
    # spin() simply keeps python from exiting until this node is stopped
    

if __name__ == '__main__':
    listener()
