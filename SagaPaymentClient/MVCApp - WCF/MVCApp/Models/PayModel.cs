using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Messages;
using PaymentMessages.MessageParts;

namespace MVCApp.Models
{
    public class PayModel
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public PaymentReq paymentReq { get; set; }

        public ErrorCodes errorCode { get; set; }
        public StateCodes state { get; set; }


    }
}