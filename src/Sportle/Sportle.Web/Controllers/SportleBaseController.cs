using Microsoft.AspNetCore.Mvc;

namespace Sportle.Web.Controllers
{
    public abstract class SportleBaseController : Controller
    {
        [TempData]
        public string? StatusMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }
    }
}
