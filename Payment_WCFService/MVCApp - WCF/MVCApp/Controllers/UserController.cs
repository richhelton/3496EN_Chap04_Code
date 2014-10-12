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


namespace MVCApp.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/


        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        
 
        public ActionResult SendWCFPay()
        {
            return View(new XMLLoads().GetPayments());
        }


        public ActionResult XMLIndexKendoGrid()
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


        public ActionResult Send(int id)
        {

            var user = new XMLLoads().GetPayments().Where(p => p.Id == id).FirstOrDefault();
            return View(user);

        }

        
    }
}
