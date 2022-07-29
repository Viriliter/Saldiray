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
    /// Page_Sub_Settings.xaml etkileşim mantığı
    /// </summary>
    public partial class Page_Sub_Settings : Page
    {
        static int com2ardu_BaudRate;
        static int ardu2uav_BaudRate;
        static string missionfile_Path;
        static double radius_Loiter;
        static double artificialhoriizontal_Delay;
        static double instrument_Delay;

        private static void Setcom2ardu_BaudRate(int new_com2ardu_BaudRate) { com2ardu_BaudRate = new_com2ardu_BaudRate; }
        private static void Setardu2uav_BaudRate(int new_ardu2uav_BaudRate) { ardu2uav_BaudRate = new_ardu2uav_BaudRate; }
        private static void Setmissionfile_Path(string new_missionfile_Path) { missionfile_Path = new_missionfile_Path; }
        private static void Setradius_Loiter(double new_radius_Loiter) { radius_Loiter = new_radius_Loiter; }
        private static void Setartificialhoriizontal_Delay(double new_artificialhoriizontal_Delay) { artificialhoriizontal_Delay = new_artificialhoriizontal_Delay; }
        private static void Setinstrument_Delay(double new_instrument_Delay) { instrument_Delay = new_instrument_Delay; }


        public Page_Sub_Settings()
        {
            InitializeComponent();
        }

        public static void loadGUI2ArduinoConnection() { }
        public static void loadArduino2UAVConnection() { }
        public static void loadMissionPlanner() { }
        public static void loadSimulationTest() { }
        public static void loadFlightControl() { }
    }
}
