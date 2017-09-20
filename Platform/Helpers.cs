namespace WindowSnap
{
    using System;
    using System.Windows;

    internal static class Helpers
    {
        internal static Func<WINDOWPOS, bool> WindowChanged = (windowPos) =>
        {
            return (windowPos.flags & SWP.NOMOVE) != SWP.NOMOVE ||
                   (windowPos.flags & SWP.NOSIZE) != SWP.NOSIZE;
        };

        internal static Func<Window, bool> SizeRestored = (window) =>
        {
            return window.Left == window.RestoreBounds.Left &&
                   window.Top == window.RestoreBounds.Top &&
                   window.Width == window.RestoreBounds.Width &&
                   window.Height == window.RestoreBounds.Height;
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
    }
}
