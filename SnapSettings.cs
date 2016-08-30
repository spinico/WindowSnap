namespace WindowSnap
{
    using System;

    public static class SnapSettings
    {
        /// <summary>
        /// Window arrangement reduces the number of mouse, pen, or touch interactions 
        /// needed to move and size top-level windows by simplifying the default 
        /// behavior of a window when it is dragged or sized.
        /// </summary>
        public static bool WindowArranging
        {
            get { return GetValue(SPI.GETWINARRANGING); }
            set { SetValue(SPI.SETWINARRANGING, value); }
        }

        /// <summary>
        /// Allow a window to be vertically maximized when it is sized to the top 
        /// or bottom of the monitor
        /// </summary>
        public static bool SnapSizing
        {
            get { return WindowArranging ? GetValue(SPI.GETSNAPSIZING) : false; }
            set { SetValue(SPI.SETSNAPSIZING, value); }
        }

        /// <summary>
        /// Allow a window is docked when it is moved to the top, left, or right
        /// docking targets on a monitor or monitor array. 
        /// </summary>
        public static bool DockMoving
        {
            get { return WindowArranging ? GetValue(SPI.GETDOCKMOVING) : false; }
            set { SetValue(SPI.SETDOCKMOVING, value); }
        }

        /// <summary>
        /// Allow a maximized window to be restored when its caption bar is dragged.
        /// </summary>
        public static bool DragFromMaximize
        {
            get { return WindowArranging ? GetValue(SPI.GETDRAGFROMMAXIMIZE) : false; }
            set { SetValue(SPI.SETDRAGFROMMAXIMIZE, value); }
        }

        private static bool GetValue(SPI parameter)
        {
            IntPtr value = IntPtr.Zero;

            bool success = SafeNativeMethods.SystemParametersInfo(parameter, 0, ref value, SPIF.NONE);

            if (!success)
            {
                throw new Exception("SystemParametersInfo call failed.");
            }

            return value != IntPtr.Zero;
        }

        private static void SetValue(SPI parameter, bool value)
        {
            IntPtr nullPtr = IntPtr.Zero;

            bool success = SafeNativeMethods.SystemParametersInfo(parameter, (uint)(value ? 1 : 0), ref nullPtr, SPIF.NONE);

            if (!success)
            {
                throw new Exception("SystemParametersInfo call failed.");
            }
        }

    }
}
