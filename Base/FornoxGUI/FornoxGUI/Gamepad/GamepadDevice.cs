using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FornoxGUI
{
    class GamepadDevice
    {
        ///<summary>
        ///Constructor of GamepadDevice. It stores DeviceName and DeviceGuid of the gamepad
        ///</summary>
        public GamepadDevice()
        {

        }

        public string DeviceName { get; set; }
        public Guid DeviceGuid { get; set; }
        //public string Result { get; private set; }

    }
}
