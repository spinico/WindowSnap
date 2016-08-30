namespace WindowSnap
{
    using System;

    enum MONITOR : UInt32
    {
        DEFAULTTONULL    = 0,
        DEFAULTTOPRIMARY = 1,
        DEFAULTTONEAREST = 2
    }

    enum MONITORINFOF : UInt32
    {
        PRIMARY = 1
    }

    [Flags()]
    enum SWP : UInt32
    {
        NOSIZE         = 0x00000001,
        NOMOVE         = 0x00000002,
        NOZORDER       = 0x00000004,
        FRAMECHANGED   = 0x00000020,

        /// <summary>
        /// Undocumented flag - Based on the window current position, this 
        /// flag indicates if the window frame will be docked to one or 
        /// more edges of the monitor
        /// </summary>
        /// <remarks>
        /// Only available on Windows 10
        /// </remarks>
        DOCKFRAME      = 0x00100000
    }

    enum WM : Int32
    {
        SHOWWINDOW        = 0x0018,
        WINDOWPOSCHANGING = 0x0046
    }

    enum GWL : Int32
    {
        STYLE   = (-16),
        EXSTYLE = (-20)
    }

    [Flags()]
    enum WS : UInt32
    {
        SIZEBOX  = 0x00040000,
        BORDER   = 0x00800000,
        MINIMIZE = 0x20000000,
        MAXIMIZE = 0x01000000        
    }

    [Flags()]
    enum WS_EX : Int32
    {        
        TOOLWINDOW = 0x00000080,
        WINDOWEDGE = 0x00000100,
        CLIENTEDGE = 0x00000200
    }

    enum SPI : UInt32 
    {
        GETWINARRANGING           = 0x0082,
        SETWINARRANGING           = 0x0083,
        GETDRAGFROMMAXIMIZE       = 0x008C,
        SETDRAGFROMMAXIMIZE       = 0x008D,
        GETSNAPSIZING             = 0x008E,
        SETSNAPSIZING             = 0x008F,
        GETDOCKMOVING             = 0x0090,
        SETDOCKMOVING             = 0x0091
    }

    [Flags]
    enum SPIF : Int32
    {
        NONE             = 0x00,
        UPDATEINIFILE    = 0x01,
        SENDCHANGE       = 0x02,
        SENDWININICHANGE = SENDCHANGE
    }
}
