using System;
using MyMessages;
using MyMessages.IMessages;
using NLog;
using NServiceBus;
using NServiceBus.Saga;

namespace MySaga
{

    public class PaymentRequestSaga : NServiceBus.Saga.Saga<PaymentRequestData>,
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
