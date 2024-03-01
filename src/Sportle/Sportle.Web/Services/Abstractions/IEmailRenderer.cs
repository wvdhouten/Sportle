namespace Sportle.Web.Services.Abstractions
{
    using System.Threading.Tasks;

    public interface IEmailRenderer
    {
        Task<string> Render<TModel>(string templateName, TModel model);
    }
}
