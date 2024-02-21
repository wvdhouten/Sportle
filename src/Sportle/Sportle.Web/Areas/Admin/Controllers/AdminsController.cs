using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportle.Web.Areas.Admin.Models;
using Sportle.Web.Data;

namespace Sportle.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        private readonly SportleDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminsController(SportleDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminsViewModel()
            {
                Users = await _context.Users.ToListAsync(),
                Admins = await _userManager.GetUsersInRoleAsync("Admin")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user is not null)
                {
                    switch (model.Action)
                    {
                        case "Add":
                            await _userManager.AddToRoleAsync(user, "Admin");
                            break;
                        case "Remove":
                            if (user.NormalizedUserName?.ToUpperInvariant() == "PATIENT-ZERO")
                                break;

                            await _userManager.RemoveFromRoleAsync(user, "Admin");
                            break;
                    }
                }
            }

            model.Users = await _context.Users.ToListAsync();
            model.Admins = await _userManager.GetUsersInRoleAsync("Admin");

            return View(model);
        }
    }
}
