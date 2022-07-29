; Auto-generated. Do not edit!


(cl:in-package SupreMe_Vehicle-srv)


;//! \htmlinclude TrialService-request.msg.html

(cl:defclass <TrialService-request> (roslisp-msg-protocol:ros-message)
  ((user
    :reader user
    :initarg :user
    :type cl:string
    :initform "")
   (state
    :reader state
    :initarg :state
    :type cl:integer
    :initform 0))
)

(cl:defclass TrialService-request (<TrialService-request>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <TrialService-request>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'TrialService-request)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name SupreMe_Vehicle-srv:<TrialService-request> is deprecated: use SupreMe_Vehicle-srv:TrialService-request instead.")))

(cl:ensure-generic-function 'user-val :lambda-list '(m))
(cl:defmethod user-val ((m <TrialService-request>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader SupreMe_Vehicle-srv:user-val is deprecated.  Use SupreMe_Vehicle-srv:user instead.")
  (user m))

(cl:ensure-generic-function 'state-val :lambda-list '(m))
(cl:defmethod state-val ((m <TrialService-request>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader SupreMe_Vehicle-srv:state-val is deprecated.  Use SupreMe_Vehicle-srv:state instead.")
  (state m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <TrialService-request>) ostream)
  "Serializes a message object of type '<TrialService-request>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'user))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'user))
  (cl:let* ((signed (cl:slot-value msg 'state)) (unsigned (cl:if (cl:< signed 0) (cl:+ signed 18446744073709551616) signed)))
    (cl:write-byte (cl:ldb (cl:byte 8 0) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 32) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 40) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 48) unsigned) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 56) unsigned) ostream)
    )
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <TrialService-request>) istream)
  "Deserializes a message object of type '<TrialService-request>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'user) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'user) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
    (cl:let ((unsigned 0))
      (cl:setf (cl:ldb (cl:byte 8 0) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 32) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 40) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 48) unsigned) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 56) unsigned) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'state) (cl:if (cl:< unsigned 9223372036854775808) unsigned (cl:- unsigned 18446744073709551616))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<TrialService-request>)))
  "Returns string type for a service object of type '<TrialService-request>"
  "SupreMe_Vehicle/TrialServiceRequest")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'TrialService-request)))
  "Returns string type for a service object of type 'TrialService-request"
  "SupreMe_Vehicle/TrialServiceRequest")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<TrialService-request>)))
  "Returns md5sum for a message object of type '<TrialService-request>"
  "9f8b5f87d09954c41f9cec2b5c553306")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'TrialService-request)))
  "Returns md5sum for a message object of type 'TrialService-request"
  "9f8b5f87d09954c41f9cec2b5c553306")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<TrialService-request>)))
  "Returns full string definition for message of type '<TrialService-request>"
  (cl:format cl:nil "string user~%int64 state~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'TrialService-request)))
  "Returns full string definition for message of type 'TrialService-request"
  (cl:format cl:nil "string user~%int64 state~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <TrialService-request>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'user))
     8
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <TrialService-request>))
  "Converts a ROS message object to a list"
  (cl:list 'TrialService-request
    (cl:cons ':user (user msg))
    (cl:cons ':state (state msg))
))
;//! \htmlinclude TrialService-response.msg.html

(cl:defclass <TrialService-response> (roslisp-msg-protocol:ros-message)
  ((name
    :reader name
    :initarg :name
    :type cl:string
    :initform ""))
)

(cl:defclass TrialService-response (<TrialService-response>)
  ())

(cl:defmethod cl:initialize-instance :after ((m <TrialService-response>) cl:&rest args)
  (cl:declare (cl:ignorable args))
  (cl:unless (cl:typep m 'TrialService-response)
    (roslisp-msg-protocol:msg-deprecation-warning "using old message class name SupreMe_Vehicle-srv:<TrialService-response> is deprecated: use SupreMe_Vehicle-srv:TrialService-response instead.")))

(cl:ensure-generic-function 'name-val :lambda-list '(m))
(cl:defmethod name-val ((m <TrialService-response>))
  (roslisp-msg-protocol:msg-deprecation-warning "Using old-style slot reader SupreMe_Vehicle-srv:name-val is deprecated.  Use SupreMe_Vehicle-srv:name instead.")
  (name m))
(cl:defmethod roslisp-msg-protocol:serialize ((msg <TrialService-response>) ostream)
  "Serializes a message object of type '<TrialService-response>"
  (cl:let ((__ros_str_len (cl:length (cl:slot-value msg 'name))))
    (cl:write-byte (cl:ldb (cl:byte 8 0) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 8) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 16) __ros_str_len) ostream)
    (cl:write-byte (cl:ldb (cl:byte 8 24) __ros_str_len) ostream))
  (cl:map cl:nil #'(cl:lambda (c) (cl:write-byte (cl:char-code c) ostream)) (cl:slot-value msg 'name))
)
(cl:defmethod roslisp-msg-protocol:deserialize ((msg <TrialService-response>) istream)
  "Deserializes a message object of type '<TrialService-response>"
    (cl:let ((__ros_str_len 0))
      (cl:setf (cl:ldb (cl:byte 8 0) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 8) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 16) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:ldb (cl:byte 8 24) __ros_str_len) (cl:read-byte istream))
      (cl:setf (cl:slot-value msg 'name) (cl:make-string __ros_str_len))
      (cl:dotimes (__ros_str_idx __ros_str_len msg)
        (cl:setf (cl:char (cl:slot-value msg 'name) __ros_str_idx) (cl:code-char (cl:read-byte istream)))))
  msg
)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql '<TrialService-response>)))
  "Returns string type for a service object of type '<TrialService-response>"
  "SupreMe_Vehicle/TrialServiceResponse")
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'TrialService-response)))
  "Returns string type for a service object of type 'TrialService-response"
  "SupreMe_Vehicle/TrialServiceResponse")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql '<TrialService-response>)))
  "Returns md5sum for a message object of type '<TrialService-response>"
  "9f8b5f87d09954c41f9cec2b5c553306")
(cl:defmethod roslisp-msg-protocol:md5sum ((type (cl:eql 'TrialService-response)))
  "Returns md5sum for a message object of type 'TrialService-response"
  "9f8b5f87d09954c41f9cec2b5c553306")
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql '<TrialService-response>)))
  "Returns full string definition for message of type '<TrialService-response>"
  (cl:format cl:nil "string name~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:message-definition ((type (cl:eql 'TrialService-response)))
  "Returns full string definition for message of type 'TrialService-response"
  (cl:format cl:nil "string name~%~%~%~%"))
(cl:defmethod roslisp-msg-protocol:serialization-length ((msg <TrialService-response>))
  (cl:+ 0
     4 (cl:length (cl:slot-value msg 'name))
))
(cl:defmethod roslisp-msg-protocol:ros-message-to-list ((msg <TrialService-response>))
  "Converts a ROS message object to a list"
  (cl:list 'TrialService-response
    (cl:cons ':name (name msg))
))
(cl:defmethod roslisp-msg-protocol:service-request-type ((msg (cl:eql 'TrialService)))
  'TrialService-request)
(cl:defmethod roslisp-msg-protocol:service-response-type ((msg (cl:eql 'TrialService)))
  'TrialService-response)
(cl:defmethod roslisp-msg-protocol:ros-datatype ((msg (cl:eql 'TrialService)))
  "Returns string type for a service object of type '<TrialService>"
  "SupreMe_Vehicle/TrialService")