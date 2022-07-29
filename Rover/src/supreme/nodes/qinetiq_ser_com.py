#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
'''serial comm test for reading qinetiq q20hd gnss'''

import time
import serial
import numpy as np
import rospy
from std_msgs.msg import String
def DaysDictionary(x):
    '''dictionary for days of the week'''
    x = x % 7
    return {0 : "Sunday",
            1 : "Monday",
            2 : "Tuesday",
            3 : "Wednesday",
            4 : "Thursday",
            5 : "Friday",
            6 : "Saturday"
           }[x]

def ConvertToInt32(byte1,byte2,byte3,byte4):   
    '''combine 4 bytes of data and convert into (signed) int32 (long)'''
    return np.int32(byte1 << 24 | byte2 << 16 | byte3 << 8 | byte4)

def ConvertToInt16(byte1,byte2):
    '''combine 2 bytes of data and convert into (signed) int16 (integer)'''
    return np.int16(byte1 << 8 | byte2)
def gpsfinder():
    SER = serial.Serial('/dev/ttyMXUSB0')
    time.sleep(.2)                                #wait a little after connecting to the port
    SER.baudrate = 230400

    timeByte_1 = 0 
    timeByte_2 = 0
    timeByte_3 = 0
    timeByte_4 = 0
    latitudeByte_1 = 0  
    latitudeByte_2 = 0
    latitudeByte_3 = 0
    latitudeByte_4 = 0
    longitudeByte_1 = 0
    longitudeByte_2 = 0
    longitudeByte_3 = 0
    longitudeByte_4 = 0
    altitudeByte_1 = 0
    altitudeByte_2 = 0
    altitudeByte_3 = 0
    altitudeByte_4 = 0
    eastVeloByte_1 = 0
    eastVeloByte_2 = 0
    northVeloByte_1 = 0
    northVeloByte_2 = 0
    upVeloByte_1 = 0 
    upVeloByte_2 = 0
    checkSumByte = 0
    kConstant = 500                                #arbitrary constant for the amount of bytes read, may be reduced

    while SER.is_open:
        myData = bytearray(SER.read(kConstant))    #write incoming data into a byte array
        myIndex = kConstant
        myList = [None]*kConstant
        for idx, b in enumerate(myData):           #for loop to check bytes one by one
            myList[idx] = b                        #save the bytes into myList with their indices
            if myList[idx] == 77:                  #if the byte gives 77 (which is M in ASCII table)
                if myList[idx-1] == 84:            #if the preceding byte to M is T
                    if myList[idx-2] == 86:        #if the preceding byte is V
                        if myList[idx-3] == 80:    #if the preceding byte is P
                            myIndex = idx          #the message header PVMT is caught
                                                #the index of the byte giving T is marked
            if idx == myIndex + 23:                #we are interested for only the 23 bytes
                                                #after the 4 byte header (PVTM)
                timeByte_1 = myList[myIndex+1]
                timeByte_2 = myList[myIndex+2]
                timeByte_3 = myList[myIndex+3]
                timeByte_4 = myList[myIndex+4]
                latitudeByte_1 = myList[myIndex+5]
                latitudeByte_2 = myList[myIndex+6]
                latitudeByte_3 = myList[myIndex+7]
                latitudeByte_4 = myList[myIndex+8]
                longitudeByte_1 = myList[myIndex+9]
                longitudeByte_2 = myList[myIndex+10]
                longitudeByte_3 = myList[myIndex+11]
                longitudeByte_4 = myList[myIndex+12]
                altitudeByte_1 = myList[myIndex+13]
                altitudeByte_2 = myList[myIndex+14]
                altitudeByte_3 = myList[myIndex+15]
                altitudeByte_4 = myList[myIndex+16]
                eastVeloByte_1 = myList[myIndex+17]
                eastVeloByte_2 = myList[myIndex+18]
                northVeloByte_1 = myList[myIndex+19]
                northVeloByte_2 = myList[myIndex+20]
                upVeloByte_1 = myList[myIndex+21]
                upVeloByte_2 = myList[myIndex+22]
                checkSumByte = myList[myIndex+23]
                myList = [None]*kConstant          #flush myList
                break

        timeByte_1 = np.uint8(timeByte_1)
        timeByte_2 = np.uint8(timeByte_2)
        timeByte_3 = np.uint8(timeByte_3)
        timeByte_4 = np.uint8(timeByte_4)
        epoch = np.uint32(timeByte_1 << 24 | timeByte_2 << 16 | timeByte_3 << 8 | timeByte_4) #time of the week in miliseconds (week starts on Sunday)
        epoch = round(epoch / 1000)                                                           #rounded to seconds
        dayOfTheWeek = int(epoch / 86400)
        remainder = int(epoch % 86400)
        hourOfTheDay = int(remainder / 3600)
        hourOfTheDay = hourOfTheDay + 3                                                       #Turkish time
        if hourOfTheDay > 23:
            hourOfTheDay = hourOfTheDay % 24
            dayOfTheWeek = dayOfTheWeek + 1
        remainder = int(remainder % 3600)
        minuteOfTheHour = int(remainder / 60)
        secondOfTheMinute = int(remainder % 60)
        dayOfTheWeek_h = DaysDictionary(dayOfTheWeek)
        print '\t' + dayOfTheWeek_h + ' {}:{}:{}'.format(hourOfTheDay, minuteOfTheHour, secondOfTheMinute)

        latitudeByte_1 = np.uint8(latitudeByte_1)
        latitudeByte_2 = np.uint8(latitudeByte_2)
        latitudeByte_3 = np.uint8(latitudeByte_3)
        latitudeByte_4 = np.uint8(latitudeByte_4)
        myLatitude = np.float32(ConvertToInt32(latitudeByte_1,latitudeByte_2,latitudeByte_3,latitudeByte_4)) / 16777216            #divided by 2^24 scaling factor
        if myLatitude >= 0:
            latHemisphere = 'N'
        else:
            latHemisphere = 'S'
        print '\t\t\t\tlatitude:  {} {} degrees'.format(latHemisphere, round(myLatitude,6))
        
        longitudeByte_1 = np.uint8(longitudeByte_1)
        longitudeByte_2 = np.uint8(longitudeByte_2)
        longitudeByte_3 = np.uint8(longitudeByte_3)
        longitudeByte_4 = np.uint8(longitudeByte_4)
        myLongitude = np.float32(ConvertToInt32(longitudeByte_1,longitudeByte_2,longitudeByte_3,longitudeByte_4)) / 8388608        #divided by 2^23 scaling factor
        if myLongitude >= 0:
            longHemisphere = 'E'
        else:
            longHemisphere = 'W'
        print '\t\t\t\tlongitude: {} {} degrees'.format(longHemisphere, round(myLongitude,6))
        
        altitudeByte_1 = np.uint8(altitudeByte_1)
        altitudeByte_2 = np.uint8(altitudeByte_2)
        altitudeByte_3 = np.uint8(altitudeByte_3)
        altitudeByte_4 = np.uint8(altitudeByte_4)
        myAltitude = np.float32(ConvertToInt32(altitudeByte_1,altitudeByte_2,altitudeByte_3,altitudeByte_4)) * 0.3048              #converting feet to meters
        print '\t\t\t\taltitude:  {} meters'.format(round(myAltitude,1))
        #pub.publish(str(round(myLatitude,6)) + "," + str(round(myLongitude,6)) + "," + str(round(myAltitude,1)))
        
        createmsg(myLatitude,myLongitude,myAltitude)
        print str("Ali") + str(format(round(myLatitude,6),'.6f'))
        eastVeloByte_1 = np.uint8(eastVeloByte_1)
        eastVeloByte_2 = np.uint8(eastVeloByte_2)
        myEastVel = np.float16(ConvertToInt16(eastVeloByte_1,eastVeloByte_2)) * 0.3048
        print '\t\t\t\t\t\t\t\teast velocity:  {} m/s'.format(round(myEastVel,1))

        northVeloByte_1 = np.uint8(northVeloByte_1)
        northVeloByte_2 = np.uint8(northVeloByte_2)
        myNorthVel = np.float16(ConvertToInt16(northVeloByte_1,northVeloByte_2)) * 0.3048
        print '\t\t\t\t\t\t\t\tnorth velocity: {} m/s'.format(round(myNorthVel,1))

        upVeloByte_1 = np.uint8(upVeloByte_1)
        upVeloByte_2 = np.uint8(upVeloByte_2)
        myUpVel = np.float16(ConvertToInt16(upVeloByte_1,upVeloByte_2)) * 0.3048
        print '\t\t\t\t\t\t\t\tup velocity:    {} m/s'.format(round(myUpVel,1))

        #Checksum is calculated by bitwise XOR'ing the 22 bytes of data (checksum byte and the 4 byte header 'PVTM' is not included)
        myCheckSum = timeByte_1 ^ timeByte_2 ^ timeByte_3 ^ timeByte_4 ^ latitudeByte_1 ^ latitudeByte_2 ^ latitudeByte_3 ^ latitudeByte_4 ^ longitudeByte_1 ^ longitudeByte_2 ^ longitudeByte_3 ^ longitudeByte_4 ^ altitudeByte_1 ^ altitudeByte_2 ^ altitudeByte_3 ^ altitudeByte_4 ^ eastVeloByte_1 ^ eastVeloByte_2 ^ northVeloByte_1 ^ northVeloByte_2^ upVeloByte_1 ^ upVeloByte_2
    
        #print 'checksum: {}'.format(bin(checkSumByte))
        #print '{} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {} - {}'.format(timeByte_1, timeByte_2 , timeByte_3 , timeByte_4 , latitudeByte_1 , latitudeByte_2 , latitudeByte_3 , latitudeByte_4 , longitudeByte_1 , longitudeByte_2 , longitudeByte_3 , longitudeByte_4 , altitudeByte_1 , altitudeByte_2 , altitudeByte_3 , altitudeByte_4 , eastVeloByte_1 , eastVeloByte_2 , northVeloByte_1 , northVeloByte_2, upVeloByte_1 , upVeloByte_2)
        #print 'my checksum: {}'.format(bin(myCheckSum))

        if bin(checkSumByte) == bin(myCheckSum):
            print '\t\t  Received data is correct!'
            print ''
            print ''
        else:
            print '\t\t  Received data is not correct...'
            print ''
            print ''
            break
def DecToHex(decvalue):
    out = hex(decvalue).split('x')[-1]
    if len(out) == 1:
        out = "0" + out
    return out.upper()
def createmsg(lat,lng,alt):
    39.9250180
    lat = 39.92501800
    lng = 032.8369560
    alt = 1100.33
    new_lat = str(format(round(lat,8),'.8f')).zfill(11)
    new_lat = new_lat.replace(".","")
    empty_lat = str(DecToHex(int(new_lat[0:2])))+ str(DecToHex(int(new_lat[2:4]))) + str(DecToHex(int(new_lat[4:6]))) + \
        str(DecToHex(int(new_lat[6:8]))) + str(DecToHex(int(new_lat[8:10])))

    new_long = str(format(round(lng,8),'.7f')).zfill(11)
    new_long = new_long.replace(".","")
    empty_long = str(DecToHex(int(new_long[0:2])))+ str(DecToHex(int(new_long[2:4]))) + str(DecToHex(int(new_long[4:6]))) + \
        str(DecToHex(int(new_long[6:8]))) + str(DecToHex(int(new_long[8:10])))
    
    new_alt = str(format(round(alt,5),'.2f')).zfill(7)
    new_alt = new_alt.replace(".","")
    empty_alt = str(DecToHex(int(new_alt[0:2])))+ str(DecToHex(int(new_alt[2:4]))) + str(DecToHex(int(new_alt[4:6])))
    pub.publish("FE03" + empty_lat + empty_long + empty_alt + "00000000")


if __name__=="__main__":
    pub = rospy.Publisher('sup/telemetry/TX', String ,queue_size=10) #
    rospy.init_node('gpsnode', anonymous=True)
    rate = rospy.Rate(10)
    gpsfinder()