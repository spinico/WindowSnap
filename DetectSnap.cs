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
        /// of the available monitors and the corresponding monitor area
        /// </returns>
        internal static SnapResult IsSnapped(WINDOWPOS windowPos, List<MonitorArea> monitors)
        {
            bool snapped = false;
            MonitorArea monitorArea = monitors[0];

            for (int i = 0; i < monitors.Count; i++)
            {
                snapped = IsSnapped(windowPos, monitors[i]);

                if (snapped)
                {
                    monitorArea = monitors[i];
                    break;
                }
            }

            return new SnapResult(snapped, monitorArea);
        }

        /// <summary>
        /// Indicate if the window is snapped or not on the given monitor area
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitorArea"></param>
        /// <returns>True when window is snapped, false otherwise</returns>
        private static bool IsSnapped(WINDOWPOS windowPos, MonitorArea monitorArea)
        {
            // For Windows 10 and up, check if the docked flag is set
            if (Helpers.IsDocked(windowPos))
            {
                return true;
            }

            // Otherwise, use the standard snap detection            
            SnapBounds bounds = GetBounds(windowPos, monitorArea);

            // Maximized (top, left, bottom right)            
            // Vertical snap (top, bottom)
            // Vertical left snap (top, bottom, left) 
            // Vertical right snap (top, bottom, right)             
            if (windowPos.cy == monitorArea.Work.Height ||
                windowPos.cy == monitorArea.Display.Height)
            {
                return bounds != SnapBounds.None;
            }

            return false;
        }

        /// <summary>
        /// Obtain the corresponding snapped bounds of the window
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitorArea"></param>
        /// <returns>The snapped bounds</returns>
        private static SnapBounds GetBounds(WINDOWPOS windowPos, MonitorArea monitorArea)
        {
            SnapBounds bounds = SnapBounds.None;

            int left = windowPos.x;
            int top = windowPos.y;
            int right = windowPos.x + windowPos.cx;
            int bottom = windowPos.y + windowPos.cy;

            // TOP
            if (top == monitorArea.Work.Top)
            {
                bounds |= SnapBounds.Top;
            }

            // LEFT
            if (left == monitorArea.Work.Left)
            {
                bounds |= SnapBounds.Left;
            }

            // RIGHT
            if (right == monitorArea.Work.Left + monitorArea.Work.Width)
            {
                bounds |= SnapBounds.Right;
            }

            // BOTTOM
            if (bottom == monitorArea.Work.Top + monitorArea.Work.Height)
            {
                bounds |= SnapBounds.Bottom;
            }

            return bounds;
        }
    }
}
