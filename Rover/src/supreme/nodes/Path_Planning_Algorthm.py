#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
import math
import numpy as np
import bisect
import matplotlib.pyplot as plt
import time
import sys
import rospy
from std_msgs.msg import Float32MultiArray,Float64MultiArray,Int64MultiArray
from visualization_msgs.msg import Marker
from std_msgs.msg import Header, ColorRGBA, String
from sensor_msgs.msg import PointCloud2, PointField
from tf.transformations import euler_from_quaternion, quaternion_from_euler
from nav_msgs.msg import Odometry
from std_msgs.msg import Float64
emergency_state = 0
Sub_path_markers_id = -1
points = np.zeros(list((0,)) + [3], dtype=np.float)
passed_marker_id = 99
pose_x = 0
pose_y = 0
pose_z = 0
roll = 0
pitch = 0
yaw = 0
rx, ry, rz, ryaw, rk = [], [], [], [], []
dest_x = -1
dest_y = -1
lasttime = 0
safetymarkind = -1
class Spline:
    u"""
    Cubic Spline class
    """

    def __init__(self, x, y):
        self.b, self.c, self.d, self.w = [], [], [], []

        self.x = x
        self.y = y

        self.nx = len(x)  # dimension of x
        h = np.diff(x)

        # calc coefficient c
        self.a = [iy for iy in y]

        # calc coefficient c
        A = self.__calc_A(h)
        B = self.__calc_B(h)
        self.c = np.linalg.solve(A, B)
        #  print(self.c1)
        
        # calc spline coefficient b and d
        for i in range(self.nx - 1):
            self.d.append((self.c[i + 1] - self.c[i]) / (3.0 * h[i]))
            tb = (self.a[i + 1] - self.a[i]) / h[i] - h[i] * \
                (self.c[i + 1] + 2.0 * self.c[i]) / 3.0
            self.b.append(tb)
        
    def calc(self, t):
        u"""
        Calc position
        if t is outside of the input x, return None
        """

        if t < self.x[0]:
            return None
        elif t > self.x[-1]:
            return None

        i = self.__search_index(t)
        dx = t - self.x[i]
        result = self.a[i] + self.b[i] * dx + \
            self.c[i] * dx ** 2.0 + self.d[i] * dx ** 3.0

        return result

    def calcd(self, t):
        u"""
        Calc first derivative
        if t is outside of the input x, return None
        """

        if t < self.x[0]:
            return None
        elif t > self.x[-1]:
            return None

        i = self.__search_index(t)
        dx = t - self.x[i]
        result = self.b[i] + 2.0 * self.c[i] * dx + 3.0 * self.d[i] * dx ** 2.0
        return result
    def calcdd(self, t):
        u"""
        Calc second derivative
        """

        if t < self.x[0]:
            return None
        elif t > self.x[-1]:
            return None

        i = self.__search_index(t)
        dx = t - self.x[i]
        result = 2.0 * self.c[i] + 6.0 * self.d[i] * dx
        return result

    def __search_index(self, x):
        u"""
        search data segment index
        """
        return bisect.bisect(self.x, x) - 1

    def __calc_A(self, h):
        u"""
        calc matrix A for spline coefficient c
        """
        A = np.zeros((self.nx, self.nx))
        A[0, 0] = 1.0
        for i in range(self.nx - 1):
            if i != (self.nx - 2):
                A[i + 1, i + 1] = 2.0 * (h[i] + h[i + 1])
            A[i + 1, i] = h[i]
            A[i, i + 1] = h[i]

        A[0, 1] = 0.0
        A[self.nx - 1, self.nx - 2] = 0.0
        A[self.nx - 1, self.nx - 1] = 1.0
        #  print(A)
        return A

    def __calc_B(self, h):
        u"""
        calc matrix B for spline coefficient c
        """
        B = np.zeros(self.nx)
        for i in range(self.nx - 2):
            B[i + 1] = 3.0 * (self.a[i + 2] - self.a[i + 1]) / \
                h[i + 1] - 3.0 * (self.a[i + 1] - self.a[i]) / h[i]
        #  print(B)
        return B


class Spline2D:
    u"""
    2D Cubic Spline class
    """
    def __init__(self, x, y):
        self.s = self.__calc_s(x, y)
        self.sx = Spline(self.s, x)
        self.sy = Spline(self.s, y)

    def __calc_s(self, x, y):
        dx = np.diff(x)
        dy = np.diff(y)
        self.ds = [math.sqrt(idx ** 2 + idy ** 2)
                   for (idx, idy) in zip(dx, dy)]
        s = [0]
        s.extend(np.cumsum(self.ds))
        return s

    def calc_position(self, s):
        u"""
        calc position
        """
        x = self.sx.calc(s)
        y = self.sy.calc(s)

        return x, y

    def calc_curvature(self, s):
        u"""
        calc curvature
        """
        dx = self.sx.calcd(s)
        ddx = self.sx.calcdd(s)
        dy = self.sy.calcd(s)
        ddy = self.sy.calcdd(s)
        k = (ddy * dx - ddx * dy) / (dx ** 2 + dy ** 2)
        return k

    def calc_yaw(self, s):
        u"""
        calc yaw
        """
        dx = self.sx.calcd(s)
        dy = self.sy.calcd(s)
        yaw = math.atan2(dy, dx)
        return yaw


def calc_spline_course(x, y, ds=0.1):
    sp = Spline2D(x, y)
    s = np.arange(0, sp.s[-1], ds)

    rx, ry, ryaw, rk = [], [], [], []
    for i_s in s:
        ix, iy = sp.calc_position(i_s)
        rx.append(ix)
        ry.append(iy)
        ryaw.append(sp.calc_yaw(i_s))
        rk.append(sp.calc_curvature(i_s))

    return rx, ry, ryaw, rk, s


def test_spline2d():
    print("Spline 2D test")
    
    
    t = time.time()
    x = [1.0, 1.03, 1.47, 1.5]
    y = [1.0, 1.1, 1.4, 1.5]
    #x = [1.5, 1.45, 1.05, 1.0]
    
    sp = Spline2D(x, y)
    
    s = np.arange(0, sp.s[-1], 0.1)
    
    rx, ry, ryaw, rk = [], [], [], []
    for i_s in s:
        ix, iy = sp.calc_position(i_s)
        rx.append(ix)
        ry.append(iy)
        ryaw.append(sp.calc_yaw(i_s))
        rk.append(sp.calc_curvature(i_s))
    print time.time()-t
    flg, ax = plt.subplots(1)
    plt.plot(x, y, "xb", label="input")
    plt.plot(rx, ry, "-r", label="spline")
    plt.grid(True)
    plt.axis("equal")
    plt.xlabel("x[m]")
    plt.ylabel("y[m]")
    plt.legend()

    flg, ax = plt.subplots(1)
    plt.plot(s, [math.degrees(iyaw)-90 for iyaw in ryaw], "-r", label="yaw")
    plt.grid(True)
    plt.legend()
    plt.xlabel("line length[m]")
    plt.ylabel("yaw angle[deg]")

    flg, ax = plt.subplots(1)
    plt.plot(s, rk, "-r", label="curvature")
    plt.grid(True)
    plt.legend()
    plt.xlabel("line length[m]")
    plt.ylabel("curvature [1/m]")
    
    plt.show()
    


def test_spline():
    print("Spline test")
    import matplotlib.pyplot as plt
    x = [1.0, 1.05, 1.45, 1.5]
    y = [1.0, 1.1, 1.4, 1.5]

    spline = Spline(x, y)
    rx = np.arange(-2.0, 4, 0.01)
    ry = [spline.calc(i) for i in rx]

    plt.plot(x, y, "xb")
    plt.plot(rx, ry, "-r")
    plt.grid(True)
    plt.axis("equal")
    plt.show()
def delete_marker():
    global Sub_path_markers_id
    marker = Marker()
    marker.header.frame_id = "map"
    marker.type = marker.SPHERE
    marker.action = marker.DELETE
    Sub_path_markers_id = Sub_path_markers_id +1
    marker.id = Sub_path_markers_id
    pub_marker.publish(marker)
    rospy.sleep(0.001)
def shower_marker(x,y,c1,c2,c3,s):
    global Sub_path_markers_id
    marker = Marker()
    marker.header.frame_id = "map"
    marker.type = marker.SPHERE
    marker.action = marker.ADD
    marker.scale.x = s
    marker.scale.y = s
    marker.scale.z = s
    marker.color = ColorRGBA(c1, c2, c3, 0.8)
    marker.pose.orientation.w = 1.0
    marker.pose.position.x = x
    marker.pose.position.y = y
    marker.pose.position.z = 0
    Sub_path_markers_id = Sub_path_markers_id +1
    marker.id = Sub_path_markers_id
    pub_marker.publish(marker)
    rospy.sleep(0.001)
def SubDestionation_callback(data):
    global Sub_path_markers_id
    global points
    global rx, ry, rz, ryaw, rk
    global dest_x,dest_y
    global emergency_state
    global safetymarkind
    x_1 = data.data[0]
    y_1 = data.data[1]
    x_2 = data.data[2]
    y_2 = data.data[3]
    x_d = data.data[4]
    y_d = data.data[5]
    curve_state = data.data[6]
    dest_x = x_d
    dest_y = y_d
    if curve_state == 2.0:
        emergency_state = 1
        pub_speed.publish(0.0)
        pub_servo.publish(0.45)
        return
    emergency_state = 0
    if x_2>x_d:
        diff = -0.3
    else:
        diff = 0.3
    p_x = x_2 - diff
    p_y = y_2 - diff*(y_2-y_d)/(x_2-x_d)
    Sub_path_markers_id = -1
    for i in range(0,20):
        delete_marker()
    shower_marker(x_1,y_1,3.0,2.0,0.0,0.2)
    shower_marker(x_2,y_2,3.0,2.0,0.0,0.2)

    if curve_state ==0:
        x = np.linspace(x_1,x_2,50)
        y = np.linspace(y_1,y_2,50)
    else:

        if x_2>x_1:
            x_c = 0.15
        else:
            x_c = -0.15
        if y_2>y_1:
            y_c = 0.04
        else:
            y_c = -0.04

        x = [x_1, p_x, x_2]
        y = [y_1, p_y, y_2]
    if math.sqrt((x_2-x_1)*(x_2-x_1) + (y_2-y_1)*(y_2-y_1))>1.6:
        x_f = ((x_2 - x_1)/10)*3
        y_f = ((y_2 - y_1)/10)*3
        x = [x_1, x_1+x_f, p_x, x_2]
        y = [y_1, y_1+y_f, p_y, y_2]
    #x = [1.5, 1.45, 1.05, 1.0]
    
    sp = Spline2D(x, y)
    
    s = np.arange(0, sp.s[-1], 0.1)
    
    rx, ry, rz, ryaw, rk = [], [], [], [], []
    doub = 0
    for i_s in s:
        if doub == 0:
            doub = 1
        else:
            doub = 0
            ix, iy = sp.calc_position(i_s)
            shower_marker(ix,iy,3.0,0.0,0.0,0.1)
            rx.append(ix)
            ry.append(iy)
            rz.append(0)
            ryaw.append(sp.calc_yaw(i_s))
            rk.append(sp.calc_curvature(i_s))
    #for i in range(10):
        #delete_marker()
    points = np.zeros(list((len(rx),)) + [3], dtype=np.float)
    points[...,0] = rx
    points[...,1] = ry
    points[...,2] = rz
    msg = xyz_array_to_pointcloud2(points)
    pub_path_pc2.publish(msg)
    #flg, ax = plt.subplots(1)
    #plt.plot(s, [math.degrees(iyaw) for iyaw in ryaw], "-r", label="yaw")
    #plt.grid(True)
    #plt.legend()
    #plt.xlabel("line length[m]")
    #plt.ylabel("yaw angle[deg]")
    #plt.show()
def xyz_array_to_pointcloud2(points, stamp=None, frame_id=map):
    '''
    Create a sensor_msgs.PointCloud2 from an array
    of points.
    '''
    msg = PointCloud2()
    if stamp:
        msg.header.stamp = stamp
    if frame_id:
        msg.header.frame_id = "map"
    if len(points.shape) == 3:
        msg.height = points.shape[1]
        msg.width = points.shape[0]
    else:
        msg.height = 1
        msg.width = len(points)
    msg.fields = [
        PointField('x', 0, PointField.FLOAT32, 1),
        PointField('y', 4, PointField.FLOAT32, 1),
        PointField('z', 8, PointField.FLOAT32, 1)]
    msg.is_bigendian = False
    msg.point_step = 12
    msg.row_step = 12*points.shape[0]
    msg.is_dense = int(np.isfinite(points).all())
    msg.data = np.asarray(points, np.float32).tostring()

    return msg
p_farkx = 0
p_farkoldx = 0
p_oldx=0
p_farky = 0
p_farkoldy = 0
p_oldy=0
p_farkz = 0
p_oldz = 0
wait_second = 0.8
def odom_calldata(msg):
    global p_farkx,p_farkoldx,p_farkoldy,p_farky,p_oldx,p_oldy
    global lasttime
    global pose_x
    global pose_y
    global pose_z
    global roll
    global pitch
    global yaw
    global ryaw
    global dest_x,dest_y
    global emergency_state
    global safetymarkind
    global Sub_path_markers_id
    global passed_marker_id
    global wait_second
    pose_y = msg.pose.pose.position.x
    pose_z = msg.pose.pose.position.y
    pose_x = msg.pose.pose.position.z
    or_x = msg.pose.pose.orientation.x
    or_y = msg.pose.pose.orientation.y
    or_z = msg.pose.pose.orientation.z
    or_w = msg.pose.pose.orientation.w
    p_farkx = pose_x-p_oldx
    p_farky = pose_y-p_oldy

    quaternion = (
        msg.pose.pose.orientation.x,
        msg.pose.pose.orientation.y,
        msg.pose.pose.orientation.z,
        msg.pose.pose.orientation.w)
    euler = euler_from_quaternion(quaternion)
    pitch = euler[0]
    yaw = euler[1]
    roll = euler[2]
    if len(points)==0:
        return
    if emergency_state == 1:
        return
    rx = points[...,0]
    ry = points[...,1]
    array = np.square(rx-pose_x) + np.square(ry-pose_y)
    if min(array)>0.35:
        points_empty = np.zeros(list((len(rx),)) + [3], dtype=np.float)
        msg = xyz_array_to_pointcloud2(points_empty)
        pub_path_pc2.publish(msg)
        return
    print "ind : " , np.argmin(array)
    print "yaw : " , math.degrees(ryaw[np.argmin(array)])
    if math.sqrt((dest_x-pose_x)*(dest_x-pose_x) + (dest_y-pose_y)*(dest_y-pose_y))<1.5:
        output_float = Int64MultiArray()
        output_float.data = [126,126]
        pub_tank.publish(output_float)
        return
    turn_angle = -math.degrees(ryaw[np.argmin(array)])+math.degrees(yaw)
    Sub_path_markers_id = passed_marker_id
    passed_marker_id = passed_marker_id +1
    #shower_marker(points[np.argmin(array),0],points[np.argmin(array),1],3.0,0.0,1.0,0.1)
    shower_marker(pose_x,pose_y,3.0,0.0,1.0,0.1)
    #turn_angle = turn_angle/100
    left_track_speed = 0
    right_track_speed = 0
    # 0.50 >> turn right
    # -0.50 << turn left
    c = 1.0/500.0 #velocity constant #565
    c2 = 1.0/300.0
    if turn_angle>0:
        left_track_speed = 127
        right_track_speed = 127 - math.radians(turn_angle)*0.3 / c
    else:
        left_track_speed = 127 + math.radians(turn_angle)*0.3 / c2
        right_track_speed = 127 
    #if turn_angle<0.0:
    #    turn_angle = 0.0
    #if turn_angle>0.99:
    #    turn_angle = 0.99
    #if turn_angle <0.50:
    #    speed = turn_angle * 3000 + 1000.0
    #else:
    #    speed = (0.99-turn_angle)*3000 + 1000.0
    
    #pub_speed.publish(speed)
    #pub_servo.publish(turn_angle)
    if left_track_speed<-126:
        left_track_speed = -126
    if right_track_speed<-126:
        right_track_speed = -126
    print "here"
    if time.time()-lasttime> wait_second:
        output_float = Int64MultiArray()
        left_track_speed = 128 - left_track_speed
        right_track_speed = 128 - right_track_speed
        if left_track_speed > right_track_speed + 60:
            right_track_speed = right_track_speed +30
        if right_track_speed > left_track_speed +60:
            left_track_speed = left_track_speed +30
        output_float.data = [left_track_speed,right_track_speed]
        print output_float.data
        pub_tank.publish(output_float)
        lasttime = time.time()
def listener():
    rospy.Subscriber('supreme/scanner/subdest', Float64MultiArray, SubDestionation_callback)
    rospy.Subscriber("/integrated_to_init",Odometry, odom_calldata)
    rospy.spin()
if __name__ == '__main__':
    time.sleep(8)
    #test_spline()
    rospy.init_node('Path_Planner', anonymous=True)
    pub_marker = rospy.Publisher('supreme/path/marker', Marker,queue_size=100)
    pub_path_pc2 = rospy.Publisher('supreme/path/pc2', PointCloud2,queue_size=100)
    pub_servo = rospy.Publisher('/commands/servo/position', Float64,queue_size=100) #
    pub_speed = rospy.Publisher('/commands/motor/speed', Float64,queue_size=100) #
    pub_tank = rospy.Publisher('/sup/tankk', Int64MultiArray,queue_size=100) #
    listener()
    #test_spline2d()    