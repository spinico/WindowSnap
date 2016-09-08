﻿namespace WindowSnap
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// Completely harmless for any code, even malicious code, to call.
    /// Can be used just like other managed code.
    /// For example, a function that gets the time of day is typically safe.
    ///
    /// Naming Convention for Unmanaged Code Methods 
    /// <devdoc>http://msdn.microsoft.com/en-us/library/btadwd4w(vs.80).aspx</devdoc>    
    internal static class SafeNativeMethods
    {
        #region EnumDisplayMonitors

        internal static List<MonitorArea> GetDisplayMonitors(RECT? rc = null)
        {
            var monitors = new List<MonitorArea>();
            IntPtr rcClip = IntPtr.Zero;

            if (rc != null)
            {
                rcClip = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECT)));

                Marshal.StructureToPtr(rc, rcClip, true);
            }

            EnumDisplayMonitors(IntPtr.Zero, rcClip,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
                {
                    var monitorArea = GetMonitorArea(hMonitor);

                    monitors.Add(monitorArea);

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

        static MonitorArea GetMonitorArea(IntPtr hMonitor)
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

            return success ? new MonitorArea(mi.rcMonitor, mi.rcWork) : null;
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
    }
}
