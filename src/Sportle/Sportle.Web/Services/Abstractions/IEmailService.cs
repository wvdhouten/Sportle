namespace Sportle.Web.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipient, string subject, string body);
    }
}
