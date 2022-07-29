using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FornoxGUI;
using FornoxGUI.Communication;
using Microsoft.Maps.MapControl.WPF;

namespace FornoxGUI.ExternalScripts
{
    /// <summary>
    /// Reads mission file that is located in the specified local repo.
    /// Includes message generator for the mission to send to the vehicle.
    /// </summary>
    class MissionReader
    {
        public byte _STARTFIELD = 0xfe;
        public byte _MESSAGEID = 0x02;
        static string missionName;
        static int numberWaypoint;
        static List<double> waypoint_Lat = new List<double>();
        static List<double> waypoint_Long = new List<double>();
        static List<double> waypoint_Alt = new List<double>();
        static List<double> waypoint_Command = new List<double>();

        private static void MissionParser()
        {
            //Call this function to send navigation points.
        }

        public string Get_missionName { get { return missionName; } }

        public int Get_numberWayPoint { get { return numberWaypoint; } }


        /// <summary>
        /// Gets the text file as string from the predefined path.
        /// </summary>
        /// <returns></returns>
        private static string GetText()
        {
            try
            {
                //string path = ((string)App.Current.Resources["FlightDirectory"])+ "\\PathPlan.txt";
                string path = "C:\\Users\\ASUS\\PathPlan.txt";
                return System.IO.File.ReadAllText(path);
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Parses text in a suitable format for user inteface.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static List<NavigationPoint> ParseMainText()
        {
            //Get mission text f,le from local resource
            string mainText = GetText();
            //Check the text format is in the predefined format.
            if (mainText == null) { return null; }
            if (mainText.First().Equals('<'))
            {
                mainText.Remove(0, 1);
                if (mainText.Last().Equals('>'))
                {
                    mainText.Remove(mainText.Length - 1, 1);
                }
                else throw new Exception("Target script is not in predefined format.");
            }
            else throw new Exception("Target script is not in predefined format.");


            string[] tokens = mainText.Split(new[] { "//" }, StringSplitOptions.None);

            List<NavigationPoint> list = new List<NavigationPoint>();

            foreach (string element in tokens)
            {
                string[] str = element.Split(new[] { "-" }, StringSplitOptions.None);
                NavigationPoint navigationPoint = new NavigationPoint();
                switch (element.Length)
                {
                    case (2):
                    {
                        missionName = str[0];
                        numberWaypoint = Convert.ToInt16(str[1]);
                        break;
                    }
                    case (5):
                    {
                        navigationPoint.Latitude  = Convert.ToDouble(str[0]);
                        navigationPoint.Longitude = Convert.ToDouble(str[1]);
                        navigationPoint.Altitude  = Convert.ToDouble(str[2]);
                        navigationPoint.Command   = Convert.ToByte(str[2]);

                        switch (str[3])
                        {
                            case ("Pass"):
                            {
                                navigationPoint.Command = 0x00;
                                break;
                            }
                            case ("Arm Weapon"):
                            {
                                navigationPoint.Command = 0x01;
                                break;
                            }
                            case ("Surveillance"):
                            {
                                navigationPoint.Command = 0x02;
                                break;
                            }
                            default:
                            {
                                navigationPoint.Command = 0x00;
                                break;
                            }
                        }
                        break;
                    }
                }
                list.Add(navigationPoint);
            }
            return list;
        }

        /// <summary>
        /// Generates message that defines the waypoints on the map
        /// </summary>
        /// <returns></returns>
        private byte[] generateMessage()
        {
            try
            {
                List<Byte> message = new List<Byte>();
                List<NavigationPoint> list = ParseMainText();
                if (list == null) { throw new Exception(); }
                int list_length = list.Count-1;
                Console.WriteLine("list_length");

                message.Add(_STARTFIELD);
                message.Add(_MESSAGEID);
                message.Add((byte)list_length);

                foreach (Location element in FornoxGUI.MainWindow.pinArray)
                {
                    Console.WriteLine(element.Latitude);
                    Console.WriteLine(element.Longitude);

                    double lat1 = Math.Floor(element.Latitude);
                    double lat2 = Math.Floor((element.Latitude - lat1) * 100);
                    double lat3 = Math.Floor(((element.Latitude - lat1) * 100 - lat2) * 100);
                    double lat4 = Math.Floor((((element.Latitude - lat1) * 100 - lat2) * 100 - lat3) * 100);
                    double lat5 = Math.Floor(((((element.Latitude - lat1) * 100 - lat2) * 100 - lat3) * 100 - lat4) * 100);

                    double lng1 = Math.Floor(element.Longitude);
                    double lng2 = Math.Floor((element.Longitude - lng1) * 100);
                    double lng3 = Math.Floor(((element.Longitude - lng1) * 100 - lng2) * 100);
                    double lng4 = Math.Floor((((element.Longitude - lng1) * 100 - lng2) * 100 - lng3) * 100);
                    double lng5 = Math.Floor(((((element.Longitude - lng1) * 100 - lng2) * 100 - lng3) * 100 - lng4) * 100);

                    double alt1 = Math.Floor(element.Altitude / 100);
                    double alt2 = Math.Floor((element.Altitude / 100 - alt1) * 100);
                    double alt3 = Math.Floor(((element.Altitude /100 - alt1) * 100 - alt2) * 100);

                    byte lat1_byte = (byte)lat1;
                    byte lat2_byte = (byte)lat2;
                    byte lat3_byte = (byte)lat3;
                    byte lat4_byte = (byte)lat4;
                    byte lat5_byte = (byte)lat5;

                    byte lng1_byte = (byte)lng1;
                    byte lng2_byte = (byte)lng2;
                    byte lng3_byte = (byte)lng3;
                    byte lng4_byte = (byte)lng4;
                    byte lng5_byte = (byte)lng5;
                          
                    byte alt1_byte = (byte)alt1;
                    byte alt2_byte = (byte)alt2;
                    byte alt3_byte = (byte)alt3;

                    //byte cmd_byte  = element.Command;

                    //byte wrad_byte = (byte)element.WayRadius;

                    message.Add(lat1_byte);message.Add(lat2_byte);message.Add(lat3_byte);message.Add(lat4_byte);message.Add(lat5_byte);

                    message.Add(lng1_byte);message.Add(lng2_byte);message.Add(lng3_byte);message.Add(lng4_byte);message.Add(lng5_byte);

                    message.Add(alt1_byte);message.Add(alt2_byte);message.Add(alt3_byte);

                    //message.Add(cmd_byte); message.Add(wrad_byte);

                }
                //Get points
                return message.ToArray<byte>();
            }
            catch (Exception) { Console.WriteLine("Exception is thrown."); return null; }
        }

        /// <summary>
        /// Sends the generated message over serial communication.
        /// </summary>
        public void sendNavigationPoints()
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += SendPoints_DoWork;
                if (worker.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    worker.RunWorkerAsync();
                }
            }
            catch (Exception){ }
        }

        private void SendPoints_DoWork(object sender, DoWorkEventArgs e)
        {

            //generate message
            byte[] message = generateMessage();
            //Call serial port object from teleoperator class
            if (true)
            {
                foreach (byte msg in message) { Console.Write(msg); }
                Teleoperation.port.Write(message, 0, message.Length);
                Teleoperation.port.Write("\r\n");
                
                
                Console.WriteLine("Mission points are succesfully loaded to the vehicle.");
            }
            else
            {
                Console.WriteLine("Telemetry port is not opened");
            }
        }
    }

    class NavigationPoint
    {
        public NavigationPoint()
        {
            this.Latitude   = 0.0  ;
            this.Longitude  = 0.0  ;
            this.Altitude   = 0.0  ;
            this.Command    = 0    ;
        }
        public double Latitude  { get; internal set; }
        public double Longitude { get; internal set; }
        public double Altitude  { get; internal set; }
        public byte Command     { get; internal set; }
        public int WayRadius    { get; internal set; }
    }
}
