using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportle.Web.Data;
using Sportle.Web.Models;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Controllers
{
    [Authorize]
    [Route("Leaderboard")]
    public class LeaderboardController : Controller
    {
        private readonly SportleDbContext _context;
        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(SportleDbContext context, ILogger<LeaderboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var events = _context.Seasons.FirstOrDefault(s => s.Year == 2024)?.Events.Select(e => e.Id).ToList() ?? [];
            var userScores = _context.Users.ToList().Select(u => new UserScore { User = u, Score = GetUserScore(_context, u, events) }).OrderByDescending(s => s.Score).ToList();

            ViewBag.Events = _context.Seasons.First(s => s.Year == 2024).Events
                .OrderBy(e => e.Sessions.First(s => s.Type == SessionType.Race).Start)
                .ToList();

            return View(userScores);
        }

        [HttpGet("Event/{eventId}")]
        public IActionResult Index(Guid? eventId)
        {
            if (eventId is null) {
                return NotFound();
            }

            ViewBag.Events = _context.Seasons.First(s => s.Year == 2024).Events
                .OrderBy(e => e.Sessions.First(s => s.Type == SessionType.Race).Start)
                .ToList();
            ViewBag.Event = _context.Events.FirstOrDefault(e => e.Id == eventId);

            var events = new List<Guid> { eventId.Value };
            var userScores = _context.Users.ToList().Select(u => new UserScore { User = u, Score = GetUserScore(_context, u, events) }).OrderByDescending(s => s.Score).ToList();

            return View(userScores);
        }

        [ActionName("User")]
        [HttpGet("User/{userId}")]
        public IActionResult UserScore(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserScoreViewModel
            {
                User = user,
                Events = _context.Seasons.FirstOrDefault(s => s.Year == 2024)?.Events.OrderBy(e => e.Sessions.First(s => s.Type == Models.Formula1.SessionType.Race).Start).ToList() ?? []
            };

            var eventIds = model.Events.Select(e => e.Id);
            model.Predictions = _context.Predictions2024.Where(p => eventIds.Contains(p.EventId) && p.UserId == userId).ToList() ?? [];

            return View(model);
        }

        private static double GetUserScore(SportleDbContext context, IdentityUser user, List<Guid> eventIds)
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


