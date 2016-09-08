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

        internal static Func<WINDOWPOS, bool> IsRestoring = (windowPos) =>
        {
            return (windowPos.flags & SWP.FRAMECHANGED) == SWP.FRAMECHANGED &&
                   (windowPos.flags & SWP.NOSIZE) != SWP.NOSIZE;
        };

        internal static Func<WINDOWPOS, bool> IsDocked = (windowPos) =>
        {
            return (windowPos.flags & SWP.DOCKFRAME) == SWP.DOCKFRAME;
        };

        internal static Func<WMSZ, bool> SizingTop = (sizing) =>
        {
            return sizing == WMSZ.TOP || sizing == WMSZ.TOPLEFT || sizing == WMSZ.TOPRIGHT;
        };

        internal static Func<WMSZ, bool> SizingBottom = (sizing) =>
        {
            return sizing == WMSZ.BOTTOM || sizing == WMSZ.BOTTOMLEFT || sizing == WMSZ.BOTTOMRIGHT;
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
