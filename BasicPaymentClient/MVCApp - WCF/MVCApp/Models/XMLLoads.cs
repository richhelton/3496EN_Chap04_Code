using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using PaymentMessages.IMessages;

namespace MVCApp.Models
{
    public class XMLLoads
    {

        public List<PayModel> GetPayments()
        {

            List<PayModel> list = new List<PayModel>();

            
            string[] dirs = Directory.GetFiles(@"C:\temp\", "temp?.xml");

            foreach (string filename in dirs)
            {
                /***
                 * De-serialize the XML into the objects
                 * *****/
                PaymentMessage details = DeserializeEventMessage(filename);

                PayModel model = new PayModel();

                model.Id = list.Count + 1;

                model.EventId = details.EventId;

                /***
                 *  Add to my list for later use
                 * ****/
                list.Add(model);
 
            }


            return list; 

        }


        public List<PaymentMessage> GetMessages()
        {

            List<PaymentMessage> list = new List<PaymentMessage>();

            string[] dirs2 = Directory.GetFiles(@".");

            string[] dirs = Directory.GetFiles(@"C:\temp\", "temp?.xml");

            foreach (string filename in dirs)
            {
                /***
                 * De-serialize the XML into the objects
                 * *****/
                PaymentMessage message = DeserializeEventMessage(filename);


                /***
                 *  Add to my list for later use
                 * ****/
                list.Add(message);

            }


            return list;

        }



        static public PaymentMessage DeserializeEventMessage(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PaymentMessage));
            using (TextReader reader = new StreamReader(filename))
            {
                PaymentMessage eventMsg = (PaymentMessage)serializer.Deserialize(reader);
                return eventMsg;
            }
        }    
    
    }





}