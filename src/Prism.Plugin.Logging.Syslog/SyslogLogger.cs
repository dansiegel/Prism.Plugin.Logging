using Prism.Logging.Extensions;
using Prism.Logging.Syslog.Extensions;
using System.Collections.Generic;
using Prism.Logging.Sockets;
using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace Prism.Logging.Syslog
{
    public class SyslogLogger : SocketMessenger, ILoggerFacade, ISyslogLogger
    {
        public SyslogLogger(ISyslogOptions options)
        {
            HostNameOrIp = ValueOrDefault(options?.HostNameOrIp, "localhost");
            AppNameOrTag = ValueOrDefault(options?.AppNameOrTag, "PrismApp");
            Port = options?.Port ?? 514;
        }

        protected string HostNameOrIp { get; }

        protected string AppNameOrTag { get; }

        protected int Port { get; }

        private string LocalHostName { get; set; }

        public virtual void Log(string message, Category category, Priority priority) =>
            Log(message, category.ToLevel());

        public virtual void Log(string message, Level level, Facility facility = Facility.Local0)
        {
            var syslogMessage = GetSyslogMessage(null, level, facility);

            // Ensure message is split into manageable chunks
            foreach(string chunk in Chunkify(syslogMessage, message))
            {
                syslogMessage.Text = message;
                SendMessage(syslogMessage);
            }
        }

        protected virtual SyslogMessage GetSyslogMessage(string message, Level level, Facility facility) =>
            new SyslogMessage(facility, level, message)
            {
                AppName = AppNameOrTag
            };

        private string GetLocalIP()
        {
            if(string.IsNullOrEmpty(LocalHostName))
            {
                var addressTask = Dns.GetHostAddressesAsync(Dns.GetHostName());
                addressTask.Wait();

                foreach(var ip in addressTask.Result)
                {
                    if(ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        LocalHostName = ip.ToString();
                    }
                }
            }

            return LocalHostName;
        }

        private string ValueOrDefault(string value, string defaultValue)
        {
            if(string.IsNullOrEmpty(value))
                return defaultValue;

            return value;
        }

        protected IEnumerable<string> Chunkify(SyslogMessage baseMessage, string text) =>
            Chunkify(baseMessage.ToString(), text);

        protected bool SendMessage(SyslogMessage message) => 
            SendMessage(message, HostNameOrIp, Port);
    }
}