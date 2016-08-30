namespace WindowSnap
{
    using System.Windows;

    public class EdgeOffset
    {
        public Size FixedFrame { get; set; }
        public Size ResizeFrame { get; set; }
        public Size ThickBorder { get; set; }
        public Size ThinBorder { get; set; }

        public Size Value
        {
            get
            {
                return new Size()
                {
                    Width = FixedFrame.Width + ResizeFrame.Width +
                    ThickBorder.Width + ThinBorder.Width,

                    Height = FixedFrame.Height + ResizeFrame.Height +
                    ThickBorder.Height + ThinBorder.Height
                };
            }
        }
    }
}
