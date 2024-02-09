using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Extensions;
using Sportle.Web.Models.Formula1;
using System.Security.Claims;

namespace Sportle.Web.Controllers
{
    [Authorize]
    public class PredictionsController : Controller
    {
        private readonly SportleDbContext _context;

        public PredictionsController(SportleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            if (id is null)
                return NotFound();

            var @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event is null)
                return NotFound();

            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return NotFound();

            var prediction = await _context.Predictions2024.FirstOrDefaultAsync(p => p.EventId == id && p.UserId == userId) ?? new EventPrediction2024 { EventId = id.Value, UserId = userId };

            var drivers = _context.Drivers.ToList();

            ViewData["Event"] = @event;
            ViewData["Drivers"] = drivers;

            return View(prediction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Guid? id, EventPrediction2024 model)
        {
            if (id is null)
                return NotFound();

            var @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event is null)
                return NotFound();

            if (!User.HasId(out var userId))
                return NotFound();

            ValidatePrediction(model, @event);

            if (ModelState.IsValid)
            {
                try
                {
                    model.PredictedOn = DateTime.UtcNow;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Predictions2024.Any(p => p.EventId == id && p.UserId == userId))
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

        private void ValidatePrediction(EventPrediction2024 prediction, Event @event)
        {
            if (!User.HasId(out var userId))
            {
                ModelState.AddModelError("", "Could not find user.");
                return;
            }

            if (prediction.UserId != userId)
            {
                ModelState.AddModelError("", "You're only allowed to change your own prediction.");
                return;
            }

            var existingPrediction = _context.Predictions2024.AsNoTracking().FirstOrDefault(p => p.EventId == prediction.EventId && p.UserId == userId);
            var now = DateTime.UtcNow;

            ValidateShootoutPrediction(prediction, @event, existingPrediction, now);
            ValidateSprintPrediction(prediction, @event, existingPrediction, now);
            ValidateQualificationPrediction(prediction, @event, existingPrediction, now);
            ValidateRacePrediction(prediction, @event, existingPrediction, now);
        }

        private void ValidateShootoutPrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var shootout = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Shootout);
            if (shootout is not null && shootout.Start < now)
            {
                if (prediction.SprintPP != existingPrediction?.SprintPP)
                    ModelState.AddModelError(nameof(prediction.SprintPP), "Cannot change the Pole Position after the Shootout has started.");
            }
        }

        private void ValidateSprintPrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var sprint = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Sprint);
            if (sprint is not null && sprint.Start < now)
            {
                if (prediction.SprintP1 != existingPrediction?.SprintP1)
                    ModelState.AddModelError(nameof(prediction.SprintP1), "Cannot change the Winner after the Sprint has started.");
                if (prediction.SprintFL != existingPrediction?.SprintFL)
                    ModelState.AddModelError(nameof(prediction.SprintFL), "Cannot change the Fastest Lap after the Sprint has started.");
            }
        }

        private void ValidateQualificationPrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var qualification = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Qualification);
            if (qualification is not null && qualification.Start < now)
            {
                if (prediction.RacePP != existingPrediction?.RacePP)
                    ModelState.AddModelError(nameof(prediction.RacePP), "Cannot change the Pole Position after the Qualification has started.");
            }
        }

        private void ValidateRacePrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var race = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Race);
            if (race is not null && race.Start < now)
            {
                if (prediction.RaceP1 != existingPrediction?.RaceP1)
                    ModelState.AddModelError(nameof(prediction.RaceP1), "Cannot change the P1 after the Race has started.");
                if (prediction.RaceP2 != existingPrediction?.RaceP2)
                    ModelState.AddModelError(nameof(prediction.RaceP2), "Cannot change the P2 after the Race has started.");
                if (prediction.RaceP3 != existingPrediction?.RaceP3)
                    ModelState.AddModelError(nameof(prediction.RaceP3), "Cannot change the P3 after the Race has started.");
                if (prediction.RaceP4 != existingPrediction?.RaceP4)
                    ModelState.AddModelError(nameof(prediction.RaceP4), "Cannot change the P4 after the Race has started.");
                if (prediction.RaceP5 != existingPrediction?.RaceP5)
                    ModelState.AddModelError(nameof(prediction.RaceP5), "Cannot change the P5 after the Race has started.");
                if (prediction.RaceP6 != existingPrediction?.RaceP6)
                    ModelState.AddModelError(nameof(prediction.RaceP6), "Cannot change the P6 after the Race has started.");
                if (prediction.RaceP7 != existingPrediction?.RaceP7)
                    ModelState.AddModelError(nameof(prediction.RaceP7), "Cannot change the P7 after the Race has started.");
                if (prediction.RaceP8 != existingPrediction?.RaceP8)
                    ModelState.AddModelError(nameof(prediction.RaceP8), "Cannot change the P8 after the Race has started.");
                if (prediction.RaceP9 != existingPrediction?.RaceP9)
                    ModelState.AddModelError(nameof(prediction.RaceP9), "Cannot change the P9 after the Race has started.");
                if (prediction.RaceP10 != existingPrediction?.RaceP10)
                    ModelState.AddModelError(nameof(prediction.RaceP10), "Cannot change the P10 after the Race has started.");
                if (prediction.RaceFL != existingPrediction?.RaceFL)
                    ModelState.AddModelError(nameof(prediction.RaceFL), "Cannot change the Fastest Lap after the Race has started.");
            }

            var predictions = new List<Guid?> {
                prediction.RaceP1,
                prediction.RaceP2,
                prediction.RaceP3,
                prediction.RaceP4,
                prediction.RaceP5,
                prediction.RaceP6,
                prediction.RaceP7,
                prediction.RaceP8,
                prediction.RaceP9,
                prediction.RaceP10
            };

            var duplicates = predictions.Where(p => p is not null).GroupBy(p => p).Where(g => g.Count() > 1);
            if (duplicates.Any())
                ModelState.AddModelError("", "P1 - P10 must all have a unique driver.");
        }
    }
}
