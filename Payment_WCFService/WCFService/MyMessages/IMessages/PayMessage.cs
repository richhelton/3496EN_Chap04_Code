using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMessages.MessageParts;
using NServiceBus;

namespace MyMessages.IMessages
{
    public class PayMessage : IMessage
    {
        public Guid EventId { get; set; }
        public PaymentReq paymentReq { get; set; }
    }

}
