using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class ResultViewModel
    {
        public required Event Event { get; set; }

        public required EventResult2024 Result { get; set; }

        public EventPrediction2024? Prediction { get; set; }

        public Dictionary<Guid, string> Drivers { get; set; } = [];

        public List<Guid> Top10 { get; set; } = [];

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
