using Microsoft.AspNetCore.Identity;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models.Email
{
    public class PredictionReminder
    {
        public IdentityUser? User { get; set; }

        public Event? Event { get; set; }
    }
}
