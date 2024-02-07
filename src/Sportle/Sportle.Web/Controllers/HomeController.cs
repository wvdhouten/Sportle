using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportle.Web.Data;
using Sportle.Web.Models;
using System.Diagnostics;

namespace Sportle.Web.Controllers
{
    public class HomeController : SportleBaseController
    {
        private readonly SportleDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(SportleDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Repair([FromServices] RoleManager<IdentityRole> roleManager, [FromServices] UserManager<IdentityUser> userManager)
        {
            if (!roleManager.Roles.Any())
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            var firstUser = userManager.Users.FirstOrDefault();
            if (firstUser is not null)
                await userManager.AddToRoleAsync(firstUser, "Admin").ConfigureAwait(false);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var events = _context.Seasons.First(s => s.Year == 2024).Events;
            var now = DateTime.Now;

            var model = new DashboardViewModel
            {
                Events = events,
                NextEvent = events.OrderBy(e => e.Sessions.First(s => s.Type == Models.Formula1.SessionType.Race).Start).FirstOrDefault(e => e.Sessions.Any(s => s.Start > now))               
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
