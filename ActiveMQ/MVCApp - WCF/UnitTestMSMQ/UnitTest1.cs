using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMessages;
using NServiceBus;
using NServiceBus.Installation.Environments;

namespace UnitTestMSMQ
{
    [TestClass]
    public class UnitTest1
    {

        private static IBus _bus;
        private IStartableBus _startableBus;
        public static IBus Bus
        {
            get { return _bus; }
        }
         
         
        
        [TestMethod]
        public void TestMethod1()
        {

            //Use a single queue
            Configure.ScaleOut(s => s.UseSingleBrokerQueue());

            _startableBus = Configure.With()
           .DefaultBuilder()
           .RijndaelEncryptionService()
           .UseTransport<NServiceBus.ActiveMQ>()
           .UnicastBus()
           .CreateBus();

            Configure.Instance.ForInstallationOn<Windows>().Install();


            /**
              * State sent to Saga
              * ***/
            SendCommand command = new SendCommand();
            command.RequestId = new Guid("8b265223-dc9e-4789-a6df-69d19f644ad7");
            command.state = MyMessages.MessageParts.StateCodes.SentMySaga;

 
            _bus = _startableBus.Start();


            _bus.Send(command);

        }
    }
}
