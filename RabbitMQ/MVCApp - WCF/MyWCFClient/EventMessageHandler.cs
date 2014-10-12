using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Messages;
using MyMessages;
using MyMessages.IMessages;
using MyMessages.MessageParts;
using NServiceBus;
using NServiceBus.Logging;

namespace MyWCFClient
{
    /****
      * The message handler
      * Matches a XML message GUID from a file and the command sent 
      * to it from MVC
      * If found, sends it to the WCf Server and responds
      * with the state of what happened.
      * The WCF Service must be running to complete.
      * 
      * ****/
    public class EventMessageHandler : IHandleMessages<SendCommand>
    {

        public IBus Bus { get; set; }

        public void Handle(SendCommand message)
        {

            ServiceReference1.WcfServiceOf_PayMessage_ErrorCodesClient client1 =
                  new ServiceReference1.WcfServiceOf_PayMessage_ErrorCodesClient();

            // Create the response message
            ResponseCommand command = new ResponseCommand();
            command.RequestId = message.RequestId;
            /****
             * Get the XML messages from the temp direcotry.
             * Find a match from the GUID
             * ****/
            List<PayMessage> list = EventMessageHandler.GetMessages();
            PayMessage payMessage = null;
            foreach (var temp_message in list)
            {
                if (message.RequestId == temp_message.EventId)
                {
                    payMessage = temp_message;
                }
            }
            // if no XML, just fail
            if (payMessage == null)
            {
                command.state = StateCodes.MyWCFClientFailXML;
                Bus.Reply(command);
                Console.WriteLine("No XML Found");
            }
            else
            {

                ErrorCodes returnCode = client1.Process(payMessage);

                if (returnCode == ErrorCodes.None)
                {
                    command.state = StateCodes.CompleteMyWCFClient;
                }
                else
                {
                    command.state = StateCodes.MyWCFClientFail;
                }

                Bus.Reply(command);
                Console.WriteLine("Success");
            }

            Console.WriteLine("==========================================================================");
        }


        /***
          * 
          * Get PayMessages from XML files
          * 
          * ***/

        public static List<PayMessage> GetMessages()
        {

            List<PayMessage> list = new List<PayMessage>();

            string[] dirs = Directory.GetFiles(@"C:\temp\", "temp?.xml");

            foreach (string filename in dirs)
            {
                /***
                 * De-serialize the XML into the objects
                 * *****/
                PayMessage message = DeserializeEventMessage(filename);


                /***
                 *  Add to my list for later use
                 * ****/
                list.Add(message);

            }
            return list;
        }

        /***
         * 
         * De-serialize a XML Payment file
         * 
         * ***/
        static public PayMessage DeserializeEventMessage(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PayMessage));
            using (TextReader reader = new StreamReader(filename))
            {
                PayMessage eventMsg = (PayMessage)serializer.Deserialize(reader);
                return eventMsg;
            }
        }
    }
}
