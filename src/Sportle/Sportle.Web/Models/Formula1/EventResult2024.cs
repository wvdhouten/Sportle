using System.ComponentModel;

namespace Sportle.Web.Models.Formula1
{
    public class EventResult2024
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public DateTime ModifiedOn { get; set; }

        [DisplayName("Pole Position")]
        public Guid? SprintPP { get; set; }

        [DisplayName("Winner")]
        public Guid? SprintP1 { get; set; }

        [DisplayName("Fastest Lap")]
        public Guid? SprintFL { get; set; }

        [DisplayName("Pole Position")]
        public Guid? RacePP { get; set; }

        [DisplayName("P1")]
        public Guid? RaceP1 { get; set; }

        [DisplayName("P2")]
        public Guid? RaceP2 { get; set; }

        [DisplayName("P3")]
        public Guid? RaceP3 { get; set; }

        [DisplayName("P4")]
        public Guid? RaceP4 { get; set; }

        [DisplayName("P5")]
        public Guid? RaceP5 { get; set; }

        [DisplayName("P6")]
        public Guid? RaceP6 { get; set; }

        [DisplayName("P7")]
        public Guid? RaceP7 { get; set; }

        [DisplayName("P8")]
        public Guid? RaceP8 { get; set; }

        [DisplayName("P9")]
        public Guid? RaceP9 { get; set; }

        [DisplayName("P10")]
        public Guid? RaceP10 { get; set; }

        [DisplayName("Fastest Lap")]
        public Guid? RaceFL { get; set; }
    }
}