using Prism.Logging.Syslog;

namespace Prism.Logging.Loggly
{
    public class LogglySyslogLogger : SyslogLogger
    {
        protected ILogglyOptions _options;

        public LogglySyslogLogger(ILogglyOptions options)
            : base(LogglySyslogOptions.Create(options))
        {
            _options = options;
        }

        protected override SyslogMessage GetSyslogMessage(string message, Level level, Facility facility) =>
            new LogglySyslogMessage(facility, level, message) {
                AppName = _options.AppName,
                Token = _options.Token
            };
    }
}