using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Extensions;
using Sportle.Web.Models.Formula1;

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
        public async Task<IActionResult> Edit(Guid? id, EventResult2024 model)
        {
            if (id is null)
                return NotFound();

            var @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event is null)
                return NotFound();

            if (!User.HasId(out var userId))
                return NotFound();

            ValidateResult(model, @event);

            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifiedOn = DateTime.UtcNow;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Results2024.Any(p => p.EventId == id))
                    {
                        _context.Add(model);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var drivers = _context.Drivers.ToList();

            ViewData["Event"] = @event;
            ViewData["Drivers"] = drivers;

            return View(model);
        }

        private void ValidateResult(EventResult2024 result, Event @event)
        {
            var raceStart = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Race)?.Start;
            if (raceStart is null) { 
                ModelState.AddModelError("", "Event doesn't have a race session.");
                return;
            }

            if (raceStart.Value > DateTime.UtcNow)
                ModelState.AddModelError("", "Cannot enter results before the race has started.");

            var hasSprint = @event.Sessions.Any(s => s.Type == SessionType.Sprint);
            if (hasSprint)
            {
                if (result.SprintPP is null)
                    ModelState.AddModelError(nameof(result.SprintPP), "Sprint Pole Position is required.");
                if (result.SprintP1 is null)
                    ModelState.AddModelError(nameof(result.SprintP1), "Sprint P1 is required.");
                if (result.SprintPP is null)
                    ModelState.AddModelError(nameof(result.SprintPP), "Sprint Fastest Lap is required.");
            }

            if (result.RacePP is null)
                ModelState.AddModelError(nameof(result.RacePP), "Sprint Pole Position is required.");
            if (result.RaceP1 is null)
                ModelState.AddModelError(nameof(result.RaceP1), "Race P1 is required.");
            if (result.RaceP2 is null)
                ModelState.AddModelError(nameof(result.RaceP2), "Race P2 is required.");
            if (result.RaceP3 is null)
                ModelState.AddModelError(nameof(result.RaceP3), "Race P3 is required.");
            if (result.RaceP4 is null)
                ModelState.AddModelError(nameof(result.RaceP4), "Race P4 is required.");
            if (result.RaceP5 is null)
                ModelState.AddModelError(nameof(result.RaceP5), "Race P5 is required.");
            if (result.RaceP6 is null)
                ModelState.AddModelError(nameof(result.RaceP6), "Race P6 is required.");
            if (result.RaceP7 is null)
                ModelState.AddModelError(nameof(result.RaceP7), "Race P7 is required.");
            if (result.RaceP8 is null)
                ModelState.AddModelError(nameof(result.RaceP8), "Race P8 is required.");
            if (result.RaceP9 is null)
                ModelState.AddModelError(nameof(result.RaceP9), "Race P9 is required.");
            if (result.RaceP10 is null)
                ModelState.AddModelError(nameof(result.RaceP10), "Race P10 is required.");
            if (result.RaceFL is null)
                ModelState.AddModelError(nameof(result.RaceFL), "Race Fastest Lap is required.");
        }
    }
}
