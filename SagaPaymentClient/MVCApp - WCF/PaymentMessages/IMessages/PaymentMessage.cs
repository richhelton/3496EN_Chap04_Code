using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentMessages.MessageParts;
using NServiceBus;

namespace PaymentMessages.IMessages
{
    public class PaymentMessage : IMessage
    {
        public Guid EventId { get; set; }
        public PaymentReq paymentReq { get; set; }
    }

}
