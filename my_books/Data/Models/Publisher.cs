﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        //Navigation Properties
        public List<Book> Books { get; set; }
        public List<Book_Author> Book_Authors { get; set; }
    }
}