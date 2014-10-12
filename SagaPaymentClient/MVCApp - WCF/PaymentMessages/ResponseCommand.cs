﻿using System;
using Messages;
using PaymentMessages.MessageParts;
using NServiceBus;

namespace PaymentMessages
{
    public class ResponseCommand : ICommand
    {
        public Guid RequestId { get; set; }
         public StateCodes state { get; set; }

    }
}
