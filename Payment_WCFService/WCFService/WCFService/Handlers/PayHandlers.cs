using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using PaymentMessages.IMessages;
using NServiceBus;

namespace WCFServer.Handlers
{
    public class PayHandlers : IHandleMessages<PaymentMessage>
    {
        private readonly IBus bus;

        public PayHandlers(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(PaymentMessage message)
        {
            Console.WriteLine("======================================================================");
            Console.WriteLine(message.EventId);
            Console.WriteLine("======================================================================");

            bus.Return((int)ErrorCodes.None);


        }

    }
}