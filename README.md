
![WindowSnap][logo] **WindowSnap**
=======

Sample WPF/C# project to demonstrate detection of a snapped window.

![Sample application preview](https://github.com/spinico/WindowSnap/blob/master/Images/demo.png?raw=true)

#### **System-wide behavior settings**
 - Window arranging: Enable or disable the simplified move and size behavior of a top-level windows when it is dragged or sized.
 - Snap sizing: Enable or disable windows to be vertically maximized when it is sized to the top or bottom of the monitor. Window arranging must be checked to enable this behavior.
 - Dock moving: Enable or disable window docking when it is moved to the top, left, or right docking targets on a monitor or monitor array. Window arranging must be checked to enable this behavior.
 - Drag from maximize: Enable or disable maximized windows to be restored when its caption bar is dragged. Window arranging must be checked to enable this behavior.
 
Theses settings affect the snapping behavior for all windows (for testing/demonstration purposes only).

#### **Remarks**

.NET version 4.5 is the minimum target framework required to properly detect top left/right snap on Windows 10.

> This is a design decision made by Microsoft that causes the OS to change the system's parameters value of certain properties depending on how the application is compiled.
>  
> More specifically, when the targeted framework is .NET 4.5+, the *SM\_CXSIZEFRAME* and *SM\_CYSIZEFRAME* value returned by *GetSystemMetrics()* is 4. With previous target framework, the value returned is 8. 
> 
> See the [following link](https://connect.microsoft.com/VisualStudio/feedback/details/763767/the-systemparameters-windowresizeborderthickness-seems-to-return-incorrect-value "The SystemParameters.WindowResizeBorderThickness seems to return incorrect value") for a discussion on this issue. 

----------
The MIT License (MIT)


[logo]: https://github.com/spinico/WindowSnap/blob/master/Images/logo.png?raw=true "WindowSnap"