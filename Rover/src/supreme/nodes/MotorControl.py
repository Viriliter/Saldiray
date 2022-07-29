#!/usr/bin/env python
import rospy
from std_msgs.msg import Float64
from std_msgs.msg import String
import time
motorstate = -1.0
komut = ""
turndelay = 0.0
lastturn = 0.0
bekle = 0
speed = 0.0
def callback(data):
    global komut
    global turndelay
    global bekle
    global lastturn
    global speed
    turn_state = data.data
    if komut == "t":
        print "up"
        speed = speed +1000.0
        komut = ""
    if komut == "g" :
        print "down"
        speed = speed - 1000.0
        komut = ""
    if komut == "m" :
        speed = 0.0
        komut = ""
    r = rospy.Rate(20)
    pub.publish(speed)
    r.sleep
    elaps = time.time()-turndelay
    if elaps> 1 :
        pub2.publish(0.45)
        if abs(turn_state)<114.5 and turn_state<>0.45:
            turndelay = time.time()
            pub2.publish(turn_state)
            print(" turn : " + str(turn_state))
            #speed = 1000.0
            #pub.publish(speed)
    #lastturn = turn_state

def key_callback(data):
    global komut
    print(data.data)
    komut = data.data    
def listener():
    rospy.init_node('motor_node', anonymous=True)
    rospy.Subscriber("supreme/motor/state", Float64, callback)
    rospy.Subscriber('key/rccar', String, key_callback)
    rospy.spin()

if __name__ == '__main__':
    pub = rospy.Publisher('/commands/motor/speed', Float64,queue_size=100) #
    pub2 = rospy.Publisher('/commands/servo/position', Float64,queue_size=100) #
    listener()