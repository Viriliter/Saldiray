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
using Microsoft.Maps.MapControl.WPF;
using DraggablePushpin;
using System.IO;
using FornoxGUI.MainPages;

namespace FornoxGUI
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {

        static public List<Location> pinArray = new List<Location>();
        static public int[] elevationArray;

        Page page_home = new Page();
        Page page_missionplanner = new Page();
        Page page_drivecontrol = new Page();
        Page page_simulationtest = new Page();
        Page page_settings = new Page();
        Page page_aboutus = new Page();

        //Page Conditions

        bool IsHomeLoaded = false;
        bool IsMissionPlannerLoaded = true;
        bool IsSimulationTestLoaded = false;
        bool IsDriveControlLoaded = false;
        bool IsSettingsLoaded = false;
        bool IsAboutUsLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializePages();
        }

        public void InitializePages()
        {
            //Load Pages
            page_home = new Page_Home();
            page_missionplanner = new Page_MissionPlanner();
            page_simulationtest = new Page_SimulationTest();
            page_drivecontrol = new Page_DriveControl();
            page_settings = new Page_Settings();
            page_aboutus = new Page_AboutUs();

            IsHomeLoaded = true;
            IsSettingsLoaded = true;
            IsAboutUsLoaded = true;

            Main.NavigationService.Navigate(page_home);
        }


        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            if (IsHomeLoaded)
            {
                Main.NavigationService.Navigate(page_home);
                IsMissionPlannerLoaded = true;
            }
        }

        public void Btn_MissionPlanner_Click(object sender, RoutedEventArgs e)
        {

            if (IsMissionPlannerLoaded)
            {
                Main.NavigationService.Navigate(page_missionplanner);
                IsSimulationTestLoaded = true;
                IsMissionPlannerLoaded = true;
                page_simulationtest = new Page_SimulationTest();
            }

        }

        private void Btn_SimulationTest_Click(object sender, RoutedEventArgs e)
        {
            if (IsSimulationTestLoaded)
            {
                Main.NavigationService.Navigate(page_simulationtest);
            }
            else
            {
                MessageBox.Show("First ensure that mission is planned from mission planner section.","Warning");
            }
        }

        private void Btn_DriveControl_Click(object sender, RoutedEventArgs e)
        {
            if (IsMissionPlannerLoaded)
            {
                if (!IsDriveControlLoaded)
                {
                    /*if (!page_drivecontrol.IsInitialized)
                    {
                        page_drivecontrol = new Page_DriveControl();
                    }*/
                    page_drivecontrol = new Page_DriveControl();
                    Main.NavigationService.Navigate(page_drivecontrol);
                    IsDriveControlLoaded = true;
                }
            }

            else
            {
                MessageBox.Show("First ensure that mission is planned from mission planner section.", "Warning");
            }
        }

        private void Btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (IsSettingsLoaded)
            {
                Main.NavigationService.Navigate(page_settings);
            }
        }

        private void Btn_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            if (IsAboutUsLoaded)
            {
                Main.NavigationService.Navigate(page_aboutus);
            }
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            string path = ((string)App.Current.Resources["FlightDirectory"]) + "\\UAVMissions.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Environment.Exit(0);
            this.Close();
        }

    }
}
