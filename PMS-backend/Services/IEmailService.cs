
using System.Net;
using System.Net.Mail;

namespace PMS_backend.Services
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {

            var fromAddress = new MailAddress(_configuration["EmailSettings:Sender"], _configuration["EmailSettings:SenderName"]);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = _configuration["EmailSettings:MailServer"],
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _configuration["EmailSettings:Password"])
            };

            using (var message2 = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = message
            })
            {
                await smtp.SendMailAsync(message2);
            }

        }
    }


}
