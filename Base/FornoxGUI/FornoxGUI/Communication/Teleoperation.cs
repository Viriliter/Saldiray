using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FornoxGUI.ExternalScripts;
using Microsoft.Maps.MapControl;
using Microsoft.Maps.MapControl.WPF;

namespace FornoxGUI.Communication
{
    /// <summary>
    /// Initializes serial communication.
    /// Encodes messages to byte form that is ready to transmission.
    /// Decodes incomming byte arrays and call relevant function within the app.
    /// </summary>
    class Teleoperation
    {
        public Teleoperation()
        {
            //InitializeThread();
        }
        //Message Field IDs
        public static byte START_FIELD = 0xFE;
        public static byte _VEHICLE_COMMAND = 0x00;
        public static byte _WEAPON_COMMAND = 0x01;
        public static byte _NAVIGATION = 0x02;

        //Messages in byte
        public static byte vehicleCommand { get; private set; }
        public static byte weaponCommand { get; private set; }
        public static byte navigation { get; private set; }

        public static Message VehicleMessage = new Message(_VEHICLE_COMMAND);
        public static Message WeaponMessage = new Message(_WEAPON_COMMAND);
        public static Message Navigation = new Message(_NAVIGATION);

        public static MessageHandler messageHandler = new MessageHandler();

        //Thread setter and getter
        public static Thread TeleopThread { get; private set; }
        public static int poolThreadSleep { get; private set; }

        //Initialize timer for telemetry device
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        //Serial Port
        public static SerialPort port = new SerialPort();
        //public static SerialPort getport { get =>port; private set; }
        public static int baudrate { get; private set; }
        public static string portName { get; private set; }
        public static bool isPortOpened { get; private set; }
        public string status { get; private set; }

        public static bool IsFirstTimeDataReceieved = false;


        Location ugvLocation = new Location();


        /// <summary>
        /// Opens new thread that initializes serial communication.
        /// </summary>
        public void InitializeThread()
        {
            try
            {
                //Open Serial Port
                bool isInitialized = InitSerial();
                if (isInitialized)
                {
                    //Start AppPool thread
                    ThreadStart poolref = new ThreadStart(AppPool);
                    //Define thread's sleep time in ms
                    poolThreadSleep = 100;
                    TeleopThread = new Thread(poolref);
                    //Start thread
                    TeleopThread.Start();
                }


            }
            catch (Exception)
            {
                Console.WriteLine("Thread exception is raised.");
            }
        }

        /// <summary>
        /// Pools App resources
        /// </summary>
        public void AppPool()
        {
            while (true)
            {
                //****************************
                //For Vehicle Message Command
                //****************************
                //Pool vehicle state from the resource
                bool UGVIsStarted            = (bool)App.Current.Resources["UGVIsStarted"         ];
                bool UGVIsPlugHeated         = (bool)App.Current.Resources["UGVIsPlugHeated"      ];
                bool UGVIsEngineStarted      = (bool)App.Current.Resources["UGVIsEngineStarted"   ];
                bool UGVIsDeadManActivated   = (bool)App.Current.Resources["UGVIsDeadManActivated"];
                bool UGVThrottleInc          = (bool)App.Current.Resources["UGVThrottleInc"       ];
                bool UGVThrottleDec          = (bool)App.Current.Resources["UGVThrottleDec"       ];

                bool Key_X = (bool)App.Current.Resources["Key_X"];

                bool[] vehicleMessageBool = new bool[] {UGVIsStarted, UGVIsPlugHeated, UGVIsEngineStarted, false, UGVIsDeadManActivated, Key_X, UGVThrottleInc, UGVThrottleDec };
                BitArray vehicleMessageBits = new BitArray(vehicleMessageBool);
                //First Byte of the vehicle message payload
                Byte vehicleStateByte = ConvertToByte(vehicleMessageBits);

                //Pool gamepad state from the resource
                byte JoyL_X = (byte)App.Current.Resources["JoyL_X"];
                byte JoyL_Y = (byte)App.Current.Resources["JoyL_Y"];
                byte JoyR_X = (byte)App.Current.Resources["JoyR_X"];
                byte JoyR_Y = (byte)App.Current.Resources["JoyR_Y"];

                //bool Key_X  = (bool)App.Current.Resources["Key_X" ]; //Mode
                bool Key_Y  = (bool)App.Current.Resources["Key_Y" ];   //Heat Plug
                bool Key_A  = (bool)App.Current.Resources["Key_A" ];   //Start
                bool Key_B  = (bool)App.Current.Resources["Key_B" ];   //Stop
                bool Key_LB = (bool)App.Current.Resources["Key_LB"];   //ArmWeapon
                bool Key_RB = (bool)App.Current.Resources["Key_RB"];   //FireWeapon
                bool Key_LT = (bool)App.Current.Resources["Key_LT"];   //Throttle-
                bool Key_RT = (bool)App.Current.Resources["Key_RT"];   //Throttle+

                bool Cam_Switch = (bool)App.Current.Resources["CamSwitch"];   //CameraSwitch

                //Update VehicleMessage
                bool[] buttonStateBool = new bool[] { false, Key_Y, Key_A, Key_B, Key_LB, Key_RB, Key_LT, Key_RT };
                BitArray buttonStateBits = new BitArray(buttonStateBool);
                Byte buttonSMessageByte = ConvertToByte(buttonStateBits);
                VehicleMessage.Payload = new Byte[] { vehicleStateByte, JoyL_X, JoyL_Y, JoyR_X, JoyR_Y, buttonSMessageByte };

                //****************************
                //For Weapon Message Command
                //****************************
                bool[] weaponMessageBool = new bool[] { (bool)Key_LB, (bool)Cam_Switch, false, false, false, false, false, false };
                BitArray weaponMessageBits = new BitArray(weaponMessageBool);
                Byte weaponMessageByte = ConvertToByte(weaponMessageBits);
                //Update WeaponMessage
                WeaponMessage.Payload = new Byte[] { weaponMessageByte, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                //****************************
                //For Navigation Command
                //****************************
                //Update Navigation
                Navigation.Payload      = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                byte[] encoded1 = messageHandler.Encoder(VehicleMessage);//VehicleMessage.Payload; 
                byte[] encoded2 = messageHandler.Encoder(WeaponMessage );//WeaponMessage.Payload ; 
                byte[] encoded3 = messageHandler.Encoder(Navigation    );//Navigation.Payload    ; 

                //Create message byte array
                byte[] message = new byte[encoded1.Length + encoded2.Length + encoded3.Length + 1];
                byte[] message1 = new byte[encoded1.Length + 1];
                byte[] message2 = new byte[encoded2.Length + 1];
                byte[] message3 = new byte[encoded3.Length + 1];

                //Concat all messages
                /*
                message[0] = START_FIELD;//Add start field of the message
                encoded1.CopyTo(message, 1);
                encoded2.CopyTo(message, encoded1.Length + 1);
                encoded3.CopyTo(message, encoded1.Length + encoded2.Length + 1);
                PushMessage(message);
                */
                byte[] startfieldArray = new[] { START_FIELD };//Add start field of the message
                startfieldArray.CopyTo(message1, 0);
                encoded1.CopyTo(message1, 1);

                startfieldArray.CopyTo(message2, 0);
                encoded2.CopyTo(message2, 1);

                startfieldArray.CopyTo(message3, 0);
                encoded3.CopyTo(message3, 1);

                PushMessage(message1);
                PushMessage(message2);
                PushMessage(message3);
                Thread.Sleep(poolThreadSleep);
            }
        }

        public void AppPush(byte[] incommingByteArray)
        {
            //Check start field
            if (incommingByteArray[0] == 0x3f)
            {
                //Check message ID
                switch (incommingByteArray[1])
                {
                    case (0x00)://vehicle status
                        var statusBits = new BitArray(incommingByteArray[1]);
                        App.Current.Resources["InStat_IsStarted"] = statusBits[4];
                        App.Current.Resources["InStat_IsSparkReady"] = statusBits[5];
                        App.Current.Resources["InStat_IsRun"] = statusBits[6];
                        App.Current.Resources["InStat_IsOilAlarmed"] = statusBits[7];
                        App.Current.Resources["InStat_UGVMode"] = statusBits[8];
                        break;
                    case (0x01)://weapon status
                        decimal w_yaw1   = incommingByteArray[2];
                        decimal w_yaw2   = incommingByteArray[3];
                        decimal w_pitch1 = incommingByteArray[4];
                        decimal w_pitch2 = incommingByteArray[5];

                        double w_combinedyaw =(double) (w_yaw1 + w_yaw2/100);
                        double w_combinedpitch =(double) (w_pitch1 + w_pitch2/100);
                        //Update App

                        break;
                    case (0x02)://attitude status
                        decimal yaw1   = incommingByteArray[2];
                        decimal yaw2   = incommingByteArray[3];
                        decimal yaw3   = incommingByteArray[4];
                        decimal pitch1 = incommingByteArray[5];
                        decimal pitch2 = incommingByteArray[6];
                        decimal pitch3 = incommingByteArray[7];
                        decimal roll1  = incommingByteArray[8];
                        decimal roll2  = incommingByteArray[9];
                        decimal roll3  = incommingByteArray[10];
                        decimal ax1    = incommingByteArray[11];
                        decimal ax2    = incommingByteArray[12];
                        decimal ax3    = incommingByteArray[13];
                        decimal ay1    = incommingByteArray[14];
                        decimal ay2    = incommingByteArray[15];
                        decimal ay3    = incommingByteArray[16];
                        decimal az1    = incommingByteArray[17];
                        decimal az2    = incommingByteArray[18];
                        decimal az3    = incommingByteArray[19];
                        decimal gx1    = incommingByteArray[20];
                        decimal gx2    = incommingByteArray[21];
                        decimal gx3    = incommingByteArray[22];
                        decimal gy1    = incommingByteArray[23];
                        decimal gy2    = incommingByteArray[24];
                        decimal gy3    = incommingByteArray[25];
                        decimal gz1    = incommingByteArray[26];
                        decimal gz2    = incommingByteArray[27];
                        decimal gz3    = incommingByteArray[28];
                        double combinedYaw   = (double)(yaw1*10 + yaw2 / 10 + yaw3 / 1000) - 360;
                        double combinedPitch = (double)(pitch1*10 + pitch2 / 10 + pitch3 / 1000) - 360;
                        double combinedRoll  = (double)(roll1*10 + roll2 / 10 + roll3 / 1000) - 360;
                        double combinedAx    = (double)(ax1*10 + ax2 / 10 + ax3 / 1000) - 360;
                        double combinedAy    = (double)(ay1*10 + ay2 / 10 + ay3 / 1000) - 360;
                        double combinedAz    = (double)(az1*10 + az2 / 10 + az3 / 1000) - 360;
                        double combinedGx    = (double)(gx1*10 + gx2 / 10 + gx3 / 1000) - 360;
                        double combinedGy    = (double)(gy1*10 + gy2 / 10 + gy3 / 1000) - 360;
                        double combinedGz    = (double)(gz1*10 + gz2 / 10 + gz3 / 1000) - 360;
                        Console.WriteLine(Math.Round(combinedAx, 3));
                        Console.WriteLine(Math.Round(combinedGz, 3));
                        App.Current.Resources["UGVyaw"]   = Math.Round(combinedYaw,3).ToString();
                        App.Current.Resources["UGVpitch"] = Math.Round(combinedPitch,3).ToString();
                        App.Current.Resources["UGVroll"]  = Math.Round(combinedRoll,3).ToString();
                        App.Current.Resources["UGVax"]    = Math.Round(combinedAx,3).ToString();
                        App.Current.Resources["UGVay"]    = Math.Round(combinedAy,3).ToString();
                        App.Current.Resources["UGVaz"]    = Math.Round(combinedAz,3).ToString();
                        App.Current.Resources["UGVmx"]    = Math.Round(combinedGx,3).ToString();
                        App.Current.Resources["UGVmy"]    = Math.Round(combinedGy,3).ToString();
                        App.Current.Resources["UGVmz"]    = Math.Round(combinedGz,3).ToString();
                        break;

                    case (0x03)://navigation status
                        decimal lat1 = incommingByteArray[2];
                        decimal lat2 = incommingByteArray[3];
                        decimal lat3 = incommingByteArray[4];
                        decimal lat4 = incommingByteArray[5];
                        decimal lat5 = incommingByteArray[6];
                        double combinedLat = (double) (lat1 + lat2 / 100 + lat3 / 10000 + lat4 / 1000000 + lat5 / 100000000);
                        Console.WriteLine(combinedLat);
                        decimal lng1 = incommingByteArray[7];
                        decimal lng2 = incommingByteArray[8];
                        decimal lng3 = incommingByteArray[9];
                        decimal lng4 = incommingByteArray[10];
                        decimal lng5 = incommingByteArray[11];
                        double combinedLng = (double) (lng1*10 + lng2 / 10 + lng3 / 1000 + lng4 / 100000 + lng5 / 10000000);
                        Console.WriteLine(combinedLng);

                        decimal alt1 = incommingByteArray[12];
                        decimal alt2 = incommingByteArray[13];
                        decimal alt3 = incommingByteArray[14];
                        double combinedAlt = (double) (alt1*100 + alt2 + alt3 / 100);
                        Console.WriteLine(combinedAlt);

                        decimal speed1 = incommingByteArray[15];
                        decimal speed2 = incommingByteArray[16];
                        decimal speed3 = incommingByteArray[17];
                        decimal speed4 = incommingByteArray[18];
                        double combinedSpeed = (double) (speed1 + speed2 / 100 + speed3 / 10000 + speed4 / 1000000);
                        Console.WriteLine(combinedSpeed);


                        double numSat = (double) incommingByteArray[19];
                        Console.WriteLine(numSat);

                        //Update App
                        ugvLocation = new Location(combinedLat, combinedLng, combinedAlt);
                        App.Current.Resources["UGVPinLocation"] = ugvLocation;
                        App.Current.Resources["UGVSpeed_Double"] = combinedSpeed;
                        App.Current.Resources["AirSpeedRotation"] = combinedSpeed*80/360;

                        App.Current.Resources["SatalliteCount"] = numSat;
                        break;
                }
            }
        }
        /// <summary>
        /// Initilizes serial port
        /// </summary>
        public bool InitSerial()
        {
            try
            { 
                //Get usb port name within usb devices
                status = string.Empty;
                //Enable automatic search of telemetry device if "AUTO" is selected
                if ((string)App.Current.Resources["GroundTelemetry_PortName"] == "AUTO") {

                    List<USBDeviceInfo> devices = GroundTelemetryConnection.GetUSB();

                    if (devices==null)
                    {

                        return false;
                    }
                    foreach (var device in devices)
                    {
                        //Update port name of ground telemetry to app resource
                        App.Current.Resources["GroundTelemetry_PortName"] = device.PortName;
                        portName = device.PortName;
                        status = device.Status;
                        break;
                    }
                }
                Console.Write("Telemetry status:");
                Console.WriteLine(status);

                if (status==null)
                {
                    ErrorHandling("Telemetry device is not found!");
                    return false;
                }



                //Initialize serial port
                baudrate = (int)App.Current.Resources["UGVTelemetry_BaudRate"];
                portName = (string)App.Current.Resources["GroundTelemetry_PortName"];
                port.BaudRate = baudrate;
                port.PortName = portName;
                port.DataReceived += new SerialDataReceivedEventHandler(MessageReceivedHandler);
                port.Open();
                port.PinChanged += new SerialPinChangedEventHandler(PinChangedHandler);
                //Fire event trigger in order to notify telemetry is connected
                TelemetryConnected(this, null);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("An exception is occured during the initialization of serial port.");
                DisposeSerial();
                return false;
            }
        }

        /// <summary>
        /// Disposes serial port
        /// </summary>
        public void DisposeSerial()
        {
            port.Dispose();
        }

        public void PushMessage(byte[] message)
        {
            /*
            BitArray bits = new BitArray(message);
            //Writes message to serial
            
            foreach (bool item in bits)
            {
                Console.Write(item ? 1 : 0);
            }
            Console.WriteLine("");
            */

            if (port.IsOpen)
            {
                port.Write(message,0,message.Length);
                byte[] messageEnd = {0x6F,0x6F};
                port.Write(messageEnd,0,messageEnd.Length);
                //port.Write("\r\n");
                //string messageStr = BitConverter.ToString(message);
                //string messageStr2 = System.Text.Encoding.UTF8.GetString(message);
                
                //Console.WriteLine(messageStr);
                  
                //Console.WriteLine("");

                //port.WriteLine(messageStr);
            }
        }
        
        /// <summary>
        /// Reads message from serial when data is received to serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MessageReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (IsFirstTimeDataReceieved)
                {

                    //UGVConnected(this, null);
                    IsFirstTimeDataReceieved = false;
                }

                //------------------------------------------------------//
                string incommingMessage = port.ReadTo("oo");
                //string incommingMessage = port.ReadLine();
                //Console.WriteLine(incommingMessage);
                //byte[] ba = Encoding.Default.GetBytes(incommingMessage);
                //Console.WriteLine(BitConverter.ToString(ba));
                //------------------------------------------------------//
                PullMessage(incommingMessage);
                IsFirstTimeDataReceieved = true;
            }

            catch (Exception) { Console.WriteLine("Exception is occured during receiving message."); }

        }

        public void PinChangedHandler(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                port.Dispose();
                Console.WriteLine("disconnected");
                TelemetryDisconnected(this, null);
            }
            catch (Exception) { }
        }
        
        public void PullMessage(string incommingMessage)
        {
            //incommingMessage = "FE03200C22390003175B17320B001E000000";

            byte[] byteArray = Encoding.Default.GetBytes(incommingMessage);

            //Console.WriteLine(byteArray[0]);
            //var hexString = BitConverter.ToString(byteArray);
            //Console.WriteLine(hexString);
            AppPush(byteArray);
            /*
            var bytes = GetBytesFromByteString(incommingMessage).ToArray();
            foreach (byte element in bytes)
            {
                Console.WriteLine(element);
            }
            */

        }

        /// <summary>
        /// Converts 8-sized bit array to a byte
        /// </summary>
        /// <param name="bits">Bit array intended to convert</param>
        /// <returns></returns>
        private byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        public IEnumerable<byte> GetBytesFromByteString(string s)
        {
            for (int index = 0; index < s.Length; index += 2)
            {
                yield return Convert.ToByte(s.Substring(index, 2), 16);
            }
        }

        void ErrorHandling(string error)
        {
            MessageBox.Show(error, "Error");
            return;
        }

        public void dispatcherTimer_Telemetry(object sender, EventArgs e)
        {
            // code goes here
        }

        public event EventHandler TelemetryConnected;

        public event EventHandler TelemetryDisconnected;

        public event EventHandler UGVConnected;

        public event EventHandler UGVDisconnected;

        /// <summary>
        /// Calculates the checksum of the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private byte calcChecksum(byte[] message)
        {
            Byte chkSumByte = 0x00;
            for (int i = 0; i < message.Length; i++)
                chkSumByte ^= message[i];
            return chkSumByte;
        }

        public static byte ComputeAdditionChecksum(byte[] data)
        {
            byte sum = 0;
            unchecked // Let overflow occur without exceptions
            {
                foreach (byte b in data)
                {
                    sum += b;
                }
            }
            return sum;
        }



    }


}
