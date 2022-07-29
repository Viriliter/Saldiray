using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
namespace FornoxGUI.ExternalScripts
{
    class UpdateUGV
    {

        public void updateLocation()
        {
            string locString = (string)App.Current.Resources["UGVLocation"];
            if (locString != ";;")
            {
                string[] locStringArray = locString.Split(';');
                App.Current.Resources["UGVPinLocation"] = new Location(Convert.ToDouble(locStringArray[0]),
                                                                        Convert.ToDouble(locStringArray[1]),
                                                                        Convert.ToDouble(locStringArray[2]));
            }
            else
            {

            }
            Console.WriteLine("Location is updated");
        }

        
    }
}
