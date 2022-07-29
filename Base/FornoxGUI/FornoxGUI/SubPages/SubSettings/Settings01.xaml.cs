using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using FornoxGUI.ExternalScripts;

namespace FornoxGUI.SubPages.SubSettings
{
    /// <summary>
    /// Settings01.xaml etkileşim mantığı
    /// </summary>
    public partial class Settings01 : Page
    {
        public Settings01()
        {
            InitializeComponent();
            
            ComboBoxItem portItem = new ComboBoxItem();
            ComboBoxItem gbaudItem = new ComboBoxItem();
            ComboBoxItem ugvbaudItem = new ComboBoxItem();

            portItem.Content = (string) App.Current.Resources["GroundTelemetry_PortName"];
            Box1.SelectedItem = portItem;
           
            gbaudItem.Content = (Int32) App.Current.Resources["GroundTelemetry_BaudRate"];
            Box2.SelectedItem = gbaudItem;

            ugvbaudItem.Content = (Int32)App.Current.Resources["UGVTelemetry_BaudRate"];
            Box5.SelectedItem = ugvbaudItem;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch ((Box1.Text).ToString())
            {
                case ("COM1"): { App.Current.Resources["GroundDeviceID"] = "COM1"; break; }
                case ("COM2"): { App.Current.Resources["GroundDeviceID"] = "COM2"; break; }
                case ("COM3"): { App.Current.Resources["GroundDeviceID"] = "COM3"; break; }
                case ("COM4"): { App.Current.Resources["GroundDeviceID"] = "COM4"; break; }
                case ("COM5"): { App.Current.Resources["GroundDeviceID"] = "COM5"; break; }
                case ("COM6"): { App.Current.Resources["GroundDeviceID"] = "COM6"; break; }
                case ("AUTO"):
                    {
                        var devices = GroundTelemetryConnection.GetUSB();
                        string portName = devices[0].PortName;
                        App.Current.Resources["GroundTelemetry_PortName"] = portName;
                        break;
                    }
            }

            //MessageBox.Show(App.Current.Resources["GroundDeviceID"].ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["GroundTelemetry_BaudRate"] = Convert.ToInt32(Box2.Text);
            //MessageBox.Show(App.Current.Resources["GroundDeviceID_BaudRate"].ToString());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["GroundDelay"] = Convert.ToDouble(Box3.Text);
            //MessageBox.Show(App.Current.Resources["GroundDelay"].ToString());

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["UAVSystemID"] = Box4.Text;
            //MessageBox.Show(App.Current.Resources["UAVSystemID"].ToString());

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["UAVSystemID_BaudRate"] = Convert.ToInt32(Box5.Text.ToString());
            //MessageBox.Show(App.Current.Resources["UAVSystemID_BaudRate"].ToString());
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
