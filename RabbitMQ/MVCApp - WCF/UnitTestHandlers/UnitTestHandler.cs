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
    public class UnitTestHandler
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

            Test.Handler<MyHandler>()
                .ExpectReply<ResponseMessage>(m => m.String == "hello")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }

        public class MyHandler :
            IHandleMessages<RequestMessage>
        {
            public IBus Bus { get; set; }

            public void Handle(RequestMessage message)
            {
                var reply = new ResponseMessage
                            {
                                String = message.String
                            };
                Bus.Reply(reply);
            }
        }

        public class ResponseMessage : IMessage
        {
            public string String { get; set; }
        }

        public class RequestMessage : IMessage
        {
            public string String { get; set; }
        }

    }
}