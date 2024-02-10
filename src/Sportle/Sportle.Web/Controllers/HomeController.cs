using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            var now = DateTime.UtcNow;
            var events = _context.Seasons.First(s => s.Year == 2024).Events
                .OrderByDescending(e => e.Sessions.First(s => s.Type == SessionType.Race).Start > now)
                .ThenBy(e => e.Sessions.First(s => s.Type == SessionType.Race).Start)
                .ToList();

            _ = User.HasId(out var userId);
            var predictions = _context.Predictions2024.Where(p => p.UserId == userId).ToList();

            var model = new DashboardViewModel
            {
                Events = events,
                Predictions = predictions
            };

            return View(model);
        }

        public IActionResult Leaderboard()
        {
            var events = _context.Seasons.FirstOrDefault(s => s.Year == 2024)?.Events.Select(e => e.Id).ToList() ?? [];

            var userScores = _context.Users.Select(u => new UserScore { User = u, Score = GetUserScore(_context, u, events) }).ToList();

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
