using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emails.Models
{
    public class Email
    {
        public int EmailID { get; set; }

        public string FromAddress { get; set; }

        public string FromAddressTitle { get; set; }

        public string ToAddress { get; set; }

        public string ToAddressTitle { get; set; }

        public string Message { get; set; }

        public string BodyContent { get; set; }


    }
}
