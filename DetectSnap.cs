namespace WindowSnap
{
    using System;

    [Flags()]
    enum SnapRegion
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
        /// Indicate if the window is snapped or not
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitorArea"></param>
        /// <returns>True when window is snapped, false otherwise</returns>
        public static bool IsSnapped(WINDOWPOS windowPos, MonitorArea monitorArea)
        {
            // For Windows 10 and up, check if the docked flag is set
            if (Helpers.IsDocked(windowPos))
            {
                return true;
            }

            // Otherwise, use the standard snap detection            
            SnapRegion region = GetRegion(windowPos, monitorArea);

            // Maximized (top, left, bottom right)            
            // Vertical snap (top, bottom)
            // Vertical left snap (top, bottom, left) 
            // Vertical right snap (top, bottom, right)             
            if (windowPos.cy == monitorArea.Work.Height ||
                windowPos.cy == monitorArea.Display.Height)
            {
                return ValidateRegion(region) != SnapRegion.None;
            }

            return false;
        }

        /// <summary>
        /// Obtain the corresponding snap region using the 
        /// specified window and monitor coordinates
        /// </summary>
        /// <param name="windowPos"></param>
        /// <param name="monitorArea"></param>
        /// <returns>The active snap region flags</returns>
        private static SnapRegion GetRegion(WINDOWPOS windowPos, MonitorArea monitorArea)
        {
            SnapRegion region = SnapRegion.None;

            int left = windowPos.x;
            int top = windowPos.y;
            int right = windowPos.x + windowPos.cx;
            int bottom = windowPos.y + windowPos.cy;

            // TOP
            if (top <= monitorArea.Work.Top)
            {
                region |= SnapRegion.Top;
            }

            // LEFT
            if (left == monitorArea.Work.Left)
            {
                region |= SnapRegion.Left;
            }

            // RIGHT
            if (right == monitorArea.Work.Left + monitorArea.Work.Width)
            {
                region |= SnapRegion.Right;
            }

            // BOTTOM
            if (bottom >= monitorArea.Work.Top + monitorArea.Work.Height)
            {
                region |= SnapRegion.Bottom;
            }

            return region;
        }

        /// <summary>
        /// Filter invalid region
        /// </summary>
        /// <param name="region"></param>
        /// <returns>The region if validated, no region otherwise</returns>
        private static SnapRegion ValidateRegion(SnapRegion region)
        {
            // A single region is not a valid snap area
            return region == SnapRegion.Left || region == SnapRegion.Right ||
                   region == SnapRegion.Top || region == SnapRegion.Bottom ?
                   SnapRegion.None : region;
        }
    }
}
