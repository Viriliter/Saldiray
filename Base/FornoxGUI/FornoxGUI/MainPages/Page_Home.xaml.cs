extern alias notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FornoxGUI.ExternalScripts;
using FornoxGUI.Communication;
using notification::WPFNotification.Services;
using notification::WPFNotification.Model;
using System.Globalization;

namespace FornoxGUI.MainPages
{
    /// <summary>
    /// Page_Home.xaml etkileşim mantığı
    /// </summary>
    public partial class Page_Home : Page
    {
        bool isTelemetryConnected = false;
        bool isUGVConnected = false;
        string ID = null;
        BackgroundWorker bw = null;
        //TelemetryHandler tHandler = new TelemetryHandler();
        Thread loadingBarThread;

        private static Teleoperation teleoperator = new Teleoperation();

        public Page_Home()
        {
            InitializeComponent();
            teleoperator.TelemetryConnected += HandleTelemetryConnected;
            teleoperator.UGVConnected += HandleUGVConnected;
            teleoperator.UGVDisconnected += HandleUGVDisconnected;
            teleoperator.TelemetryDisconnected += HandleTelemetryDisconnected;

        }
        /*
        private void Btn_ConnectUGV_Click(object sender, RoutedEventArgs e)
        {
            //Try to connect ground telemetry first.
            if (bw == null)
            {
                bw = new BackgroundWorker();
                bw.DoWork += ReceiveData_DoWork;
                bw.RunWorkerAsync();
            }
            //Handle connection status animation
            loadingAnimation(isTelemetryConnected);
            loadingAnimation2Async(isUGVConnected);

            if (isTelemetryConnected)
            {
                //Now, try to connect UGV. Wait the receive heartbeat signal
                TryToConnectUGV();
                if (isUGVConnected)
                {
                    MessageBox.Show("Connection is established.", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ground telemetry cannot receive heartbeat.", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Disconnection in ground telemetry.\nTry to Reconnect", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void loadingAnimation(bool isTelemetryConnected)
        {
            if (isTelemetryConnected)
            {
                Computer.Fill = Brushes.PaleGreen;
            }
            else
            {
                var converter = new System.Windows.Media.BrushConverter();
                Computer.Fill = Brushes.White;
            }
        }

        private async Task loadingAnimation2Async(bool isConnected)
        {
            int pos = 0;
            int loop = 0;
            List<Ellipse> EllipseArray = new List<Ellipse>();
            EllipseArray.Add(C1); EllipseArray.Add(C2); EllipseArray.Add(C3);
            EllipseArray.Add(C4); EllipseArray.Add(C5); EllipseArray.Add(C6);
            EllipseArray.Add(C7); EllipseArray.Add(C8);
            for (pos = 0; pos < 8; pos++)
            {
                if (EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill == Brushes.LightGreen)
                {
                    EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.White;
                }
                else
                {
                    EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.LightGreen;
                }
                if (isConnected)
                {
                    for (pos = 0; pos < 8; pos++)
                    {
                        EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.LightGreen;
                    }
                    break;
                }
                await Task.Delay(300);
                //System.Threading.Thread.Sleep(300);
                if (pos == 7)
                {
                    pos = -1;
                    loop++;
                }
                if (loop > 5)
                {
                    loop = 0;
                    MessageBox.Show("Timeout Error:\nApplication is failed to connect UGV.\nTry to Reconnect.", "Connection Failure", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
                }
            }
        }

        private void loadingAnimation3(bool isUGVConnected)
        {
            if (isUGVConnected && isTelemetryConnected)
            {
                Btn_ConnectUGV.Background = Brushes.LawnGreen;
                Btn_ConnectUGV.BorderBrush = Brushes.PaleGreen;
                UGV.Fill = Brushes.PaleGreen;
            }
            else
            {
                var converter = new System.Windows.Media.BrushConverter();
                Btn_ConnectUGV.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
                Btn_ConnectUGV.BorderBrush = Brushes.White;
                UGV.Fill = Brushes.White;
            }

        }

        private async Task TryToConnectGround()
        {
            try
            {
                isTelemetryConnected = ConnectGround();
                while (true)
                {
                    if (isTelemetryConnected) { break; }
                    await Task.Delay(5000);
                    break;
                }
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message.ToString(), "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void TryToConnectUGV()
        {
            try
            {
                //ConnectUGV();
                ConnectUGV_test();
                while (isUGVConnected)
                {
                    ReceiveData();
                    //await Task.Delay(5000);
                    //break;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString(), "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private bool ConnectGround()
        {
            bool isInitialized = TelemetryHandler.Initialize();
            */
            /*
            switch ((byte)App.Current.Resources["IsConnected"])
            {
                case 0x01: isTelemetryConnected = true; break;
                default: isTelemetryConnected = false; break;
            }
            */
            /*
            return isInitialized;
        }

        private bool ConnectUGV_test()
        {
            isUGVConnected = true;
            loadingAnimation3(isUGVConnected);
            return isUGVConnected;
        }

        private void ReceiveData()
        {
            TelemetryHandler.readTelemetry2();
        }

        private void ConnectUGV()
        {
            try
            {
                isUGVConnected = false;//It is for test!!!!--->"false"
                loadingAnimation3(isUGVConnected);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString(), "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        */
        /*
        private void capturePageChange(object sender, PagesChangedEventArgs result)
        {
            MessageBox.Show("Changing is not recommeded during setup process.");
        }*/
        

        /********************************************************************
         * 
         ********************************************************************/
        private void ComputerAnimation(bool isConnected)
        {
            if (isConnected)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Computer.Fill = Brushes.PaleGreen;
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Computer.Fill = Brushes.White;
                });
            }
        }

        private void LoadingBarAnimation()
        {
            List<Ellipse> EllipseArray = new List<Ellipse>();
            int pos = 0;
            int loop=0;

            while ( !isUGVConnected && loop<5)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    EllipseArray.Add(C1); EllipseArray.Add(C2); EllipseArray.Add(C3);
                    EllipseArray.Add(C4); EllipseArray.Add(C5); EllipseArray.Add(C6);
                    EllipseArray.Add(C7); EllipseArray.Add(C8);

                    for (pos = 0; pos < 8; pos++)
                    {
                        if (EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill == Brushes.LightGreen)
                        {
                            EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.White;
                        }
                        else
                        {
                            EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.LightGreen;
                        }
                        //Thread.Sleep(500);

                        /*if (isConnected)
                        {
                            for (pos = 0; pos < 8; pos++)
                            {
                                EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.LightGreen;
                            }
                            break;
                        }*/
                    }
                });
                loop++;
                //if (isConnected) { break; }
            }
            if (!isUGVConnected)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    for (pos = 0; pos < 8; pos++)
                    {
                        EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.White;
                    }
                });
                MessageBox.Show("Timeout error in communication with vehicle.");
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    for (pos = 0; pos < 8; pos++)
                    {
                        EllipseArray.OfType<Ellipse>().ElementAt(pos).Fill = Brushes.LightGreen;
                    }
                });
            }
            loadingBarThread.Abort();
        }

        private void UGVAnimation(bool isConnected)
        {
            if (isConnected)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    UGV.Fill = Brushes.PaleGreen;
                    C1.Fill = Brushes.PaleGreen;
                    C2.Fill = Brushes.PaleGreen;
                    C3.Fill = Brushes.PaleGreen;
                    C4.Fill = Brushes.PaleGreen;
                    C5.Fill = Brushes.PaleGreen;
                    C6.Fill = Brushes.PaleGreen;
                    C7.Fill = Brushes.PaleGreen;
                    C8.Fill = Brushes.PaleGreen;
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    UGV.Fill = Brushes.White;
                    C1.Fill = Brushes.White;
                    C2.Fill = Brushes.White;
                    C3.Fill = Brushes.White;
                    C4.Fill = Brushes.White;
                    C5.Fill = Brushes.White;
                    C6.Fill = Brushes.White;
                    C7.Fill = Brushes.White;
                    C8.Fill = Brushes.White;
                });
            }
        }

        private void Btn_ConnectUGV_Click2(object sender, RoutedEventArgs e)
        {
            if (bw == null)
            {
                bw = new BackgroundWorker();
                bw.DoWork += ReceiveData_DoWork;
                //bw.RunWorkerCompleted();
                bw.RunWorkerAsync();
            }
        }

        private void ReceiveData_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Teleoperator is initalizing...\n\n");
            teleoperator.InitializeThread();
            //bool isInitialized = TelemetryHandler.Initialize();
            //ReceiveData();
        }

        private void HandleTelemetryConnected(object sender, EventArgs e)
        {
            //Console.WriteLine("Telemetry connected");
            //end animation
            ComputerAnimation(true);

            //ThreadStart loadbref = new ThreadStart(LoadingBarAnimation);
            //Define thread's sleep time in ms
            //loadingBarThread = new Thread(loadbref);
            //Start thread
            //loadingBarThread.Start();
        }


        private void HandleUGVConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connection is established");
            //end animation
            isUGVConnected = true;
            UGVAnimation(true);
            //LoadingBarAnimation(false);
            
            INotificationDialogService _dailogService = new NotificationDialogService();
            var newNotification = new Notification()
            {
                Title = "Machine error",
                Message = "Error!! Please check your Machine Code and Try Again"
            };
            //Console.WriteLine(_dailogService.ToString());
            //_dailogService.ShowNotificationWindow(newNotification);

            Btn_ConnectUGV.Content = "Disconnect from UGV";

        }
        private void HandleUGVDisconnected(object sender,EventArgs e)
        {
            Btn_ConnectUGV.Content = "Connect to UGV";
        }
        private void HandleTelemetryDisconnected(object sender, EventArgs e)
        {
            Btn_ConnectUGV.Content = "Connect to UGV";
        }
    }
}
