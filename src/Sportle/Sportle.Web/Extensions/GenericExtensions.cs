using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sportle.Web.Extensions
{
    public static class GenericExtensions
    {
        public static string GetBase64Resource(this IHtmlHelper helper, string content)
        {
            ArgumentNullException.ThrowIfNull(helper);

            var path = Path.Combine(Environment.CurrentDirectory, "Content", "Files", content);
            byte[] imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }
    }
}
