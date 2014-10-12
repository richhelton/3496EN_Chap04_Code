using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using MyMessages.IMessages;
using MyMessages.MessageParts;

namespace MVCApp.Models
{
    public class XMLLoads
    {

        /***
           * 
           * Get PayModels from Paymessages
           * 
           * ***/
        public List<PayModel> GetPayments()
        {

            List<PayModel> list = new List<PayModel>();

            
            string[] dirs = Directory.GetFiles(@"C:\temp\", "temp?.xml");

            foreach (string filename in dirs)
            {
                /***
                 * De-serialize the XML into the objects
                 * *****/
                PayMessage details = DeserializeEventMessage(filename);

                PayModel model = new PayModel();

                model.Id = list.Count + 1;

                model.EventId = details.EventId;

                model.state = StateCodes.initial;

                /***
                 *  Add to my list for later use
                 * ****/
                list.Add(model);
 
            }


            return list; 

        }

        /***
          * 
          * Get PayMessages from XML files
          * 
          * ***/
  
        public List<PayMessage> GetMessages()
        {

            List<PayMessage> list = new List<PayMessage>();

            string[] dirs = Directory.GetFiles(@"C:\temp\", "temp?.xml");

            foreach (string filename in dirs)
            {
                /***
                 * De-serialize the XML into the objects
                 * *****/
                PayMessage message = DeserializeEventMessage(filename);


                /***
                 *  Add to my list for later use
                 * ****/
                list.Add(message);

            }


            return list;

        }


        /***
         * 
         * De-serialize a XML Payment file
         * 
         * ***/
        static public PayMessage DeserializeEventMessage(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PayMessage));
            using (TextReader reader = new StreamReader(filename))
            {
                PayMessage eventMsg = (PayMessage)serializer.Deserialize(reader);
                return eventMsg;
            }
        }    
    
    }





}