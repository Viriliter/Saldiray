;; Auto-generated. Do not edit!


(when (boundp 'SupreMe_Vehicle::msgstring)
  (if (not (find-package "SUPREME_VEHICLE"))
    (make-package "SUPREME_VEHICLE"))
  (shadow 'msgstring (find-package "SUPREME_VEHICLE")))
(unless (find-package "SUPREME_VEHICLE::MSGSTRING")
  (make-package "SUPREME_VEHICLE::MSGSTRING"))

(in-package "ROS")
;;//! \htmlinclude msgstring.msg.html


(defclass SupreMe_Vehicle::msgstring
  :super ros::object
  :slots (_msgtext ))

(defmethod SupreMe_Vehicle::msgstring
  (:init
   (&key
    ((:msgtext __msgtext) "")
    )
   (send-super :init)
   (setq _msgtext (string __msgtext))
   self)
  (:msgtext
   (&optional __msgtext)
   (if __msgtext (setq _msgtext __msgtext)) _msgtext)
  (:serialization-length
   ()
   (+
    ;; string _msgtext
    4 (length _msgtext)
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; string _msgtext
       (write-long (length _msgtext) s) (princ _msgtext s)
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; string _msgtext
     (let (n) (setq n (sys::peek buf ptr- :integer)) (incf ptr- 4) (setq _msgtext (subseq buf ptr- (+ ptr- n))) (incf ptr- n))
   ;;
   self)
  )

(setf (get SupreMe_Vehicle::msgstring :md5sum-) "bf820a8e464d1831c7daa93d2905b133")
(setf (get SupreMe_Vehicle::msgstring :datatype-) "SupreMe_Vehicle/msgstring")
(setf (get SupreMe_Vehicle::msgstring :definition-)
      "string msgtext

")



(provide :SupreMe_Vehicle/msgstring "bf820a8e464d1831c7daa93d2905b133")


