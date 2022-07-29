; Auto-generated. Do not edit!


(cl:in-package canusb-msg)


;//! \htmlinclude CAN.msg.html

(cl:defclass <CAN> (roslisp-msg-protocol:ros-message)
  ((timestamp
    :reader timestamp
    :initarg :timestamp
    :type cl:real
    :initform 0)
   (stdId
    :reader stdId
    :initarg :stdId
    :type cl:fixnum
    :initform 0)
   (extId
    :reader extId
    :initarg :extId
    :type cl:integer
    :initform 0)
   (data
    :reader data
    :initarg :data
    :type (cl:vector cl:fixnum)
   :initform (cl:make-array 0 :element-type 'cl:fixnum :initial-element 0)))
)

(cl:defclass CAN (<CAN>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <CAN>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'CAN)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name canusb-msg:<CAN> is deprecated: use canusb-msg:CAN instead.")))

(cl:ensure-generic-function 'timestamp-val :lambda-list '(m))
(cl:defmethod timestamp-val ((m <CAN>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader canusb-msg:timestamp-val is deprecated.  Use canusb-msg:timestamp instead.")
  (timestamp m))

(cl:ensure-generic-function 'stdId-val :lambda-list '(m))
(cl:defmethod stdId-val ((m <CAN>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader canusb-msg:stdId-val is deprecated.  Use canusb-msg:stdId instead.")
  (stdId m))

(cl:ensure-generic-function 'extId-val :lambda-list '(m))
(cl:defmethod extId-val ((m <CAN>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader canusb-msg:extId-val is deprecated.  Use canusb-msg:extId instead.")
  (extId m))

(cl:ensure-generic-function 'data-val :lambda-list '(m))
(cl:defmethod data-val ((m <CAN>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader canusb-msg:data-val is deprecated.  Use canusb-msg:data instead.")
  (data m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <CAN>) ostream)
  "Serializes a message object of type '<CAN>"
  (cl:let ((__sec (cl:floor (cl:slot-value msg 'timestamp)))
        (__nsec (cl:round (cl:* 1e9 (cl:- (cl:slot-value msg 'timestamp) (cl:floor (cl:slot-value msg 'timestamp)))))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __sec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __sec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __sec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __sec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 0) __nsec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __nsec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __nsec) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __nsec) ostream))
  (cl:write-byte (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'stdId)) ostream)
  (cl:write-byte (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'stdId)) ostream)
  (cl:let* ((signed (cl:slot-value msg 'extId)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 4294967296) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) unsigned) ostream)
    )
  (cl:let ((__ros_arr_len (cl:length (cl:slot-value msg 'data))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_arr_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_arr_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_arr_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_arr_len) ostream))
  (cl:map cl:nil #'(cl:lambda (ele) (cl:write-byte (cl:ldb (cl:byte 8 0) ele) ostream))
   (cl:slot-value msg 'data))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <CAN>) istream)
  "Deserializes a message object of type '<CAN>"
    (cl:let ((__sec 0) (__nsec 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __sec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __sec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __sec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __sec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 0) __nsec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __nsec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __nsec) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __nsec) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'timestamp) (cl:+ (cl:coerce __sec 'cl:double-float) (cl:/ __nsec 1e9))))
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:slot-value msg 'stdId)) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) (cl:slot-value msg 'stdId)) (cl:read-byte istream))
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'extId) (cl:if (cl:< unsigned 2147483648) unsigned (cl:- unsigned 4294967296))))
  (cl:let ((__ros_arr_len 0))
    (cl:setf (cl:ldb (cl:byte 8 0) __ros_arr_len) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 8) __ros_arr_len) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 16) __ros_arr_len) (cl:read-byte istream))
    (cl:setf (cl:ldb (cl:byte 8 24) __ros_arr_len) (cl:read-byte istream))
  (cl:setf (cl:slot-value msg 'data) (cl:make-array __ros_arr_len))
  (cl:let ((vals (cl:slot-value msg 'data)))
    (cl:dotimes (i __ros_arr_len)
    (cl:setf (cl:ldb (cl:byte 8 0) (cl:aref vals i)) (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<CAN>)))
  "Returns string type for a message object of type '<CAN>"
  "canusb/CAN")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'CAN)))
  "Returns string type for a message object of type 'CAN"
  "canusb/CAN")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<CAN>)))
  "Returns md5sum for a message object of type '<CAN>"
  "538459a17b716b5d40481761364b2a7c")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'CAN)))
  "Returns md5sum for a message object of type 'CAN"
  "538459a17b716b5d40481761364b2a7c")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<CAN>)))
  "Returns full string definition for message of type '<CAN>"
  (cl:format cl:nil "time timestamp~%uint16 stdId~%int32 extId~%uint8[] data~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'CAN)))
  "Returns full string definition for message of type 'CAN"
  (cl:format cl:nil "time timestamp~%uint16 stdId~%int32 extId~%uint8[] data~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <CAN>))
  (cl:+ 0
     8
     2
     4
     4 (cl:reduce #'cl:+ (cl:slot-value msg 'data) :key #'(cl:lambda (ele) (cl:declare (cl:ignorable ele)) (cl:+ 1)))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <CAN>))
  "Converts a ROS message object to a list"
  (cl:list 'CAN
    (cl:cons ':timestamp (timestamp msg))
    (cl:cons ':stdId (stdId msg))
    (cl:cons ':extId (extId msg))
    (cl:cons ':data (data msg))
))
