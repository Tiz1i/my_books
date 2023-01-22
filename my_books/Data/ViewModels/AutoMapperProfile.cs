using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using my_books.Data.Models;

namespace my_books.Data.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthorVM, Author>();
            CreateMap<BookVM, Book>().ReverseMap();
            CreateMap<PublisherVM, Publisher>();
            CreateMap<BookAuthorVM, Book_Author>();
            CreateMap<Exel, Book>().ReverseMap();
            CreateMap<BookEcxel, Book>().ReverseMap(); 
        }
    }
}
