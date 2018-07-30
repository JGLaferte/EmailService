using EmailService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public class MailService : IEmailService
    {
        private readonly IConfiguration _Configuration;
        public MailService(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public async Task send( string Subject , string Message , MailAddress To)
        {
            await Task.Run(() => 
            {
                var fromAddress = new MailAddress(_Configuration["FromEmail:User"], _Configuration["FromEmail:Name"]);
                var message = new MailMessage(fromAddress, To);
                message.Subject = Subject;
                message.Body = Message;


                using (var smtp = new SmtpClient()
                {
                    Host = _Configuration["Smtp:Host"],
                    Port = int.Parse(_Configuration["Smtp:Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, _Configuration["FromEmail:Password"])
                })
                {
                    smtp.Send(message);
                }
            });
        }
    }
}
