using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportle.Web.Data;
using Sportle.Web.Models;

namespace Sportle.Web.Controllers
{
    public class LeaderboardsController : SportleBaseController
    {
        private readonly SportleDbContext _context;
        private readonly ILogger<LeaderboardsController> _logger;

        public LeaderboardsController(SportleDbContext context, ILogger<LeaderboardsController> logger)
        {
            _context = context;
            _logger = logger;
        }


    }
}
