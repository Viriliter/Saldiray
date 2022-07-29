#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import sys
sys.path.append("/home/monster/catkin_ws/src/supreme/nodes/d_star")
from d_star import DStar
import a_star
import numpy
import numpy as np
from std_msgs.msg import Float32MultiArray
import rospy
from sensor_msgs.msg import PointCloud2
import time
import matplotlib.pyplot as plt
from scipy.ndimage import rotate
from std_msgs.msg import Header, ColorRGBA, String
from visualization_msgs.msg import Marker
import math
save = 0
path_mark_id=-1
def show_marker_path_plan(marker_x,marker_y):
        global path_mark_id
        marker = Marker()
        marker.header.frame_id = "map"
        marker.type = marker.SPHERE
        marker.action = marker.ADD
        marker.scale.x = 0.2
        marker.scale.y = 0.2
        marker.scale.z = 0.2
        marker.color = ColorRGBA(0.0, 2.0, 2.0, 0.8)
        marker.pose.orientation.w = 1.0
        marker.pose.position.x = marker_x
        marker.pose.position.y = marker_y
        marker.pose.position.z = 0
        path_mark_id = path_mark_id+1
        marker.id = path_mark_id
        pub_marker.publish(marker)
        rospy.sleep(0.001)
def pointcloud2_to_array(cloud_msg):
    ''' 
    Converts a rospy PointCloud2 message to a numpy recordarray 
    
    Assumes all fields 32 bit floats, and there is no padding.
    '''
    dtype_list = [(f.name, np.float32) for f in cloud_msg.fields]
    cloud_arr = np.fromstring(cloud_msg.data, dtype_list)
    return np.reshape(cloud_arr, (cloud_msg.height, cloud_msg.width))  
def get_xyz_points(cloud_array, remove_nans=True):
    '''
    Pulls out x, y, and z columns from the cloud recordarray, and returns a 3xN matrix.
    '''
    # remove crap points
    if remove_nans:
        mask = np.isfinite(cloud_array['x']) & np.isfinite(cloud_array['y']) & np.isfinite(cloud_array['z'])
        cloud_array = cloud_array[mask]
    
    # pull out x, y, and z values
    points = np.zeros(list(cloud_array.shape) + [3], dtype=np.float)
    points[...,0] = cloud_array['x']
    points[...,1] = cloud_array['y']
    points[...,2] = cloud_array['z']

    return points
def pointcloud2_to_xyz_array(cloud_msg, remove_nans=True):
    return get_xyz_points(pointcloud2_to_array(cloud_msg), remove_nans=remove_nans)
def path_callback(ddd):
        global save
        global path_mark_id
        pointss = pointcloud2_to_xyz_array(ddd)
        #print pointss
        #pointss = pointss[::2]
        shift = 25
        xx = np.array(pointss[...,0]*10,dtype=np.float)
        yy = np.array(shift-pointss[...,1]*10,dtype=np.float)
        xx_cos = np.multiply(xx,float(math.cos(math.radians(45))))
        yy_cos = np.multiply(yy,float(math.cos(math.radians(45))))
        xx = xx_cos - yy_cos
        yy = xx_cos + yy_cos
        xx = np.array(xx,dtype=np.integer)
        yy = np.array(yy,dtype=np.integer)

        test_array = np.zeros((71,71),dtype=np.integer)
        print min(xx)
        print max(xx)
        print min(yy)
        print max(yy)
        test_array[xx,yy] = 1
        #print test_array[0,-15]
        print test_array
        if save==1:
                save==1
                plt.imshow(test_array,origin="lower")
                plt.show()
                numpy.savetxt("/home/monster/catkin_ws/src/supreme/nodes/Output.txt", test_array, fmt="%d")
                print "saved"
                
        var1 = 15
        var2 = 15 #+ shift
        pf = DStar(x_start=0, y_start=0, x_goal=var1, y_goal=var2)

        var = 1
        
        # create empty matrix // gerek kalmayabilir
        #a = numpy.zeros(shape=(50,50))
        a = test_array
        while (var == 1):
                
                #a = a[0:41,0:30]
                
                plt.imshow(a,origin="lower")
                plt.show()
                # read matrix from file
                #a = np.loadtxt('/home/monster/catkin_ws/src/supreme/nodes/inputt.txt', dtype='i', delimiter=',')
               # print a
               # print len(a)
                t = a
                for i in range(len(a)):
                        for j in range(len(a[i])):
                                if (a[i][j] == 1):
                                        pf.update_cell(i, j, -1)
                                else:
                                        pf.update_cell(i, j, 0)
                print "testtt"
                pf.replan()
                path = pf.get_path()
                path_mark_id = -1
                for i in range(len(path)):
                        x_val = float(str(str(path[i]).split(':')[1]).split(',')[0])
                        y_val = float(str(str(path[i]).split(':')[1]).split(',')[1])
                        x_val2 = x_val/10
                        y_val2 = (y_val-shift)/10
                        show_marker_path_plan(x_val2,y_val2)
                        t[int(x_val)][int(y_val)] = 5
                #print a
                print "heree"
                plt.imshow(t,origin="lower")
                plt.show()
                #print path
                var=0
        # update start point for next cycle // gpsten input alarak update et!!!
                #pf.update_start(x, y)

        
def listener():
        rospy.Subscriber('sup/zed2',PointCloud2,path_callback)
        rospy.spin()
if __name__=="__main__":
       # var1, var2 = [float(x) for x in input("Enter goal coordinates(x,y) here: ").split()]
        rospy.init_node('Path_Planner', anonymous=True)
        pub_marker = rospy.Publisher('sup/path_planner', Marker,queue_size=100)
        #time.sleep(10)
        listener()