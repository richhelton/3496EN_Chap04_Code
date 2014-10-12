using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCApp.DAL;
using MyMessages;
using NLog;
using NServiceBus;

namespace MVCApp.Handlers
{
    public class SendCommandHandler : IHandleMessages<ResponseCommand>
    {
        public void Handle(ResponseCommand message)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine(message.RequestId);

            // Update the state of the message
            MVCAppDAL dal = new MVCAppDAL();

            dal.changeState(message.RequestId, message.state);

        }
    }
}