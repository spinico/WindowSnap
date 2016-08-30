namespace Demo
{
    using WindowSnap;
    using System;
    using System.ComponentModel;
    using System.Windows;    
    using System.Diagnostics;

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private WindowSnap _windowSnap;

        public event PropertyChangedEventHandler PropertyChanged;

        public EdgeOffset EdgeOffset
        {
            get { return (EdgeOffset)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }       
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("EdgeOffset", typeof(EdgeOffset), typeof(MainWindow), 
                new PropertyMetadata(new EdgeOffset()));

        public bool IsSnapped
        {
            get { return _windowSnap != null ? _windowSnap.IsSnapped : false; }
        }

        public bool WindowArranging
        {
            get { return SnapSettings.WindowArranging; }
            set
            {
                SnapSettings.WindowArranging = value;

                NotifyPropertyChanged("WindowArranging");
            }
        }

        public bool SnapSizing
        {
            get { return SnapSettings.SnapSizing; }
            set
            {
                SnapSettings.SnapSizing = value;

                NotifyPropertyChanged("SnapSizing");
            }
        }

        public bool DockMoving
        {
            get { return SnapSettings.DockMoving; }
            set
            {
                SnapSettings.DockMoving = value;

                NotifyPropertyChanged("DockMoving");
            }
        }

        public bool DragFromMaximize
        {
            get { return SnapSettings.DragFromMaximize; }
            set
            {
                SnapSettings.DragFromMaximize = value;

                NotifyPropertyChanged("DragFromMaximize");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Must be called on source initialized event so the 
            // window handle can be obtain to hook the message 
            // proc handler properly
            _windowSnap = new WindowSnap(this);

            _windowSnap.Snapped += Snapped;
            _windowSnap.Unsnapped += Unsnapped;
            _windowSnap.EdgeOffsetChanged += EdgeOffsetChanged;
        }

        private void Unsnapped(object sender, EventArgs e)
        {
            Debug.Print("Unsnapped");

            NotifyPropertyChanged("IsSnapped");            
        }

        private void Snapped(object sender, EventArgs e)
        {       
            Debug.Print("Snapped");

            NotifyPropertyChanged("IsSnapped");
        }

        private void EdgeOffsetChanged(object sender, EventArgs e)
        {
            EdgeOffset = (e as EdgeOffsetChangedEventArgs).Offset;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _windowSnap.Snapped -= Snapped;
            _windowSnap.Unsnapped -= Unsnapped;
            _windowSnap.EdgeOffsetChanged -= EdgeOffsetChanged;
            _windowSnap = null;
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
