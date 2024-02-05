using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Sportle.Web.Models.Formula1
{
    [PrimaryKey(nameof(User), nameof(Event))]
    public class EventPrediction2024
    {
        public required IdentityUser User { get; set; }

        public required Event Event { get; set; }

        public DateTime PredictedOn { get; set; }

        public Guid? SprintPP { get; set; }

        public Guid? SprintP1 { get; set; }

        public Guid? SprintFL { get; set; }

        public Guid? RacePP { get; set; }

        public Guid? RaceP1 { get; set; }

        public Guid? RaceP2 { get; set; }

        public Guid? RaceP3 { get; set; }

        public Guid? RaceP4 { get; set; }

        public Guid? RaceP5 { get; set; }

        public Guid? RaceP6 { get; set; }

        public Guid? RaceP7 { get; set; }

        public Guid? RaceP8 { get; set; }

        public Guid? RaceP9 { get; set; }

        public Guid? RaceP10 { get; set; }

        public Guid? RaceFL { get; set; }

        public int Points { get; set; }

        public int EarlyBonus { get; set; }

        public int SprintBonus { get; set; }

        public int PositionBonus { get; set; }

        public int PodiumBonus { get; set; }
    }
}
