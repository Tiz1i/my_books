using MailKit;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private EmailService _emailService;
        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("get-email")]
        public async Task<string> SendMail(string text, string subject)
        {
            try
            {
                var _result = _emailService.SendMailMethod(text, subject);
                return _result;

            }
            catch (Exception)
            {
                return "Nuk u dergua";
            }
        }

    }
}
