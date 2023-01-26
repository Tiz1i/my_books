using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public BooksService _booksService;
        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            var allBooks = _booksService.GetAllBooks();
            return Ok(allBooks);
        }

        [HttpGet("get-all-books-mapp")]
        public ActionResult<List<BookVM>> GetAllBooksMAPPING()
        {
            var allBooks = _booksService.GetAllBooks();
            return Ok(allBooks);
        }

        [HttpGet("get-all-newbooks")]
        public IActionResult GetNewBooks(int authorId, int publisherId)
        {
            var allBooks = _booksService.GetNewBooks(authorId, publisherId);
            return Ok(allBooks);
        }

        [HttpGet("get-book-by-id/{id}")]
        public ActionResult<BookWithAuthorsVM> GetAllBooksByIdByMapp(int bookId)
        {
            var book = _booksService.GetAllBooksByIdByMapp(bookId);
            return Ok(book);
        }

        [HttpGet("get-book-by-id-mappp/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _booksService.GetBookById(id);
            return Ok(book);
        }

        [HttpPost("add-book-with-authors")]
        public IActionResult AddBook([FromBody] BookVM book)
        {
            _booksService.AddBookWithAuthors(book);
            return Ok();
        }

        [HttpPost("add-book-with-authors-newbook")]
        public IActionResult AddNewBooks([FromBody] BookVM book)
        {
            var books = _booksService.AddNewBook(book);
            return Ok(books);
        }
        [HttpGet("filter-books-orderingbydate")]
        public List<Book> FilteredBook(DateTime DateRead, DateTime DateAdded)
        {
            var filteredbook = _booksService.FilteredBook(DateRead, DateAdded);
            return filteredbook;
        }
        [HttpGet("filter-books-orderingbydate-bymapp")]
        public List<Book> FilteredBookByMapp(DateTime DateRead, DateTime DateAdded)
        {
            var filteredbook = _booksService.FilteredBook(DateRead, DateAdded);
            return filteredbook;
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookId(int id, [FromBody] BookVM book)
        {
            var updateBook = _booksService.UpdateBookById(id, book);
            return Ok(updateBook);
        }

        [HttpPut("update-book-by-id-bymapp/{id}")]
        public IActionResult UpdateBookByIdByMapp(int id, [FromBody] BookVM book)
        {
            var updateBook = _booksService.UpdateBookById(id, book);
            return Ok(updateBook);
        }

        [HttpDelete("delete-bookId")]
        public IActionResult DeleteBookId(int id)
        {
            _booksService.DeleteBookById(id);
            return Ok();
        }

        [HttpGet("download-book-list")]
        public IActionResult DownloadExcelData(DateTime DateRead, DateTime DateAdded)
        {
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "ListaLibrave" + DateTime.Now + ".xlsx";
            var content = _booksService.DownloadExcel(DateRead, DateAdded);

            return File(content, contentType, fileName);
        }

        [HttpPost("upload-book-img")]
        public IActionResult UploadBookImg(IFormFile file)
        {
            var uploadBookImg = _booksService.UploadFile(file);
            return Ok(uploadBookImg);
        }

        [HttpPut("update-book-img")]
        public async Task<Book> UpdateBookImg(int bookId, IFormFile file)
        {
            var uploadBookImg = await _booksService.UpdateImgBookById(bookId, file);
            return uploadBookImg;
        }

        [HttpPost("uploadData-book-by-exel")]
        public async Task<List<Exel>> UploadData(IFormFile file)
        {
            var uploadData = await _booksService.UploadDataExel(file);
            return uploadData;
        }

        [HttpPost("UploadDataExelBook-by-exel")]
        public async Task<List<BookExel>> UploadDataBookExel(IFormFile file)
        {
            var uploadDataExelBook = await _booksService.UploadDataBookExel(file);
            return uploadDataExelBook;
        }

        [HttpPost("UploadBookExel-by-exel")]
        public async Task<List<BookExel>> UploadBookExel(IFormFile file)
        {
            var uploadBookExel = await _booksService.UploadBookExel(file);
                return uploadBookExel;
        }
    }
}

