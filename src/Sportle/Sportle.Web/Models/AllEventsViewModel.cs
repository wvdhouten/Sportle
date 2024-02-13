using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class AllEventsViewModel
    {
        public List<Event> Events { get; set; } = [];

        public List<EventPrediction2024> Predictions { get; set; } = [];

        public League? League { get; set; }
    }
}
