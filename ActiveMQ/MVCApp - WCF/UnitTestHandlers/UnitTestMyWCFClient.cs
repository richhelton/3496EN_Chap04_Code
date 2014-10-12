using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMessages;
using MyMessages.IMessages;
using MyWCFClient;
using NServiceBus;
using NServiceBus.Testing;

namespace UnitTestHandlers
{
    [TestClass]
    public class UnitTestMyWCFClient
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

            PayMessage message = new PayMessage();

            Guid test = new Guid("8b265223-dc9e-4789-a6df-69d19f644ad7");

            SendCommand command = new SendCommand();
            command.RequestId = test;
            command.state = MyMessages.MessageParts.StateCodes.SentMyWCFClient;

            Test.Handler<EventMessageHandler>()
                   .ExpectReply<ResponseCommand>(m=>m.state == MyMessages.MessageParts.StateCodes.CompleteMyWCFClient)
                     .OnMessage<SendCommand>(command);
         }

    }
}