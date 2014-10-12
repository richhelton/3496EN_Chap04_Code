using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessages.MessageParts
{
    public class Address
    {
        public string streetAddress1 { get; set; }
        public string streetAddress2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }  // 2-chars
        public string zip { get; set; }  // 5-digits
    }
}
