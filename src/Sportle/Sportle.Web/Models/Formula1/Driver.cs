using System.ComponentModel.DataAnnotations.Schema;

namespace Sportle.Web.Models.Formula1
{
    [Table("Drivers")]
    public class Driver
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int? Number { get; set; }

        public string? Team { get; set; }

        public required string Country {  get; set; }
    }
}
