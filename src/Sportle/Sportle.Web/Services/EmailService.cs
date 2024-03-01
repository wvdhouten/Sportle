using Microsoft.Extensions.Options;
using Sportle.Web.Services.Abstractions;
using System.Net.Mail;
using System.Net;

namespace Sportle.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IEmailRenderer _renderer;

        public EmailService(IOptionsSnapshot<EmailSettings> options, IEmailRenderer renderer)
        {
            _settings = options.Value;
            _renderer = renderer;
        }

        public async Task SendEmailAsync<T>(string recipient, string subject, T model)
        {
            var htmlBody = await _renderer.Render(typeof(T).Name, model);

            using var client = GetClient();
            var mailMessage = new MailMessage(_settings.Sender, recipient, subject, htmlBody)
            {
                IsBodyHtml = true,
            };

            await client.SendMailAsync(mailMessage);
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
