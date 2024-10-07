using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Helpers
{
    public class MailHelper : IMailHelper
    {

        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
           _configuration = configuration;
        }


        public Response SendEmail(string to, string subject, string body)
        {
            var nameFrom = _configuration["Mail:NameFrom"];
            var from = _configuration["Mail:From"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;


            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, SslPolicyErrors) =>
                    {
                        return true;
                    };

                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);

                }
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSucces = false,
                    Message = ex.ToString()
                };
            }

            return new Response
            {
                IsSucces = true
            };

        }
    }
}
