using Microsoft.AspNetCore.Identity;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class UserScoreViewModel
    {
        public required IdentityUser User { get; set; }

        public List<Event> Events { get; set; } = [];

        public List<EventPrediction2024> Predictions { get; set; } = [];
    }
}
