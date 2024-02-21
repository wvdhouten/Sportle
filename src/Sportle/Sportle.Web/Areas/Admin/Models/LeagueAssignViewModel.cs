using Microsoft.AspNetCore.Identity;
using Sportle.Web.Models.Formula1;
using System.ComponentModel;

namespace Sportle.Web.Areas.Admin.Models
{
    public class LeagueAssignViewModel
    {
        public IList<IdentityUser> Users { get; set; } = [];

        public IList<League> Leagues { get; set; } = [];

        [DisplayName("User")]
        public Guid UserId { get; set; }

        [DisplayName("League")]
        public Guid LeagueId { get; set; }

        public string? Action { get; set; }
    }
}
