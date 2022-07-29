#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#

import math
import sys
import time
import threading
import ctypes
import ctypes.util
import collections
from sweeppy import Sweep
import itertools
import sys
import rospy
from std_msgs.msg import MultiArrayDimension
from std_msgs.msg import Int32MultiArray
from std_msgs.msg import String
import matplotlib.pyplot as plt
import cv2
import numpy as np
import time
def talker():
    pub = rospy.Publisher('sweep/coordinat_array', Int32MultiArray,queue_size=1000) #
    rospy.init_node('Sweep1', anonymous=True)
    rate = rospy.Rate(1000)
    dev = '/dev/sweep'
    with Sweep(dev) as sweep:
        #speed1 = sweep.set_motor_speed(5)
        speed = sweep.get_motor_speed()
        rate1 = sweep.set_sample_rate(1000)
        rate = sweep.get_sample_rate()

        print('Motor Speed: {} Hz'.format(speed))
        print('Sample Rate: {} Hz'.format(rate))

        # Starts scanning as soon as the motor is ready
        sweep.start_scanning()
        i=0
        kDegreeToRadian = 0.017453292519943295
        kMaxLaserDistance = 500
        # get_scans is coroutine-based generator lazily returning scans ad infinitum
        #for scan in itertools.islice(sweep.get_scans(), 100):
        send_output = Int32MultiArray()
        send_output.data = []
        while not rospy.is_shutdown():
            for scan in itertools.islice(sweep.get_scans(), 100):
            
                #print((scan[0]))
                xarr =[]
                yarr = []
                for samples in scan:
                
                    #fig.clear()
                    for sample in samples:
                        if sample[1] < 1000:
                            i=i+1
                            #while not rospy.is_shutdown():
                            angl= int(sample[0])/ 1000 + 90
                            angl=angl%360
                            radAngle = math.radians(angl)
                            x = float()
                            y = float()
                            dist = float()
                            x = math.cos(radAngle) * sample[1]
                            y = math.sin(radAngle) * sample[1]
                            x = x + kMaxLaserDistance
                            x = (x / (2 * kMaxLaserDistance))*8
                        
                            y = y + kMaxLaserDistance
                            y = 8 - (y / (2 * kMaxLaserDistance))*8
                            #xarr.append(x)
                            #yarr.append(y)
                            dist= math.sqrt(x*x+y*y)
                            send_output.data = []
                            #update_line(x,y)
                            if dist < 1500:
                                i = i+1
                                send_output.data = [i,int(x*1000),int(y*1000),int(dist*1000)]
                                pub.publish(send_output)
                                #time.sleep(0.001)
                                #rate.sleep()
                    send_output.data = [i,-9999,-9999,-9999]
                    pub.publish(send_output)
                    #rate.sleep()
                # plt.axis([2, 6, 2, 6])
                    #plt.plot([xarr],[yarr],'k.')
                    #plt.plot([4],[4],'ro')
                    #ax.relim()
                    #ax.autoscale_view()
                    #fig.canvas.draw()
                    #fig.canvas.flush_events()
         
                      
    

if __name__ == '__main__':
    #plt.ion()
    #fig = plt.figure(figsize=(8,8))
    #ax = fig.add_subplot(111)
    #fig.canvas.set_window_title('Lidar')
    try:
        talker()
        #rospy.spin()
    except :
        pass
