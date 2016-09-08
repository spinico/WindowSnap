namespace WindowSnap
{
    internal class SnapResult
    {
        public bool IsSnapped { get; }

        public MonitorArea MonitorArea { get; }

        public SnapResult(bool isSnapped, MonitorArea monitorArea)
        {
            IsSnapped = isSnapped;
            MonitorArea = monitorArea;
        }
    }
}
