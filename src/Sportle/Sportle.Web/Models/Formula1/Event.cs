using System.ComponentModel.DataAnnotations.Schema;

namespace Sportle.Web.Models.Formula1
{
    [Table("Events")]
    public class Event
    {
        public Guid Id { get; set; }

        public Season Season { get; set; }

        public Venue Venue { get; set; }

        public List<Session> Sessions { get; set; } = [];
    }
}
