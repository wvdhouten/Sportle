using Microsoft.AspNetCore.Identity;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class LeagueDetailsViewModel
    {
        public required League League { get; set; }

        public required List<UserScore> Users { get; set; } = [];
    }
}
