using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FornoxGUI.ExternalScripts
{
    class GroundTelemetryConnection
    {
        static string telemetryDeviceID = (string)("USB\\VID_10C4&PID_EA60\\0001");
        public static List<USBDeviceInfo> GetUSB()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
            int i= 0;

            foreach (ManagementObject device in searcher.Get())
            {
                if (device != null)
                {
                    if (device.GetPropertyValue("Name").ToString().Contains("(COM") && device.GetPropertyValue("DeviceID").ToString()== telemetryDeviceID)
                    {

                        devices.Add(new USBDeviceInfo(
                        (string)device.GetPropertyValue("Status"),
                        (string)device.GetPropertyValue("Name"),
                        (string)device.GetPropertyValue("DeviceID"),
                        (string)device.GetPropertyValue("Description"),
                        ((string)device.GetPropertyValue("Name")).Split('(')[1].Split(')')[0])
                        );
                        break;
                    }
                }
            }

            searcher.Dispose();
            return devices;
        }

      /*  public static void connectGround()
        {
            SerialPortInterface.OpenPort();
        }
        */
    }

    class USBDeviceInfo
    {
        public USBDeviceInfo(string status, string name, string deviceID, string description, string portName)
        {
            this.Status = status;
            this.DeviceID = deviceID;
            this.PortName = portName;
            this.Description = description;
            this.Name = name;
        }
        public string Status { get; private set; }
        public string Name { get; private set; }
        public string DeviceID { get; private set; }
        public string PortName { get; private set; }
        public string Description { get; private set; }
    }

    class SerialPortInterface
    {
        private static SerialPort _serialPort = new SerialPort();
        private static int _baudRate = (int)App.Current.Resources["GroundDeviceID_BaudRate"];
        private static int _dataBits = 8;
        private static Handshake _handshake = Handshake.None;
        private static Parity _parity = Parity.None;
        private static string _portName = (string)App.Current.Resources["GroundDeviceID"];
        private static StopBits _stopBits = StopBits.One;

        /// <summary> 
        /// Holds data received until we get a terminator. 
        /// </summary> 
        private static string tString = string.Empty;
        /// <summary> 
        /// End of transmition byte in this case EOT (ASCII 4). 
        /// </summary> 
        private static byte _terminator = 0x4;

        public static int BaudRate { get { return _baudRate; } set { _baudRate = value; } }
        public static int DataBits { get { return _dataBits; } set { _dataBits = value; } }
        public static Handshake Handshake { get { return _handshake; } set { _handshake = value; } }
        public static Parity Parity { get { return _parity; } set { _parity = value; } }
        public static string PortName { get { return _portName; } set { _portName = value; } }

        public static bool OpenPort()
        {
            try
            {
                _serialPort.BaudRate = _baudRate;
                _serialPort.DataBits = _dataBits;
                _serialPort.Handshake = _handshake;
                _serialPort.Parity = _parity;
                _serialPort.PortName = _portName;
                _serialPort.StopBits = _stopBits;
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            }
            catch { return false; }
            return true;
        }

        static void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Initialize a buffer to hold the received data 
            byte[] buffer = new byte[_serialPort.ReadBufferSize];

            //There is no accurate method for checking how many bytes are read 
            //unless you check the return from the Read method 
            int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

            //For the example assume the data we are received is ASCII data. 
            tString += Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //Check if string contains the terminator  
            if (tString.IndexOf((char)_terminator) > -1)
            {
                //If tString does contain terminator we cannot assume that it is the last character received 
                string workingString = tString.Substring(0, tString.IndexOf((char)_terminator));
                //Remove the data up to the terminator from tString 
                tString = tString.Substring(tString.IndexOf((char)_terminator));
                //Do something with workingString 
                Console.WriteLine(workingString);
            }
        }
    }
}


