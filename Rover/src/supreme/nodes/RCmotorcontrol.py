#!/usr/bin/env python
import rospy
from std_msgs.msg import Float64
from std_msgs.msg import String
import time
speed = 0.0
servo = 0.0

def callback1(data):
    global speed
    speed = data.data
def callback2(data):
    global servo
    servo = data.data
def listener():
    rospy.init_node('motor_node', anonymous=True)
    rospy.Subscriber("joystick1", Float64, callback1)
    rospy.Subscriber('joystick2', Float64, callback2)
    pub.publish(speed)
    pub2.publish(servo)
    print speed
    print servo
    rospy.spin()

if __name__ == '__main__':
    global speed
    global servo
    pub = rospy.Publisher('/commands/motor/speed', Float64,queue_size=100) #
    pub2 = rospy.Publisher('/commands/servo/position', Float64,queue_size=100) #
    while 1:
        listener()
