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
    }
}