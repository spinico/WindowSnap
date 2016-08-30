namespace WindowSnap
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public class WindowSnap : IDisposable
    {
        private static readonly Size NoSize = new Size(0, 0);

        private Window _window = null;
        private IntPtr _hWnd = IntPtr.Zero;
        private bool _snapped = false;
        private Nullable<Size> _offset = null;

        private WindowState ActualState
        {
            get { return Helpers.GetActualState(_hWnd); }
        }
        
        private Size Offset
        {
            get { return _offset != null ? (Size)_offset : NoSize; }
        }

        // Events        
        public event EventHandler Snapped = delegate { };
        public event EventHandler Unsnapped = delegate { };
        public event EventHandler EdgeOffsetChanged = delegate { };

        public bool IsSnapped { get { return _snapped; } }

        public WindowSnap(Window window)
        {           
            if (window == null)
            {
                throw new ArgumentNullException("The window cannot be null.");
            }

            _window = window;
            _hWnd = new WindowInteropHelper(window).Handle;

            HwndSource.FromHwnd(_hWnd).AddHook(WindowProc);
        }
        
        #region IDisposable

        private bool _disposed = false;

        ~WindowSnap()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free other managed objects that 
                // implement IDisposable  
                if (_hWnd != IntPtr.Zero)
                {
                    HwndSource.FromHwnd(_hWnd).RemoveHook(WindowProc);
                    _hWnd = IntPtr.Zero;                    
                }

                _window = null;
            }

            // Release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        public virtual IntPtr WindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((WM)msg)
            {
                case WM.SHOWWINDOW:
                    {
                        if (wParam != IntPtr.Zero && _offset == null)
                        {
                            var edgeOffset = GetEdgeOffset();

                            _offset = edgeOffset.Value;

                            EdgeOffsetChanged(this, new EdgeOffsetChangedEventArgs(edgeOffset));
                        }
                        
                        break;
                    }

                case WM.WINDOWPOSCHANGING:
                    {
                        WINDOWPOS windowPos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

                        if (Helpers.WindowChanged(windowPos))
                        {                            
                            switch (ActualState)
                            {
                                case WindowState.Minimized:

                                    break;

                                case WindowState.Maximized:

                                    break;

                                default:

                                    MonitorArea monitorArea = Helpers.GetMonitorArea(hWnd, new POINT() { x = windowPos.x, y = windowPos.y });

                                    if (DetectSnap.IsSnapped(windowPos, monitorArea))
                                    {
                                        if (!_snapped)
                                        {
                                            _snapped = true;

                                            Snapped(this, new EventArgs());
                                        }
                                    }
                                    else
                                    {
                                        if (_snapped)
                                        {
                                            _snapped = false;

                                            Unsnapped(this, new EventArgs());
                                        }
                                    }

                                    break;
                            }                            
                        }

                        break;
                    }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Obtain the edge vertical and horizontal offset
        /// based on the current type of window
        /// </summary>
        /// <remarks>
        /// ThreeDBorderWindow : WINDOWEDGE + CLIENTEDGE + SIZEBOX
        /// SingleBorderWindow : WINDOWEDGE + SIZEBOX
        /// ToolWindow         : WINDOWEDGE + TOOLWINDOW + SIZEBOX
        /// </remarks>
        /// <returns>The width and height offset</returns>
        private EdgeOffset GetEdgeOffset()
        {
            EdgeOffset edgeOffset = new EdgeOffset();

            WS_EX ws_ex = UnsafeNativeMethods.GetWindowStyleEx(_hWnd);
            WS ws = UnsafeNativeMethods.GetWindowStyle(_hWnd);

            if ((ws_ex & WS_EX.WINDOWEDGE) == WS_EX.WINDOWEDGE)
            {
                edgeOffset.FixedFrame = new Size()
                {
                    Width = SystemParameters.FixedFrameVerticalBorderWidth,
                    Height = SystemParameters.FixedFrameHorizontalBorderHeight
                };
            }

            if ((ws_ex & WS_EX.CLIENTEDGE) == WS_EX.CLIENTEDGE)
            {
                edgeOffset.ThickBorder = new Size()
                {
                    Width = SystemParameters.ThickVerticalBorderWidth,
                    Height = SystemParameters.ThickHorizontalBorderHeight
                };
            }

            if ((ws_ex & WS_EX.TOOLWINDOW) == WS_EX.TOOLWINDOW)
            {
                edgeOffset.ThinBorder = new Size()
                {
                    Width = SystemParameters.ThinVerticalBorderWidth,
                    Height = SystemParameters.ThinHorizontalBorderHeight
                };
            }

            if ((ws & WS.SIZEBOX) == WS.SIZEBOX)
            {
                edgeOffset.ResizeFrame = new Size()
                {
                    Width = SystemParameters.ResizeFrameVerticalBorderWidth,
                    Height = SystemParameters.ResizeFrameHorizontalBorderHeight
                };
            }

            return edgeOffset;
        }
    }
}
