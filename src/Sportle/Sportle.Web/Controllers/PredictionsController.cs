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
    public class PredictionsController : SportleBaseController
    {
        private readonly SportleDbContext _context;

        public PredictionsController(SportleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Event(Guid? id)
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
        public async Task<IActionResult> Event(Guid? id, EventPrediction2024 model)
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

                StatusMessage = "Predictions submitted successfully!";

                return RedirectToAction("Index", "Home");
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
            if (shootout is not null)
            {
                if (shootout.Start < now && prediction.SprintPP != existingPrediction?.SprintPP)
                    ModelState.AddModelError(nameof(prediction.SprintPP), "Cannot change the Pole Position after the Shootout has started.");
                if (shootout.Start > now && prediction.SprintPP is null)
                    ModelState.AddModelError(nameof(prediction.SprintPP), "Sprint Pole Position is required.");
            }
        }

        private void ValidateSprintPrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var sprint = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Sprint);
            if (sprint is not null)
            {
                if (sprint.Start < now)
                {
                    if (prediction.SprintP1 != existingPrediction?.SprintP1)
                        ModelState.AddModelError(nameof(prediction.SprintP1), "Cannot change the Winner after the Sprint has started.");
                    if (prediction.SprintFL != existingPrediction?.SprintFL)
                        ModelState.AddModelError(nameof(prediction.SprintFL), "Cannot change the Fastest Lap after the Sprint has started.");
                }
                else
                {
                    if (prediction.SprintP1 is null)
                        ModelState.AddModelError(nameof(prediction.SprintP1), "Sprint Winner is required.");
                    if (prediction.SprintFL is null)
                        ModelState.AddModelError(nameof(prediction.SprintFL), "Sprint Fastest Lap is required.");
                }
            }
        }

        private void ValidateQualificationPrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var qualification = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Qualification);
            if (qualification is not null)
            {
                if (qualification.Start < now && prediction.RacePP != existingPrediction?.RacePP)
                    ModelState.AddModelError(nameof(prediction.RacePP), "Cannot change the Pole Position after the Qualification has started.");
                if (qualification.Start > now && prediction.RacePP is null)
                    ModelState.AddModelError(nameof(prediction.RacePP), "Race Pole Position is required.");
            }
        }

        private void ValidateRacePrediction(EventPrediction2024 prediction, Event @event, EventPrediction2024? existingPrediction, DateTime now)
        {
            var race = @event.Sessions.FirstOrDefault(s => s.Type == SessionType.Race);
            if (race is not null)
            {
                if (race.Start < now)
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
                else if (race.Start > now)
                {
                    var uniqueResults = new Dictionary<Guid, List<string>>();

                    ValidateRacePosition(uniqueResults, prediction.RaceP1, nameof(prediction.RaceP1));
                    ValidateRacePosition(uniqueResults, prediction.RaceP2, nameof(prediction.RaceP2));
                    ValidateRacePosition(uniqueResults, prediction.RaceP3, nameof(prediction.RaceP3));
                    ValidateRacePosition(uniqueResults, prediction.RaceP4, nameof(prediction.RaceP4));
                    ValidateRacePosition(uniqueResults, prediction.RaceP5, nameof(prediction.RaceP5));
                    ValidateRacePosition(uniqueResults, prediction.RaceP6, nameof(prediction.RaceP6));
                    ValidateRacePosition(uniqueResults, prediction.RaceP7, nameof(prediction.RaceP7));
                    ValidateRacePosition(uniqueResults, prediction.RaceP8, nameof(prediction.RaceP8));
                    ValidateRacePosition(uniqueResults, prediction.RaceP9, nameof(prediction.RaceP9));
                    ValidateRacePosition(uniqueResults, prediction.RaceP10, nameof(prediction.RaceP10));

                    if (prediction.RaceFL is null)
                        ModelState.AddModelError(nameof(prediction.RaceFL), "Race Fastest Lap is required.");
                }
            }
        }

        private void ValidateRacePosition(Dictionary<Guid, List<string>> predictions, Guid? driverGuid, string position)
        {
            var positionFormatted = position.Replace("Race", "Race ");
            if (driverGuid is null)
            {
                ModelState.AddModelError(position, $"{positionFormatted} is required.");
                return;
            }

            if (predictions.TryGetValue(driverGuid.Value, out List<string>? duplicates))
            {
                if (duplicates.Count == 1)
                    AddUniqueRacePositionModelError(duplicates.First());

                AddUniqueRacePositionModelError(position);

                duplicates.Add(position);
            }
            else
            {
                predictions.Add(driverGuid.Value, [position]);
            }
        }

        private void AddUniqueRacePositionModelError(string position)
        {
            var positionFormatted = position.Replace("Race", "Race ");
            ModelState.AddModelError(position, $"{positionFormatted} has to be unique required.");
        }
    }
}
