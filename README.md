# Saldiray

Autonomous Ground Vehicle Project of Graduation Project of Mechanical Engineering at Bilkent University, in 2019.

Group: SupreME
 
## Getting Started
This repo consists last version of codes of Saldiray autonomous ground vehicle (UGV). The system consist of two major parts in terms of software which are base and rover software. Base software is a user interface that helps user to control the vehicle remotely. It is written on C# using .NET Framework. Besides, Rover software is the component that drives the vehicle and features the autonomy. It is based on Robotic Operating System (ROS) and corresponding parts are written mainly on Python 2.7.


### Prerequisites
#### Hardware:
Following hardwares were used for this project but alternative ones could be used.
```
Hydrolic driven tracked vehicle that hydrolic valves controled over CAN protocol.
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
Following dependicies should be installed before running the code.

Base Software
```
Microsoft .NET Framework 4.5.2 Developer Pack
```

Rover Software
```
ROS Kinetic Kame
Python 2.7
```

### Installing
Write following command on the terminal to install source code of this repo:
```
git clone https://github.com/Viriliter/Saldiray/
```

## Goals
The goal of the project is establish a software system that converts the regular vehicle to autonomous driven vehicle with reqired hardware package.
## Running the tests

## Contributing
Mert Limoncuoğlu <br /> Ali Subay <br />
## Versioning
1.0
## Authors
Mert Limoncuoğlu

## Acknowledgments
