using Sportle.Web.Data;

namespace Sportle.Web.Controllers
{
    public class LeaguesController
    {
        private readonly SportleDbContext _context;
        private readonly ILogger<LeaguesController> _logger;

        public LeaguesController(SportleDbContext context, ILogger<LeaguesController> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
