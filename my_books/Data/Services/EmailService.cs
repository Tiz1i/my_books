using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class EmailService
    {
        private AppDbContext _context;
        public EmailService(AppDbContext context)
        {
            _context = context;
        }

        public string SendMailMethod(string text, string subject)
        {
            List<string> List = _context.Publishers
                                .Where(i => i.Email != null).Select(b => b.Email).ToList();
            foreach (var email in List)
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                MailMessage message = new MailMessage();
                //message.Attachments.Add(new Attachment(HtmlToPdf(html), "Testing.pdf", "application/pdf"));
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                message.From = new MailAddress("noreply@dev.al");
                message.To.Add(email);
                message.Subject = subject;
                message.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential("noreply@dev.al", "mujhkhsroosvmody");
                SmtpServer.EnableSsl = true;

                try
                {
                    SmtpServer.Send(message);
                }
                catch (Exception ex)
                {
                    return "Nuk dergohet";
                }
                return "U dergua me sukses";
            }
            return "ok";
        }
    }
}
