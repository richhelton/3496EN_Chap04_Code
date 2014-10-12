using System;
using NServiceBus;

namespace PaymentMessages
{
    public interface IRequestExpiredEvent : IEvent
    {
        Guid RequestId { get; set; }
    }
}
