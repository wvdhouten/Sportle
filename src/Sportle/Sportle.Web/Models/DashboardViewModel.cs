using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class DashboardViewModel
    {
        public Event? PrevEvent { get; set; }

        public EventPrediction2024? PrevPrediction { get; set; }

        public Event? NextEvent { get; set; }

        public EventPrediction2024? NextPrediction { get; set; }
    }
}
