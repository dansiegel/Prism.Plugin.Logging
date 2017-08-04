using System;

namespace Prism.Logging.Syslog
{
    public static class ISyslogLoggerExtensions
    {
        public static void Log(ISyslogLogger logger, object message, Level level = Level.Debug, Facility facility = Facility.Local0) =>
            logger.Log($"{message}", level, facility);

        public static void Log(ISyslogLogger logger, Exception e, Level level = Level.Error, Facility facility = Facility.Local0) =>
            logger.Log(e.ToString(), level, facility);

        public static void Log(ISyslogLogger logger, string format, params object[] args) =>
            logger.Log(string.Format(format, args), Level.Debug, Facility.Local0);

        public static void Log(ISyslogLogger logger, string format, Level level, params object[] args) =>
            logger.Log(string.Format(format, args), level, Facility.Local0);

        public static void Log(ISyslogLogger logger, string format, Level level, Facility facility, params object[] args) =>
            logger.Log(string.Format(format, args), level, facility);
    }
}