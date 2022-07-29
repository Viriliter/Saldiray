; Auto-generated. Do not edit!


(cl:in-package supreme-msg)


;//! \htmlinclude msgstring.msg.html

(cl:defclass <msgstring> (roslisp-msg-protocol:ros-message)
  ((msgtext
    :reader msgtext
    :initarg :msgtext
    :type cl:string
    :initform ""))
)

(cl:defclass msgstring (<msgstring>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <msgstring>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'msgstring)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name supreme-msg:<msgstring> is deprecated: use supreme-msg:msgstring instead.")))

(cl:ensure-generic-function 'msgtext-val :lambda-list '(m))
(cl:defmethod msgtext-val ((m <msgstring>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader supreme-msg:msgtext-val is deprecated.  Use supreme-msg:msgtext instead.")
  (msgtext m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <msgstring>) ostream)
  "Serializes a message object of type '<msgstring>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'msgtext))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'msgtext))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <msgstring>) istream)
  "Deserializes a message object of type '<msgstring>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'msgtext) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'msgtext) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<msgstring>)))
  "Returns string type for a message object of type '<msgstring>"
  "supreme/msgstring")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'msgstring)))
  "Returns string type for a message object of type 'msgstring"
  "supreme/msgstring")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<msgstring>)))
  "Returns md5sum for a message object of type '<msgstring>"
  "bf820a8e464d1831c7daa93d2905b133")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'msgstring)))
  "Returns md5sum for a message object of type 'msgstring"
  "bf820a8e464d1831c7daa93d2905b133")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<msgstring>)))
  "Returns full string definition for message of type '<msgstring>"
  (cl:format cl:nil "string msgtext~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'msgstring)))
  "Returns full string definition for message of type 'msgstring"
  (cl:format cl:nil "string msgtext~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <msgstring>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'msgtext))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <msgstring>))
  "Converts a ROS message object to a list"
  (cl:list 'msgstring
    (cl:cons ':msgtext (msgtext msg))
))
