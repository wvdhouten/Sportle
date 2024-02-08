using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sportle.Web.Data;
using Sportle.Web.Models.Formula1;
using System.Security.Claims;

namespace Sportle.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ResultsController : Controller
    {
        private readonly SportleDbContext _context;

        public ResultsController(SportleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
                return NotFound();

            var @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event is null)
                return NotFound();

            var eventResult2024 = await _context.Results2024.FirstOrDefaultAsync(r => r.Id == id) ?? new EventResult2024 { EventId = id.Value };

            var drivers = _context.Drivers.ToList();

            ViewData["Event"] = @event;
            ViewData["Drivers"] = drivers;

            return View(eventResult2024);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EventId,PredictedOn,SprintPP,SprintP1,SprintFL,RacePP,RaceP1,RaceP2,RaceP3,RaceP4,RaceP5,RaceP6,RaceP7,RaceP8,RaceP9,RaceP10,RaceFL")] EventResult2024 eventResult2024)
        {
            if (id != eventResult2024.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventResult2024);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventResult2024Exists(eventResult2024.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventResult2024);
        }

        private bool EventResult2024Exists(Guid id)
        {
            return _context.Results2024.Any(e => e.Id == id);
        }
    }
}
