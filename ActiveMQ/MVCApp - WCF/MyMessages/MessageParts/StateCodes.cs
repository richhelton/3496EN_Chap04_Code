using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessages.MessageParts
{
    public enum StateCodes
    {
        initial,
        SentMySaga,
        SentMyWCFClient,
        CompleteMyWCFClient,
        MyWCFClientFail,
        MyWCFClientFailXML,
        Timeout,
        Fail
    }
}
