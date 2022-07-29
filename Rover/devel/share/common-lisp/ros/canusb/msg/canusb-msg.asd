
(cl:in-package :asdf)

(defsystem "canusb-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "CAN" :depends-on ("_package_CAN"))
    (:file "_package_CAN" :depends-on ("_package"))
  ))