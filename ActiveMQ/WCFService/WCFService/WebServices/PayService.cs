using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using MyMessages.IMessages;
using NServiceBus;

namespace WCFServer.WebServices
{

    /****
     * 
     *    WcfService -- From generic WCF service for exposing a messaging endpoint.
     *    We are handling the PayMessage and returning the ErrorCode values
     * 
     * ****/
    public class PayService : WcfService<PayMessage, ErrorCodes>
    {
    }
}
