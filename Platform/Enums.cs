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

    public enum WMSZ : Int32
    {
        NONE        = 0,
        LEFT        = 1,
        RIGHT       = 2,
        TOP         = 3,
        TOPLEFT     = 4,
        TOPRIGHT    = 5,
        BOTTOM      = 6,
        BOTTOMLEFT  = 7,
        BOTTOMRIGHT = 8,
        UNSNAP      = 9
    }

    [Flags()]
    enum SWP : UInt32
    {
        NOSIZE       = 0x00000001,
        NOMOVE       = 0x00000002,
        NOZORDER     = 0x00000004,
        FRAMECHANGED = 0x00000020
    }

    enum WM : Int32
    {
        ACTIVATE          = 0x0006,         
        WINDOWPOSCHANGING = 0x0046,
        SYSCOMMAND        = 0x0112,
        SIZING            = 0x0214,
        EXITSIZEMOVE      = 0x0232
    }

    [Flags]
    enum SC : Int32
    {
        SIZE = 0xF000,
        MOVE = 0xF010
    }

    enum GWL : Int32
    {
        STYLE   = (-16),
        EXSTYLE = (-20)
    }

    [Flags()]
    enum WS : UInt32
    {
        THICKFRAME  = 0x00040000,
        BORDER      = 0x00800000,
        MINIMIZE    = 0x20000000,
        MAXIMIZE    = 0x01000000        
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
        GETWINARRANGING     = 0x0082,
        SETWINARRANGING     = 0x0083,
        GETDRAGFROMMAXIMIZE = 0x008C,
        SETDRAGFROMMAXIMIZE = 0x008D,
        GETSNAPSIZING       = 0x008E,
        SETSNAPSIZING       = 0x008F,
        GETDOCKMOVING       = 0x0090,
        SETDOCKMOVING       = 0x0091
    }

    [Flags]
    enum SPIF : Int32
    {
        NONE             = 0x00,
        UPDATEINIFILE    = 0x01,
        SENDCHANGE       = 0x02,
        SENDWININICHANGE = SENDCHANGE
    }

    enum DEVICECAP : Int32
    {
        LOGPIXELSX = 88,
        LOGPIXELSY = 90
    }
}
