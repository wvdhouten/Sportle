using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
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

        // GET: Admin/Results
        public async Task<IActionResult> Index()
        {
            return View(await _context.Results2024.ToListAsync());
        }

        // GET: Admin/Results/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventResult2024 = await _context.Results2024.FirstOrDefaultAsync(m => m.Id == id);
            if (eventResult2024 == null)
            {
                return NotFound();
            }

            return View(eventResult2024);
        }

        // GET: Admin/Results/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,PredictedOn,SprintPP,SprintP1,SprintFL,RacePP,RaceP1,RaceP2,RaceP3,RaceP4,RaceP5,RaceP6,RaceP7,RaceP8,RaceP9,RaceP10,RaceFL")] EventResult2024 eventResult2024)
        {
            if (ModelState.IsValid)
            {
                eventResult2024.Id = Guid.NewGuid();
                _context.Add(eventResult2024);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventResult2024);
        }

        // GET: Admin/Results/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventResult2024 = await _context.Results2024.FindAsync(id);
            if (eventResult2024 == null)
            {
                return NotFound();
            }
            return View(eventResult2024);
        }

        // POST: Admin/Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/Results/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventResult2024 = await _context.Results2024
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventResult2024 == null)
            {
                return NotFound();
            }

            return View(eventResult2024);
        }

        // POST: Admin/Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var eventResult2024 = await _context.Results2024.FindAsync(id);
            if (eventResult2024 != null)
            {
                _context.Results2024.Remove(eventResult2024);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventResult2024Exists(Guid id)
        {
            return _context.Results2024.Any(e => e.Id == id);
        }
    }
}
