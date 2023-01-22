using my_books.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.ViewModels
{
    public class ImageResponseVM
    {
        public Book Book { get; set; }
        public string Message { get; set; }
    }
}
