using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Sportle.Web.Areas.Admin.Models
{
    public class AdminsViewModel
    {
        public IList<IdentityUser> Users { get; set; } = [];

        public IList<IdentityUser> Admins { get; set; } = [];

        [DisplayName("User")]
        public Guid UserId { get; set; }

        public string? Action { get; set; }
    }
}
