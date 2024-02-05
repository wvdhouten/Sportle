using System.ComponentModel.DataAnnotations.Schema;

namespace Sportle.Web.Models.Formula1
{
    [Table("Seasons")]
    public class Season
    {
        public Guid Id { get; set; }

        public int Year { get; set; }

        public List<Event> Events { get; set; } = [];

        public List<Driver> Drivers { get; set; } = [];
    }
}
