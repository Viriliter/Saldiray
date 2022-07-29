using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using FornoxGUI;
using System.Xml;
using FornoxGUI.Communication;

namespace FornoxGUI.Communication
{
    /// <summary>
    /// Class contains typical fields that a message has.
    /// </summary>
    class Message
    {
        /// <summary>
        /// Constructor method of the class
        /// </summary>
        /// <param name="MessageID">Indicates the message uique identifier</param>
        public Message(byte messageID)
        {
            /*
             * Creates custom communication message package. 
            */
            this.MessageID = messageID;
        }
        public byte MessageID { get; internal set; }
        public byte[] Payload { get; internal set; }
    }

    /*
class XMLTable
{
    public XMLTable(byte StartField, byte PacketSequence, byte PayloadLength, byte SystemID, byte ComponentID, byte MessageID, byte[] Payload, byte CRC)
    {

        //
Creates custom communication message package. 

        this.StartField = StartField;
        this.PacketSequence = PacketSequence;
        this.PayloadLength = PayloadLength;
        this.SystemID = SystemID;
        this.ComponentID = ComponentID;
        this.MessageID = MessageID;
        this.Payload = Payload;
        this.CRC = CRC;
    }

    public byte StartField { get; private set; }
    public byte PacketSequence { get; private set; }
    public byte PayloadLength { get; private set; }
    public byte SystemID { get; private set; }
    public byte ComponentID { get; private set; }
    public byte MessageID { get; private set; }
    public byte[] Payload { get; private set; }
    public byte CRC { get; private set; }
}
}
*/

        /// <summary>
        /// Class contains encoder and decoder methods for messages and also includes app updater and decoder. 
        /// </summary>
    class MessageHandler
    {
        private static byte startField = 0xFE;

        //old package sequence number that is for detection of data loss during trasmission.
        byte oldPacketNumber = 0x00;

        /// <summary>
        /// Catagorizes the incomming byte array according to field names:
        /// StartField-->PacketSequence-->MessageID-->Payload-->CRC
        /// </summary>
        /// <param name="byteArray"> Byte array comming from serial buffer</param>
        /// <returns></returns>
        public Message Decoder(byte[] byteArray)
        {
            //Define payload size ,n bytes
            int n = 0;
            //Gets start field of the message
            byte StartField = byteArray[0];
            //Gets packet sequence of transmitted message
            //byte PacketSequence = byteArray[1];
            //Gets messageID of the package
            byte MessageID = byteArray[1];
            //Reedefine payload sizes for each messageID
            switch (MessageID)
            {
                case (0x00): n = 1;  break;
                case (0x01): n = 17; break;
                case (0x02): n = 2; break;
                case (0x03): n = 21; break;
            }
            //Create empty payload array.
            byte[] Payload = new byte[n];
            //Copy payload data into the payload byte array.
            Payload.CopyTo(byteArray, 2);

            //Array.Copy(byteArray, 2, Payload, 0, n);
            //byte CRC = byteArray[n + 2];

            Message incommingMessage = new Message( MessageID);
            incommingMessage.Payload = Payload;
            return incommingMessage;
        }

        /// <summary>
        ///  Converts outcomming message to byte array format before sending it over serial.
        /// </summary>
        /// <param name="outcommingMessage">Message that is intended to transmit</param>
        /// <returns></returns>
        public byte[] Encoder(Message outcommingMessage)
        {
            //Get field names from outcomming message.
            
            byte messageID = outcommingMessage.MessageID;
            byte[] payload = outcommingMessage.Payload;
            

            //Write all values into the byte array.
            byte[] outcommingData = new byte[payload.Length + 1];
            outcommingData[0] = messageID;
            payload.CopyTo(outcommingData,1);

            return outcommingData;
        }

        /// <summary>
        /// Updates with new payload in the destination resource of app.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newPayload">New Message Payload</param>
        /// <param name="dstResource">Destination Resource</param>
        public void AppUpdate<T>(T newPayload, string dstResource)
        {
            App.Current.Resources[dstResource] = newPayload;

        }

        /// <summary>
        /// Pools values in resource for transmitting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dstResource"></param>
        public void AppPool<T>(string dstResource)
        {
            //GamepadControls
            var JoyL_Y = App.Current.Resources["JoyL_Y"];//SteeringLeft
            var JoyR_Y = App.Current.Resources["JoyR_Y"];//SteeringRight
            var JoyL_X = App.Current.Resources["JoyL_X"];
            var JoyR_X = App.Current.Resources["JoyR_X"];

            var Key_X  = App.Current.Resources["Key_X"];//Mode
            var Key_Y  = App.Current.Resources["Key_Y"];//Heat Plug
            var Key_A  = App.Current.Resources["Key_A"];//Start
            var Key_B  = App.Current.Resources["Key_B"];//Stop
            var Key_LB = App.Current.Resources["Key_LB"];//ArmWeapon
            var Key_RB = App.Current.Resources["Key_RB"];//FireWeapon
            var Key_LT = App.Current.Resources["Key_LT"];//Throttle-
            var Key_RT = App.Current.Resources["Key_RT"];//Throttle+
        }
    }
}

