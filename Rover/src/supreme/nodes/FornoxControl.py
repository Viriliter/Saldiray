#!/usr/bin/env python
# -*- coding: utf-8 -*-

'''
Created on 2015/01/30

@author: spiralray
'''

import sys
import yaml
import roslib
roslib.load_manifest("canusb");
from std_msgs.msg import String,Int64MultiArray
import struct
import rospy
from _CAN import CAN
from sys import stdin
import serial
import time
import threading
import math
testlasttime = 0
lastdatatime = 0
LeftSpeed = 127
ArmSpeed = 127
TurbinSpeed = 127
RightSpeed = 127
L_speed_otonom = 127
R_speed_otonom = 127
Kick = 127
Tambur = 127
YedekPWM1 = 136
komut = ""
komut2 = ""
basecode = "7F7F7F7F7F7F88"
modifiedbase = "7F7F7F7F7F7F88"
fullcode = "7F7F7F7F7F7F88"
motion_state = 0
data7_bits = [0,0,0,0,0,0,0,0]
isstarted = 0
arac_on = 0
arac_started = 0
keysdata = ""
arac_state = 0
throttle_up_old = 0
throttle_down_old = 0
otonomspeed_up = 0
otonomspeed_down = 0
otonombasespeed = 127
otonomleft = 0
otonomright = 0
def getbasecode():
    global LeftSpeed
    global ArmSpeed
    global TurbinSpeed
    global RightSpeed
    global Kick
    global Tambur
    global YedekPWM1
    base = DecToHex(RightSpeed) + DecToHex(ArmSpeed) + DecToHex(TurbinSpeed) \
        + DecToHex(LeftSpeed) + DecToHex(Kick) + DecToHex(Tambur) + DecToHex(YedekPWM1)
    return base
def DecToHex(decvalue):
    out = hex(decvalue).split('x')[-1]
    if len(out) == 1:
        out = "0" + out
    return out.upper()
def data7converter():
    global data7_bits
    rest=""
    for i in range (0,8):
        rest = rest + str(data7_bits[i])
    rest = str(hex(int(rest,2)))
    rest = rest.split("x")[1]
    if len(rest)==1:
        rest = "0" + rest
    return rest.upper()
def on_mode():
    global basecode
    global fullcode  
    global data7_bits 
    data7_bits[7] = 1
    #fullcode = basecode + data7converter()
    #print basecode + data7converter()
    return (basecode + data7converter()).decode('hex')
def buji():
    return ("7F7F7F7F7F7F8803").decode('hex')
def start():
    return ("7F7F7F7F7F7F8805").decode('hex')
def gazart():
    global data7_bits
    data7_bits[0] = 1
    data7_bits[1] = 0
    code = getbasecode() + data7converter()
    data7_bits[0] = 0
    return (code).decode('hex')   
def gazazalt():
    global data7_bits
    data7_bits[0] = 0
    data7_bits[1] = 1
    code = getbasecode() + data7converter()
    data7_bits[1] = 0
    return (code).decode('hex')
def up():
    global data7_bits

    data7_bits[3] = 1
    code = getbasecode() + data7converter()
    #print code
    return (getbasecode() + data7converter()).decode('hex')
def down():
    global data7_bits
    data7_bits[3] = 1
    code = getbasecode() + data7converter()
    return (code).decode('hex')

def stop():
    global data7_bits 
    data7_bits[0] = 0
    data7_bits[1] = 0
    data7_bits[2] = 0
    data7_bits[3] = 0
    data7_bits[4] = 0
    data7_bits[5] = 0
    data7_bits[6] = 0
    data7_bits[7] = 0
    return ("7F7F7F7F7F7F88" + data7converter()).decode('hex')
def controlpacket():
    global data7_bits
    return (getbasecode() + data7converter()).decode('hex')
def key_callback(data):
    global komut
    print(data.data)
    #komut = data.data

def command_callback(data):
    global LeftSpeed
    global RightSpeed
    global komut

    global arac_started
    global keysdata
    global komut
    global komut2
    global fullcode
    global basecode
    global motion_state
    global data7_bits
    global isstarted
    global keysdata
    global arac_state
    global throttle_up_old
    global throttle_down_old
    global LeftSpeed
    global RightSpeed
    global lastdatatime
    global otonomleft
    global otonomright
    global otonomspeed_up
    global otonomspeed_down
    global otonombasespeed
    global L_speed_otonom
    global R_speed_otonom
    global testlasttime
    global ArmSpeed
    global TurbinSpeed
    global Tambur
    keysdata = str(data.data)
    msg = CAN()
    msg.extId = 1
    isshutdown = 0
    isbroken = 0

    r = rospy.Rate(10) # 10Hz
    if time.time()-testlasttime<0.1:
        return
    else:
        testlasttime = time.time()
    try:
        states =  keysdata.split(',')
        #tt = tt+1
        if len(states)>3:
            #print states
            lastdatatime = time.time()
        #print "devam"
        state1 = str(states[0])
        value1 = int(states[1])
        value2 = int(states[4])
        value3 = int(states[3])
        speed1 = int(states[2])
        speed2 = int(states[4])

        state2 = str(states[5])
        if state1[7] == "1" and arac_state == 0:
            print("Vehicle ON")
            arac_state =1
            msg.data = on_mode()
            #print msg.data.encode('hex')
            pub.publish(msg)
            #r.sleep()
            
        if state1[7] =="0" and arac_state == 1:
            print("Vehicle OFF")
            msg.data = stop()
            #print msg.data.encode('hex')
            isstarted = 0
            arac_state = 0
            pub.publish( msg )
            #r.sleep()
        if state1[7] == "1" and arac_state == 1: 
            #print("Vehicle Check")
            arac_state =1
            msg.data = controlpacket()
            #print msg.data.encode('hex')
            pub.publish(msg)
            #r.sleep()   
        if state1[5] == "1" and isstarted == 0 and arac_state == 1:
            print("Vehicle Start")
            msg.data = start()
            for i in range (0,7):
                pub.publish( msg )
                time.sleep(0.1)
            #r.sleep()
            komut = ""
            isstarted = 1
        if state1[1] == "0": 
            throttle_up_old = 0
        if state1[1] == "1" and state1[0] == "0" and throttle_up_old == 0:
            print("engine +")
            throttle_up_old = 1
            msg.data = gazart()
            pub.publish( msg )
            #r.sleep()
                
        if state1[0] == "0": 
            throttle_down_old = 0  
        if state1[0] == "1" and state1[1] == "0" and throttle_down_old == 0:
            print("engine -")
            throttle_down_old = 1
            msg.data = gazazalt()
            pub.publish( msg )
            #r.sleep()
        #print speed1
        if value2 <> 127:
            if value2 <40:
                value2 = 40
            TurbinSpeed = value2
            msg.data = up()
            
            pub.publish( msg )
        if value3 <> 127:
            Tambur = value3
            msg.data = up()
            
            pub.publish( msg )
        if (value1<>127 or speed2<>127) and state1[2] == "0":
            #print speed1
            #print speed2
            speed2 = speed1
            speed_diff = 0
            if value1 >126:
                speed_diff = value1-127
                speed1 = speed1-speed_diff
            else:
                speed_diff = 127-value1
                speed2 = speed2 - speed_diff
            
            otonombasespeed = 127
            if speed1>127:
                speed1 = (speed1-127)*100
                speed1 = speed1/128
                speed1 = int(speed1)+155
            if speed1<128:
                speed1 = speed1*100
                speed1 = speed1/127
                speed1 = int(speed1)
            if speed2>127:
                speed2 = (speed2-127)*100
                speed2 = speed2/128
                speed2 = int(speed2)+155
            if speed2<128:
                speed2 = speed2*100
                speed2 = speed2/127
                speed2 = int(speed2)
            LeftSpeed = speed1
            RightSpeed = speed2
            if LeftSpeed > 255:
                LeftSpeed = 255
            elif LeftSpeed<0:
                LeftSpeed = 0
            if RightSpeed>255:
                RightSpeed = 255
            elif RightSpeed<0:
                RightSpeed =0
            #print speed1
            msg.data = up()
            
            pub.publish( msg )
            #r.sleep()
        if speed1 == 127 and speed2 == 127 and state1[2] == "0" :
            data7_bits[3] = 0
            otonombasespeed = 127
            LeftSpeed = speed1
            RightSpeed = speed2

        if state1[2] == "1" and state2[2] == "0" :
            otonomspeed_up = 0
        if state1[2] == "1" and state2[2] == "1" and otonomspeed_up == 0 :
            otonomspeed_up = 1
            otonombasespeed = otonombasespeed - 10
        if state1[2] == "1" and state2[3] == "0" :
            otonomspeed_down = 0
        if state1[2] == "1" and state2[3] == "1" and otonomspeed_down == 0 :
            otonomspeed_down = 1
            otonombasespeed = otonombasespeed + 10
        if state1[2] == "1":
            LeftSpeed = L_speed_otonom
            RightSpeed = R_speed_otonom
            LeftSpeed = int(LeftSpeed * 195.0 / 255.0 +30.0)
            RightSpeed = int((RightSpeed * 195.0 / 255.0 +30.0))
            if RightSpeed > 127:
                RightSpeed= int(RightSpeed * 1.0)
            else:
                RightSpeed = int(RightSpeed * 1.0)
            if LeftSpeed < 30:
                LeftSpeed = 30
            if LeftSpeed >255:
                LeftSpeed = 255
            if RightSpeed < 30:
                RightSpeed = 30
            if RightSpeed >255:
                RightSpeed = 255
            msg.data = up()
            pub.publish( msg )
        print "Left : " + str(LeftSpeed) + "  Right : " + str (RightSpeed)
    

    except:
        pass          
def speed_callback(data):
    global otonomleft
    global otonomright
    
    speed = data.data.split(',')
    otonomleft = int(speed[0])
    otonomright = int(speed[1])
    #print "otonom left :" + speed[0]
    #print "otonom reight :" + speed[1]
def callback(data):
    global L_speed_otonom
    global R_speed_otonom
    L_speed_otonom = data.data[0]
    R_speed_otonom = data.data[1]
def listener():

    
    #rospy.Subscriber('key/arac', String, key_callback)
    rospy.Subscriber('sup/fornox/speed', String, speed_callback) 
    rospy.Subscriber('sup/guicommand', String, command_callback)
    rospy.Subscriber("/sup/tankk", Int64MultiArray, callback)
    rospy.spin()


if __name__ == '__main__':

    rospy.init_node('canusb_test', anonymous=True)
    pub = rospy.Publisher('cantx', CAN, queue_size=100)
    listener()

