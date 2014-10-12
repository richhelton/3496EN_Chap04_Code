using System.ServiceModel;
using Messages;
using MyMessages.IMessages;
//using MyMessages.IMessages;

namespace MyMessages.IMessages
{
    /// <summary>
    /// This is an example of using a service contract interface
    /// instead of a service reference. Make sure to include the
    /// Action / ReplyAction values that correspond to your
    /// service inputs and outputs. A simple way to find out these
    /// values is to host the service and inspect the auto-generated
    /// WSDL by appending ?wsdl to the URL of the service.
    /// </summary>
    [ServiceContract(Namespace = "http://nservicebus.com")]
    public interface IPayMessageService
    {
        [OperationContract(Action = "http://nservicebus.com/IWcfServiceOf_PayMessage_ErrorCodes/Process", ReplyAction = "http://nservicebus.com/IWcfServiceOf_PayMessage_ErrorCodes/ProcessResponse")]
        ErrorCodes Process(PayMessage request);
    }
}