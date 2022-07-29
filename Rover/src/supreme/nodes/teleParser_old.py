#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
'''
***************************************************************************
This class interprets string telemetry messages come from serial interface
***************************************************************************
'''

class teleParser(object):
    def __init__(self):
        '''
        Initializes the class
        '''
        self.vehicleMessage    = VehicleMessage()
        self.weaponMessage     = WeaponMessage()
        self.navigationMessage = NavigationMessage()

    def messageParser(self,r_byte):
        '''
        Parses telemetry message in byte array format
        '''
        output = ""
        try:
            #Check Start Field in the messages
            if r_byte.encode('hex')[:2] == 'fe':
                #Checks Message ID
                #Vehicle Command
                if r_byte.encode('hex')[2:4] == '00':
                    #print r_byte.encode('hex')
                    
                    self.vehicleMessage.vehicleState = bin(int(r_byte.encode('hex')[4:6], 16))[2:]
                    self.vehicleMessage.joy_leftX    = int(r_byte.encode('hex')[6:8], 16)              
                    self.vehicleMessage.joy_leftY    = int(r_byte.encode('hex')[8:10], 16)                        
                    self.vehicleMessage.joy_rightX   = int(r_byte.encode('hex')[10:12], 16)                    
                    self.vehicleMessage.joy_rightY   = int(r_byte.encode('hex')[12:14], 16)
                    self.vehicleMessage.buttonState  = bin(int(r_byte.encode('hex')[14:16], 16))[2:]
                    output = str(self.vehicleMessage.vehicleState).zfill(8)+","+str(self.vehicleMessage.joy_leftX )+","+str(self.vehicleMessage.joy_leftY) \
                        +","+str(self.vehicleMessage.joy_rightX) +","+str(self.vehicleMessage.joy_rightY) +","+str(self.vehicleMessage.buttonState).zfill(8)
                    #print output
                #Weapon Command
                if r_byte.encode('hex')[2:4] == '01':
                    self.weaponMessage.weapon_stat = bin(int(r_byte.encode('hex')[4:6], 16))[2:]
                    self.weaponMessage.yaw         = int(r_byte.encode('hex')[4:6], 16)
                    self.weaponMessage.pitch       = int(r_byte.encode('hex')[4:6], 16)
                #Navigation Command
                if r_byte.encode('hex')[2:4] == '02':
                    self.navigationMessage.lat = ""
                    self.navigationMessage.lng = ""
                #print lat_int
                #print lon_int
            

            

        except:
            pass
        return output

    def messageParser2(self,s_message):
        '''
        Parses telemetry message in string format
        '''
        
        h_message = s_message.split("-")
        '''
        for element in h_message[:7]:
            print element.decode("hex")
        '''
        if h_message[0]=="FE":
            if h_message[1]=="00":
                print s_message
                self.vehicleMessage.vehicleState= (h_message[2].decode("hex"))
                self.vehicleMessage.left_x      = (h_message[3].decode("hex"))
                self.vehicleMessage.left_y      = (h_message[4].decode("hex"))
                self.vehicleMessage.right_x     = (h_message[5].decode("hex"))
                self.vehicleMessage.right_y     = (h_message[6].decode("hex"))

                #self.vehicleMessage.buttonState = h_message[7].decode("hex")
            if h_message[1]=="01":
                self.weaponMessage.weapon_stat  = h_message[2].decode("hex")
                self.weaponMessage.yaw          = h_message[3].decode("hex")
                self.weaponMessage.pitch        = h_message[4].decode("hex")
                
            if h_message[1]=="02":
                self.navigationMessage.lat      = h_message[2].decode("hex")
                self.navigationMessage.lng      = h_message[3].decode("hex")
                self.navigationMessage.command  = h_message[4].decode("hex")
    
        
    def disposeMessages(self):
        '''
        Disposes messages in order to be ready to next parsing
        '''
        self.vehicleMessage.dispose()
        self.weaponMessage.dispose()
        self.navigationMessage.dispose()

    def getVehicleState(self):
         return self.vehicleMessage.vehicleState + self.vehicleMessage.joy_leftX + self.vehicleMessage.joy_leftY + self.vehicleMessage.joy_rightX + self.vehicleMessage.joy_rightY     

class VehicleMessage(object):
    def __init__(self):

        self.__vehicleState= '\x00'
        self.__joy_leftX   = '\x00'
        self.__joy_leftY   = '\x00'
        self.__joy_rightX  = '\x00'
        self.__joy_rightY  = '\x00'
        self.__buttonState = '\x00'

    def __set_vehicleState(self,vehicleState):
        self.__vehicleState = vehicleState
    def __set_joy_leftX(self,joy_leftX):
        self.__joy_leftX = joy_leftX
    def __set_joy_leftY(self,joy_leftY):
        self.__joy_leftY = joy_leftY
    def __set_joy_rightX(self,joy_rightX):
        self.__joy_rightX = joy_rightX
    def __set_joy_rightY(self,joy_rightY):
        self.__joy_rightY = joy_rightY
    def __set_buttonState(self,buttonState):
        self.__buttonState = buttonState
    
    def __get_vehicleState(self):
        return self.__vehicleState
    def __get_joy_leftX(self):
        return self.__joy_leftX
    def __get_joy_leftY(self):
        return self.__joy_leftY
    def __get_joy_rightX(self):
        return self.__joy_rightX
    def __get_joy_rightY(self):
        return self.__joy_rightY
    def __get_buttonState(self):
        return self.__buttonState
    
    def dispose(self):
        self.__vehicleState= '\x00'
        self.__joy_leftX   = '\x00'
        self.__joy_leftY   = '\x00'
        self.__joy_rightX  = '\x00'
        self.__joy_rightY  = '\x00'
        self.__buttonState = '\x00'

    vehicleState = property(__get_vehicleState,__set_vehicleState)
    joy_leftX    = property(__get_joy_leftX   ,__set_joy_leftX   )
    joy_leftY    = property(__get_joy_leftY   ,__set_joy_leftY   )
    joy_rightX   = property(__get_joy_rightX  ,__set_joy_rightX  )
    joy_rightY   = property(__get_joy_rightY  ,__set_joy_rightY  )
    buttonState  = property(__get_buttonState ,__set_buttonState)


class WeaponMessage(object):
    def __init__(self):

        self.__weapon_stat = '\x00'
        self.__yaw         = '\x00\x00\x00\x00'
        self.__pitch       = '\x00\x00\x00\x00'

        
    def __set_weapon_stat(self,weapon_stat):
        self.__weapon_stat = weapon_stat
    def __set_yaw(self,yaw):
        self.__yaw = yaw
    def __set_pitch(self,pitch):
        self._pitch = pitch

    def dispose(self):
        self.__weapon_stat = '\x00'
        self.__yaw         = '\x00\x00\x00\x00'
        self.__pitch       = '\x00\x00\x00\x00'

    def __get_weapon_stat(self,weapon_stat):
        return self.__weapon_stat
    def __get_yaw(self,yaw):
        return self.__mode2
    def __get_pitch(self,pitch):
        return self.__pitch


    weapon_stat = property(__get_weapon_stat,__set_weapon_stat )
    yaw         = property(__get_yaw        ,__set_yaw )
    pitch       = property(__get_pitch      ,__set_pitch )

class NavigationMessage(object):
    def __init__(self):

        self.__lat     = '\x00\x00\x00\x00\x00'
        self.__lng     = '\x00\x00\x00\x00\x00'
        self.__command = '\x00'
        
    def __set_lat(self,lat):
        self.__lat = lat
    def __set_lng(self,lng):
        self.__lng = lng
    def __set_command(self,command):
        self.__command = command


    def __get_lat(self,lat):
        return self.__lat
    def __get_lng(self,lng):
        return self.__lng
    def __get_command(self,command):
        return self.__command

    def dispose(self):
        self.__lat     = '\x00\x00\x00\x00\x00'
        self.__lng     = '\x00\x00\x00\x00\x00'
        self.__command = '\x00'

    lat      = property(__get_lat     ,__set_lat )
    lng      = property(__get_lng     ,__set_lng )
    command  = property(__get_command ,__set_command )  
