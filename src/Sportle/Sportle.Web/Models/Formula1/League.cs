using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportle.Web.Models.Formula1
{
    [Table("Leagues")]
    public class League
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public List<IdentityUser> Users { get; set; } = [];
    }
}
