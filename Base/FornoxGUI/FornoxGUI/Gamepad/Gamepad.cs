using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SlimDX.Design;
using SlimDX.DirectInput;

namespace FornoxGUI
{
    class Gamepad
    {

    }

    public class GamepadThread
    {
        public static Thread gamepadThread { get; private set; }

        public static int threadSleep { get; private set; }

        private static GamepadDevice gamepadDevice = new GamepadDevice();

        private static List<GamepadDevice> devices = new List<GamepadDevice>();

        public static Joystick pad;

        public static bool isInitialized { get; private set; }

        private static GamepadHandler handler = new GamepadHandler();

        ///<summary>
        ///Finds and acquires connected gamepad device and starts pooling.
        ///</summary>
        public static void Initialize()
        {
            try
            {
                ThreadStart childref = new ThreadStart(Pool);
                //Set sleep time of the thread, default is 100ms
                threadSleep = 10;
                isInitialized = Acquire(Available());
                if (isInitialized)
                {
                    gamepadThread = new Thread(childref);
                    //Start thread
                    gamepadThread.Start();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Thread exception is raised.");
            }
        }

        ///<summary>
        ///Gets the list of connected gamepad devices
        ///</summary>
        public static Guid Available()
        {

            DirectInput dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                GamepadDevice gd = new GamepadDevice();
                gd.DeviceGuid = di.InstanceGuid;
                gd.DeviceName = di.InstanceName;
                devices.Add(gd);
                Console.WriteLine(di.InstanceGuid);
                Console.WriteLine(gd.DeviceGuid.ToString());
                return di.InstanceGuid;
            }
            return Guid.Empty;

        }

        ///<summary>
        ///Configures the input of gamepad device and returns true if gamepad is configured.
        ///</summary>
        /// <param name="DeviceGuid">Unique device id</param>
        public static bool Acquire(Guid DeviceGuid)
        {
            //Return false if unable to find device
            if (DeviceGuid == Guid.Empty) { return false; }

            DirectInput dinput = new DirectInput();

            pad = new Joystick(dinput, DeviceGuid);

            //Set resolution of each axis
            foreach (DeviceObjectInstance doi in pad.GetObjects(ObjectDeviceType.Axis))
            {
                //Set resolution of axes                
                pad.GetObjectPropertiesById((int)doi.ObjectType).SetRange(50, 205);
            }

            pad.Properties.AxisMode = DeviceAxisMode.Absolute;
            //pad.SetCooperativeLevel(parent, (CooperativeLevel.Nonexclusive | CooperativeLevel.Background));
            pad.Acquire();
            return true;
        }

        ///<summary>
        ///Pools button states from gamepad device in predefined intervals.
        ///</summary>
        public static void Pool()
        {
            try
            {
                while (true)
                {
                    JoystickState state = new JoystickState();

                    if (pad.Poll().IsFailure)
                    {
                        Console.WriteLine("Disconnected");
                    }

                    if (pad.GetCurrentState(ref state).IsFailure)
                    {
                        Console.WriteLine("Disconnected");
                    }

                    //Change Button states from resources
                    handler.ButtonStateChanged(state);

                    /*

                    bool[] buttons = state.GetButtons();
                    Console.WriteLine("Analog1X\t\tAnalog1Y\t\tAnalog2X\t\tAnalog2Y\t\tX\t\tA\t\tB\t\tY\t\tLB\t\tLT\t\tRB\t\tRT\t\tBack\t\tStart");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(state.X.ToString() + "\t\t" + state.Y.ToString() + "\t\t" + "\t\t" + state.Z + "\t\t" + state.RotationZ + buttons[0] + "\t\t" + buttons[1] + "\t\t" + buttons[2] + "\t\t" + buttons[3] + "\t\t" + buttons[4] + "\t\t" + buttons[5] + "\t\t" + buttons[6] + "\t\t" + buttons[7]);
                    */
                    //Sleep the thread in specified time
                    Thread.Sleep(threadSleep);
                }
            }
            catch (Exception)
            {

            }
        }

        /*
        public static void updateResources(JoystickState state)
        {
            bool[] buttons = state.GetButtons();

            //Change gamepad states in the resource

            App.Current.Resources["Key_X"] = buttons[0];           //Mode

            App.Current.Resources["Key_Y"] = buttons[3];           //Heat Plug

            if ((bool)App.Current.Resources["Key_B"] == false)
            {
                App.Current.Resources["Key_A"] = buttons[1];       //Start
                App.Current.Resources["Key_B"] = true;
            }

            if ((bool)App.Current.Resources["Key_A"] == true)
            {
                App.Current.Resources["Key_B"] = buttons[2];       //Stop
                App.Current.Resources["Key_A"] = false;
            }

            App.Current.Resources["Key_LB"] = buttons[4];          //ArmWeapon 
            App.Current.Resources["Key_LT"] = buttons[6];          //Throttle-
            App.Current.Resources["Key_RB"] = buttons[5];          //FireWeapon  
            App.Current.Resources["Key_RT"] = buttons[7];          //Throttle+ 

            //App.Current.Resources["Key_LeftRight"] = state.X;
            //App.Current.Resources["Key_UpDown"] = state.Y;

            byte byteX = (byte)state.X;
            App.Current.Resources["JoyL_X"] = byteX;

            byte byteY = (byte)state.Y;
            App.Current.Resources["JoyL_Y"] = byteY;

            byte byteZ = (byte)state.Z;
            App.Current.Resources["JoyR_X"] = byteZ;

            byte byteRZ = (byte)state.RotationZ;
            App.Current.Resources["JoyR_Y"] = byteRZ;

        }
        */

    }

    /// <summary>
    /// Gamepad handler class that controls the state of the buttons 
    /// </summary>
    class GamepadHandler
    {
        private bool[] oldState_Button;

        /// <summary>
        /// Updates the state property of the button if it detects the change in the button state 
        /// </summary>
        /// <param name="state"></param>
        public void ButtonStateChanged(JoystickState state)
        {
            bool[] buttons = state.GetButtons();

            bool[] currentState_Button = buttons;

            //for(int i = 0 ; i<currentState_Button.Length ; i++)
            for (int i = 0; i < 4; i++)
            {
                //Detect button is pressed down
                if(currentState_Button[i] == true && oldState_Button[i] == false)
                {
                    //Change property of i-th button from the resource
                    ChangeButtonProperty(i);
                }
                
            }
            //Update old states
            oldState_Button = currentState_Button;

            //Update weapon buttons
            //CHANGE THIS
            App.Current.Resources["Key_LB"] = currentState_Button[4];
            App.Current.Resources["Key_RB"] = currentState_Button[5];

            //Update throttle buttons
            App.Current.Resources["Key_LT"] = currentState_Button[6];
            App.Current.Resources["Key_RT"] = currentState_Button[7];

            //Change joysticks' state from the resource

            byte byteX = (byte)state.X;
            App.Current.Resources["JoyL_X"] = byteX;

            byte byteY = (byte)state.Y;
            App.Current.Resources["JoyL_Y"] = byteY;

            byte byteZ = (byte)state.Z;
            App.Current.Resources["JoyR_X"] = byteZ;

            byte byteRZ = (byte)state.RotationZ;
            App.Current.Resources["JoyR_Y"] = byteRZ;
        }

        /// <summary>
        /// Inverses the value of specified button state
        /// </summary>
        /// <param name="buttonIndex">Button index that is targeted to changed from the resource</param>
        private void ChangeButtonProperty(int buttonIndex)
        {
            switch (buttonIndex)
            {
                case (0): { bool oldProperty = (bool)App.Current.Resources["Key_X"] ; App.Current.Resources["Key_X"]   = !oldProperty ; break; } //Mode
                case (1): { bool oldProperty = (bool)App.Current.Resources["Key_A"] ; App.Current.Resources["Key_A" ]  = !oldProperty ; break; } //Start
                case (2): { bool oldProperty = (bool)App.Current.Resources["Key_B"] ; App.Current.Resources["Key_B" ]  = !oldProperty ; break; } //Stop
                case (3): { bool oldProperty = (bool)App.Current.Resources["Key_Y"] ; App.Current.Resources["Key_Y" ]  = !oldProperty ; break; } //Heat Plug
                //case (4): { bool oldProperty = (bool)App.Current.Resources["Key_LB"]; App.Current.Resources["Key_LB"]  = !oldProperty ; break; } //ArmWeapon 
                //case (5): { bool oldProperty = (bool)App.Current.Resources["Key_RB"]; App.Current.Resources["Key_RB"]  = !oldProperty ; break; } //FireWeapon
                //case (6): { bool oldProperty = (bool)App.Current.Resources["Key_LT"]; App.Current.Resources["Key_LT"]  = !oldProperty ; break; } //Throttle-
                //case (7): { bool oldProperty = (bool)App.Current.Resources["Key_RT"]; App.Current.Resources["Key_RT"]  = !oldProperty ; break; } //Throttle+ 
            }
        }
    }
}
