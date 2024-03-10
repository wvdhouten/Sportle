using System.ComponentModel.DataAnnotations.Schema;

namespace Sportle.Web.Models.Formula1
{
    [Table("Sessions")]
    public class Session
    {
        public Guid Id { get; set; }

        public SessionType Type { get; set; }

        public DateTime Start { get; set; }

        public double TimeZoneOffset { get; set; }

        public string FormattedStart()
        {
            return $"{Start:yyyy-MM-dd HH:mm} ({(TimeZoneOffset < 0 ? "" : "+")}{TimeZoneOffset})";
        }
    }
}
