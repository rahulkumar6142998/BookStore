using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class MailSender
    {
        private IConfiguration configuration;

        public MailSender(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public String Send(string to, string from, string subject, string content)
        {
            try
            {
                var host = configuration["Yandex:Host"];
                var port = int.Parse(configuration["Yandex:Port"]);
                var username = configuration["Yandex:Username"];
                var password = configuration["Yandex:Password"];
                var enable = bool.Parse(configuration["Yandex:SMTP:startssl:enable"]);

                var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password),
           
            };
                
                var mailMessage = new MailMessage(from, to);
                mailMessage.Subject = subject;
                mailMessage.Body = content;
                mailMessage.IsBodyHtml = true;


                smtpClient.Send(mailMessage);

                return "1";

               
            }
            catch (Exception ex)
            {
                String exp = ex.Message.ToString();
                return exp;
            }
        }
    }
}
