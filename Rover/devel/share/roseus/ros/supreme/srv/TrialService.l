;; Auto-generated. Do not edit!


(when (boundp 'supreme::TrialService)
  (if (not (find-package "SUPREME"))
    (make-package "SUPREME"))
  (shadow 'TrialService (find-package "SUPREME")))
(unless (find-package "SUPREME::TRIALSERVICE")
  (make-package "SUPREME::TRIALSERVICE"))
(unless (find-package "SUPREME::TRIALSERVICEREQUEST")
  (make-package "SUPREME::TRIALSERVICEREQUEST"))
(unless (find-package "SUPREME::TRIALSERVICERESPONSE")
  (make-package "SUPREME::TRIALSERVICERESPONSE"))

(in-package "ROS")





(defclass supreme::TrialServiceRequest
  :super ros::object
  :slots (_user _state ))

(defmethod supreme::TrialServiceRequest
  (:init
   (&key
    ((:user __user) "")
    ((:state __state) 0)
    )
   (send-super :init)
   (setq _user (string __user))
   (setq _state (round __state))
   self)
  (:user
   (&optional __user)
   (if __user (setq _user __user)) _user)
  (:state
   (&optional __state)
   (if __state (setq _state __state)) _state)
  (:serialization-length
   ()
   (+
    ;; string _user
    4 (length _user)
    ;; int64 _state
    8
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; string _user
       (write-long (length _user) s) (princ _user s)
     ;; int64 _state
#+(or :alpha :irix6 :x86_64)
       (progn (sys::poke _state (send s :buffer) (send s :count) :long) (incf (stream-count s) 8))
#-(or :alpha :irix6 :x86_64)
       (cond ((and (class _state) (= (length (_state . bv)) 2)) ;; bignum
              (write-long (ash (elt (_state . bv) 0) 0) s)
              (write-long (ash (elt (_state . bv) 1) -1) s))
             ((and (class _state) (= (length (_state . bv)) 1)) ;; big1
              (write-long (elt (_state . bv) 0) s)
              (write-long (if (>= _state 0) 0 #xffffffff) s))
             (t                                         ;; integer
              (write-long _state s)(write-long (if (>= _state 0) 0 #xffffffff) s)))
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; string _user
     (let (n) (setq n (sys::peek buf ptr- :integer)) (incf ptr- 4) (setq _user (subseq buf ptr- (+ ptr- n))) (incf ptr- n))
   ;; int64 _state
#+(or :alpha :irix6 :x86_64)
      (setf _state (prog1 (sys::peek buf ptr- :long) (incf ptr- 8)))
#-(or :alpha :irix6 :x86_64)
      (setf _state (let ((b0 (prog1 (sys::peek buf ptr- :integer) (incf ptr- 4)))
                  (b1 (prog1 (sys::peek buf ptr- :integer) (incf ptr- 4))))
              (cond ((= b1 -1) b0)
                     ((and (= b1  0)
                           (<= lisp::most-negative-fixnum b0 lisp::most-positive-fixnum))
                      b0)
                    ((= b1  0) (make-instance bignum :size 1 :bv (integer-vector b0)))
                    (t (make-instance bignum :size 2 :bv (integer-vector b0 (ash b1 1)))))))
   ;;
   self)
  )

(defclass supreme::TrialServiceResponse
  :super ros::object
  :slots (_name ))

(defmethod supreme::TrialServiceResponse
  (:init
   (&key
    ((:name __name) "")
    )
   (send-super :init)
   (setq _name (string __name))
   self)
  (:name
   (&optional __name)
   (if __name (setq _name __name)) _name)
  (:serialization-length
   ()
   (+
    ;; string _name
    4 (length _name)
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; string _name
       (write-long (length _name) s) (princ _name s)
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; string _name
     (let (n) (setq n (sys::peek buf ptr- :integer)) (incf ptr- 4) (setq _name (subseq buf ptr- (+ ptr- n))) (incf ptr- n))
   ;;
   self)
  )

(defclass supreme::TrialService
  :super ros::object
  :slots ())

(setf (get supreme::TrialService :md5sum-) "9f8b5f87d09954c41f9cec2b5c553306")
(setf (get supreme::TrialService :datatype-) "supreme/TrialService")
(setf (get supreme::TrialService :request) supreme::TrialServiceRequest)
(setf (get supreme::TrialService :response) supreme::TrialServiceResponse)

(defmethod supreme::TrialServiceRequest
  (:response () (instance supreme::TrialServiceResponse :init)))

(setf (get supreme::TrialServiceRequest :md5sum-) "9f8b5f87d09954c41f9cec2b5c553306")
(setf (get supreme::TrialServiceRequest :datatype-) "supreme/TrialServiceRequest")
(setf (get supreme::TrialServiceRequest :definition-)
      "string user
int64 state
---
string name

")

(setf (get supreme::TrialServiceResponse :md5sum-) "9f8b5f87d09954c41f9cec2b5c553306")
(setf (get supreme::TrialServiceResponse :datatype-) "supreme/TrialServiceResponse")
(setf (get supreme::TrialServiceResponse :definition-)
      "string user
int64 state
---
string name

")



(provide :supreme/TrialService "9f8b5f87d09954c41f9cec2b5c553306")


