using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Extensions
{
    public static class EventExtensions
    {
        public static string GetDateRange(this Event @event, string format = "MMM d")
        {
            var orderedSessions = @event.Sessions.OrderBy(s => s.Start);
            var firstSession = orderedSessions.First();
            var lastSession = orderedSessions.Last();

            if (firstSession == lastSession)
                return firstSession.Start.ToString(format);

            return $"{firstSession.Start.ToString(format)} - {lastSession.Start.ToString(format)}";
        }
    }
}
