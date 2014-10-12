using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMessages;
using MyMessages.IMessages;
using MyWCFClient;
using NServiceBus;
using NServiceBus.Saga;
using NServiceBus.Testing;

namespace UnitTestHandlers
{
    [TestClass]
    public class UnitTestSaga
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
            Test.Saga<MyTestSaga>()
                    .ExpectReplyToOrginator<MyResponse>()
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
                .When(s => s.Handle(new StartsSaga()))
                    .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }

        public class MyTestSaga : NServiceBus.Saga.Saga<MySagaData>,
                              IAmStartedByMessages<StartsSaga>,
                              IHandleTimeouts<StartsSaga>
        {
            public void Handle(StartsSaga message)
            {
                ReplyToOriginator(new MyResponse());
                Bus.Publish(new MyEvent());
                Bus.Send(new MyCommand());
                RequestTimeout(TimeSpan.FromDays(7), message); //RequestUtcTimeout in 3.3
            }

            public void Timeout(StartsSaga state)
            {
                Bus.Publish<MyEvent>();
                MarkAsComplete();
            }
        }
        class MyCommand : ICommand
        {
        }

        class MyEvent : IEvent
        {
        }
        public class StartsSaga : ICommand
        {
        }

        public class MyResponse : IMessage
        {
        }

        public class MySagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
        }
    }
}