# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "supreme: 2 messages, 5 services")

set(MSG_I_FLAGS "-Isupreme:/home/monster/catkin_ws/src/supreme/msg;-Istd_msgs:/opt/ros/kinetic/share/std_msgs/cmake/../msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(geneus REQUIRED)
find_package(genlisp REQUIRED)
find_package(gennodejs REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(supreme_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/Num.msg" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/msg/Num.msg" ""
)

get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" ""
)

get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" ""
)

get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" ""
)

get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" ""
)

get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" ""
)

get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" NAME_WE)
add_custom_target(_supreme_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "supreme" "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" ""
)

#
#  langs = gencpp;geneus;genlisp;gennodejs;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/msg/Num.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)
_generate_msg_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)

### Generating Services
_generate_srv_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)
_generate_srv_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)
_generate_srv_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)
_generate_srv_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)
_generate_srv_cpp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
)

### Generating Module File
_generate_module_cpp(supreme
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(supreme_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(supreme_generate_messages supreme_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/Num.msg" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" NAME_WE)
add_dependencies(supreme_generate_messages_cpp _supreme_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(supreme_gencpp)
add_dependencies(supreme_gencpp supreme_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS supreme_generate_messages_cpp)

### Section generating for lang: geneus
### Generating Messages
_generate_msg_eus(supreme
  "/home/monster/catkin_ws/src/supreme/msg/Num.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)
_generate_msg_eus(supreme
  "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)

### Generating Services
_generate_srv_eus(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)
_generate_srv_eus(supreme
  "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)
_generate_srv_eus(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)
_generate_srv_eus(supreme
  "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)
_generate_srv_eus(supreme
  "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
)

### Generating Module File
_generate_module_eus(supreme
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
  "${ALL_GEN_OUTPUT_FILES_eus}"
)

add_custom_target(supreme_generate_messages_eus
  DEPENDS ${ALL_GEN_OUTPUT_FILES_eus}
)
add_dependencies(supreme_generate_messages supreme_generate_messages_eus)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/Num.msg" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" NAME_WE)
add_dependencies(supreme_generate_messages_eus _supreme_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(supreme_geneus)
add_dependencies(supreme_geneus supreme_generate_messages_eus)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS supreme_generate_messages_eus)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/msg/Num.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)
_generate_msg_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)

### Generating Services
_generate_srv_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)
_generate_srv_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)
_generate_srv_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)
_generate_srv_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)
_generate_srv_lisp(supreme
  "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
)

### Generating Module File
_generate_module_lisp(supreme
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(supreme_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(supreme_generate_messages supreme_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/Num.msg" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" NAME_WE)
add_dependencies(supreme_generate_messages_lisp _supreme_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(supreme_genlisp)
add_dependencies(supreme_genlisp supreme_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS supreme_generate_messages_lisp)

### Section generating for lang: gennodejs
### Generating Messages
_generate_msg_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/msg/Num.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)
_generate_msg_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)

### Generating Services
_generate_srv_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)
_generate_srv_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)
_generate_srv_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)
_generate_srv_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)
_generate_srv_nodejs(supreme
  "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
)

### Generating Module File
_generate_module_nodejs(supreme
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
  "${ALL_GEN_OUTPUT_FILES_nodejs}"
)

add_custom_target(supreme_generate_messages_nodejs
  DEPENDS ${ALL_GEN_OUTPUT_FILES_nodejs}
)
add_dependencies(supreme_generate_messages supreme_generate_messages_nodejs)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/Num.msg" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" NAME_WE)
add_dependencies(supreme_generate_messages_nodejs _supreme_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(supreme_gennodejs)
add_dependencies(supreme_gennodejs supreme_generate_messages_nodejs)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS supreme_generate_messages_nodejs)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(supreme
  "/home/monster/catkin_ws/src/supreme/msg/Num.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)
_generate_msg_py(supreme
  "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)

### Generating Services
_generate_srv_py(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)
_generate_srv_py(supreme
  "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)
_generate_srv_py(supreme
  "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)
_generate_srv_py(supreme
  "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)
_generate_srv_py(supreme
  "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
)

### Generating Module File
_generate_module_py(supreme
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(supreme_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(supreme_generate_messages supreme_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/Num.msg" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/TrialService.srv" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/set_initial_pose.srv" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/msg/msgstring.msg" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_tracking.srv" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/reset_odometry.srv" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})
get_filename_component(_filename "/home/monster/catkin_ws/src/supreme/srv/AddTwoInts.srv" NAME_WE)
add_dependencies(supreme_generate_messages_py _supreme_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(supreme_genpy)
add_dependencies(supreme_genpy supreme_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS supreme_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/supreme
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_cpp)
  add_dependencies(supreme_generate_messages_cpp std_msgs_generate_messages_cpp)
endif()

if(geneus_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/supreme
    DESTINATION ${geneus_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_eus)
  add_dependencies(supreme_generate_messages_eus std_msgs_generate_messages_eus)
endif()

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/supreme
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_lisp)
  add_dependencies(supreme_generate_messages_lisp std_msgs_generate_messages_lisp)
endif()

if(gennodejs_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/supreme
    DESTINATION ${gennodejs_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_nodejs)
  add_dependencies(supreme_generate_messages_nodejs std_msgs_generate_messages_nodejs)
endif()

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme)
  install(CODE "execute_process(COMMAND \"/usr/bin/python\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/supreme
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_py)
  add_dependencies(supreme_generate_messages_py std_msgs_generate_messages_py)
endif()
