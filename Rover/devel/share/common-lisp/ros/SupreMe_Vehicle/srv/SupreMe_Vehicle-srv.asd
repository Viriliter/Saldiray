
(cl:in-package :asdf)

(defsystem "SupreMe_Vehicle-srv"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "AddTwoInts" :depends-on ("_package_AddTwoInts"))
    (:file "_package_AddTwoInts" :depends-on ("_package"))
    (:file "TrialService" :depends-on ("_package_TrialService"))
    (:file "_package_TrialService" :depends-on ("_package"))
    (:file "yedeksrv" :depends-on ("_package_yedeksrv"))
    (:file "_package_yedeksrv" :depends-on ("_package"))
  ))