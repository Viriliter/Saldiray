
(cl:in-package :asdf)

(defsystem "supreme-msg"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "Num" :depends-on ("_package_Num"))
    (:file "_package_Num" :depends-on ("_package"))
    (:file "msgstring" :depends-on ("_package_msgstring"))
    (:file "_package_msgstring" :depends-on ("_package"))
  ))