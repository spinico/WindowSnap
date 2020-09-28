namespace WindowSnap
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

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
        /// <param name="location">The current position and dimension of the window</param>
        /// <param name="monitors">A list of Monitor instances</param>  
        /// <param name="offset">The window's border offset</param>
        /// <returns>
        /// A result instance indicating if the window is snapped on one
        /// of the available monitors and the corresponding monitor area
        /// </returns>
        internal static SnapResult IsSnapped(ref Rect location, List<Monitor> monitors, Size offset)
        {
            bool snapped = false;
            Monitor monitor = null;

            for (int i = 0; i < monitors.Count; i++)
            {
                snapped = IsSnapped(ref location, monitors[i], offset);

                if (snapped)
                {
                    monitor = monitors[i];
                    break;
                }
            }

            return new SnapResult(snapped, monitor);
        }

        /// <summary>
        /// Snapped edges detection to support specific scenario on Windows 10 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="monitor"></param>
        /// <param name="offset"></param>
        /// <returns>True when window is snapped, false otherwise</returns>
        private static bool IsSnapped(ref Rect location, Monitor monitor, Size offset)
        {
            bool snapped = IsSnapped(ref location, monitor);

            // If not detected, try again with an adjusted window height.
            // This is required to detect a top corner snap on a border 
            // styled window (Windows 10+)
            if (!snapped)
            {
                location.Height -= offset.Height;

                snapped = IsSnapped(ref location, monitor);
            }

            return snapped;
        }

        /// <summary>
        /// Indicate if the window is snapped or not on the given monitor
        /// </summary>
        /// <param name="location"></param>
        /// <param name="monitor"></param>        
        /// <returns>True when window is snapped, false otherwise</returns>
        private static bool IsSnapped(ref Rect location, Monitor monitor)
        {
            // Otherwise, use the standard snap detection            
            SnapBounds bounds = GetBounds(ref location, monitor);

            double windowHeight = location.Height;
            double workHeight = monitor.Work.Height;
            double displayHeight = monitor.Display.Height;

            // For corner snap, must take possible rounding into account
            double loHalfWorkHeight = Math.Floor(workHeight / 2.0d);
            double loHalfDisplayHeight = Math.Floor(displayHeight / 2.0d);
            double hiHalfWorkHeight = Math.Ceiling(workHeight / 2.0d);
            double hiHalfDisplayHeight = Math.Ceiling(displayHeight / 2.0d);

            // Maximized (top, left, bottom right)            
            // Vertical snap (top, bottom)
            // Vertical left snap (top, bottom, left) 
            // Vertical right snap (top, bottom, right) 
            // Corner snap (top left, top right, bottom left, bottom right) 
            if (windowHeight == workHeight ||
                windowHeight == displayHeight ||
                windowHeight == loHalfWorkHeight ||
                windowHeight == loHalfDisplayHeight ||
                windowHeight == hiHalfWorkHeight ||
                windowHeight == hiHalfDisplayHeight)
            {
                return bounds != SnapBounds.None;
            }

            return false;
        }

        /// <summary>
        /// Obtain the corresponding snapped bounds of the window
        /// </summary>
        /// <param name="location"></param>
        /// <param name="monitor"></param>
        /// <returns>The snapped bounds</returns>
        private static SnapBounds GetBounds(ref Rect location, Monitor monitor)
        {
            SnapBounds bounds = SnapBounds.None;

            Monitor.Area work = monitor.Work;
            Monitor.Area display = monitor.Display;

            double left = location.Left;
            double top = location.Top;
            double right = location.Right;
            double bottom = location.Bottom;

            // TOP
            if (top == work.Top ||
                top == display.Top)
            {
                bounds |= SnapBounds.Top;
            }

            // LEFT
            if (left == work.Left ||
                left == display.Left)
            {
                bounds |= SnapBounds.Left;
            }

            // RIGHT
            if (right == work.Right ||
                right == display.Right)
            {
                bounds |= SnapBounds.Right;
            }

            // BOTTOM
            if (bottom == work.Bottom ||
                bottom == display.Bottom)
            {
                bounds |= SnapBounds.Bottom;
            }

            return bounds;
        }
    }
}
