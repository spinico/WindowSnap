namespace WindowSnap
{
    using System;

    class MonitorArea
    {
        public struct Area
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
            public int Width;
            public int Height;

            public Area(RECT rc)
            {
                Left = rc.left;
                Top = rc.top;
                Right = rc.right;
                Bottom = rc.bottom;

                Width = Math.Abs(rc.right - rc.left);
                Height = Math.Abs(rc.bottom - rc.top);
            }
        }

        public Area Work { get; }
        public Area Display { get; }
        public RECT Offset { get; }

        public MonitorArea(RECT display, RECT work)
        {
            Display = new Area(display);

            Work = new Area(work);

            Offset = new RECT
            {
                left = Math.Abs(work.left - display.left),
                top = Math.Abs(work.top - display.top),
                right = Math.Abs(display.right - work.right),
                bottom = Math.Abs(display.bottom - work.bottom)
            };
        }
    }
}
