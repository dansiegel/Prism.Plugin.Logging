using System;

namespace Prism.Logging.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToJsonIso8601(this DateTime date) =>
            date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.ffffffZ");

        public static string ToSyslog(this DateTime date) =>
            date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.ffffffzzz");

        public static string ToSyslog(this DateTimeOffset date) =>
            date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.ffffffzzz");

        public static double ToUnixTimestamp(this DateTimeOffset d)
        {
            var duration = d.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromSeconds(0));

            return duration.TotalSeconds;
        }

        public static DateTimeOffset FromUnixTimestamp(this double d)
        {
            var datetime = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(d * 1000).ToLocalTime();

            return datetime;
        }
    }
}