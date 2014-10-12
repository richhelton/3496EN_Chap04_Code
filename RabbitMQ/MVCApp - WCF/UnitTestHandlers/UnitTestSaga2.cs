using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMessages;
using MyMessages.IMessages;
using MySaga;
using MyWCFClient;
using NServiceBus;
using NServiceBus.Saga;
using NServiceBus.Testing;

namespace UnitTestHandlers
{
    [TestClass]
    public class UnitTestSaga2
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

             /**
             * State sent to Saga
             * ***/
            SendCommand command = new SendCommand();
            command.RequestId = new Guid("8b265223-dc9e-4789-a6df-69d19f644ad7");
            command.state = MyMessages.MessageParts.StateCodes.SentMyWCFClient;

            /**
              * Response from WCF and to MVCApp
              * ***/
            ResponseCommand resp = new ResponseCommand();
            resp.RequestId =  new Guid("8b265223-dc9e-4789-a6df-69d19f644ad7");
            resp.state = MyMessages.MessageParts.StateCodes.CompleteMyWCFClient;


            Test.Saga<MyTestSaga>()
                    .ExpectReplyToOrginator<ResponseCommand>()
                    .ExpectSend<SendCommand>()
                .When(s => s.Handle(command))
                   .ExpectReplyToOrginator<ResponseCommand>()
                 .When(s => s.Handle(resp))
                 .AssertSagaCompletionIs(true);

            Test.Saga<MyTestSaga>()
                      .ExpectReplyToOrginator<ResponseCommand>()
                       .ExpectSend<SendCommand>()
                       .ExpectTimeoutToBeSetIn<SendCommand>((state, span) => span == TimeSpan.FromHours(3))
                  .When(s => s.Handle(command))
                     .ExpectReplyToOrginator<ResponseCommand>()
                   .WhenSagaTimesOut()
                   .AssertSagaCompletionIs(true);


        }

        public class MyTestSaga : NServiceBus.Saga.Saga<PaymentRequestData>,
                    IAmStartedByMessages<SendCommand>,
                    IHandleMessages<ResponseCommand>,
                    IHandleTimeouts<SendCommand>
        {
            public override void ConfigureHowToFindSaga()
            {
                ConfigureMapping<ResponseCommand>(x => x.RequestId).ToSaga(x => x.RequestId);
            }

            public void Handle(SendCommand message)
            {
                ResponseCommand resp = new ResponseCommand();
                resp.RequestId = message.RequestId;
                resp.state = MyMessages.MessageParts.StateCodes.SentMyWCFClient;
                ReplyToOriginator(resp);
                Bus.Send(message);
                RequestTimeout(TimeSpan.FromHours(3), message); // 3 hours
                Data.RequestId = message.RequestId;
            }

            public void Handle(ResponseCommand message)
            {
                if (Data.RequestId != message.RequestId)
                {
                    message.state = MyMessages.MessageParts.StateCodes.Fail;
                }
                ReplyToOriginator(message);
                MarkAsComplete();
            }


            public void Timeout(SendCommand state)
            {
                ResponseCommand resp = new ResponseCommand();
                resp.RequestId = state.RequestId;
                resp.state = MyMessages.MessageParts.StateCodes.Timeout;
                ReplyToOriginator(resp);
                MarkAsComplete();
            }
        }
    }
}