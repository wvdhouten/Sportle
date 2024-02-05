using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportle.Web.Data;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Controllers
{
    [Route("[Controller]")]
    public class PredictionsController : SportleBaseController
    {
        private UserManager<IdentityUser> _userManager;
        private SportleDbContext _context;
        private ILogger<PredictionsController> _logger;

        public PredictionsController(UserManager<IdentityUser> userManager, SportleDbContext context, ILogger<PredictionsController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [Route("{eventId}")]
        public async Task<IActionResult> Index(Guid eventId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();

            var userId = Guid.Parse(user.Id);
            var model = await _context.Predictions2024.FindAsync(eventId, userId) ?? new EventPrediction2024 { UserId = userId, EventId = eventId };

            return View(model);
        }
    }
}
