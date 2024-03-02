using Microsoft.AspNetCore.Identity;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class ResultViewModel
    {
        public required Event Event { get; set; }

        public required EventResult2024 Result { get; set; }

        public EventPrediction2024? OwnPrediction { get; set; }

        public IdentityUser? CompareUser { get; set; }

        public EventPrediction2024? ComparePrediction { get; set; }

        public Dictionary<Guid, string> Drivers { get; set; } = [];

        public List<Guid> Top10 { get; set; } = [];

        public bool IsInTop10(Guid? guid)
        {
            if (guid is null)
                return false;

            return Top10.Contains(guid.Value);
        }

        public string GetDriverName(Guid? guid)
        {
            if (guid is null)
                return string.Empty;

            if (!Drivers.TryGetValue(guid.Value, out var result))
                return string.Empty;

            return result ?? string.Empty;
        }
    }
}
