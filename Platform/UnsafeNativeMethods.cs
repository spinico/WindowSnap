namespace WindowSnap
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// UnsafeNativeMethods - This class suppresses stack walks for unmanaged code permission. 
    /// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.) 
    /// This class is for potentially dangerous unmanaged code entry point with security suppressed.
    /// Any caller of these methods must perform a full security review to make sure that the usage 
    /// is secure because no stack walk will be performed.
    /// Developers should use the greatest caution when using such unmanaged code, making sure that
    /// other protections are in place to prevent a security vulnerability. 
    /// Developers must be responsible, as this keyword overrides the security system.
    /// <devdoc>http://msdn.microsoft.com/en-us/library/ms182161.aspx</devdoc>        
    /// 
    /// Naming Convention for Unmanaged Code Methods 
    /// <devdoc>https://msdn.microsoft.com/en-us/library/btadwd4w(vs.80).aspx</devdoc>        
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        #region Get / Set window style

        internal static WS GetWindowStyle(IntPtr hWnd)
        {
            return (WS)GetWindowLong(hWnd, (int)GWL.STYLE);
        }

        internal static bool SetWindowStyle(IntPtr hWnd, WS ws, bool enable)
        {
            return SetWS(hWnd, (uint)ws, (int)GWL.STYLE, enable);
        }

        internal static WS_EX GetWindowStyleEx(IntPtr hWnd)
        {
            return (WS_EX)GetWindowLong(hWnd, (int)GWL.EXSTYLE);
        }

        internal static bool SetWindowStyleEx(IntPtr hWnd, WS_EX ws_ex, bool enable)
        {
            return SetWS(hWnd, (uint)ws_ex, (int)GWL.EXSTYLE, enable);
        }

        static bool SetWS(IntPtr hWnd, uint style, int index, bool enable)
        {
            uint value = (uint)GetWindowLong(hWnd, index);

            if (enable)
            {
                value |= style;
            }
            else
            {
                value &= ~style;
            }

            // Update style (if the function fails, the return value is zero)
            int hResult = SetWindowLong(hWnd, index, value);

            // Applies new frame styles set using the SetWindowPos function (if the function fails, the return value is zero)
            // This is required for the change to take effect
            bool success = SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER | SWP.FRAMECHANGED);

            return hResult != 0 && success;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);

        #endregion Get / Set window style

        #region GetWindowLong / SetWindowLong 32-64 bits support

        static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }

        static int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLong32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int SetWindowLong32(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int SetWindowLongPtr64(IntPtr hWnd, int nIndex, uint dwNewLong);

        #endregion GetWindowLong / SetWindowLong 32-64 bits support
    }
}
