namespace WindowSnap
{
    using System;
    using System.Runtime.InteropServices;


    /// Completely harmless for any code, even malicious code, to call.
    /// Can be used just like other managed code.
    /// For example, a function that gets the time of day is typically safe.
    ///
    /// Naming Convention for Unmanaged Code Methods 
    /// <devdoc>https://msdn.microsoft.com/en-us/library/btadwd4w(vs.80).aspx</devdoc>    
    internal static class SafeNativeMethods
    {
        #region GetMonitorInfo

        /// <summary>
        /// Get the current monitor area from the specified point
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>The corresponding MonitorArea instance, or null on failure</returns>
        internal static MonitorArea GetMonitorAreaFromPoint(POINT pt)
        {
            IntPtr hMonitor = MonitorFromPoint(pt, MONITOR.DEFAULTTONEAREST);

            return GetMonitorArea(hMonitor);
        }

        /// <summary>
        /// Get the current monitor area from the specified window handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>The corresponding MonitorArea instance, or null on failure</returns>
        internal static MonitorArea GetMonitorAreaFromWindow(IntPtr hWnd)
        {
            IntPtr hMonitor = MonitorFromWindow(hWnd, MONITOR.DEFAULTTONEAREST);

            return GetMonitorArea(hMonitor);
        }

        static MonitorArea GetMonitorArea(IntPtr hMonitor)
        {
            if (hMonitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO()
                {
                    cbSize = Marshal.SizeOf(typeof(MONITORINFO))
                };

                GetMonitorInfo(hMonitor, out monitorInfo);

                return new MonitorArea(monitorInfo.rcMonitor, monitorInfo.rcWork);
            }

            return null;
        }

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/dd144901.aspx</devdoc>
        [DllImport("user32.dll", EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetMonitorInfo([In] IntPtr hMonitor, [Out] out MONITORINFO lpmi);

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/dd145064.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromWindow([In] IntPtr hWnd, [In] MONITOR dwFlags);

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint([In] POINT pt, [In] MONITOR dwFlags);

        #endregion GetMonitoInfo

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms724947.aspx</devdoc>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref IntPtr pvParam, SPIF fWinIni);
    }
}
