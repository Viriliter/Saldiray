using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using FornoxGUI;

namespace FornoxGUI.SubPages
{
    /// <summary>
    /// Interaction logic for Page_Sub_VNavigator.xaml
    /// </summary>
    public partial class Page_Sub_VNavigator : Page
    {
        //UGVState Updater Thread Properties
        public static Thread UGVStateThread { get; private set; }
        public static int threadSleep { get; private set; }

        //Button Click States
        bool isForwardClicked   =  false;
        bool isForRightClicked  =  false;
        bool isBackRightClicked =  false;
        bool isBackwardClicked  =  false;
        bool isBackLeftClicked  =  false;
        bool isLeftClicked      =  false;
        bool isRightClicked   = false;
        bool isForLeftClicked   =  false;

        public Page_Sub_VNavigator()
        {
            InitializeComponent();
            //Try to connect gamepad
            GamepadThread.Initialize();
            if (GamepadThread.isInitialized) { Console.WriteLine("Gamepad is connected"); }


            ThreadStart childref = new ThreadStart(updateUGVState);
            threadSleep = 100;

            UGVStateThread = new Thread(childref);
            //Start thread
            UGVStateThread.Start();
        }

        private void updateUGVState()
        {
            while (true)
            { 
                App.Current.Resources["UGVIsStarted"]           = App.Current.Resources["Key_A"];
                App.Current.Resources["UGVIsPlugHeated"]        = App.Current.Resources["Key_Y"];
                App.Current.Resources["UGVIsEngineStarted"]     = App.Current.Resources["Key_B"];
                App.Current.Resources["UGVIsDeadManActivated"]  = false;//App.Current.Resources[""];
                /*
                if ((bool)App.Current.Resources["Key_RT"] == true && (bool)App.Current.Resources["UGVThrottleInc"] == false)
                {
                    App.Current.Resources["UGVThrottleInc"] = true; App.Current.Resources["UGVThrottleDec"] = false;
                }
                if ((bool)App.Current.Resources["Key_LT"] == true && (bool)App.Current.Resources["UGVThrottleDec"] == false)
                {
                    App.Current.Resources["UGVThrottleInc"] = false; App.Current.Resources["UGVThrottleDec"] = true;
                }
                */
                App.Current.Resources["UGVThrottleInc"]         = App.Current.Resources["Key_RT"];
                App.Current.Resources["UGVThrottleDec"]         = App.Current.Resources["Key_LT"];
                /*
                Console.WriteLine("*-*-*-*-*-*");
                Console.WriteLine(App.Current.Resources["UGVIsStarted"]         );
                Console.WriteLine(App.Current.Resources["UGVIsPlugHeated"]      );
                Console.WriteLine(App.Current.Resources["UGVIsEngineStarted"]   );
                Console.WriteLine(App.Current.Resources["UGVIsDeadManActivated"]);
                Console.WriteLine(App.Current.Resources["UGVThrottleInc"]       );
                Console.WriteLine(App.Current.Resources["UGVThrottleDec"]       );
                Console.WriteLine("*-*-*-*-*-*");
                */
                Thread.Sleep(threadSleep);
            }
        }

        private void Btn_navForLeft_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_forleft"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0];

            if (!isForLeftClicked)
            {
                App.Current.Resources["JoyL_X"] = 0x25;
                App.Current.Resources["JoyL_Y"] = 0x25;

                colorElem.Fill = Brushes.IndianRed;
                isForLeftClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;

                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isForLeftClicked = false;
            }

        }

        private void Btn_navBackLeft_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_backleft"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[2];

            if (!isBackLeftClicked)
            {
                App.Current.Resources["JoyL_X"] = 0x25;
                App.Current.Resources["JoyL_Y"] = 0xD9;

                colorElem.Fill = Brushes.IndianRed;
                isBackLeftClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;

                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isBackLeftClicked = false;
            }

        }

        private void Btn_navBackRight_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_backright"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[4];

            if (!isBackRightClicked)
            {
                colorElem.Fill = Brushes.IndianRed;
                App.Current.Resources["JoyL_X"] = 0xD9;
                App.Current.Resources["JoyL_Y"] = 0xD9;
                isBackRightClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isBackRightClicked = false;
            }



        }

        private void Btn_navForRight_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_forright"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[4];

            if (!isForRightClicked)
            {
                App.Current.Resources["JoyL_X"] = 0xD9;
                App.Current.Resources["JoyL_Y"] = 0x25;
                colorElem.Fill = Brushes.IndianRed;
                isForRightClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isForRightClicked = false;
            }
        }

        private void Btn_navLeft_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_left"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0];

            if (!isLeftClicked)
            {
                App.Current.Resources["JoyL_X"] = 0x00;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                colorElem.Fill = Brushes.IndianRed;
                isLeftClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isLeftClicked = false;
            }
        }

        private void Btn_navRight_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_right"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0];

            if (!isRightClicked)
            {
                App.Current.Resources["JoyL_X"] = 0xFF;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                colorElem.Fill = Brushes.IndianRed;
                isRightClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isRightClicked = false;
            }
        }

        private void Btn_navBackward_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_backward"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0]).Children[0];

            if (!isBackwardClicked)
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0xFF;
                colorElem.Fill = Brushes.IndianRed;
                isBackwardClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isBackwardClicked = false;
            }
        }

        private void Btn_navForward_Click(object sender, RoutedEventArgs e)
        {

            Viewbox vb = (Viewbox)App.Current.Resources["nav_forward"];
            Path colorElem = (Path)((Canvas)((Canvas)((Canvas)((Canvas)vb.Child).Children[0]).Children[0]).Children[0]).Children[0];

            if (!isForwardClicked)
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x00;
                colorElem.Fill = Brushes.IndianRed;
                isForwardClicked = true;
            }
            else
            {
                App.Current.Resources["JoyL_X"] = 0x7F;
                App.Current.Resources["JoyL_Y"] = 0x7F;
                Color gray = Color.FromRgb(51, 51, 51);
                colorElem.Fill = new SolidColorBrush(gray);
                isForwardClicked = false;
            }
        }

    }
}
