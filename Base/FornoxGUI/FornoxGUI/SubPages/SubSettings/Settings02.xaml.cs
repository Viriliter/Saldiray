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

namespace FornoxGUI.SubPages.SubSettings
{
    /// <summary>
    /// Settings02.xaml etkileşim mantığı
    /// </summary>
    public partial class Settings02 : Page
    {

        public Settings02()
        {
            InitializeComponent();
            BrowseLocation.Content = App.Current.Resources["FlightDirectory"];
        }

        public void Button_Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BrowseDirectory();
            BrowseLocation.Content = App.Current.Resources["FlightDirectory"];
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //App.Current.Resources[""] = Convert.ToDouble(Box1.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //App.Current.Resources[""] = Convert.ToDouble(Box2.Text);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //App.Current.Resources[""] = Convert.ToDouble(Box3.Text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
