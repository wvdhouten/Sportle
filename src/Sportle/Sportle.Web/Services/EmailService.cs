using Microsoft.Extensions.Options;
using Sportle.Web.Services.Abstractions;
using System.Net.Mail;
using System.Net;

namespace Sportle.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptionsSnapshot<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            try
            {
                using var client = GetClient();
                var mailMessage = new MailMessage(_settings.Sender ?? string.Empty, recipient, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mailMessage);
            }
            catch
            {
                // Swallowed. Tried and failed.
            }
        }

        private SmtpClient GetClient()
        {
            return new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                EnableSsl = _settings.EnableSsl,
            };
        }
    }
}
