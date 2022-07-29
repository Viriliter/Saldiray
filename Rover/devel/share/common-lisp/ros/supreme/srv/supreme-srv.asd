
(cl:in-package :asdf)

(defsystem "supreme-srv"
  :depends-on (:roslisp-msg-protocol :roslisp-utils )
  :components ((:file "_package")
    (:file "AddTwoInts" :depends-on ("_package_AddTwoInts"))
    (:file "_package_AddTwoInts" :depends-on ("_package"))
    (:file "TrialService" :depends-on ("_package_TrialService"))
    (:file "_package_TrialService" :depends-on ("_package"))
    (:file "reset_odometry" :depends-on ("_package_reset_odometry"))
    (:file "_package_reset_odometry" :depends-on ("_package"))
    (:file "reset_tracking" :depends-on ("_package_reset_tracking"))
    (:file "_package_reset_tracking" :depends-on ("_package"))
    (:file "set_initial_pose" :depends-on ("_package_set_initial_pose"))
    (:file "_package_set_initial_pose" :depends-on ("_package"))
    (:file "yedeksrv" :depends-on ("_package_yedeksrv"))
    (:file "_package_yedeksrv" :depends-on ("_package"))
  ))