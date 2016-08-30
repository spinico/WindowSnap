﻿namespace WindowSnap
{
    using System;
    using System.Windows;

    internal static class Helpers
    {
        internal static Func<WINDOWPOS, bool> WindowChanged = windowPos =>
        {
            return (windowPos.flags & SWP.NOMOVE) != SWP.NOMOVE ||
                   (windowPos.flags & SWP.NOSIZE) != SWP.NOSIZE;
        };

        internal static Func<WINDOWPOS, bool> IsDocked = (windowPos) =>
        {
            return (windowPos.flags & SWP.DOCKFRAME) == SWP.DOCKFRAME;
        };

        internal static WindowState GetActualState(IntPtr hWnd)
        {
            WindowState state = WindowState.Normal;

            WS ws = UnsafeNativeMethods.GetWindowStyle(hWnd);

            if ((ws & WS.MINIMIZE) == WS.MINIMIZE)
            {
                state = WindowState.Minimized;
            }
            else if ((ws & WS.MAXIMIZE) == WS.MAXIMIZE)
            {
                state = WindowState.Maximized;
            }

            return state;
        }

        internal static MonitorArea GetMonitorArea(IntPtr hWnd, POINT? pt = null)
        {
            return pt != null ? SafeNativeMethods.GetMonitorAreaFromPoint((POINT)pt) :
                                SafeNativeMethods.GetMonitorAreaFromWindow(hWnd);
        }
    }
}
