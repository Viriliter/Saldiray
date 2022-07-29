using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Ozeki.Media;
using Ozeki.Camera;
using Ozeki.Common;
using WPFMediaKit.DirectShow.Controls;

namespace FornoxGUI.SubPages.SubSettings
{
    /// <summary>
    /// Settings03.xaml etkileşim mantığı
    /// </summary>
    public partial class Settings03 : Page
    {
        /*
        private OzekiCamera _webCamera;
        private DrawingImageProvider _imageProvider;
        private MediaConnector _mediaConnector;
        private CameraURLBuilderWPF _myCameraUrlBuilder;
        private CameraURLBuilderWF _myCameraUrlBuilderWF;
        public string CameraUrl { get; set; }
        public string CameraUrlWF { get; set; }

        public ICommand ComposeCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }

        private void InitCommands()
        {
            ComposeCommand = new RelayCommand(Compose);
            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);
        }
        private void Disconnect()
        {
            Page_SurveillanceCam.videoViewerWF.Stop();

            Page_SurveillanceCam.videoViewer.Stop();
            _webCamera.Stop();
            _mediaConnector.Disconnect(_webCamera.VideoChannel, _imageProvider);
            _webCamera = null;
        }

        private void Compose()
        {
            var data = new CameraURLBuilderData { DeviceTypeFilter = DiscoverDeviceType.USB };
            _myCameraUrlBuilderWF = new CameraURLBuilderWF(data);
            _myCameraUrlBuilder = new CameraURLBuilderWPF(data);
            var result = _myCameraUrlBuilder.ShowDialog();
            if (result != true)
                return;
            CameraUrl = _myCameraUrlBuilder.CameraURL;
            CameraUrlWF = _myCameraUrlBuilderWF.CameraURL;
            Console.WriteLine(CameraUrlWF);
            OnPropertyChanged("CameraUrl");

            InvokeGuiThread(() =>
            {
                ConnectButton.IsEnabled = true;
            });
        }

        private void InvokeGuiThread(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        public Settings03()
        {
            InitCommands();
            InitializeComponent();
            _imageProvider = new DrawingImageProvider();
            _mediaConnector = new MediaConnector();
            Page_SurveillanceCam.videoViewerWF.SetImageProvider(_imageProvider);

            //Page_SurveillanceCam.videoViewer.SetImageProvider(_imageProvider);
        }

        private void Connect()
        {
            if (_webCamera != null)
            {
                Page_SurveillanceCam.videoViewerWF.Stop();

                Page_SurveillanceCam.videoViewer.Stop();
                _webCamera.Stop();
                _mediaConnector.Disconnect(_webCamera.VideoChannel, _imageProvider);
            }
            ConnectButton.IsEnabled = false;
            _webCamera = new OzekiCamera(CameraUrl);
            _webCamera.CameraStateChanged += _webCamera_CameraStateChanged;
            Console.WriteLine(_webCamera.VideoChannel.ToString());
            _mediaConnector.Connect(_webCamera.VideoChannel, _imageProvider);
            _webCamera.Start();
            Page_SurveillanceCam.videoViewer.Start();

            Page_SurveillanceCam.videoViewerWF.Start();

        }

        void _webCamera_CameraStateChanged(object sender, CameraStateEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                switch (e.State)
                {
                    case CameraState.Streaming:
                        DisconnectButton.IsEnabled = true;
                        break;
                    case CameraState.Disconnected:
                        DisconnectButton.IsEnabled = false;
                        ConnectButton.IsEnabled = true;
                        break;
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            Console.WriteLine("****");
            var handler = PropertyChanged;
            Console.WriteLine("*");
            if (handler != null)
            {
                Console.WriteLine("///");
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    internal class RelayCommand : ICommand
    {
        Action<object> _action;

        public RelayCommand(Action action)
        {
            _action = i => action();
        }

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    */

        /********************************************************************
         * 
         * ******************************************************************/

        public static int selectedIndex { get; private set; }

        Page_SurveillanceCam page_surveillance_obj = new Page_SurveillanceCam();


        public Settings03()
        {
            InitializeComponent();


            if (MultimediaUtil.VideoInputDevices.Any())
            {
                Combo_CameraID.ItemsSource = MultimediaUtil.VideoInputNames;
            }
        }


        private void cobVideoSource_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedIndex = Combo_CameraID.SelectedIndex;
        }

        private void LoadCamViewer(object sender, RoutedEventArgs e)
        {
            //page_surveillance_obj.LoadViewer();
            CameraLoaded(this, null);
            Btn_Disconnect.IsEnabled = true;
            Btn_Connect.IsEnabled    = false;

        }

        private void UnloadCamViewer(object sender, RoutedEventArgs e)
        {
            CameraUnloaded(this, null);
            Btn_Connect.IsEnabled    = true;
            Btn_Disconnect.IsEnabled = false;
        }

        public static event EventHandler CameraLoaded;

        public static event EventHandler CameraUnloaded;

    }
}
