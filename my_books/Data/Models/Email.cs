using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Models
{
    public class Email
    {
        public string FromName { get; set; }
        public string FromMail { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }

    }
}
