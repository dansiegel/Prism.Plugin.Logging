using System;
using Prism.Logging.Syslog;

namespace Prism.Logging.Loggly
{
    internal class LogglySyslogOptions : ISyslogOptions
    {
        public string HostNameOrIp => "logs-01.loggly.com";

        public int? Port => 514;

        public string AppNameOrTag { get; private set; }

        public static ISyslogOptions Create(ILogglyOptions options) =>
            new LogglySyslogOptions() {
                AppNameOrTag = options.AppName
            };
    }
}