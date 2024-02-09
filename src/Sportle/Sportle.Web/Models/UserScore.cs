using Microsoft.AspNetCore.Identity;

namespace Sportle.Web.Models
{
    public class UserScore
    {
        public required IdentityUser User { get; set; }

        public int Score { get; set; }
    }
}
