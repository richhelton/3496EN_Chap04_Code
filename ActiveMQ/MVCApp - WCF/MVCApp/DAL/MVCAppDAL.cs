using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using MVCApp.Models;
using MyMessages;
using MyMessages.MessageParts;

namespace MVCApp.DAL
{
    public class MVCAppDAL
    {

        /*****
         * 
         * Update the table from paymodels
         * 
         * ****/
        public static void writeEventMsg(List<PayModel> paymodels)
        {
            using (var context = new MVCAppEntities())
            {
                // Get the payment rows

                var payment_rows = context.Paymessages;

                /****
                 * Remove previous rows
                 * ***/
                foreach (var row in context.Paymessages)
                {
                    payment_rows.Remove(row);
                }
                /****
                * Add rows from model
                * ***/
                int pk = 1;
                foreach (var model in paymodels)
                {
                    Paymessage new_row = new Paymessage();
                    new_row.id = pk++;
                    new_row.EventId = model.EventId;
                    new_row.state = model.state.ToString();
                    new_row.error = model.errorCode.ToString();
                    context.Paymessages.Add(new_row);
                }
                context.SaveChanges();

            }


        }

        /*****
         * 
         * Update the state in the table 
         * from the send command
         *  
         * ****/
        public void changeState(Guid RequestId, StateCodes state)
        {
            using (var context = new MVCAppEntities())
            {
                /****
                 * Update the row selected with 
                 * the new state                 * ***/
                var row = context.Paymessages.Where(x => x.EventId == RequestId).FirstOrDefault();
                if (row != null)
                {
                    Paymessage new_row = new Paymessage();
                    new_row.error = row.error;
                    new_row.EventId = row.EventId;
                    new_row.id = row.id;
                    new_row.state = state.ToString();
                    context.Entry(row).CurrentValues.SetValues(new_row);
                    context.SaveChanges();
                }
            }
        }
    }
}