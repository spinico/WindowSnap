namespace WindowSnap
{
    using System;
    using System.Collections.Generic;

    [Flags()]
    enum SnapBounds
    {
        None   = 0,
        Left   = 1,
        Right  = 2,
        Top    = 4,
        Bottom = 8
    }

    internal static class DetectSnap
    {
        /// <summary>
        /// Indicate if the window is snapped or not (support multi-monitors)
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitors">A list of monitor area</param>        
        /// <returns>
        /// A result instance indicating if the window is snapped on one
        /// of the available monitors and the corresponding monitor
        /// </returns>
        internal static SnapResult IsSnapped(WINDOWPOS windowPos, List<Monitor> monitors)
        {
            bool snapped = false;
            Monitor monitor = monitors[0];

            for (int i = 0; i < monitors.Count; i++)
            {
                snapped = IsSnapped(windowPos, monitors[i]);

                if (snapped)
                {
                    monitor = monitors[i];
                    break;
                }
            }

            return new SnapResult(snapped, monitor);
        }

        /// <summary>
        /// Indicate if the window is snapped or not on the given monitor
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitor"></param>
        /// <returns>True when window is snapped, false otherwise</returns>
        private static bool IsSnapped(WINDOWPOS windowPos, Monitor monitor)
        {
            // Otherwise, use the standard snap detection            
            SnapBounds bounds = GetBounds(windowPos, monitor);

            // Maximized (top, left, bottom right)            
            // Vertical snap (top, bottom)
            // Vertical left snap (top, bottom, left) 
            // Vertical right snap (top, bottom, right)             
            if (windowPos.cy == monitor.Work.Height ||
                windowPos.cy == monitor.Display.Height)
            {
                return bounds != SnapBounds.None;
            }

            return false;
        }

        /// <summary>
        /// Obtain the corresponding snapped bounds of the window
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitor"></param>
        /// <returns>The snapped bounds</returns>
        private static SnapBounds GetBounds(WINDOWPOS windowPos, Monitor monitor)
        {
            SnapBounds bounds = SnapBounds.None;

            Monitor.Area work = monitor.Work;

            // LEFT
            if (windowPos.x == work.Left)
            {
                bounds |= SnapBounds.Left;
            }

            // TOP
            if (windowPos.y == work.Top)
            {
                bounds |= SnapBounds.Top;
            }

            // RIGHT
            if (windowPos.x + windowPos.cx == work.Left + work.Width)
            {
                bounds |= SnapBounds.Right;
            }

            // BOTTOM
            if (windowPos.y + windowPos.cy == work.Top + work.Height)
            {
                bounds |= SnapBounds.Bottom;
            }

            return bounds;
        }
    }
}
