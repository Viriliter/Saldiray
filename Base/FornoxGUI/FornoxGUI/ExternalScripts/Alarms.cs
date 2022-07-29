using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using FornoxGUI;

namespace FornoxGUI.ExternalScripts
{
    class Alarms
    {
        public static void CallPullUpAlarm() {
        SoundPlayer Audio = new SoundPlayer(Properties.Resources.Pull_Up_Alarm);
        Audio.Play(); }

        public static void CallOverSpeedAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Over_Speed);
            Audio.Play();
        }
        public static void CallAdjustVerticalSpeedAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Adjust_Vertical_Speed);
            Audio.Play();
        }
        public static void CallAutopilotAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Autopilot);
            Audio.Play();
        }
        public static void CallClimbAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Climb);
            Audio.Play();
        }
        public static void CallDecendAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Decend);
            Audio.Play();

        }
        public static void CallThreeHunderedAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Three_Hundered);
            Audio.Play();
        }
        public static void CallTwoHunderedAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.Two_Hundered);
            Audio.Play();
        }
        public static void CallOneHunderedAlarm()
        {
            SoundPlayer Audio = new SoundPlayer(Properties.Resources.One_Hundered);
            Audio.Play();
        }
    }
}
