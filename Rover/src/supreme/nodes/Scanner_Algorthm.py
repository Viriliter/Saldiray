#!/usr/bin/env python
# Software License Agreement (BSD License)
#

import sys
import rospy
from std_msgs.msg import String
from std_msgs.msg import Int32MultiArray
import cv2
import numpy as np
import roslib
from cv_bridge import CvBridge, CvBridgeError
from geometry_msgs.msg import Twist
from sensor_msgs.msg import Image
from sensor_msgs.msg import PointCloud2, PointField
from std_msgs.msg import Float64
from sensor_msgs.msg import PointCloud
import sensor_msgs.point_cloud2 as pc2
import time
import math
from sensor_msgs import point_cloud2
from std_msgs.msg import Header
import struct
from nav_msgs.msg import Odometry
from tf.transformations import euler_from_quaternion, quaternion_from_euler
from nav_msgs.msg import Path
from geometry_msgs.msg import PoseStamped
from geometry_msgs.msg import Pose
from visualization_msgs.msg import Marker
from geometry_msgs.msg import TwistStamped, Pose, Point, Vector3, Quaternion
from std_msgs.msg import Header, ColorRGBA, String
from numba import vectorize, float32, guvectorize
from std_msgs.msg import Float32MultiArray,Float64MultiArray
import threading
from numba import njit, prange
from math import sqrt
import ros_numpy
teststate = 0
Destination = np.zeros((1,1,1), dtype=np.float)
Sub_path_marker_count=12
Sub_path_markers = np.empty((Sub_path_marker_count,Sub_path_marker_count,Sub_path_marker_count),dtype=np.float)
Sub_modified_path_markers = []
Sub_path_markers_id=-1
Sub_path_yaw_shift_angle = 0
Old_Sub_path_yaw_shift_angle = 0
Misson_state = 0
Way_point_wait = 0
created_path = np.empty(list((0,)) + [3], dtype=np.float)
lastshift = 0.0
turncount = 0
mstate = 0.0
pose_x = float(0)
pose_y = float(0)
pose_z = float(0)
or_x  = float(0)
or_y  = float(0)
or_z  = float(0)
or_w  = float(0)
or_ang = float(0)
lidar_array = np.zeros(list((1,)) + [3], dtype=np.float)
path = Path()
#marker_id = 0
#marker_id2 = 0
#marker = Marker()
#last_marker_x = float(0)
#last_marker_y = float(0)
#last_marker_z = float(0)
#marker_array = np.empty(list((0,)) + [4], dtype=np.float)
#markers_pink = np.empty(list((0,)) + [4], dtype=np.float)
#markers_red = np.empty(list((0,)) + [4], dtype=np.float)
waypoints_all = np.empty(list((0,)) + [4], dtype=np.float)
#marker_mainpoints = np.empty(list((0,)) + [2], dtype=np.float)
temp1 = np.empty([0],dtype=np.float)
temp2 = np.empty([0],dtype=np.float)
lasttime = 0
pointarray_lidar = np.empty(list((0,)) + [3], dtype=np.float)
temp_obs_pc = np.empty(list((0,)) + [3], dtype=np.float)
lastservoangle = 0
lastshiftangle = 0
imu_yaw = 0.0
imu_pitch = 0.0
imu_roll = 0.0
R_x = np.empty(list((0,)) + [3], dtype=np.float)
R_y = np.empty(list((0,)) + [3], dtype=np.float)
R_z = np.empty(list((0,)) + [3], dtype=np.float)



def appender(arr1,arr2):
    
    x = np.append(arr1[:,0],[arr2[0]])
    new_arr = np.zeros(list(x.shape) + [4], dtype=np.float)
    new_arr[:,0] = x
    new_arr[:,1] = np.append(arr1[:,1],[arr2[1]])
    new_arr[:,2] = np.append(arr1[:,2],[arr2[2]])
    new_arr[:,3] = np.append(arr1[:,3],[arr2[3]])
    return new_arr
def del4darray(arr1):
    x = arr1[:-1,0]
    new_arr = np.zeros(list(x.shape) + [4], dtype=np.float)
    new_arr[:,0] = x
    new_arr[:,1] = arr1[:-1,1]
    new_arr[:,2] = arr1[:-1,2]
    new_arr[:,3] = arr1[:-1,3]
    return new_arr
def appender2d(arr1,arr2):
    x = np.append(arr1[:,0],[arr2[0]])
    new_arr = np.zeros(list(x.shape) + [2], dtype=np.float)
    new_arr[:,0] = x
    new_arr[:,1] = np.append(arr1[:,1],[arr2[1]])
    return new_arr
def appender3d(arr1,arr2):
    x = np.append(arr1[:,0],[arr2[0]])
    new_arr = np.zeros(list(x.shape) + [3], dtype=np.float)
    new_arr[:,0] = x
    new_arr[:,1] = np.append(arr1[:,1],[arr2[1]])
    new_arr[:,2] = np.append(arr1[:,2],[arr2[2]])
    return new_arr






path = Path()
path.header.frame_id = 'odom'
seq = 0

def cartesian_to_path(x, y):
    global seq
    path.header.stamp = rospy.Time.now()

    for i, xi in enumerate(x):
        pose = PoseStamped()
        pose.header.stamp = rospy.Time.now()
        pose.header.frame_id = 'odom'
        pose.header.seq = seq
        pose.pose.position.x = xi
        pose.pose.position.y = y[i]
        pose.pose.position.z = 0
        path.poses.append(pose)
    seq += 1
    
    pub_path.publish(path)
def lidartest_callback(ddd):
    global markers_pink
    global or_ang
    global lidar_array
    global pointarray_lidar
    lidar_array = np.empty(list((0,)) + [3], dtype=np.float)
 
    #plt.plot(x1,y1,'b-', marker = 'o')
    for p in pc2.read_points(ddd, field_names = ("x", "y", "z"), skip_nans=True):
        x = p[0]
        y = p[1] #- 0.3
        if x> math.atan(math.radians(47))*math.fabs(y) and x>0.2 :
            lidar_array = appender3d(lidar_array,[x,y,0])
    #msg = xyz_array_to_pointcloud2(lidar_array)
    pointarray_lidar = lidar_array
    msg = xyz_array_to_pointcloud2(lidar_array)
    pub2.publish(msg)

def delete_marker():
    global Sub_path_markers_id
    marker = Marker()
    marker.header.frame_id = "map"
    marker.type = marker.SPHERE
    marker.action = marker.DELETE
    Sub_path_markers_id = Sub_path_markers_id +1
    marker.id = Sub_path_markers_id
    pub_marker1.publish(marker)
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
    pub_marker1.publish(marker)
    rospy.sleep(0.001)
time_test = 0
def zedtest_callback(ddd):
    global pointarray_lidar
    global newpoints
    global or_ang
    global markers_pink
    global Sub_path_markers
    global Sub_modified_path_markers
    global Sub_path_markers_id
    global Sub_path_marker_count
    global Sub_path_yaw_shift_angle
    global Old_Sub_path_yaw_shift_angle
    global Destination
    global Way_point_wait
    global temp1
    global temp2
    global lastshift
    global pose_x
    global pose_y
    global pose_z
    global lasttime
    global lastshiftangle
    global R_z
    global Misson_state
    global temp_obs_pc
    global time_test
    global created_path
    noww = time.time()
    #t = time.time() #
    if Misson_state > 1:
        return
    newpoints = []
    if Misson_state == 1 and Way_point_wait<45:
        Way_point_wait = Way_point_wait + 1
        pub_speed.publish(0.0)
        return
    velodyne_array = np.empty(list((0,)) + [3], dtype=np.float)
    pc = ros_numpy.numpify(ddd)
    pointss=np.zeros((pc.shape[0],3))
    pointss[:,0]=pc['x']
    pointss[:,1]=pc['y']
    pointss[:,2]=pc['z']

    pointss = pointss[np.where((pointss[:,2]<4.5) & (pointss[:,2]>-0.6))]
    pointss = pointss[np.where((pointss[:,1]<4.0) & (pointss[:,1]>-4.0))]
    pointss = pointss[np.where((pointss[:,0]<7.0) & (pointss[:,0]>0.0))]
    pointss[...,2] = np.zeros(pointss[:,2].shape, dtype=np.float32) #zz

    p_x = np.array(pointss[:,0], dtype=np.float32)
    p_y = np.array(pointss[:,1], dtype=np.float32)
    p_z = np.array(pointss[:,2], dtype=np.float32)
    x_arr_cos = np.zeros(pointss[:,2].shape, dtype=np.float32)
    x_arr_sin = np.zeros(pointss[:,2].shape, dtype=np.float32)
    y_arr_cos = np.zeros(pointss[:,2].shape, dtype=np.float32)
    y_arr_sin = np.zeros(pointss[:,2].shape, dtype=np.float32)
    xx = np.zeros(pointss[:,2].shape, dtype=np.float32)
    yy = np.zeros(pointss[:,2].shape, dtype=np.float32)
    temp1 = np.zeros(pointss[:,2].shape, dtype=np.float32)
    temp2 = np.zeros(pointss[:,2].shape, dtype=np.float32)
    single_multiply(p_x,float(math.cos(or_ang)),x_arr_cos)
    single_multiply(p_x,float(math.sin(or_ang)),x_arr_sin)
    single_multiply(p_y,float(math.cos(or_ang)),y_arr_cos)
    single_multiply(p_y,float(math.sin(or_ang)),y_arr_sin)

    zz = np.zeros(pointss[:,2].shape, dtype=np.float32)
     
    single_sum(arrays_subt(x_arr_cos, y_arr_sin), pose_x,xx)
    single_sum(arrays_sum(x_arr_sin, y_arr_cos), pose_y,yy)   
    single_sum(p_z, pose_z,zz)
    pointss[...,0] = xx
    pointss[...,1] = yy
    msg = xyz_array_to_pointcloud2(pointss)
    pub.publish(msg)
    #test_array = np.zeros((len(xx),len(xx)),dtype=np.integer)
    #print len(xx)
   # print test_array.shape
    #if len(temp_obs_pc)>0:
        #pointss = np.concatenate([pointss,temp_obs_pc])
    p_x = np.array(pointss[:,0], dtype=np.float32)
    p_y = np.array(pointss[:,1], dtype=np.float32)
    p_z = np.array(pointss[:,2], dtype=np.float32)
    loopstate = 1
    turn_angle = 0.45
    initial_destx = 12.5
    initial_desty = 0.0
    if Misson_state == 1:
        initial_destx = 12.5
        initial_desty = 0.0
    #Sub_path_marker_count = 25
    #
    t=time.time()
    path_curve = 0
    check_path = 1
    clearance = 1.00
    if len(created_path)>0:
        check_path = 0
        rx = created_path[...,0]
        ry = created_path[...,1]
        array = np.square(rx-pose_x) + np.square(ry-pose_y)
        if min(array)>0.45:
            check_path = 1
        else:
            if  np.argmin(array) < 6:
                for i in range(len(created_path)):
                    markx = created_path[i,0]
                    marky = created_path[i,1]
                    #shower_marker(markx,marky,2.0,0.0,2.0)
                    temp1 = arrays_subt(p_x,markx)
                    temp2 = arrays_subt(p_y,marky)
                    pointstry = pointss[np.where((np.abs(temp1)<0.8*clearance) & (np.abs(temp2)<0.8*clearance))]     
                    
                    if len(pointstry[:,0])>20 :#X.min() <0.2:
                        #lastobs = pointss[np.where((np.abs(temp1)<2*clearance) & (np.abs(temp2)<2*clearance))]
                        #temp_obs_pc = np.concatenate([temp_obs_pc,lastobs])
                        check_path = 1
                        break
            else:
                check_path = 1
    if check_path == 0:
        loopstate = 0
    lastobs = np.empty(list((0,)) + [3], dtype=np.float)
    global teststate
    Sub_path_marker_count = 30
    distance = math.sqrt((initial_destx-pose_x)*(initial_destx-pose_x) + (initial_desty-pose_y)*(initial_desty-pose_y))
    if distance >6.0:
        distance = 6.0
    print "ttttt"
    print time.time()-noww
    shower_marker(initial_destx,initial_desty,2.0,2.0,2.0,0.5)
    while loopstate == 1:
        if time.time()-t>0.8:
            break
        Temp_Mark_X = []#np.empty((0),dtype=np.float)
        Temp_Mark_Y = []#np.empty((0),dtype=np.float)
        Sub_path_markers = np.empty((Sub_path_marker_count,Sub_path_marker_count,Sub_path_marker_count),dtype=np.float)
        Sub_path_markers_id = -1
        for i in range(20):
            delete_marker()
        dest_x = initial_destx
        dest_y = initial_desty

        
        dest_x_sin = dest_x * float(math.sin(math.radians(Sub_path_yaw_shift_angle)))
        dest_x_cos = dest_x * float(math.cos(math.radians(Sub_path_yaw_shift_angle)))
        dest_y_sin = dest_y * float(math.sin(math.radians(Sub_path_yaw_shift_angle)))
        dest_y_cos = dest_y * float(math.cos(math.radians(Sub_path_yaw_shift_angle)))
        dest_x = dest_x_cos - dest_y_sin
        dest_y = dest_x_sin + dest_y_cos
        rad_Q = (math.atan(((dest_y-pose_y)/(dest_x-pose_x))))
        #print rad_Q
        
        step_btw_x = 0.75
        step_btw_y = 0.75 * math.tan(rad_Q)
        
        change_state = 0
        max_range=0
        
        for i in range(Sub_path_marker_count+1):
            multp = i
            markx = pose_x + step_btw_x * multp
            marky = pose_y + step_btw_y * multp
            if math.sqrt(abs(step_btw_x*multp*step_btw_x*multp) + abs(step_btw_y*multp*step_btw_y*multp)) > distance:
                break
            if math.sqrt(abs(step_btw_x*multp*step_btw_x*multp) + abs(step_btw_y*multp*step_btw_y*multp)) < 1.0:
                next
            Temp_Mark_X = np.append(Temp_Mark_X,[markx])
            Temp_Mark_Y = np.append(Temp_Mark_Y,[marky])
            
            temp1 = arrays_subt(p_x,markx)
            temp2 = arrays_subt(p_y,marky)
            pointstry = pointss[np.where((np.abs(temp1)<clearance) & (np.abs(temp2)<clearance))]     
            
            if len(pointstry[:,0])>20 :#X.min() <0.2:
                Sub_path_marker_count = i + 2
                #distance = math.sqrt(abs(step_btw_x*multp*step_btw_x*multp) + abs(step_btw_y*multp*step_btw_y*multp))
                #shower_marker(markx,marky,1.0,0.0,1.0,0.1)
                if change_state == 0:
                    path_curve = 1
                    change_state = 1
                    lastobs = pointss[np.where((np.abs(temp1)<clearance) & (np.abs(temp2)<clearance))]
                    if Sub_path_yaw_shift_angle>0:
                        Sub_path_yaw_shift_angle = Sub_path_yaw_shift_angle * -1
                    else:
                        Sub_path_yaw_shift_angle = Sub_path_yaw_shift_angle * -1 + 10
                    break
            #else:
                #shower_marker(markx,marky,0.0,2.0,0.0,0.1)
        
        
        if change_state == 0:
            #for i in range(len(Temp_Mark_X)):
                #shower_marker(Temp_Mark_X[i],Temp_Mark_Y[i],0.0,2.0,0.0,0.1)
            for i in range(5):
                delete_marker()
            
            #turn_angle = 45 - math.degrees(rad_Q) + math.degrees(or_ang) - Sub_path_yaw_shift_angle
            # = turn_angle/100
            #print "angle turn : " + str(turn_angle)
            #if abs(Old_Sub_path_yaw_shift_angle- Sub_path_yaw_shift_angle) > 5:
            #    Old_Sub_path_yaw_shift_angle = Sub_path_yaw_shift_angle
            #    check_path = 0
            #else:
            if len(lastobs)>0:
                temp_obs_pc = np.concatenate([temp_obs_pc,lastobs])
            Sub_path_yaw_shift_angle = 0
            loopstate = 0
            
            break
    if check_path == 1:
        path_message = Float64MultiArray()
        path_message.data = [pose_x,pose_y,Temp_Mark_X[-1],Temp_Mark_Y[-1],initial_destx,initial_desty,path_curve]
        pub_subdest.publish(path_message)

    


def rigid_transform_3D(A, B):
    #A = A[0::4]
    #B = B[0::4]
    assert len(A) == len(B)
    N = A.shape[0]; # total points

    centroid_A = np.mean(A, axis=0)
    centroid_B = np.mean(B, axis=0)
    
    # centre the points
    AA = A - np.tile(centroid_A, (N, 1))
    BB = B - np.tile(centroid_B, (N, 1))
    # dot is matrix multiplication for array
    H = np.dot(np.transpose(AA) , BB)

    U, S, Vt = np.linalg.svd(H)

    R = np.dot(Vt.T , U.T)
    # special reflection case

    if np.linalg.det(R) < 0:
       print "Reflection detected"
       Vt[2,:] *= -1
       R = np.dot(Vt.T , U.T)

    t = np.dot(-R,centroid_A.T) + centroid_B.T

    return R, t    
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
   
prev_yaw = 0.0
prev_pitch = 0.0
prev_roll = 0.0
def calldata(msg):
    global prev_pitch,prev_roll,prev_yaw
    global pose_x
    global pose_y
    global pose_z
    global or_x
    global or_y
    global or_z
    global or_w
    global or_ang
    global marker_id
    global final_x
    global final_y
    global final_z
    global R_x
    global R_y
    global R_z
    #print (msg.pose.pose.orientation.x)
    pose_y = msg.pose.pose.position.x
    pose_z = msg.pose.pose.position.y
    pose_x = msg.pose.pose.position.z
    or_x = msg.pose.pose.orientation.x
    or_y = msg.pose.pose.orientation.y
    or_z = msg.pose.pose.orientation.z
    or_w = msg.pose.pose.orientation.w
        


    quaternion = (
        msg.pose.pose.orientation.x,
        msg.pose.pose.orientation.y,
        msg.pose.pose.orientation.z,
        msg.pose.pose.orientation.w)
    euler = euler_from_quaternion(quaternion)
    pitch = euler[0]
    yaw = euler[1]
    roll = euler[2]
    if abs(prev_yaw-math.degrees(yaw)) > 2:
        temp = prev_yaw
        prev_yaw = yaw
        yaw = temp
    #print "roll : ",roll 
    #print "pitch : ",pitch 
    #print "yaw : ",yaw
    #print "pose x :", pose_x
    #print "pose y : ", pose_y
    #print "pose z: ", pose_z
    or_ang=yaw
    #print yaw
   # print "x : " + str(pose_x) + "   y : " + str(pose_y) + "    z : " + str(pose_z)
    #pose_x = 0.0
    #pose_y = 0.0
    #pose_z = 0.0
    #delmarker()
    #show_marker(final_x+pose_x,final_y+pose_y,final_z)
    #or_ang = 0.0
        

@vectorize([float32(float32, float32)])
def arrays_sum(x, y):
    return x + y
@vectorize([float32(float32, float32)])
def arrays_subt(x, y):
    return x - y
@vectorize([float32(float32, float32)])
def arrays_multp(x, y):
    return x * y
@guvectorize([(float32[:], float32, float32[:])], '(n),()->(n)')
def single_sum(x, y, res):
    for i in range(x.shape[0]):
        res[i] = x[i] + y
@guvectorize([(float32[:], float32, float32[:])], '(n),()->(n)')
def single_multiply(x, y, res):
    for i in range(x.shape[0]):
        res[i] = x[i] * y
@guvectorize([(float32[:], float32, float32[:])], '(n),()->(n)')
def single_subt(x, y, res):
    for i in range(x.shape[0]):
        res[i] = x[i] - y



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
def key_callback(data):
    global waypoints_all
    global Destination
    if (data.data[0] == "[" ):
        print(data.data[1:-1])
        str_waypoints = data.data[1:-1]
        x = float(str_waypoints.split(",")[0])
        y = float(str_waypoints.split(",")[1])
        z = float(str_waypoints.split(",")[2])
        Destination = [x,y,z]
    #if (data.data == "del"):
        #delmarker()
        

def camera_left_callback(data):
		
		try:
			# We select bgr8 because its the OpneCV encoding by default
			cv_image1 = bridge_object.imgmsg_to_cv2(data, desired_encoding='bgr8')
		except CvBridgeError as e:
			print(e)
		
		left=cv2.cvtColor(cv_image1,cv2.COLOR_BGR2GRAY)
		cv2.imshow("left",cv_image1)
		cv2.waitKey(1)
lat1 = 39.8714500
lon1 = 32.7504900
def telemetry_callback(lat2,lon2):
    global lat1
    global lon1
    global marker_mainpoints
    
    #lat2 = ddd.data[0]
    #lon2 = ddd.data[1]
    #print(ddd.data)
    dx = (lon2-lon1)*40000*math.cos((lat1+lat2)*math.pi/360)/360
    dx = dx * 1000
    dy = (lat1-lat2)*40000/360
    dy = dy * 1000
   # indx  = np.where(abs(marker_mainpoints - (dx,dy))<0.5)
    #finding_arr = marker_mainpoints[indx]
   # if len(finding_arr) == 0:
        #show_marker(dx,dy,0)
        #shower_marker()
        #marker_mainpoints = appender2d(marker_mainpoints, [dx,dy])
       # print (dx,dy)
    
def imu_callback(data):
    global imu_yaw
    global imu_pitch
    global imu_roll
    imu_yaw = data.data[0]
    imu_pitch = data.data[1]
    imu_roll = data.data[2]
    print imu_yaw,imu_pitch,imu_roll
def createdpath_callback(data):
    global created_path
    created_path = pointcloud2_to_xyz_array(data)
def listener(): 

    rospy.Subscriber('supreme/sweep/pcloud2', PointCloud2, lidartest_callback)
    rospy.Subscriber("/integrated_to_init",Odometry, calldata)
    #rospy.Subscriber("/zed/point_cloud/cloud_registered",PointCloud2, zedtest_callback)
    rospy.Subscriber("/velodyne_points",PointCloud2, zedtest_callback)
    rospy.Subscriber('key/zoom', String, key_callback)

    rospy.Subscriber('sup/imudata',Float64MultiArray,imu_callback)
    rospy.Subscriber('supreme/path/pc2', PointCloud2, createdpath_callback)
    
    rospy.spin()
   
    

if __name__ == '__main__':
    time.sleep(5)
    bridge_object = CvBridge()
    rospy.init_node('Sweep1_1', anonymous=True)
    pub = rospy.Publisher('sup/zed2', PointCloud2,queue_size=100) #
    pub2 = rospy.Publisher('sup/lidar', PointCloud2,queue_size=100) #
    pub3 = rospy.Publisher('supreme/motor/state', Float64,queue_size=100) #
    pub_marker1 = rospy.Publisher('sup/path', Marker,queue_size=100)
    pub_marker2 = rospy.Publisher('sup/path2', Marker,queue_size=100)
    pub4 = rospy.Publisher('key/arac', String,queue_size=100) #
    pub5 = rospy.Publisher('sup/fornox/speed', String,queue_size=100)
    pub_path = rospy.Publisher('/supreme/supreme_path', Path,queue_size=100)
    pub_servo = rospy.Publisher('/commands/servo/position2', Float64,queue_size=100) #
    pub_speed = rospy.Publisher('/commands/motor/speed2', Float64,queue_size=100) #
    pub_subdest = rospy.Publisher('/supreme/scanner/subdest',Float64MultiArray,queue_size=100)
    listener()