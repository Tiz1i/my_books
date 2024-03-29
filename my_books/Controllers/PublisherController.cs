﻿using Microsoft.AspNetCore.Mvc;
using my_books.Data.Exceptions;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublisherService _publishersService;
        //public BadRequestResult(PublishersService publishersService)
        //{
        //    _publishersService = publishersService;
        //}
        //private readonly ILogger<PublishersController> _logger;
        public PublishersController(PublisherService publishersService /*ILogger<PublishersController> logger*/ )
        {
            _publishersService = publishersService;
            //_logger = logger;
        }


        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers()
        {
            try
            {
                var _result = _publishersService.GetAllPublishers();
                return Ok(_result);

            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the publishers");
            }
        }


        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var _response = _publishersService.GetPublisherById(id);

            if (_response != null)
            {
                return Ok(_response);
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpGet("get-publisher-books-by-id/{id}")]
        //public /*Publisher*/ /*IActionResult*/ /*ActionResult <Publisher>*/ CustomActionResult GetPublisherById(int id)
        //{
        //    var _response = _publishersService.GetPublisherById(id);
        //    if (_response != null)
        //    {
        //        return Ok(_response);
        //        var _responseObj = new CustomActionResultVM()
        //        {
        //            Publisher = _response
        //        };
        //        return new CustomActionResult(_responseObj);
        //        return _response;
        //    }
        //    else
        //        return null;
        //}

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var _response = _publishersService.GetPublisherData(id);
            return Ok(_response);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]

        public IActionResult DeletePublisherById(int id)
        {
            try
            {
                _publishersService.DeletePublisherById(id);
                return Ok(_publishersService);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("return-publishername")]
        public List<string> GetPublisher(DateTime FirstDate, DateTime EndDate, int BookId, int AuthorId)
        {
            var response = _publishersService.GetPublisherR(FirstDate, EndDate, BookId, AuthorId);

            return response;
        }
    }

    //[Serializable]
    //internal class Expetion : Exception
    //{
    //    public Expetion()
    //    {
    //    }

    //    public Expetion(string message) : base(message)
    //    {
    //    }

    //    public Expetion(string message, Exception innerException) : base(message, innerException)
    //    {
    //    }

    //    protected Expetion(SerializationInfo info, StreamingContext context) : base(info, context)
    //    {
    //    }
    //}
}

