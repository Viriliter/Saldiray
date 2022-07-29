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

namespace FornoxGUI.SubPages
{
    /// <summary>
    /// Interaction logic for Page_Sub_Turret.xaml
    /// </summary>
    public partial class Page_Sub_Turret : Page
    {
        public Page_Sub_Turret()
        {
            InitializeComponent();
            updateTargetDistance();
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mouseCord = this.PointToScreen(Mouse.GetPosition(this));
            TranslateTransform myTranslate = new TranslateTransform();

            myTranslate.X = mouseCord.X-683;
            myTranslate.Y = mouseCord.Y-384;
            Grid_CrossHair.RenderTransform = myTranslate;
            //Grid_CrossHair.Children.Add(myTranslate);
            updateTargetDistance();
        }

        private void updateTargetDistance()
        {
            //Int32 distance = (Int32)Application.Current.MainWindow.Resources["tarDist"];
            //Txt_TargetDist.Text = "Target Distance" + distance.ToString() + " m";
        }
    }
}
