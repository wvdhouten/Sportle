using System.ComponentModel.DataAnnotations.Schema;

namespace Sportle.Web.Models.Formula1
{
    [Table("Venues")]
    public class Venue
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Country { get; set; }
    }
}
