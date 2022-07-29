#!/usr/bin/env python



import rospy
import sys, select, termios, tty
from std_msgs.msg import String
def getKey():
    tty.setraw(sys.stdin.fileno())
    select.select([sys.stdin], [], [], 0)
    key = sys.stdin.read(1)
    termios.tcsetattr(sys.stdin, termios.TCSADRAIN, settings)
    return key

if __name__=="__main__":
    settings = termios.tcgetattr(sys.stdin)
    try:
        print("basladi")
        while(1):
            key = getKey()
            print(key)
            pub = rospy.Publisher('key/zoom', String ,queue_size=1000) #
            rospy.init_node('key1', anonymous=True)
            if (key == '\x03'):
                    break

    except Exception as e:
        print(e)
