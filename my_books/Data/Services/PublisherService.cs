using Microsoft.EntityFrameworkCore;
using my_books.Data.Exceptions;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace my_books.Data.Services
{
    public class PublisherService
    {
        private AppDbContext _context;

        public PublisherService(AppDbContext context)
        {
            _context = context;
        }

        public List<Publisher> GetAllPublishers() => _context.Publishers.ToList();
        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (StringStartsWithNumber(publisher.Name)) throw new PublisherNameException("Name Starts with number",
                publisher.Name);

            var _publisher = new Publisher()
            {
                Name = publisher.Name  
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public Publisher GetPublisherById(int id) => _context.Publishers.FirstOrDefault(n => n.Id == id);

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM()
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n => new BookAuthorVM()
                    {
                        BookName = n.Title,
                        BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return _publisherData;
        }

        public void DeletePublisherById(int id)
        {
           var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);

            if(_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }else
            {
                throw new System.Exception($"The publisher with id: {id} does not exist");
            }
        }

        public List<string> GetPublisherR(DateTime FirstDate, DateTime EndDate, int BookId, int AuthorId)
        {
            var gr = _context.Publishers
                .Include(a => a.Book_Authors)
                .Where(a => a.Book_Authors.Any(a => a.AuthorId == AuthorId && a.BookId == BookId && a.Book.DateRead >= FirstDate && a.Book.DateAdded >= EndDate))
                .Select(p => p.Name)
                .ToList();


            return gr;
        }


        private bool StringStartsWithNumber(string name)
        {
            return (Regex.IsMatch(name, @"^\d"));
        }

       
    }
}
