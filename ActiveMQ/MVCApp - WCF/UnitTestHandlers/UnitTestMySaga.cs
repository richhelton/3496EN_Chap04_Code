using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMessages;
using MyMessages.IMessages;
using MyWCFClient;
using NServiceBus;
using NServiceBus.Saga;
using NServiceBus.Testing;
using MySaga;

namespace UnitTestHandlers
{
    [TestClass]
    public class UnitTestMySaga
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

            Guid test = new Guid("8b265223-dc9e-4789-a6df-69d19f644ad7");

            /**
             * State sent to Saga
             * ***/
            SendCommand command = new SendCommand();
            command.RequestId = test;
            command.state = MyMessages.MessageParts.StateCodes.SentMyWCFClient;
            /**
             * State sent to WCF CLient
             * ***/
            ResponseCommand resp = new ResponseCommand();
            resp.RequestId = test;
            resp.state = MyMessages.MessageParts.StateCodes.CompleteMyWCFClient;


            Test.Saga<PaymentRequestSaga>()
                  .ExpectTimeoutToBeSetIn<SendCommand>((state, span) => span == TimeSpan.FromHours(3))
                .ExpectSend<SendCommand>()
                .ExpectReplyToOrginator<ResponseCommand>()
                .When(s => s.Handle(command));

            Test.Saga<PaymentRequestSaga>()
                 .ExpectReplyToOrginator<ResponseCommand>()
                 .When(s => s.Handle(resp));


            Test.Saga<PaymentRequestSaga>()
                 .ExpectSend<ResponseCommand>()
               .WhenSagaTimesOut();

        }


    }
}