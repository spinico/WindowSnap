namespace WindowSnap.Platform
{
    using System.Windows;
    using System.Windows.Media;

    static class Dpi
    {
        private static Matrix _transformToDevice;
        private static Matrix _transformToDip;
        private static Point _systemDpi;
        private static Point _ratio;

        static Dpi()
        {
            _systemDpi = SafeNativeMethods.GetDotsPerInch();

            _transformToDip = Matrix.Identity;
            _transformToDip.Scale(96d / _systemDpi.X, 96d / _systemDpi.Y);

            _transformToDevice = Matrix.Identity;
            _transformToDevice.Scale(_systemDpi.X / 96d, _systemDpi.Y / 96d);

            _ratio = new Point(_transformToDevice.M11, _transformToDevice.M22);
        }

        public static double ToLogicalX(double deviceX)
        {
            return deviceX / _ratio.X;
        }

        public static double ToLogicalY(double deviceY)
        {
            return deviceY / _ratio.Y;
        }
    }
}
