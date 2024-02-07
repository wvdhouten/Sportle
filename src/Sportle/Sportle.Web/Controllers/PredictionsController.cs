using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Models;
using Sportle.Web.Models.Formula1;
using System.Security.Claims;

namespace Sportle.Web.Controllers
{
    [Authorize]
    [Route("Predictions")]
    public class PredictionsController : Controller
    {
        private readonly SportleDbContext _context;

        public PredictionsController(SportleDbContext context)
        {
            _context = context;
        }

        [Route("{eventId}")]
        public async Task<IActionResult> Index(Guid? eventId)
        {
            if (eventId is null)
                return NotFound();

            var @event = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (@event is null)
                return NotFound();

            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return NotFound();

            var prediction = await _context.Predictions2024.FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId) ?? new EventPrediction2024 { EventId = eventId.Value, UserId = userId };
            if (prediction == null)
            {
                return NotFound();
            }

            var drivers = _context.Drivers.ToList();

            ViewData["Event"] = @event;
            ViewData["Drivers"] = drivers;
            var model = new PredictionViewModel { Prediction =  prediction };

            return View(model);
        }

        [HttpPost("{eventId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Guid? eventId, PredictionViewModel model)
        {
            if (eventId is null)
                return NotFound();

            var @event = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (@event is null)
                return NotFound();

            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    model.Prediction.PredictedOn = DateTime.UtcNow;

                    _context.Update(model.Prediction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Predictions2024.Any(p => p.EventId ==eventId && p.UserId == userId))
                    {
                        _context.Add(model.Prediction);
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
    }
}
