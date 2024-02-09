using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
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

        public IActionResult LeaderBoard()
        {
            var events = _context.Seasons.FirstOrDefault(s => s.Year == 2024)?.Events.Select(e => e.Id).ToList() ?? [];

            var userScores = _context.Users.Select((u, i) => new UserScore { Position = i + 1, User = u, Score = GetUserScore(_context, u, events) }).ToList();

            return View(userScores);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static int GetUserScore(SportleDbContext context, IdentityUser user, List<Guid> eventIds)
        {
            if (eventIds.Count == 0)
                return 0;

            if (!Guid.TryParse(user.Id, out var userId))
                return 0;

            var predictions = context.Predictions2024.Where(p => eventIds.Contains(p.EventId) && p.UserId == userId);
            return predictions.Sum(p => p.Points + p.EarlyBonus + p.SprintBonus + p.PodiumBonus + p.PositionBonus);
        }
    }
}
