using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using AutoMapper;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static my_books.Controllers.BooksController;
using Fare;
using OfficeOpenXml;

namespace my_books.Data.Services
{
    public class BooksService
    {

        private AppDbContext _context;
        private IMapper _mapper;
        public BooksService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddBookWithAuthors(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);
            _context.SaveChanges();

            //foreach (var id in book.AuthorIds)
            //{
            //    Book_Author _book_author = new Book_Author()
            //    {
            //        BookId = _book.Id,
            //        AuthorId = id
            //    };
            //    _context.Book_Authors.Add(_book_author);
            //    _context.SaveChanges();
            //}
        }

        public Book AddNewBook(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);
            _context.SaveChanges();

            //foreach (var id in book.AuthorIds)
            //{
            //    Book_Author _book_author = new Book_Author()
            //    {
            //        BookId = _book.Id,
            //        AuthorId = id
            //    };
            //    _context.Book_Authors.Add(_book_author);
            //    _context.SaveChanges();
            //}
            return _book;
        }
        //public List<Book> GetAllBooks() => _context.Books.ToList();
        public ActionResult<List<BookVM>> GetAllBooks()
        {
            var book = _context.Books.Include(i => i.Publisher).ThenInclude(i => i.Book_Authors).ToList();
            return _mapper.Map<List<BookVM>>(book);
        }

        public List<Book> FilteredBook(DateTime DateRead, DateTime DateAdded)
        {
            List<Book> Books = _context.Books
                                        .Include(p => p.Publisher)
                                        .Include(ba => ba.Book_Authors)
                                        .Where(e => e.DateRead >= DateRead && e.DateAdded <= DateAdded)
                                        .ToList();
            return Books;

        }

        public List<Book> FilteredBookByMapp(DateTime DateRead, DateTime DateAdded)
        {
            var allbooks = _context.Books.Include(i => i.Publisher).ThenInclude(i => i.Book_Authors).Where(e => e.DateRead >= DateRead && e.DateAdded <= DateAdded).ToList();
            return _mapper.Map<List<Book>>(allbooks);
        }

        public BookWithAuthorsVM GetBookById(int bookId)
        {
            var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();

            return _bookWithAuthors;
        }

        public ActionResult<BookWithAuthorsVM> GetAllBooksByIdByMapp(int bookId)
        {
            var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).FirstOrDefault();
            return _mapper.Map<BookWithAuthorsVM>(_bookWithAuthors);
        }

        public List<Book> GetNewBooks(int authorId, int publisherId)
        {
            var _bookthAuthorPublishers = _context.Books.Include(n => n.Book_Authors).Where(row => row.PublisherId == publisherId && row.Book_Authors.Any(i => i.AuthorId == authorId)).Select(book => new Book()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                Book_Authors = book.Book_Authors,
                PublisherId = book.PublisherId,
            });
            return _bookthAuthorPublishers.ToList();
        }

        public List<Book> GetNewBooksByMapp(int authorId, int publisherId)
        {
            var _bookAuthorPublishers = _context.Books.Include(n => n.Book_Authors).Where(row => row.PublisherId == publisherId && row.Book_Authors.Any(i => i.AuthorId == authorId)).Select(book => new Book()).ToList();
            return _mapper.Map<List<Book>>(_bookAuthorPublishers);
        }

        public Book UpdateBookById(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;

                _context.SaveChanges();
            }
            return _book;
        }

        public Book UpdateBookByIdByMapp(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            return _mapper.Map<Book>(_book);
        }

        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }

        public byte[] DownloadExcel(DateTime DateRead, DateTime DateAdded)
        {
            List<Book> downlodbooks = FilteredBook(DateRead, DateAdded);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ListaLibrave");
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Genre";
                worksheet.Cell(1, 5).Value = "DateRead";
                worksheet.Cell(1, 6).Value = "DateAdded";
                worksheet.Cell(1, 7).Value = "IsRead";
                worksheet.Cell(1, 8).Value = "Rate";


                var cellIndex = 2;

                for (var index = 1; index <= downlodbooks.Count(); index++)
                {
                    for (int i = 0; i < downlodbooks.ElementAt(index - 1).Book_Authors.Count(); i++, cellIndex++)
                    {
                        worksheet.Cell(cellIndex, 1).Value = "Id";
                        worksheet.Cell(cellIndex, 2).Value = downlodbooks.ElementAt(index - 1).Title != null ?
                            downlodbooks.ElementAt(index - 1).Title : "";
                        worksheet.Cell(cellIndex, 3).Value =
                            downlodbooks.ElementAt(index - 1).DateAdded.Value;
                        worksheet.Cell(cellIndex, 4).Style.DateFormat.Format = "dd/MM/yyyy";
                        worksheet.Cell(cellIndex, 5).Value =
                            downlodbooks.ElementAt(index - 1).Description;
                        worksheet.Cell(cellIndex, 6).Value = "";
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public string UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileurl = Path.GetFileName(file.FileName);
                string replacestr = Regex.Replace(fileurl, "[ë,Ç,Ë,ç, ]", "-");
                var fileName = "my_books" + DateTime.Now + "_" + replacestr;
                var path = "\\downloads\\" + fileName;

                //Validimi i filave
                string extension = Path.GetExtension(file.FileName).ToLower();

                if (extension != ".jpg" && extension != ".png" && extension != ".tiff" && extension != ".jpeg")
                {
                    return "Imazhi nuk u ngarkua!";
                }
                return path;
            }
            else
            {
                return "Ju nuk keni zgjedhur imazh!";
            }

        }

        public async Task<Book> UpdateImgBookById(int bookId, IFormFile file)
        {
            var image = UploadFile(file);
            var book = _context.Books.Find(bookId);
            if (book.CoverUrl != null)
            {
                return book;
            }
            else
            {
                book.CoverUrl = image;
                _context.Books.Update(book);

            }
            return book;
        }

        public async Task<List<Exel>> UploadDataExel(IFormFile file)
        {

            var list = new List<Exel>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 1; row <= rowcount; row++)
                    {
                        if (row != 1)
                        {
                            list.Add(new Exel
                            {
                                Title = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Description = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Genre = worksheet.Cells[row, 3].Value.ToString().Trim(),

                            });
                        }
                    }
                    foreach (var item in list)
                    {
                        _context.Exels.Add(item);
                        _context.SaveChanges();
                    }

                }
            }
            return list;
        }

        public async Task<List<BookEcxel>> UploadDataExelBook(IFormFile file)
        {

            var list = new List<Book>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 1; row <= rowcount; row++)
                    {
                        if (row != 1)
                        {
                            list.Add(new Book
                            {
                                Title = Convert.ToString(worksheet.Cells[row, 1].Value),
                                Description = Convert.ToString(worksheet.Cells[row, 2].Value),
                                Genre = Convert.ToString(worksheet.Cells[row, 3].Value),
                                IsRead = Convert.ToBoolean(worksheet.Cells[row, 4].Value),
                                DateRead = Convert.ToDateTime(worksheet.Cells[row, 5].Value),
                                Rate = Convert.ToInt32(worksheet.Cells[row, 6].Value),
                                DateAdded = Convert.ToDateTime(worksheet.Cells[row, 7].Value),
                                CoverUrl = Convert.ToString(worksheet.Cells[row, 8].Value),

                            });
                        }
                    }
                    foreach (var item in list)
                    {
                        _context.Books.Add(item);
                        _context.SaveChanges();
                    }

                }
            }
            return _mapper.Map<List<BookEcxel>>(list);
        }












        //public Book UpdateBookById(int bookId, BookVM book)
        //{
        //    var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
        //    if (_book != null)
        //    {
        //        _book.Title = book.Title;
        //        _book.Description = book.Description;
        //        _book.IsRead = book.IsRead;
        //        _book.DateRead = book.IsRead ? book.DateRead.Value : null;
        //        _book.Rate = book.IsRead ? book.Rate.Value : null;
        //        _book.Genre = book.Genre;
        //        _book.CoverUrl = book.CoverUrl;

        //        _context.SaveChanges();
        //    }
        //    return _book;
        //}

    }
}

   
