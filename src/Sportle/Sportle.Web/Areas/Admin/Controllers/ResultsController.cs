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


    }
}
