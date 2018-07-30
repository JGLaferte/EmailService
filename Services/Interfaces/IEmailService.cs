using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailService.Services.Interfaces
{
    public interface IEmailService
    {
        Task send(string Subject, string Message, MailAddress To);
    }
}
