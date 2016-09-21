namespace WindowSnap
{
    internal class SnapResult
    {
        public bool IsSnapped { get; }

        public Monitor Monitor { get; }

        public SnapResult(bool isSnapped, Monitor monitor)
        {
            IsSnapped = isSnapped;
            Monitor = monitor;
        }
    }
}
