#!/usr/bin/env python
# Software License Agreement (BSD License)
#
# Copyright (c) 2008, Willow Garage, Inc.
# All rights reserved.
#
'''
************************************************************
This class interprets NMEA messages come from GNSS Sensor
************************************************************
NMEA Message Headers:

   -$GPGGA - Global Positioning System Fix Data
   -$GPGLL - Geographic position, latitude / longitude
   -$GPGSA - GPS DOP and active satellites 
   -$GPGSV - GPS Satellites in view

   -$GPRMC - Recommended minimum specific GPS/Transit data

   -$GPVTG - Track made good and ground speed
************************************************************
Further info at http://aprs.gids.nl/nmea/
************************************************************
'''
#Install following library with 'pip install LatLon'
import LatLon

class nmeaParser(object):
    def __init__(self):
        self.gpgga = GPGGA()
        self.gpgll = GPGLL()
        self.gpgsa = GPGSA()
        self.gpgsv = GPGSV()
        self.gprmc = GPRMC()
        self.gpvtg = GPVTG()

    def NMEAparser(self,msg):
        '''
        Parses NMEA message
        '''
        
        try:
            splitMsg = msg.split(",")            
            if (splitMsg[0]=="$GPGGA"):
                # Global Positioning System Fix Data
                self.gpgga.time   =  splitMsg[1]
                self.gpgga.lat    =  splitMsg[2]
                self.gpgga.lat2   =  splitMsg[3]
                self.gpgga.lng    =  splitMsg[4]
                self.gpgga.lng2   =  splitMsg[5]
                self.gpgga.fixq   =  splitMsg[6]
                self.gpgga.n_sat  =  splitMsg[7]
                self.gpgga.hDOP   =  splitMsg[8]
                self.gpgga.alt    =  splitMsg[9]
                self.gpgga.alt2   =  splitMsg[10]
                self.gpgga.h_geoid =  splitMsg[11]
                self.gpgga.h_geoid2 =  splitMsg[12]
                self.gpgga.t_update =  splitMsg[13]
                #self.gpgga.GPGGA_station_id =  splitMsg[14]
                self.gpgga.GPGGA_checksum =  splitMsg[14]

            elif (splitMsg[0]=="$GPGLL"):
                #  Geographic position, latitude / longitude
                self.gpgll.lat      = splitMsg[1]
                self.gpgll.lat2     = splitMsg[2]
                self.gpgll.lng      = splitMsg[3]
                self.gpgll.lng2     = splitMsg[4]
                self.gpgll.fix_time = splitMsg[5]
                self.gpgll.valid    = splitMsg[6]
                self.gpgll.checksum = splitMsg[7]
                
            elif (splitMsg[0]=="$GPGSA"):
                # GPS DOP and active satellites 
                self.gpgsa.mode1 = splitMsg[1]
                self.gpgsa.mode2 = splitMsg[2]
                self.gpgsa.idSV  = [splitMsg[3], splitMsg[4], splitMsg[5], splitMsg[6], splitMsg[7],
                                        splitMsg[8], splitMsg[9], splitMsg[10], splitMsg[11], splitMsg[12],
                                        splitMsg[13], splitMsg[14]]
                self.gpgsa.pDop  = splitMsg[15]
                self.gpgsa.hDop  = splitMsg[16]
                self.gpgsa.vDop  = splitMsg[17]

            elif (splitMsg[0]=="$GPGSV"):
                # GPS Satellites in view
                self.gpgsv.tn_msg = splitMsg[1]
                self.gpgsv.n_msg  = splitMsg[2]
                self.gpgsv.n_sv   = splitMsg[3]
                self.gpgsv.sv_PNR = splitMsg[4]
                self.gpgsv.elev   = splitMsg[5]
                self.gpgsv.azim   = splitMsg[6]
                self.gpgsv.SNR    = splitMsg[7]
                self.gpgsv.secSVR = [splitMsg[8], splitMsg[9], splitMsg[10], splitMsg[11]]
                self.gpgsv.thrSVR = [splitMsg[12], splitMsg[13], splitMsg[14], splitMsg[15]]
                self.gpgsv.frtSVR = [splitMsg[16], splitMsg[17], splitMsg[18], splitMsg[19]]

            elif (splitMsg[0]=="$GPRMC"):
                # Recommended minimum specific GPS/Transit data
                self.gprmc.t_fix     = splitMsg[1]
                self.gprmc.d_stat    = splitMsg[2]
                self.gprmc.lat       = splitMsg[3]
                self.gprmc.lat2      = splitMsg[4]
                self.gprmc.lng       = splitMsg[5]
                self.gprmc.lng2      = splitMsg[6]
                self.gprmc.speed_knt = splitMsg[7]
                self.gprmc.t_course  = splitMsg[8]
                self.gprmc.date_fix  = splitMsg[9]
                self.gprmc.mag_var   = splitMsg[10]
                self.gprmc.mag_var2  = splitMsg[11]
                self.gprmc.checksum  = splitMsg[12]

            elif (splitMsg[0]=="$GPVTG"):
                # Track made good and ground speed
                self.gpvtg.track      = splitMsg[1]
                self.gpvtg.track2     = splitMsg[2]
                self.gpvtg.empty1     = splitMsg[3]
                self.gpvtg.empty2     = splitMsg[4]
                self.gpvtg.speed_knt  = splitMsg[5]
                self.gpvtg.speed_knt2 = splitMsg[6]
                self.gpvtg.speed_km   = splitMsg[7]
                self.gpvtg.speed_km2  = splitMsg[8]
                self.gpvtg.checksum   = splitMsg[9]
        except:
            pass

    def getLocation(self,Type="GPGGA"):
        '''
        Returns current location
        '''
        if(Type=="GPGGA"):
            latlon = self.gpgga.lat+self.gpgga.lat2+self.gpgga.lng+self.gpgga.lng2
            if(latlon ==""):
                return 0,0
            lat = latlon[0:2]+" "+latlon[2:10]+" "+latlon[10]
            lon = latlon[11:14]+" "+latlon[14:22]+" "+latlon[22]
            loc = LatLon.string2latlon(lat,lon,"d% %m% %H")
            return round(loc.lat,6), round(loc.lon,6)
            
        elif (Type=="GPGLL"):
            latlon = self.gpgga.lat+self.gpgga.lat2+self.gpgga.lng+self.gpgga.lng2
            
            lat = latlon[0:2]+" "+latlon[2:10]+" "+latlon[10]
            lon = latlon[11:14]+" "+latlon[14:22]+" "+latlon[22]
            loc = LatLon.string2lanlon(lat,lon,"d% %m% %H")
            return round(loc.lat,6), round(loc.lon,6)

    def getSpeed(self,Type="GPVTG",Unit="km/h"):
        '''
        Returns speed in either knot/h or km/h
        '''
        if(Unit=="km/h"):
            #GPVTG
            return self.gpvtg.speed_km            
        elif(Unit=="knt/h"):
            if(Type=="GPRMC"):
                return self.gptmc.speed_knt
            elif(Type=="GPVTG"):
                return self.gpvtg.speed_knt
            return -1
        return -1

    def getMagneticField(self):
        '''
        Returns the magnetic field variation in string format
        '''
        return self.gprmc.mag_var+","+self.gprmc.mag_var2


    def getTime(self):
        '''
        Returns the fixed time and date in string format
        '''
        time = self.gpgga.time[0:2]+":"+self.gpgga.time[2:4]+":"+self.gpgga.time[4:6]
        date = self.gprmc.date_fix[0:2]+"-"+self.gprmc.date_fix[2:4]+"-"+self.gprmc.date_fix[4:6]
        return time+","+date

    def getAltitude(self):
        '''
        Returns the altitude
        '''
        if (self.gpgga.alt==""):
            return 0
        return float(self.gpgga.alt)

    def getOrientation(self):
        '''
        Returns the azimuth and elevation information in array format
        '''
        return self.gpgsv.azim,self.gpgsv.elev
    
    def getStatus(self):
        '''
        Returns the status of GNSS module in array format
        >>Mode1, Mode2, Fix Quality, Number of Satellites<<
        '''
        mode1 = self.gpgsa.mode1
        mode2 = self.gpgsa.mode2
        fixQ = self.gpgga.fixq
        numSat = self.gpgga.n_sat
        return [mode1, mode2, fixQ, numSat]

class GPGGA(object):
    def __init__(self):
        self.__time        = ""   
        self.__lat         = ""
        self.__lat2        = ""
        self.__lng         = ""
        self.__lng2        = ""
        self.__fixq        = ""
        self.__n_sat       = ""
        self.__hDOP        = ""
        self.__alt         = ""
        self.__alt2        = ""
        self.__h_geoid     = ""
        self.__h_geoid2    = ""
        self.__t_update    = ""
        self.__set_checksum= ""
        
    def __set_time(self,time):
        self.__time = time

    def __set_lat(self,lat):
        self.__lat = lat
    def __set_lat2(self,lat2):
        self.__lat2 = lat2
    def __set_lng(self,lng):
        self.__lng = lng
    def __set_lng2(self,lng2):
        self.__lng2 = lng2
    def __set_fixq(self,fixq):
        self.__fixq = fixq
    def __set_n_sat(self,n_sat):
        self.__n_sat = n_sat
    def __set_hDOP(self,hDOP):
        self.__hDOP = hDOP
    def __set_alt(self,alt):
        self.__alt = alt
    def __set_alt2(self,alt2):
        self.__alt2 = alt2
    def __set_h_geoid(self,h_geoid):
        self.__h_geoid = h_geoid
    def __set_h_geoid2(self,h_geoid2):
        self.__h_geoid2 = h_geoid2
    def __set_t_update(self,t_update):
        self.__t_update = t_update
    def __set_station_id(self,station_id):
        self.__station_id = station_id
    def __set_checksum(self,checksum):
        self.__checksum = checksum

    def __get_time(self):
        return self.__time
    def __get_lat(self):
        return self.__lat
    def __get_lat2(self):
        return self.__lat2
    def __get_lng(self):
        return self.__lng
    def __get_lng2(self ):
        return self.__lng2
    def __get_fixq(self ):
        return self.__fixq
    def __get_n_sat(self):
        return self.__n_sat
    def __get_hDOP(self ):
        return self.__hDOP
    def __get_alt(self):
        return self.__alt
    def __get_alt2(self ):
        return self.__alt2
    def __get_h_geoid(self ):
        return self.__h_geoid
    def __get_h_geoid2(self):
        return self.__h_geoid2
    def __get_t_update(self):
         return self.__t_update
    def __get_station_id(self):
         return self.__station_id
    def __get_checksum(self):
         return self.__checksum

    time     = property(__get_time     ,__set_time     )
    lat      = property(__get_lat      ,__set_lat      )
    lat2     = property(__get_lat2     ,__set_lat2     )
    lng      = property(__get_lng      ,__set_lng      )
    lng2     = property(__get_lng2     ,__set_lng2     )
    fixq     = property(__get_fixq     ,__set_fixq     )
    n_sat    = property(__get_n_sat    ,__set_n_sat    )
    hDOP     = property(__get_hDOP     ,__set_hDOP     )
    alt      = property(__get_alt      ,__set_alt      )
    alt2     = property(__get_alt2     ,__set_alt2     )
    h_geoid  = property(__get_h_geoid  ,__set_h_geoid  )
    h_geoid2 = property(__get_h_geoid2 ,__set_h_geoid2 )
    t_update = property(__get_t_update ,__set_t_update )
    station_id= property(__get_station_id,__set_station_id)
    checksum = property(__get_checksum ,__set_checksum )

class GPGLL(object):
    def __init__(self):
        self.__lat     = ""
        self.__lat2    = ""
        self.__lng     = ""
        self.__lng2    = ""
        self.__fix_time= ""
        self.__valid   = ""
        self.__checksum= ""

    def __set_lat(self,lat):
        self.__lat = lat
    def __set_lat2(self,lat2):
        self.__lat2 = lat2
    def __set_lng(self,lng):
        self.__lng = lng
    def __set_lng2(self,lng2):
        self.__lng2 = lng2
    def __set_fix_time(self,fix_time):
        self.__fix_time = fix_time
    def __set_valid(self,valid):
        self._valid = valid
    def __set_checksum(self,checksum):
        self.__checksum = checksum

    def __get_lat(self):
        return self.lat
    def __get_lat2(self):
        return self.lat2
    def __get_lng(self):
        return self.lng
    def __get_lng2(self ):
        return self.lng2
    def __get_fix_time(self ):
        return self.fix_time
    def __get_valid(valid):
        return self.valid
    def __get_checksum(checksum ):
        return self.checksum
        
    lat = property(__get_lat,__set_lat)
    lat2 = property(__get_lat2,__set_lat2)
    lng = property(__get_lng,__set_lng)
    lng2  = property(__get_lng2,__set_lng2)
    fix_time = property(__get_fix_time,__set_fix_time)
    valid = property(__get_valid,__set_valid)
    checksum = property(__get_checksum ,__set_checksum)

class GPGSA(object):
    def __init__(self):
        self.__mode1 = ""
        self.__mode2 = ""
        self.__idSV  = ""
        self.__pDop  = ""
        self.__hDop  = ""
        self.__vDop  = ""
        
    def __set_mode1(self,mode1):
        self.__mode1 = mode1
    def __set_mode2(self,mode2):
        self.__mode2 = mode2
    def __set_idSV(self,idSV):
        self.__idSV = idSV
    def __set_pDop(self,pDop):
        self.__pDop = pDop
    def __set_hDop(self,hDop):
        self.__hDop = hDop
    def __set_vDop(self,vDop):
        self._vDop = vDop

    def __get_mode1(self):
        return self.__mode1
    def __get_mode2(self):
        return self.__mode2
    def __get_idSV(self):
        return self.__idSV
    def __get_pDop(self):
        return self.__pDop
    def __get_hDop(self):
        return self.__hDop
    def __get_vDop(self):
        return self._vDop

    mode1 = property(__get_mode1,__set_mode1 )
    mode2 = property(__get_mode2,__set_mode2 )
    idSV  = property(__get_idSV ,__set_idSV  )
    pDop  = property(__get_pDop ,__set_pDop  )
    hDop  = property(__get_hDop ,__set_hDop  )
    vDop  = property(__get_vDop ,__set_vDop  )

class GPGSV(object):
    def __init__(self):
        self.__tn_msg = ""
        self.__n_msg  = ""
        self.__n_sv   = ""
        self.__sv_PN  = ""
        self.__elev   = ""
        self.__azim   = ""
        self.__SNR    = ""
        self.__secSV  = ""
        self.__thrSV  = ""
        self.__frtSV  = ""
        
    def __set_tn_msg (self,tn_msg):
        self.__tn_msg = tn_msg
    def __set_n_msg  (self,n_msg):
        self.__n_msg = n_msg
    def __set_n_sv   (self,n_sv ):
        self.__n_sv = n_sv
    def __set_sv_PNR (self,sv_PN):
        self.__sv_PN = sv_PN
    def __set_elev   (self,elev ):
        self.__elev = elev
    def __set_azim   (self,azim ):
        self.__azim = azim
    def __set_SNR    (self,SNR  ):
        self.__SNR = SNR
    def __set_secSVR (self,secSVR):
        self.__secSVR = secSVR
    def __set_thrSVR (self,thrSVR):
        self.__thrSVR = thrSVR
    def __set_frtSVR (self,frtSVR):
        self.__frtSVR = frtSVR

    def __get_tn_msg(self):
        return self.__tn_msg
    def __get_n_msg (self):
        return self.__n_msg
    def __get_n_sv  (self):
        return self.__n_sv
    def __get_sv_PNR(self):
        return self.__sv_PNR
    def __get_elev  (self):
        return self.__elev
    def __get_azim  (self):
        return self.__azim
    def __get_SNR   (self):
        return self.__SNR
    def __get_secSVR(self):
        return self.__secSVR
    def __get_thrSVR(self):
        return self.__thrSVR
    def __get_frtSVR(self):
        return self.__frtSVR

    tn_msg = property(__get_tn_msg, __set_tn_msg )
    n_msg = property(__get_n_msg , __set_n_msg  )
    n_sv  = property(__get_n_sv  , __set_n_sv   )
    sv_PNR = property(__get_sv_PNR, __set_sv_PNR )
    elev  = property(__get_elev  , __set_elev   )
    azim  = property(__get_azim  , __set_azim   )
    SNR   = property(__get_SNR   , __set_SNR    )
    secSVR = property(__get_secSVR, __set_secSVR )
    thrSVR = property(__get_thrSVR, __set_thrSVR )
    frtSVR = property(__get_frtSVR, __set_frtSVR )

class GPRMC(object):
    def __init__(self):
        self.__t_fix     = ""
        self.__d_stat    = ""
        self.__lat       = ""
        self.__lat2      = ""
        self.__lng       = ""
        self.__lng2      = ""
        self.__speed_knt = ""
        self.__t_course  = ""
        self.__date_fix  = ""
        self.__mag_var   = ""
        self.__mag_var2  = ""
        self.__checksum  = ""
        
    def __set_t_fix    (self,t_fix )   :
        self.__t_fix = t_fix
    def __set_d_stat   (self,d_stat)   :
        self.__d_stat = d_stat
    def __set_lat      (self,lat )     :
        self.__lat = lat
    def __set_lat2     (self,lat2)     :
        self.__lat2 = lat2
    def __set_lng      (self,lng )     :
        self.__lng = lng
    def __set_lng2     (self,lng2 )    :
        self.__lng2 = lng2
    def __set_speed_knt(self,speed_knt):
        self.__speed_knt = speed_knt
    def __set_t_course (self,t_course ):
        self.__t_course = t_course
    def __set_date_fix (self,date_fix ):
        self.__date_fix = date_fix
    def __set_mag_var  (self,mag_var)  :
        self.__mag_var = mag_var
    def __set_mag_var2 (self,mag_var2) :
        self.__mag_var2 = mag_var2
    def __set_checksum (self,checksum) :
        self.__checksum = checksum

    def __get_t_fix    (self):
        return self.__t_fix  
    def __get_d_stat   (self):
        return self.__d_stat 
    def __get_lat      (self):
        return self.__lat
    def __get_lat2     (self):
        return self.__lat2
    def __get_lng      (self):
        return self.__lng
    def __get_lng2     (self):
        return self.__lng2
    def __get_speed_knt(self):
        return self.__speed_knt
    def __get_t_course (self):
        return self.__t_course
    def __get_date_fix (self):
        return self.__date_fix
    def __get_mag_var  (self):
        return self.__mag_var
    def __get_mag_var2 (self):
        return self.__mag_var2
    def __get_checksum (self):
        return self.__checksum

    t_fix     = property (__get_t_fix    , __set_t_fix    )
    d_stat    = property (__get_d_stat   , __set_d_stat   )
    lat       = property (__get_lat      , __set_lat      )
    lat2      = property (__get_lat2     , __set_lat2     )
    lng       = property (__get_lng      , __set_lng      )
    lng2      = property (__get_lng2     , __set_lng2     )
    speed_knt = property (__get_speed_knt, __set_speed_knt)
    t_course  = property (__get_t_course , __set_t_course )
    date_fix  = property (__get_date_fix , __set_date_fix )
    mag_var   = property (__get_mag_var  , __set_mag_var  )
    mag_var2  = property (__get_mag_var2 , __set_mag_var2 )
    checksum  = property (__get_checksum , __set_checksum )

class GPVTG(object):
    def __init__(self):
        self.__track      = ""
        self.__track2     = ""
        self.__empty1     = ""
        self.__empty2     = ""
        self.__speed_knt  = ""
        self.__speed_knt2 = ""
        self.__speed_km   = ""
        self.__speed_km2  = ""
        self.__checksum   = ""
        
    def __set_track      (self,track      ):
        self.__track = track
    def __set_track2     (self,track2     ):
        self.__track2 = track2
    def __set_empty1     (self,empty1     ):
        self.__empty1 = empty1
    def __set_empty2     (self,empty2     ):
        self.__empty2 = empty2
    def __set_speed_knt  (self,speed_knt  ):
        self.__speed_knt = speed_knt
    def __set_speed_knt2 (self,speed_knt2):
        self.__speed_knt2 = speed_knt2
    def __set_speed_km   (self,speed_km   ):
        self.__speed_km = speed_km
    def __set_speed_km2  (self,speed_km2  ):
        self.__speed_km2 = speed_km2
    def __set_checksum   (self,checksum   ):
        self.__checksum  = checksum    
        
    def __get_track      (self):
        return self.__track
    def __get_track2     (self):
        return self.__track2
    def __get_empty1     (self):
        return self.__empty1
    def __get_empty2     (self):
        return self.__empty2
    def __get_speed_knt  (self):
        return self.__speed_knt
    def __get_speed_knt2 (self):
        return self.__speed_knt2
    def __get_speed_km   (self):
        return self.__speed_km
    def __get_speed_km2  (self):
        return self.__speed_km2
    def __get_checksum   (self):
        return self.__checksum

    track      = property(__get_track     ,__set_track     )
    track2     = property(__get_track2    ,__set_track2    )
    empty1     = property(__get_empty1    ,__set_empty1    )
    empty2     = property(__get_empty2    ,__set_empty2    )
    speed_knt  = property(__get_speed_knt ,__set_speed_knt )
    speed_knt2 = property(__get_speed_knt2,__set_speed_knt2)
    speed_km   = property(__get_speed_km  ,__set_speed_km  )
    speed_km2  = property(__get_speed_km2 ,__set_speed_km2 )
    checksum   = property(__get_checksum  ,__set_checksum  )
