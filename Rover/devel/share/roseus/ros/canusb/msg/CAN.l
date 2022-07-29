;; Auto-generated. Do not edit!


(when (boundp 'canusb::CAN)
  (if (not (find-package "CANUSB"))
    (make-package "CANUSB"))
  (shadow 'CAN (find-package "CANUSB")))
(unless (find-package "CANUSB::CAN")
  (make-package "CANUSB::CAN"))

(in-package "ROS")
;;//! \htmlinclude CAN.msg.html


(defclass canusb::CAN
  :super ros::object
  :slots (_timestamp _stdId _extId _data ))

(defmethod canusb::CAN
  (:init
   (&key
    ((:timestamp __timestamp) (instance ros::time :init))
    ((:stdId __stdId) 0)
    ((:extId __extId) 0)
    ((:data __data) (make-array 0 :initial-element 0 :element-type :char))
    )
   (send-super :init)
   (setq _timestamp __timestamp)
   (setq _stdId (round __stdId))
   (setq _extId (round __extId))
   (setq _data __data)
   self)
  (:timestamp
   (&optional __timestamp)
   (if __timestamp (setq _timestamp __timestamp)) _timestamp)
  (:stdId
   (&optional __stdId)
   (if __stdId (setq _stdId __stdId)) _stdId)
  (:extId
   (&optional __extId)
   (if __extId (setq _extId __extId)) _extId)
  (:data
   (&optional __data)
   (if __data (setq _data __data)) _data)
  (:serialization-length
   ()
   (+
    ;; time _timestamp
    8
    ;; uint16 _stdId
    2
    ;; int32 _extId
    4
    ;; uint8[] _data
    (* 1    (length _data)) 4
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; time _timestamp
       (write-long (send _timestamp :sec) s) (write-long (send _timestamp :nsec) s)
     ;; uint16 _stdId
       (write-word _stdId s)
     ;; int32 _extId
       (write-long _extId s)
     ;; uint8[] _data
     (write-long (length _data) s)
     (princ _data s)
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; time _timestamp
     (send _timestamp :sec (sys::peek buf ptr- :integer)) (incf ptr- 4)  (send _timestamp :nsec (sys::peek buf ptr- :integer)) (incf ptr- 4)
   ;; uint16 _stdId
     (setq _stdId (sys::peek buf ptr- :short)) (incf ptr- 2)
   ;; int32 _extId
     (setq _extId (sys::peek buf ptr- :integer)) (incf ptr- 4)
   ;; uint8[] _data
   (let ((n (sys::peek buf ptr- :integer))) (incf ptr- 4)
     (setq _data (make-array n :element-type :char))
     (replace _data buf :start2 ptr-) (incf ptr- n))
   ;;
   self)
  )

(setf (get canusb::CAN :md5sum-) "538459a17b716b5d40481761364b2a7c")
(setf (get canusb::CAN :datatype-) "canusb/CAN")
(setf (get canusb::CAN :definition-)
      "time timestamp
uint16 stdId
int32 extId
uint8[] data

")



(provide :canusb/CAN "538459a17b716b5d40481761364b2a7c")


