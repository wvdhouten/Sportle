namespace Sportle.Web.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync<TModel>(string recipient, string subject, TModel model);
    }
}
