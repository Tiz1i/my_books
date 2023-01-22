using my_books.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.ViewModels
{
    public class CustomActionResultVM
    {
        private CustomActionResultVM responseObj;

        public CustomActionResultVM()
        {
        }

        public CustomActionResultVM(CustomActionResultVM responseObj)
        {
            this.responseObj = responseObj;
        }

        public Exception Exception { get; set; }
        public Publisher Publisher { get; set; }
    }
}
