using PRJ_MKS_BTT.IService;
using System.Net;
using System.Net.Mail;
namespace PRJ_MKS_BTT.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtp = _config.GetSection("SmtpSettings");

            var message = new MailMessage();
            message.From = new MailAddress(smtp["SenderEmail"], smtp["SenderName"]);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(smtp["Server"], int.Parse(smtp["Port"]))
            {
                Credentials = new NetworkCredential(smtp["SenderEmail"], smtp["Password"]),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

    }
}
