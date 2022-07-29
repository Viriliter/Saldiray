using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.IO.Ports;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;
using FornoxGUI.ExternalScripts;
using FornoxGUI.Communication;
using Microsoft.Maps.MapControl.WPF;

namespace FornoxGUI.ExternalScripts
{
    class TelemetryHandler
    {
        public static USBDeviceInfo telemetryInfo = null;
        public static SerialPort port = null;
        //byte[] readBuffer = new byte[1024];
        //MessageHandler messageHandler = new MessageHandler();

        public static bool Initialize()
        {
            /*
             * Open specific port for establishing serial communication between telemetry and serial bus. As connection established
             * returns true, vice versa.
             */
             //Get baudrate for serial communication for ground telemetry from app resource
            Int32 groundTelemetry_BaudRate = (Int32) App.Current.Resources["GroundTelemetry_BaudRate"];
            string portName = null;
            string status = null;
            
            //Get usb port name within usb devices
            List<USBDeviceInfo> devices = GroundTelemetryConnection.GetUSB();
            foreach (var device in devices)
            {
                //Update port name of ground telemetry to app resource
                App.Current.Resources["GroundTelemetry_PortName"] = device.PortName;
                portName = device.PortName;
                status = device.Status;
                break;
            }

            if (status =="OK" && portName != null && port==null)
            {
                Console.WriteLine("STATUS:OK");
                //Create a serial port
                port = new SerialPort(portName, groundTelemetry_BaudRate, Parity.None, 8, StopBits.One);
                //Open the serial port
                port.Open();
                //Set readtimeout for port to prevent waiting time to read from stream
                port.ReadTimeout = 2000;
                //Run asyncronous functions to write and read packages
                /*readTelemetry();await Task.Delay(100);
                await Task.Run(() => readTelemetry());
                //return true;
                */
                //readTelemetry2(port);
                return true;
            }
            else
            {
                return false;
            }
        }
        /*
                public static async Task ReceiveMessage()
                {

                }
        */

        /*
                public static bool waitUGVResponse()
                {
                    //
                    return true;
                }
        */

        /*
                private async Task TryToReadTelemetry()
                {
                    await Task.Run(() => readTelemetry());
                }
        */

        /*
                private async Task readTelemetry()
                {
                    if (port.IsOpen)
                    {
                        try
                        {
                            port.ReadTimeout = 3000;
                            int numBytes = port.Read(readBuffer, 0, 1024);
                            while (numBytes != -1)
                            {
                                Console.WriteLine("Byte Size:");
                                Console.Write(numBytes);
                                Message message = messageHandler.Reader(readBuffer);
                                Console.WriteLine(System.Text.Encoding.UTF8.GetString(message.Payload));
                                messageHandler.MessageInterpreter(message);
                                port.DiscardInBuffer();
                                //Do something with readBuffer[0] to readBuffer[numBytes]


                                //using (FileStream file = File.OpenRead("numbers.txt")) {
                                //file.Read(buffer, 0, 100);
                                 }

                            }
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); }
                    }
                }
        */

        /*
                private async Task writeTelemetry()
                {
                    if (port.IsOpen)
                    {

                    }
                }
        */
//*****************************************************************************************************//
        /*
         * BELOW HERE IS TEST SCRIPT
         */
//*****************************************************************************************************//
        public static void readTelemetry2()
        {
            /*
             * It is experimental method to test serial communication
             */
            try
            {
                while (port.IsOpen)
                {
                    Thread.Sleep(300);
                    //Console.WriteLine(port.ReadExisting());
                    //Console.WriteLine("****");
                    string mystring = port.ReadLine();
                    //Console.WriteLine(mystring);
                    //port.DiscardInBuffer();
                    bool isValid = checkValidty(mystring);
                    //Console.WriteLine(isValid);  
                }          
                Console.WriteLine("Port is closed");
            }
            catch (Exception)
            {
                //In case of exception close the port and restart it
                Console.WriteLine("Port is closing");
                //port.Close();
                //readTelemetry2();
            }

        }

        private static bool checkValidty(String mystring)
        {
            /*
             * It is experimental method to verify the message and update the app resource
             */
            if (mystring.Contains("_NAV_LOCATION"))
            {
                string myLocString = mystring.Split('/')[1];
                double lat = Convert.ToDouble(myLocString.Split(';')[0], System.Globalization.CultureInfo.InvariantCulture);
                double longa = Convert.ToDouble(myLocString.Split(';')[1], System.Globalization.CultureInfo.InvariantCulture);
                double alt = Convert.ToDouble(myLocString.Split(';')[2], System.Globalization.CultureInfo.InvariantCulture);

                Location loc = new Location(lat,longa,alt);
                App.Current.Resources["UGVPinLocation"] = loc;
                App.Current.Resources["UGVLat"] = lat.ToString();
                App.Current.Resources["UGVLong"] = longa.ToString();
                App.Current.Resources["UGVAlt"] = alt.ToString();

                Console.WriteLine(App.Current.Resources["UGVPinLocation"]);
                return true;
            }
            return false;
        }
        
    }
}
