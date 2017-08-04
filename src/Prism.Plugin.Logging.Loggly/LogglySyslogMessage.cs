using System.Net;
using Prism.Logging.Extensions;
using Prism.Logging.Syslog;

namespace Prism.Logging.Loggly
{
    public sealed class LogglySyslogMessage : SyslogMessage
    {
        public LogglySyslogMessage(Facility facility, Level level, string text)
            : base(facility, level, text)
        {
        }

        public string Token { get; set; }

        public override string ToString()
        {
            return $"<{Priority}>1 {Timestamp.ToSyslog()} {Dns.GetHostName()} {AppName} {Token} {MessageId} {Text}";
        }
    }
}