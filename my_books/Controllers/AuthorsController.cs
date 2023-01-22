using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private AuthorsService _authorsService;
        public AuthorsController(AuthorsService authorsServices)
        {
            _authorsService = authorsServices;
        }

        [HttpPost("add-author")]
        public IActionResult AddBook([FromBody] AuthorVM author)
        {
            _authorsService.AddAuthor(author);
            return Ok();
        }
        [HttpGet("get-author-with-books-by-id/{id}")]
        public IActionResult GetAuthorWithBooks(int id)
        {
            var response = _authorsService.GetAuthorWithBooks(id);
            return Ok(response);
        }

        [HttpGet("get-author-with-genre")]
        public IActionResult GetAuthorWithGenre(string genre, bool IsRead, string title, int year)
        {
            var response = _authorsService.GetAuthorWithGenre(genre, IsRead, title, year); 
            return Ok(response);
        }

        [HttpGet("get-author-book-count")]
        public IActionResult GetAuthorsBookCount(DateTime FirstDate, DateTime EndDate, DateTime DateRead, DateTime DateAdded, int AuthorId)
        {
            var response = _authorsService.GetAuthorsBookCount(FirstDate, EndDate, DateRead , DateAdded, AuthorId);
            return Ok(response);
        }
        
    }
}
