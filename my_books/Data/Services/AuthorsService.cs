using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.EntityFrameworkCore;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;

using System.Linq;
using Author = my_books.Data.Models.Author;

namespace my_books.Data.Services
{
    public class AuthorsService
    {
        private AppDbContext _context;

        public AuthorsService(AppDbContext context)
        {
            _context = context;
        }
        public void AddAuthor(AuthorVM book)
        {
            var _author = new Author()
            {
                FullName = book.FullName,

            };
            _context.Authors.Add(_author);
            _context.SaveChanges();
        }

        public AuthorWithBooksVM GetAuthorWithBooks(int authorId)
        {
            var _author = _context.Authors.Where(n => n.Id == authorId).Select(n => new AuthorWithBooksVM()
            {
                FullName = n.FullName,
                BookTitles = n.Book_Authors.Select(n => n.Book.Title).ToList()
            }).FirstOrDefault();

            return _author;
        }
        public List<Author> GetAuthorWithGenre(string genre, bool IsRead, string title, int year)
        {
            //njeri version 


            //var booklist = _context.Books.Where(row => row.Genre == genre && row.IsRead == IsRead
            //&& row.DateAdded.Value.Year.ToString() == year).Select(i => i.Id).ToList();
            //List<string> Author = _context.Book_Authors
            //                            .Where(row => booklist.Contains(row.BookId))
            //                            .Select(y => y.Author.FullName)
            //                            .ToList();
            //return Author;

            var booklist = _context.Authors
                .Include(ba => ba.Book_Authors)
                .ThenInclude(b => b.Book)
                .Where(row => row.Book_Authors.Any(b => b.Book.Genre.ToLower().Contains("romance") && b.Book.IsRead == true && b.Book.Title.Contains("i") && b.Book.DateRead.Value.Year == 2017))

                .ToList();

            return booklist;
        }

        public int GetAuthorsBookCount(DateTime FirstDate, DateTime EndDate, DateTime DateRead, DateTime DateAdded, int AuthorId)
        {
            var totalCount = _context.Authors
                .Include(ba => ba.Book_Authors)
                .ThenInclude(b => b.Book)
                .Where(a => a.Id == AuthorId && a.Book_Authors.Any(b => b.Book.DateRead >= FirstDate && DateAdded >= EndDate))
                .ToList().Count();

            return totalCount;
        }
       

        //public AuthorWithBooksVM GetAuthorWithBooks(int authorId)
        //{
        //    var _author = _context.Authors.Where(n => n.Id == authorId).Select(n => new AuthorWithBooksVM()
        //    {
        //        FullName = n.FullName,
        //        BookTitles = n.Book_Authors.Select(n => n.Book.Title).ToList()
        //    }).FirstOrDefault();`

        //    return _author;
        //}

    }
}
