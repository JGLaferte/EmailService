using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers
{
    [Route("[controller]")]
    public class EmailController : Controller
    {
        private readonly IEmailService _EmailService;
        public EmailController(IEmailService EmailService)
        {
            _EmailService = EmailService;
        }
        [HttpPost("Send")]
        public async Task<IActionResult> Send(string Subject, string Message , string Adress  )
        {
            await _EmailService.send(Subject , Message , new MailAddress(Adress));
            return Ok();
        }
    }
}