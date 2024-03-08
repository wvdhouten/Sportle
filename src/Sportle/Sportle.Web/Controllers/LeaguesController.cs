using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Extensions;
using Sportle.Web.Models;
using Sportle.Web.Models.Formula1;
using Sportle.Web.Services;

namespace Sportle.Web.Controllers
{
    [Authorize]
    [Route("Leagues")]
    public class LeaguesController : SportleBaseController
    {
        private readonly SportleDbContext _context;
        private readonly ILogger<LeaguesController> _logger;

        public LeaguesController(SportleDbContext context, ILogger<LeaguesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _ = User.HasId(out var userId);
            
            var model = _context.Leagues.Where(l => l.Users.Any(u => u.Id == userId.ToString()));

            return View(model);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] League league, [FromServices] StringService stringService)
        {
            var newCode = stringService.GetRandomString(8);
            _ = User.HasId(out var userId);

            league.Code = newCode;
            ModelState.Remove(nameof(league.Code));
            league.Admin = _context.Users.First(u => u.Id == userId.ToString());
            ModelState.Remove(nameof(league.Admin));
            league.Users.Add(league.Admin);

            if (ModelState.IsValid)
            {
                league.Id = Guid.NewGuid();

                _context.Add(league);
                await _context.SaveChangesAsync();

                StatusMessage = $"League successfully created. Code: {league.Code}";

                return RedirectToAction(nameof(Index));
            }

            return View(league);
        }

        [HttpGet("Join/{code?}")]
        public IActionResult Join(string? code)
        {
            var league = _context.Leagues.FirstOrDefault(l => l.Code == code);

            return View(league);
        }

        [HttpPost("Join/{code}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteJoin(string code)
        {
            _ = User.HasId(out var userId);
            var league = _context.Leagues.FirstOrDefault(l => l.Code == code);
            if (league is null)
                ModelState.AddModelError(nameof(code), "No league with this code exists.");
            else if (league.Users.Any(u => u.Id == userId.ToString()))
                ModelState.AddModelError(nameof(code), "You are already part of this league.");

            if (ModelState.IsValid)
            {
                if (league is null)
                    throw new InvalidOperationException("League cannot be null.");

                var user = await _context.Users.FirstAsync(u => u.Id == userId.ToString());
                league.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(league);
        }

        [HttpGet("{id}/Details")]
        public IActionResult Details(Guid id)
        {
            var league = _context.Leagues.FirstOrDefault(l => l.Id == id);
            if (league is null)
                return NotFound();

            var events = _context.Seasons.FirstOrDefault(s => s.Year == 2024)?.Events.Select(e => e.Id).ToList() ?? [];
            var userScores = league.Users.Select(u => new UserScore { User = u, Score = GetUserScore(_context, u, events) }).OrderByDescending(s => s.Score).ToList();
            var model = new LeagueDetailsViewModel { League = league, Users = userScores };

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
