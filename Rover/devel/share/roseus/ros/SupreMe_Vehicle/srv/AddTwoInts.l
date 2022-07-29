;; Auto-generated. Do not edit!


(when (boundp 'SupreMe_Vehicle::AddTwoInts)
  (if (not (find-package "SUPREME_VEHICLE"))
    (make-package "SUPREME_VEHICLE"))
  (shadow 'AddTwoInts (find-package "SUPREME_VEHICLE")))
(unless (find-package "SUPREME_VEHICLE::ADDTWOINTS")
  (make-package "SUPREME_VEHICLE::ADDTWOINTS"))
(unless (find-package "SUPREME_VEHICLE::ADDTWOINTSREQUEST")
  (make-package "SUPREME_VEHICLE::ADDTWOINTSREQUEST"))
(unless (find-package "SUPREME_VEHICLE::ADDTWOINTSRESPONSE")
  (make-package "SUPREME_VEHICLE::ADDTWOINTSRESPONSE"))

(in-package "ROS")





(defclass SupreMe_Vehicle::AddTwoIntsRequest
  :super ros::object
  :slots (_a _b ))

(defmethod SupreMe_Vehicle::AddTwoIntsRequest
  (:init
   (&key
    ((:a __a) 0)
    ((:b __b) 0)
    )
   (send-super :init)
   (setq _a (round __a))
   (setq _b (round __b))
   self)
  (:a
   (&optional __a)
   (if __a (setq _a __a)) _a)
  (:b
   (&optional __b)
   (if __b (setq _b __b)) _b)
  (:serialization-length
   ()
   (+
    ;; int64 _a
    8
    ;; int64 _b
    8
    ))
  (:serialize
   (&optional strm)
   (let ((s (if strm strm
              (make-string-output-stream (send self :serialization-length)))))
     ;; int64 _a
#+(or :alpha :irix6 :x86_64)
       (progn (sys::poke _a (send s :buffer) (send s :count) :long) (incf (stream-count s) 8))
#-(or :alpha :irix6 :x86_64)
       (cond ((and (class _a) (= (length (_a . bv)) 2)) ;; bignum
              (write-long (ash (elt (_a . bv) 0) 0) s)
              (write-long (ash (elt (_a . bv) 1) -1) s))
             ((and (class _a) (= (length (_a . bv)) 1)) ;; big1
              (write-long (elt (_a . bv) 0) s)
              (write-long (if (>= _a 0) 0 #xffffffff) s))
             (t                                         ;; integer
              (write-long _a s)(write-long (if (>= _a 0) 0 #xffffffff) s)))
     ;; int64 _b
#+(or :alpha :irix6 :x86_64)
       (progn (sys::poke _b (send s :buffer) (send s :count) :long) (incf (stream-count s) 8))
#-(or :alpha :irix6 :x86_64)
       (cond ((and (class _b) (= (length (_b . bv)) 2)) ;; bignum
              (write-long (ash (elt (_b . bv) 0) 0) s)
              (write-long (ash (elt (_b . bv) 1) -1) s))
             ((and (class _b) (= (length (_b . bv)) 1)) ;; big1
              (write-long (elt (_b . bv) 0) s)
              (write-long (if (>= _b 0) 0 #xffffffff) s))
             (t                                         ;; integer
              (write-long _b s)(write-long (if (>= _b 0) 0 #xffffffff) s)))
     ;;
     (if (null strm) (get-output-stream-string s))))
  (:deserialize
   (buf &optional (ptr- 0))
   ;; int64 _a
#+(or :alpha :irix6 :x86_64)
      (setf _a (prog1 (sys::peek buf ptr- :long) (incf ptr- 8)))
#-(or :alpha :irix6 :x86_64)
      (setf _a (let ((b0 (prog1 (sys::peek buf ptr- :integer) (incf ptr- 4)))
                  (b1 (prog1 (sys::peek buf ptr- :integer) (incf ptr- 4))))
              (cond ((= b1 -1) b0)
                     ((and (= b1  0)
                           (<= lisp::most-negative-fixnum b0 lisp::most-positive-fixnum))
                      b0)
                    ((= b1  0) (make-instance bignum :size 1 :bv (integer-vector b0)))
                    (t (make-instance bignum :size 2 :bv (integer-vector b0 (ash b1 1)))))))
   ;; int64 _b
#+(or :alpha :irix6 :x86_64)
      (setf _b (prog1 (sys::peek buf ptr- :long) (incf ptr- 8)))
#-(or :alpha :irix6 :x86_64)
      (setf _b (let ((b0 (prog1 (sys::peek buf ptr- :integer) (incf ptr- 4)))
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

(defclass SupreMe_Vehicle::AddTwoIntsResponse
  :super ros::object
  :slots (_name ))

(defmethod SupreMe_Vehicle::AddTwoIntsResponse
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

(defclass SupreMe_Vehicle::AddTwoInts
  :super ros::object
  :slots ())

(setf (get SupreMe_Vehicle::AddTwoInts :md5sum-) "7727609bad06c3920b54439f5d0595ec")
(setf (get SupreMe_Vehicle::AddTwoInts :datatype-) "SupreMe_Vehicle/AddTwoInts")
(setf (get SupreMe_Vehicle::AddTwoInts :request) SupreMe_Vehicle::AddTwoIntsRequest)
(setf (get SupreMe_Vehicle::AddTwoInts :response) SupreMe_Vehicle::AddTwoIntsResponse)

(defmethod SupreMe_Vehicle::AddTwoIntsRequest
  (:response () (instance SupreMe_Vehicle::AddTwoIntsResponse :init)))

(setf (get SupreMe_Vehicle::AddTwoIntsRequest :md5sum-) "7727609bad06c3920b54439f5d0595ec")
(setf (get SupreMe_Vehicle::AddTwoIntsRequest :datatype-) "SupreMe_Vehicle/AddTwoIntsRequest")
(setf (get SupreMe_Vehicle::AddTwoIntsRequest :definition-)
      "int64 a
int64 b
---
string name

")

(setf (get SupreMe_Vehicle::AddTwoIntsResponse :md5sum-) "7727609bad06c3920b54439f5d0595ec")
(setf (get SupreMe_Vehicle::AddTwoIntsResponse :datatype-) "SupreMe_Vehicle/AddTwoIntsResponse")
(setf (get SupreMe_Vehicle::AddTwoIntsResponse :definition-)
      "int64 a
int64 b
---
string name

")



(provide :SupreMe_Vehicle/AddTwoInts "7727609bad06c3920b54439f5d0595ec")


