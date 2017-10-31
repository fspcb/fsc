## FlightSim PCB Control

##### Current Release 1.0.0
Download <a href="https://github.com/fspcb/fsc/releases/download/1.0.0/FSCSetup-1.0.0.msi">FSCSetup-1.0.0.msi</a>

Requirements:

* SimConnect 
* .NET 4.6

Supported Hardware: 

* <a href="https://github.com/fspcb/A320RadioManagementPanel">Radio Management Panel</a>

### Manual
- Install
- Start  the FSC exe

Every second the app checks for a connection to a new hardware device and/or the flight simulator. Reconnect supported.

#### How to find SimConnect:

Open your fsx/prepar3d folder and look into 

```redist\Interface\<*>\retail\lib```

<*> folder names can be FSX-SP2-XPACK, FSX-SP1, ...

Execute the SimConnect.msi to install SimConnect
