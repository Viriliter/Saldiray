# generated from genmsg/cmake/pkg-genmsg.cmake.em

message(STATUS "canusb: 1 messages, 0 services")

set(MSG_I_FLAGS "-Icanusb:/home/monster/catkin_ws/src/canusb/msg;-Istd_msgs:/opt/ros/kinetic/share/std_msgs/cmake/../msg")

# Find all generators
find_package(gencpp REQUIRED)
find_package(geneus REQUIRED)
find_package(genlisp REQUIRED)
find_package(gennodejs REQUIRED)
find_package(genpy REQUIRED)

add_custom_target(canusb_generate_messages ALL)

# verify that message/service dependencies have not changed since configure



get_filename_component(_filename "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" NAME_WE)
add_custom_target(_canusb_generate_messages_check_deps_${_filename}
  COMMAND ${CATKIN_ENV} ${PYTHON_EXECUTABLE} ${GENMSG_CHECK_DEPS_SCRIPT} "canusb" "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" ""
)

#
#  langs = gencpp;geneus;genlisp;gennodejs;genpy
#

### Section generating for lang: gencpp
### Generating Messages
_generate_msg_cpp(canusb
  "/home/monster/catkin_ws/src/canusb/msg/CAN.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/canusb
)

### Generating Services

### Generating Module File
_generate_module_cpp(canusb
  ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/canusb
  "${ALL_GEN_OUTPUT_FILES_cpp}"
)

add_custom_target(canusb_generate_messages_cpp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_cpp}
)
add_dependencies(canusb_generate_messages canusb_generate_messages_cpp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" NAME_WE)
add_dependencies(canusb_generate_messages_cpp _canusb_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(canusb_gencpp)
add_dependencies(canusb_gencpp canusb_generate_messages_cpp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS canusb_generate_messages_cpp)

### Section generating for lang: geneus
### Generating Messages
_generate_msg_eus(canusb
  "/home/monster/catkin_ws/src/canusb/msg/CAN.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/canusb
)

### Generating Services

### Generating Module File
_generate_module_eus(canusb
  ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/canusb
  "${ALL_GEN_OUTPUT_FILES_eus}"
)

add_custom_target(canusb_generate_messages_eus
  DEPENDS ${ALL_GEN_OUTPUT_FILES_eus}
)
add_dependencies(canusb_generate_messages canusb_generate_messages_eus)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" NAME_WE)
add_dependencies(canusb_generate_messages_eus _canusb_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(canusb_geneus)
add_dependencies(canusb_geneus canusb_generate_messages_eus)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS canusb_generate_messages_eus)

### Section generating for lang: genlisp
### Generating Messages
_generate_msg_lisp(canusb
  "/home/monster/catkin_ws/src/canusb/msg/CAN.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/canusb
)

### Generating Services

### Generating Module File
_generate_module_lisp(canusb
  ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/canusb
  "${ALL_GEN_OUTPUT_FILES_lisp}"
)

add_custom_target(canusb_generate_messages_lisp
  DEPENDS ${ALL_GEN_OUTPUT_FILES_lisp}
)
add_dependencies(canusb_generate_messages canusb_generate_messages_lisp)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" NAME_WE)
add_dependencies(canusb_generate_messages_lisp _canusb_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(canusb_genlisp)
add_dependencies(canusb_genlisp canusb_generate_messages_lisp)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS canusb_generate_messages_lisp)

### Section generating for lang: gennodejs
### Generating Messages
_generate_msg_nodejs(canusb
  "/home/monster/catkin_ws/src/canusb/msg/CAN.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/canusb
)

### Generating Services

### Generating Module File
_generate_module_nodejs(canusb
  ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/canusb
  "${ALL_GEN_OUTPUT_FILES_nodejs}"
)

add_custom_target(canusb_generate_messages_nodejs
  DEPENDS ${ALL_GEN_OUTPUT_FILES_nodejs}
)
add_dependencies(canusb_generate_messages canusb_generate_messages_nodejs)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" NAME_WE)
add_dependencies(canusb_generate_messages_nodejs _canusb_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(canusb_gennodejs)
add_dependencies(canusb_gennodejs canusb_generate_messages_nodejs)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS canusb_generate_messages_nodejs)

### Section generating for lang: genpy
### Generating Messages
_generate_msg_py(canusb
  "/home/monster/catkin_ws/src/canusb/msg/CAN.msg"
  "${MSG_I_FLAGS}"
  ""
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/canusb
)

### Generating Services

### Generating Module File
_generate_module_py(canusb
  ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/canusb
  "${ALL_GEN_OUTPUT_FILES_py}"
)

add_custom_target(canusb_generate_messages_py
  DEPENDS ${ALL_GEN_OUTPUT_FILES_py}
)
add_dependencies(canusb_generate_messages canusb_generate_messages_py)

# add dependencies to all check dependencies targets
get_filename_component(_filename "/home/monster/catkin_ws/src/canusb/msg/CAN.msg" NAME_WE)
add_dependencies(canusb_generate_messages_py _canusb_generate_messages_check_deps_${_filename})

# target for backward compatibility
add_custom_target(canusb_genpy)
add_dependencies(canusb_genpy canusb_generate_messages_py)

# register target for catkin_package(EXPORTED_TARGETS)
list(APPEND ${PROJECT_NAME}_EXPORTED_TARGETS canusb_generate_messages_py)



if(gencpp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/canusb)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gencpp_INSTALL_DIR}/canusb
    DESTINATION ${gencpp_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_cpp)
  add_dependencies(canusb_generate_messages_cpp std_msgs_generate_messages_cpp)
endif()

if(geneus_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/canusb)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${geneus_INSTALL_DIR}/canusb
    DESTINATION ${geneus_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_eus)
  add_dependencies(canusb_generate_messages_eus std_msgs_generate_messages_eus)
endif()

if(genlisp_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/canusb)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genlisp_INSTALL_DIR}/canusb
    DESTINATION ${genlisp_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_lisp)
  add_dependencies(canusb_generate_messages_lisp std_msgs_generate_messages_lisp)
endif()

if(gennodejs_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/canusb)
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${gennodejs_INSTALL_DIR}/canusb
    DESTINATION ${gennodejs_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_nodejs)
  add_dependencies(canusb_generate_messages_nodejs std_msgs_generate_messages_nodejs)
endif()

if(genpy_INSTALL_DIR AND EXISTS ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/canusb)
  install(CODE "execute_process(COMMAND \"/usr/bin/python\" -m compileall \"${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/canusb\")")
  # install generated code
  install(
    DIRECTORY ${CATKIN_DEVEL_PREFIX}/${genpy_INSTALL_DIR}/canusb
    DESTINATION ${genpy_INSTALL_DIR}
  )
endif()
if(TARGET std_msgs_generate_messages_py)
  add_dependencies(canusb_generate_messages_py std_msgs_generate_messages_py)
endif()
