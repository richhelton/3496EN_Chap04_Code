using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NLog;
using NServiceBus;
using Messages;
using MVCApp.DAL;
using MyMessages;
using MyMessages.MessageParts;


namespace MVCApp.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/


        private static Logger logger = LogManager.GetCurrentClassLogger();


        /***
         * 
         * Will load the table from XML files
         * 
         * ****/
        public ActionResult LoadTable()
        {

            List<PayModel> paymodels = new XMLLoads().GetPayments();

            MVCAppDAL.writeEventMsg(paymodels);

            return View(paymodels);
        }

 
        public ActionResult SendWCFPay()
        {
            return View(new XMLLoads().GetPayments());
        }

        public ActionResult SendSagaPay()
        {
            return View(new XMLLoads().GetPayments());
        }


  
        public ActionResult SendWCF(int id)
        {

            var user = new XMLLoads().GetPayments().Where(p => p.Id == id).FirstOrDefault();

            var message = new XMLLoads().GetMessages().Where(p => p.EventId == user.EventId).FirstOrDefault();
           
            
            ServiceReference1.WcfServiceOf_PayMessage_ErrorCodesClient client1 = 
                new ServiceReference1.WcfServiceOf_PayMessage_ErrorCodesClient();


            ErrorCodes returnCode = client1.Process(message);

            user.errorCode = returnCode;

            return View(user);

        }

        public ActionResult SendSaga(int id)
        {

            //Get the GUID from the available XML files fro, the id passed from the page
            var user = new XMLLoads().GetPayments().Where(p => p.Id == id).FirstOrDefault();
            var message = new XMLLoads().GetMessages().Where(p => p.EventId == user.EventId).FirstOrDefault();

            // Create the send
            SendCommand command = new SendCommand();
            command.RequestId = message.EventId;
            command.state = StateCodes.SentMySaga;

            // Change the state in the EF DAL
            MVCAppDAL dal = new MVCAppDAL();
            dal.changeState(command.RequestId, command.state);

            // Send the command on the Bus
            MvcApplication.Bus.Send(command);

            return View(user);
        }


        public ActionResult Send(int id)
        {

            var user = new XMLLoads().GetPayments().Where(p => p.Id == id).FirstOrDefault();
            return View(user);

        }

        
    }
}
