;; Auto-generated. Do not edit!


(when (boundp 'supreme::msgstring)
  (if (not (find-package "SUPREME"))
    (make-package "SUPREME"))
  (shadow 'msgstring (find-package "SUPREME")))
(unless (find-package "SUPREME::MSGSTRING")
  (make-package "SUPREME::MSGSTRING"))

(in-package "ROS")
;;//! \htmlinclude msgstring.msg.html


(defclass supreme::msgstring
  :super ros::object
  :slots (_msgtext ))

(defmethod supreme::msgstring
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

(setf (get supreme::msgstring :md5sum-) "bf820a8e464d1831c7daa93d2905b133")
(setf (get supreme::msgstring :datatype-) "supreme/msgstring")
(setf (get supreme::msgstring :definition-)
      "string msgtext

")



(provide :supreme/msgstring "bf820a8e464d1831c7daa93d2905b133")


