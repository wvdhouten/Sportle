using System.Globalization;

namespace Sportle.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime WithOffset(this DateTime dateTime, int offsetHours, int offsetMinutes = 0)
        {
            return new DateTimeOffset(dateTime, new TimeSpan(offsetHours, offsetMinutes, 0)).DateTime;
        }
    }
}
