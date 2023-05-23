using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeService _homeService;
        private IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, HomeService homeService, IConfiguration configuration)
        {
            _logger = logger;
            this._homeService = homeService;
            this.configuration = configuration;
        }

        public IActionResult Index(string encryptEmail)
        {
            var book = _homeService.GetTop10Books();
            return View(book);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Contact()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactModel contact)
        {
            var body = GenerateEmailBody("Query From Customer", contact.Message, contact.Name, contact.Email);

            var mailSender = new MailSender(configuration);
            var value = mailSender.Send(configuration["Admin:Email"], configuration["Yandex:Username"], "Query From Customer", body);
            if(value=="1")
            {
                ViewBag.value = 1;
            }
            else
            {
                ViewBag.value = -1;
            }

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult SearchBook(string query, string encryptEmail)
        {
            if (query == null)
            {
                var r = _homeService.GetAllBooks();
                ViewBag.Count = r.Count();
                return View(r);
            }
            var result = _homeService.SearchBooks(query);

            ViewBag.Count = result.Count();

            return View(result);


        }
        public string GenerateEmailBody(string subject, string message, string senderName, string senderEmail)
        {
            
            string emailBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                }}
        
                .email-container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f5f5f5;
                    border-radius: 5px;
                }}
        
                .message-header {{
                    font-size: 18px;
                    font-weight: bold;
                    margin-bottom: 10px;
                }}
        
                .message-body {{
                    font-size: 16px;
                    margin-bottom: 20px;
                }}
        
                .sender-details {{
                    font-size: 14px;
                    margin-top: 20px;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <h2>New Message</h2>
                <p class='message-header'>Subject: {subject}</p>
                <p class='message-body'>{message}</p>
                <p class='sender-details'>From: {senderName} &lt;{senderEmail}&gt;</p>
            </div>
        </body>
        </html>";

            return emailBody;
        }

    }
}
