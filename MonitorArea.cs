namespace WindowSnap
{
    using System;

    class MonitorArea
    {
        public struct Region
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
            public int Width;
            public int Height;
        }

        public Region Work;
        public Region Display;
        public RECT Offset;

        public MonitorArea(RECT display, RECT work)
        {
            Display.Left = display.left;
            Display.Right = display.right;
            Display.Top = display.top;
            Display.Bottom = display.bottom;
            Display.Width = Math.Abs(display.right - display.left);
            Display.Height = Math.Abs(display.bottom - display.top);

            Work.Left = work.left;
            Work.Right = work.right;
            Work.Top = work.top;
            Work.Bottom = work.bottom;
            Work.Width = Math.Abs(work.right - work.left);
            Work.Height = Math.Abs(work.bottom - work.top);

            Offset = new RECT()
            {
                left = Math.Abs(work.left - display.left),
                top = Math.Abs(work.top - display.top),
                right = Math.Abs(display.right - work.right),
                bottom = Math.Abs(display.bottom - work.bottom)
            };
        }
    }
}
