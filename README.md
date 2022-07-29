# Saldiray
Autonomous Ground Vehicle Project of Graduation Project of Mechanical Engineering at Bilkent University, in 2019.

Group: SupreME

## Getting Started
This repo consists of the last version of the code of Saldiray autonomous ground vehicle (UGV). The system consists of two major parts in terms of software, which are base and rover software. Base software is a user interface that helps users to control the vehicle remotely. It is written on C# using the .NET Framework. Besides, Rover software is the component that drives the vehicle and features autonomy. It is based on Robotic Operating System (ROS) and corresponding parts are written mainly on Python 2.7.

### Prerequisites

#### Hardware:
The following hardwares were used for this project:
```
Hydraulic driven tracked vehicle (hydrolic valves can be controled over CAN protocol.)
Nvidia Jetson TX2 Developer Board
Velodyne VLP16 Lidar
Velodyne Interface Box
ZED 2 Stereo Camera
UBlox M6N GNSS receiver
QinetiQ GNSS receiver
SparkFun 9DoF Razor IMU
CAN transreceiver
RS232-USB transreceiver
TP-Link UH700 USB Hub
3DR radio telemetry 433MHz
Arduino Due
```

#### Software:
The following dependencies should be installed before running the code.

Base Software
```
Microsoft .NET Framework 4.5.2 Developer Pack
Bing Maps REST Services
Newtonsoft.Json
OzekiSDK
SlimDX
```

Rover Software
```
ROS Kinetic Kame
Python 2.7
Numpy
```

### Installing
Write the following command on the terminal to install the source code of this repo:

```
git clone https://github.com/Viriliter/Saldiray/
```

## Base Software
It provides users with controlling the vehicle by gamepad controller inputs over radio telemetry. It uses Bing Map REST services in order to track the vehicle on the map simultaneously. With the gauges provided on the driver's page, the user can monitor the status of the vehicle like orientation, speed, location, etc. Additionally, the user can monitor the environment with live video feedback transmitted from the ground vehicle. However, the live video feedback feature is not tested on the vehicle apart from ground tests. With the control of the gamepad, the user can remotely start/stop the engine and manually steer the vehicle. In the user interface, the user can plan a mission and pin the waypoints on the map. Then, these waypoints can be sent over telemetry messages. With the key switch on the gamepad, it can be switched between autonomous and manual modes.

## Rover Software
Robotic operating system (ROS) runs on the rover platform. Data acquired from a variety of sensors (GNSS and IMU) is sent to the base station simultaneously. In autonomous mode, according to the mission plan created in the base software, the vehicle detects obstacles in the environment with lidar and stereocamera. With lidar odeometry, the vehicle estimates the relative position from the last waypoint. Then, it moves to the next target waypoint with coordinates provided by GNSS receivers. While on the path, according to the depth map provided by lidar, it finds out the shortest path allowable and the mission plan is recalculated dynamically. 

## Goals
The goal of the project is to establish a software system that converts a CAN driven regular vehicle to an autonomous driven vehicle with a required hardware package.

## Contributing
Ali Subay  <br /> Mert Limoncuoğlu <br />

## Versioning
1.0

## Known Issues
* In base software, there is not adaptive resolution so there could be offsets in the widgets depending on the screen resolution. Best results are at 1368x768 screens.

## Authors
Mert Limoncuoğlu
