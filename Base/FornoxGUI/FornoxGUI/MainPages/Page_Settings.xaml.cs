using System.Windows;
using System.Windows.Controls;

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
using FornoxGUI.SubPages.SubSettings;

namespace FornoxGUI.MainPages
{
    /// <summary>
    /// Page_Settings.xaml etkileşim mantığı
    /// </summary>
    public partial class Page_Settings : Page
    {
        Page page_setting01 = new Settings01();
        Page page_setting02 = new Settings02();
        Page page_setting03 = new Settings03();
        Page page_setting04 = new Settings04();
        Page page_setting05 = new Settings05();

        public Page_Settings()
        {
            InitializeComponent();
            SubSettings.NavigationService.Navigate(page_setting01);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SubSettings.NavigationService.Navigate(page_setting01);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SubSettings.NavigationService.Navigate(page_setting02);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SubSettings.NavigationService.Navigate(page_setting03);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SubSettings.NavigationService.Navigate(page_setting04);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SubSettings.NavigationService.Navigate(page_setting05);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }
    }
}
