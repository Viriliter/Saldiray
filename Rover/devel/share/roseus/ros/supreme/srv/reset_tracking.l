;; Auto-generated. Do not edit!


(when (boundp 'supreme::reset_tracking)
  (if (not (find-package "SUPREME"))
    (make-package "SUPREME"))
  (shadow 'reset_tracking (find-package "SUPREME")))
(unless (find-package "SUPREME::RESET_TRACKING")
  (make-package "SUPREME::RESET_TRACKING"))
(unless (find-package "SUPREME::RESET_TRACKINGREQUEST")
  (make-package "SUPREME::RESET_TRACKINGREQUEST"))
(unless (find-package "SUPREME::RESET_TRACKINGRESPONSE")
  (make-package "SUPREME::RESET_TRACKINGRESPONSE"))

(in-package "ROS")





(defclass supreme::reset_trackingRequest
  :super ros::object
  :slots ())

(defmethod supreme::reset_trackingRequest
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

(defclass supreme::reset_trackingResponse
  :super ros::object
  :slots (_reset_done ))

(defmethod supreme::reset_trackingResponse
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

(defclass supreme::reset_tracking
  :super ros::object
  :slots ())

(setf (get supreme::reset_tracking :md5sum-) "e1e87fc9e9e6c154eca93b9fa292cc05")
(setf (get supreme::reset_tracking :datatype-) "supreme/reset_tracking")
(setf (get supreme::reset_tracking :request) supreme::reset_trackingRequest)
(setf (get supreme::reset_tracking :response) supreme::reset_trackingResponse)

(defmethod supreme::reset_trackingRequest
  (:response () (instance supreme::reset_trackingResponse :init)))

(setf (get supreme::reset_trackingRequest :md5sum-) "e1e87fc9e9e6c154eca93b9fa292cc05")
(setf (get supreme::reset_trackingRequest :datatype-) "supreme/reset_trackingRequest")
(setf (get supreme::reset_trackingRequest :definition-)
      "---
bool reset_done
")

(setf (get supreme::reset_trackingResponse :md5sum-) "e1e87fc9e9e6c154eca93b9fa292cc05")
(setf (get supreme::reset_trackingResponse :datatype-) "supreme/reset_trackingResponse")
(setf (get supreme::reset_trackingResponse :definition-)
      "---
bool reset_done
")



(provide :supreme/reset_tracking "e1e87fc9e9e6c154eca93b9fa292cc05")


