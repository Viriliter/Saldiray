# Saldiray

Autonomous Ground Vehicle Project of Graduation Project of Mechanical Engineering at Bilkent University, in 2019.
Group: SupreME
 
## Getting Started
This repo consists last version of codes of Saldiray autonomous ground vehicle (UGV). The system consist of two major parts in terms of software which are base and rover software. Base software is a user interface that helps user to control the vehicle remotely. It is written on C#. Besides, Rover software is the component that drives the vehicle and features the autonomy. 

The software system is
In manuel mode, the user will interact with the car and perform basic steering function. Besides, in autonomous mode, using OpenCV library and the neural network algorithm that is developed will steer the car. In this mode, the camera will detect only two white line which is requirement of the project. The system will communicate with Arduino board for motor and servo controlling over USB port. It also performs data acqusitions comes from sensors for controlling tasks.


### Prerequisites
#### Hardware:
Following hardwares were used for this project but alternative ones could be used.
```
Hydrolic driven tracked vehicle that is controled by CAN protocol.
Nvidia Jetson TX 2
Velodyne VLP16
ZED 2 Stereo Camera
UBlox M6N GNSS receiver
QinetiQ GNSS receiver


```
#### Software:
Following dependicies should be installed before running the code.

Base
```


Rover
```

### Installing
Write following command on the terminal to setup the algorithm on Raspberry Pi:
```
git clone https://github.com/Viriliter/Saldiray/
```

## Goals
The goal of the project is establish a software system that converts the regular vehicle to autonomous drivven vehicle with reqired hardware package.
## Running the tests

## Contributing
Mert Limoncuoğlu <br /> Ali Subay <br />
## Versioning
1.0
## Authors
Mert Limoncuoğlu

## Acknowledgments
