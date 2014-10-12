using System;
using Messages;
using MyMessages.MessageParts;
using NServiceBus;

namespace MyMessages
{
    public class ResponseCommand : ICommand
    {
        public Guid RequestId { get; set; }
         public StateCodes state { get; set; }

    }
}
