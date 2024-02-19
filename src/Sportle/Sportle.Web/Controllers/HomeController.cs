using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportle.Web.Data;
using Sportle.Web.Extensions;
using Sportle.Web.Models;
using Sportle.Web.Models.Formula1;
using System.Diagnostics;

namespace Sportle.Web.Controllers
{
    [Authorize]
    public class HomeController : SportleBaseController
    {
        private readonly SportleDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(SportleDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                NextEvent = _context.Seasons.First(s => s.Year == 2024).Events
                .Where(e => e.Sessions.First(s => s.Type == SessionType.Race).Start > DateTime.Now)
                .OrderBy(e => e.Sessions.First(s => s.Type == SessionType.Race).Start)
                .FirstOrDefault(),

                PrevEvent = _context.Seasons.First(s => s.Year == 2024).Events
                .Where(e => e.Sessions.First(s => s.Type == SessionType.Race).Start < DateTime.Now)
                .OrderBy(e => e.Sessions.First(s => s.Type == SessionType.Race).Start)
                .FirstOrDefault()
            };

            _ = User.HasId(out var userId);

            if (model.NextEvent != null)
                model.NextPrediction = _context.Predictions2024.FirstOrDefault(p => p.UserId == userId && p.EventId == model.NextEvent.Id);

            if (model.PrevEvent != null)
                model.PrevPrediction = _context.Predictions2024.FirstOrDefault(p => p.UserId == userId && p.EventId == model.PrevEvent.Id);

            return View(model);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
