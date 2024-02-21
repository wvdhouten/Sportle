using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Areas.Admin.Models;
using Sportle.Web.Data;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class LeaguesController : Controller
    {
        private readonly SportleDbContext _context;

        public LeaguesController(SportleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Assign()
        {
            var model = new LeagueAssignViewModel()
            {
                Leagues = await _context.Leagues.ToListAsync(),
                Users = await _context.Users.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(LeagueAssignViewModel model)
        {
            model.Leagues = await _context.Leagues.ToListAsync();
            model.Users = await _context.Users.ToListAsync();

            if (ModelState.IsValid)
            {
                var league = await _context.Leagues.FirstAsync(l => l.Id == model.LeagueId);
                var user = await _context.Users.FirstAsync(u => u.Id == model.UserId.ToString());
                if (league is not null && user is not null)
                {
                    switch (model.Action)
                    {
                        case "Add":
                            league.Users.Add(user);
                            break;
                        case "Remove":
                            league.Users.Remove(user);
                            break;
                    }
                }

                _context.SaveChanges();
            }

            return View(model);
        }
    }
}
