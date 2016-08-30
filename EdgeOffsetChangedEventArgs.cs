namespace WindowSnap
{
    using System;

    public class EdgeOffsetChangedEventArgs : EventArgs
    {
        public EdgeOffset Offset { get; private set; }

        public EdgeOffsetChangedEventArgs(EdgeOffset offset)
        {
            Offset = offset;
        }
    }
}
