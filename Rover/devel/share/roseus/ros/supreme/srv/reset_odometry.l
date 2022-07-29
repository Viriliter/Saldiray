;; Auto-generated. Do not edit!


(when (boundp 'supreme::reset_odometry)
  (if (not (find-package "SUPREME"))
    (make-package "SUPREME"))
  (shadow 'reset_odometry (find-package "SUPREME")))
(unless (find-package "SUPREME::RESET_ODOMETRY")
  (make-package "SUPREME::RESET_ODOMETRY"))
(unless (find-package "SUPREME::RESET_ODOMETRYREQUEST")
  (make-package "SUPREME::RESET_ODOMETRYREQUEST"))
(unless (find-package "SUPREME::RESET_ODOMETRYRESPONSE")
  (make-package "SUPREME::RESET_ODOMETRYRESPONSE"))

(in-package "ROS")





(defclass supreme::reset_odometryRequest
  :super ros::object
  :slots ())

(defmethod supreme::reset_odometryRequest
  (:init
   (&key
    )
   (send-super :init)
   self)
  (:serialization-length
   ()
   (+
    0
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;;
   self)
  )

(defclass supreme::reset_odometryResponse
  :super ros::object
  :slots (_reset_done ))

(defmethod supreme::reset_odometryResponse
  (:init
   (&key
    ((:reset_done __reset_done) nil)
    )
   (send-super :init)
   (setq _reset_done __reset_done)
   self)
  (:reset_done
   (&optional __reset_done)
   (if __reset_done (setq _reset_done __reset_done)) _reset_done)
  (:serialization-length
   ()
   (+
    ;; bool _reset_done
    1
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; bool _reset_done
       (if _reset_done (write-byte -1 s) (write-byte 0 s))
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; bool _reset_done
     (setq _reset_done (not (= 0 (sys::peek buf ptr- :char)))) (incf ptr- 1)
   ;;
   self)
  )

(defclass supreme::reset_odometry
  :super ros::object
  :slots ())

(setf (get supreme::reset_odometry :md5sum-) "e1e87fc9e9e6c154eca93b9fa292cc05")
(setf (get supreme::reset_odometry :datatype-) "supreme/reset_odometry")
(setf (get supreme::reset_odometry :request) supreme::reset_odometryRequest)
(setf (get supreme::reset_odometry :response) supreme::reset_odometryResponse)

(defmethod supreme::reset_odometryRequest
  (:response () (instance supreme::reset_odometryResponse :init)))

(setf (get supreme::reset_odometryRequest :md5sum-) "e1e87fc9e9e6c154eca93b9fa292cc05")
(setf (get supreme::reset_odometryRequest :datatype-) "supreme/reset_odometryRequest")
(setf (get supreme::reset_odometryRequest :definition-)
      "---
bool reset_done
")

(setf (get supreme::reset_odometryResponse :md5sum-) "e1e87fc9e9e6c154eca93b9fa292cc05")
(setf (get supreme::reset_odometryResponse :datatype-) "supreme/reset_odometryResponse")
(setf (get supreme::reset_odometryResponse :definition-)
      "---
bool reset_done
")



(provide :supreme/reset_odometry "e1e87fc9e9e6c154eca93b9fa292cc05")


