using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMessages;
using MyMessages.IMessages;
using MyMessages.MessageParts;
using MyWCFClient;
using NServiceBus;
using NServiceBus.Testing;

namespace UnitTestHandlers
{
    [TestClass]
    public class UnitTestHandler2
    {
        /***
         * 
         * Test the message handler for MYWCFClient
         * This will call the WCF Service for a completion
         * 
         * ****/
        [TestMethod]
        public void Run()
        {
            Test.Initialize();
            /*****
             * 
             *  Create a Command message
             *  used to look up an XML Message file
             *  on Disk, send to WCF Server
             *  
             * ******/
            SendCommand command = new SendCommand();
            command.RequestId = new Guid("8b265223-dc9e-4789-a6df-69d19f644ad7");
            command.state = MyMessages.MessageParts.StateCodes.SentMyWCFClient;

            // The Test code
            Test.Handler<EventMessageHandler>()
                     .ExpectReply<ResponseCommand>(m => m.state == MyMessages.MessageParts.StateCodes.CompleteMyWCFClient)
                      .OnMessage<SendCommand>(command);
        }

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
}