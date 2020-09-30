namespace WindowSnap
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Windows;

    /// Completely harmless for any code, even malicious code, to call.
    /// Can be used just like other managed code.
    /// For example, a function that gets the time of day is typically safe.
    ///
    /// Naming Convention for Unmanaged Code Methods 
    /// <devdoc>http://msdn.microsoft.com/en-us/library/btadwd4w(vs.80).aspx</devdoc>    
    internal static class SafeNativeMethods
    {
        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms633519.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect([In] IntPtr hWnd, [Out] out RECT lpRect);

        #region EnumDisplayMonitors

        internal static List<Monitor> GetDisplayMonitors(RECT? rc = null)
        {
            var monitors = new List<Monitor>();
            IntPtr rcClip = IntPtr.Zero;

            if (rc != null)
            {
                rcClip = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECT)));

                Marshal.StructureToPtr(rc, rcClip, true);
            }

            // The MonitorEnumProc delegate will not be called if the clipping rectangle (rcClip) is
            // set to be outside of a visible region: the monitors list will remains empty.
            bool result = EnumDisplayMonitors(IntPtr.Zero, rcClip,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
                {
                    var monitor = GetMonitor(hMonitor);

                    monitors.Add(monitor);

                    return true;

                }, IntPtr.Zero);

            if (rcClip != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(rcClip);
                rcClip = IntPtr.Zero;
            }

            return monitors;
        }

        /// <devdoc>http://msdn.microsoft.com/en-ca/library/windows/desktop/dd162610.aspx</devdoc>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumDisplayMonitorsDelegate lpfnEnum, IntPtr dwData);

        /// <devdoc>http://msdn.microsoft.com/en-ca/library/windows/desktop/dd145061.aspx</devdoc>
        delegate bool EnumDisplayMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        #endregion EnumDisplayMonitors

        #region GetMonitorInfo

        static Monitor GetMonitor(IntPtr hMonitor)
        {
            bool success = false;

            var mi = new MONITORINFO()
            {
                cbSize = Marshal.SizeOf(typeof(MONITORINFO))
            };

            if (hMonitor != IntPtr.Zero)
            {
                success = GetMonitorInfo(hMonitor, out mi);
            }

            return success ? new Monitor(mi.rcMonitor, mi.rcWork) : null;
        }

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/dd144901.aspx</devdoc>
        [DllImport("user32.dll", EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetMonitorInfo([In] IntPtr hMonitor, [Out] out MONITORINFO lpmi);       

        #endregion GetMonitoInfo

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms724947.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref IntPtr pvParam, SPIF fWinIni);

        #region GetDotsPerInch

        internal static Point GetDotsPerInch()
        {
            Point dpi = new Point(96d, 96d);

            IntPtr hDC = IntPtr.Zero;

            try
            {
                hDC = GetDC(IntPtr.Zero);

                if (hDC != IntPtr.Zero)
                {
                    uint dpiX = (uint)GetDeviceCaps(hDC, DEVICECAP.LOGPIXELSX);
                    uint dpiY = (uint)GetDeviceCaps(hDC, DEVICECAP.LOGPIXELSY);

                    dpi = new Point(dpiX, dpiY);
                }
            }
            finally
            {
                SafeReleaseDC(IntPtr.Zero, hDC);
            }

            return dpi;
        }
        
        /// <summary>
        /// Release GDI device context safely
        /// </summary>
        /// <param name="hObject"></param>
        static void SafeReleaseDC(IntPtr hWnd, IntPtr hDC)
        {
            if (hDC != IntPtr.Zero)
            {
                ReleaseDC(hWnd, hDC);
            }
        }

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd144877.aspx</devdoc>
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps([In] IntPtr hdc, DEVICECAP nIndex);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd144871.aspx</devdoc>
        [DllImport("user32.dll")]
        static extern IntPtr GetDC([In] IntPtr hWnd);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/dd162920.aspx</devdoc>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ReleaseDC([In] IntPtr hWnd, [In] IntPtr hDC);

        #endregion GetDotsPerInch
    }
}
