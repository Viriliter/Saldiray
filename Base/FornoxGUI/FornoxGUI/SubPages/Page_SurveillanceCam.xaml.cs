using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using WPFMediaKit.DirectShow.Controls;
using FornoxGUI.SubPages.SubSettings;

namespace FornoxGUI.SubPages
{
    /// <summary>
    /// Page_RawVideo.xaml etkileşim mantığı
    /// </summary>
    public partial class Page_SurveillanceCam : Page
    {
        /*
        // private Uri videoAddress = new Uri("C:\video.wmv");
        public static VideoViewerWPF videoViewer = new VideoViewerWPF();
        public static VideoViewerWF videoViewerWF = new Ozeki.Media.VideoViewerWF();

        public static bool isViewerAdded = false;

        public Page_SurveillanceCam()
        {
            InitializeComponent();
            //addVideoViewer();
        }

        private void addVideoViewer()
        {               
            //videoViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            videoViewer.VerticalAlignment = VerticalAlignment.Stretch;
            if (!isViewerAdded)
            {
                this.Grid_Viewer.Children.Insert(0, videoViewer);
                isViewerAdded = true;
            }
        }
        */

        /********************************************************************************
         * 
        *********************************************************************************/

        public bool isViewerLoaded = false;
        
        public Page_SurveillanceCam()
        {
            InitializeComponent();
            //LoadViewer();
            //Add camera handlers
            Settings03.CameraLoaded -= HandleCameraLoaded;
            Settings03.CameraLoaded -= HandleCameraUnloaded;

            Settings03.CameraLoaded   += HandleCameraLoaded;
            Settings03.CameraUnloaded += HandleCameraUnloaded;
        }

        public void HandleCameraLoaded(object sender, EventArgs e)
        {
            if (!isViewerLoaded)
            {
                LoadViewer();
                isViewerLoaded = true;

            }
        }

        public void HandleCameraUnloaded(object sender, EventArgs e)
        {
            UnloadViewer();
        }

        public void  LoadViewer()
        {
            //Console.WriteLine(Settings03.selectedIndex);
            if (Settings03.selectedIndex < 0)
            {
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                cameraCaptureElement.VideoCaptureDevice = MultimediaUtil.VideoInputDevices[Settings03.selectedIndex];
            });
        }

        public void UnloadViewer()
        {
            Console.WriteLine("disposeee");
            cameraCaptureElement.VideoCaptureDevice.Dispose();
        }

        /*
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        */

    }
}
