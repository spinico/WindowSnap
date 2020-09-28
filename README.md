
![WindowSnap][logo] **WindowSnap**
=======

Sample WPF/C# project to demonstrate detection of a snapped window (AeroSnap).

![Sample application preview](https://github.com/spinico/WindowSnap/blob/master/Images/demo.png?raw=true)


#### **Update (2020-09-28)**

Fixed an exception occuring on monitor detection with a non-visible clipping rectangle region.

#### **Update (2018-04-30)**

Added support to detect corner snap on Windows 10+.

#### **Update (2016-09-08)**

Fixed multi-monitor issue when a snapped window is moved from one monitor to another.

#### **System-wide snap settings**
 - Window arranging: Enable or disable the simplified move and size behavior of a top-level windows when it is dragged or sized.
 - Snap sizing: Enable or disable windows to be vertically maximized when it is sized to the top or bottom of the monitor. Window arranging must be checked to enable this behavior.
 - Dock moving: Enable or disable window docking when it is moved to the top, left, or right docking targets on a monitor or monitor array. Window arranging must be checked to enable this behavior.
 - Drag from maximize: Enable or disable maximized windows to be restored when its caption bar is dragged. Window arranging must be checked to enable this behavior.
 
Theses settings affect the snapping behavior for all windows (for testing/demonstration purposes only).

----------
The MIT License (MIT)


[logo]: https://github.com/spinico/WindowSnap/blob/master/Images/logo.png?raw=true "WindowSnap"