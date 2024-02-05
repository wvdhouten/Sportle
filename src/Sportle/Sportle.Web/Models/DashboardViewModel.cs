using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Models
{
    public class DashboardViewModel
    {
        public Event? NextEvent { get; set; }

        public List<Event> Events { get; set; } = [];
    }
}
