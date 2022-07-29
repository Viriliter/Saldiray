#!/usr/bin/python

###############################################################                                                                  
# Find details on this script at
# http://wp.me/p5kNk-37
#
# History
# ------------------------------------------------
# Author                Date      		Comments
# Eric Goebelbecker     Jun 6 2015 		Initial Authoring
# 			                                                         
'''
## License
 GoPiGo for the Raspberry Pi: an open source robotics platform for the Raspberry Pi.
 Copyright (C) 2017  Dexter Industries
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
'''      
#
###############################################################
from evdev import InputDevice, categorize, ecodes, KeyEvent,list_devices
import rospy
import time 
import threading
from std_msgs.msg import Float64,Int64MultiArray
def deviceListing() :
    print("That is all the deveices that I can detect")
    devices = [InputDevice(fn) for fn in list_devices()]
    for device in devices:
        print(device.name)
        print device.fn


# Open the device


#
active = 0
speed = 127
speed2 = 127
servo = 0.45
def joysticklisten():
    global speed
    global servo
    global active
    global speed2
    for event in gamepad.read_loop():
        if event.type == ecodes.EV_KEY:
            keyevent = categorize(event)
            if keyevent.keystate == KeyEvent.key_down:
                if keyevent.keycode[0] == 'BTN_A':
                    print "BLUE"
                    active = 1
                elif keyevent.keycode[0] == 'BTN_B':
                    print "GREEN"
                    active = 0
                elif keyevent.keycode == 'BTN_C':
                    rospy.signal_shutdown("ss")
                    exit()
                    print "RED"

                elif keyevent.keycode[1] == 'BTN_X':
                    print "YELLOW"

                elif keyevent.keycode == 'BTN_Z':
                    print "RB"

                elif keyevent.keycode == 'BTN_TR':
                    print "RT"

                elif keyevent.keycode[1] == 'BTN_Y':
                    print "LB"
                elif keyevent.keycode == 'BTN_TL':
                    print "LT"    

        elif event.type == ecodes.EV_ABS:
            #print ecodes.ABS[event.code]
            if ecodes.ABS[event.code] == 'ABS_Y':
                speed = event.value
                if event.value  < 120:
                    
                    print 'UP ' + str(event.value) +  "  speed :" +str(speed)
                    
                elif event.value > 130:
                    print 'DOWN ' + str(event.value) +  "  speed :" +str(speed)
            elif ecodes.ABS[event.code] == 'ABS_X':
                if event.value   < 120:
                    print 'LEFT ' + str(event.value)
                elif event.value > 130:
                    print 'RIGHT ' + str(event.value)
            elif ecodes.ABS[event.code] == 'ABS_RZ':
                speed2 = event.value
                if event.value   < 120:
                    
                    print 'UP_R ' + str(event.value)
                elif event.value > 130:
                    print 'DOWN_R ' + str(event.value)
            elif ecodes.ABS[event.code] == 'ABS_Z':
                if event.value   < 120:
                    servo = 0.45 - ((255-event.value) * 0.40 ) /255
                    print 'LEFT_R ' + str(event.value) +  "  servo :" +str(servo)
                elif event.value > 130:
                    servo = 0.45 + (event.value * 0.40 ) /255
                    print 'RIGHT_R ' + str(event.value) +  "  servo :" +str(servo)
                else:
                    servo = 0.45
def talker():
    global active
    global speed
    global speed2
    #print speed
    #if active == 1:
        #pub.publish(speed)
        #pub2.publish(servo)

    if speed == 0:
        speed = 1
    if speed == 255:
        speed = 254
    if speed2 == 0:
        speed2 = 1
    if speed2 == 255:
        speed2 = 254        
    output_float = Int64MultiArray()
    output_float.data = [speed,speed2]
    pub_tank.publish(output_float)
    time.sleep(0.3)

if __name__ == '__main__':
    global speed
    global servo
    global speed2
    deviceListing()
    rospy.init_node('joystick_node', anonymous=True)
    gamepad = InputDevice('/dev/input/event9')
    pub = rospy.Publisher('/commands/motor/speed', Float64,queue_size=100) #
    pub2 = rospy.Publisher('/commands/servo/position', Float64,queue_size=100) #
    pub_tank = rospy.Publisher('/sup/tank', Int64MultiArray,queue_size=100) #
    t = threading.Thread(name='joysticklisten', target=joysticklisten)
    t.start()
    while 1:
        talker()
    t.join()
        
