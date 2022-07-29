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

namespace FornoxGUI.SubPages
{
    /// <summary>
    /// Interaction logic for WeaponCam.xaml
    /// </summary>
    public partial class Page_WeaponCam : Page
    {
        // private Uri videoAddress = new Uri("C:\video.wmv");
        /*

        private OzekiCamera _webCamera;
        private DrawingImageProvider _imageProvider;
        private MediaConnector _mediaConnector;
        private CameraURLBuilderWPF _myCameraUrlBuilder;
        public string CameraUrl { get; set; }

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
            videoViewer.Stop();
            _webCamera.Stop();
            _mediaConnector.Disconnect(_webCamera.VideoChannel, _imageProvider);
            _webCamera = null;
        }

        private void Compose()
        {
            var data = new CameraURLBuilderData { DeviceTypeFilter = DiscoverDeviceType.USB };
            _myCameraUrlBuilder = new CameraURLBuilderWPF(data);
            var result = _myCameraUrlBuilder.ShowDialog();
            if (result != true)
                return;
            CameraUrl = _myCameraUrlBuilder.CameraURL;

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

        public Page_WeaponCam()
        {
            InitCommands();
            InitializeComponent();
            _imageProvider = new DrawingImageProvider();
            _mediaConnector = new MediaConnector();
            videoViewer.SetImageProvider(_imageProvider);
        }



        private void Connect()
        {
            if (_webCamera != null)
            {
                videoViewer.Stop();
                _webCamera.Stop();
                _mediaConnector.Disconnect(_webCamera.VideoChannel, _imageProvider);
            }
            ConnectButton.IsEnabled = false;
            _webCamera = new OzekiCamera(CameraUrl);
            _webCamera.CameraStateChanged += _webCamera_CameraStateChanged;
            _mediaConnector.Connect(_webCamera.VideoChannel, _imageProvider);
            _webCamera.Start();
            videoViewer.Start();
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
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    */
    }
    
}