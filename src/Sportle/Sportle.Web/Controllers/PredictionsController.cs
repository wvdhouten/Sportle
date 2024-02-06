﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Models.Formula1;

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

        // GET: Predictions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Predictions2024.ToListAsync());
        }

        // GET: Predictions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPrediction2024 = await _context.Predictions2024
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (eventPrediction2024 == null)
            {
                return NotFound();
            }

            return View(eventPrediction2024);
        }

        // GET: Predictions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EventId,PredictedOn,SprintPP,SprintP1,SprintFL,RacePP,RaceP1,RaceP2,RaceP3,RaceP4,RaceP5,RaceP6,RaceP7,RaceP8,RaceP9,RaceP10,RaceFL,Points,EarlyBonus,SprintBonus,PositionBonus,PodiumBonus")] EventPrediction2024 eventPrediction2024)
        {
            if (ModelState.IsValid)
            {
                eventPrediction2024.UserId = Guid.NewGuid();
                _context.Add(eventPrediction2024);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventPrediction2024);
        }

        // GET: Predictions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPrediction2024 = await _context.Predictions2024.FindAsync(id);
            if (eventPrediction2024 == null)
            {
                return NotFound();
            }
            return View(eventPrediction2024);
        }

        // POST: Predictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,EventId,PredictedOn,SprintPP,SprintP1,SprintFL,RacePP,RaceP1,RaceP2,RaceP3,RaceP4,RaceP5,RaceP6,RaceP7,RaceP8,RaceP9,RaceP10,RaceFL,Points,EarlyBonus,SprintBonus,PositionBonus,PodiumBonus")] EventPrediction2024 eventPrediction2024)
        {
            if (id != eventPrediction2024.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventPrediction2024);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventPrediction2024Exists(eventPrediction2024.UserId))
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
            return View(eventPrediction2024);
        }

        // GET: Predictions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPrediction2024 = await _context.Predictions2024
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (eventPrediction2024 == null)
            {
                return NotFound();
            }

            return View(eventPrediction2024);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var eventPrediction2024 = await _context.Predictions2024.FindAsync(id);
            if (eventPrediction2024 != null)
            {
                _context.Predictions2024.Remove(eventPrediction2024);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventPrediction2024Exists(Guid id)
        {
            return _context.Predictions2024.Any(e => e.UserId == id);
        }
    }
}
