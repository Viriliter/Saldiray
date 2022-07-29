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
using System.Windows.Shapes;

namespace FornoxGUI
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();

        }
        /*
        public void InitializeComponent()
        {
            Uri resourceLocater = new System.Uri("/FornoxGUI;component/SplashWindow.xaml", System.UriKind.Relative);

            System.Windows.Application.LoadComponent(this, resourceLocater);

        }
    */
    }
}
