namespace Sportle.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime WithOffset(this DateTime dateTime, int offsetHours)
        {
            return new DateTimeOffset(dateTime, new TimeSpan(offsetHours, 0, 0)).DateTime;
        }
    }
}
