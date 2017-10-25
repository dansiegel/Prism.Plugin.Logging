using Prism.Logging.Extensions;
using Prism.Logging.Syslog.Extensions;
using System.Collections.Generic;
using Prism.Logging.Sockets;
using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Prism.Logging.Logger;

namespace Prism.Logging.Syslog
{
    public class SyslogLogger : SocketMessenger, ILoggerFacade, ISyslogLogger, ILogger
    {

        public SyslogLogger(ISyslogOptions options)
        {
            HostNameOrIp = ValueOrDefault(options?.HostNameOrIp, "localhost");
            AppNameOrTag = ValueOrDefault(options?.AppNameOrTag, "PrismApp");
            Port = options?.Port ?? 514;
        }

        protected string HostNameOrIp { get; set; }

        protected string AppNameOrTag { get; }

        protected int Port { get; set; }

        private string LocalHostName { get; set; }

        public virtual void Log(string message, Category category, Priority priority) =>
            LogAsync(message, category, priority);

        public async Task<bool> LogAsync(string message, Category category, Priority priority)
        {
            var level = category.ToLevel();
            var facility = Facility.Local0;

            var syslogMessage = GetSyslogMessage(null, level, facility);

            // Ensure message is split into manageable chunks
            bool isSuccess = true;
            foreach (string chunk in Chunkify(syslogMessage, message))
            {
                syslogMessage.Text = message;
                var result = await SendMessageAsync(syslogMessage).ConfigureAwait(continueOnCapturedContext: false);
                if (!result)
                    isSuccess = false;
            }

            return isSuccess;
        }

        public virtual void Log(string message, Level level, Facility facility = Facility.Local0)
        {
            var syslogMessage = GetSyslogMessage(null, level, facility);

            // Ensure message is split into manageable chunks
            foreach(string chunk in Chunkify(syslogMessage, message))
            {
                syslogMessage.Text = message;
                SendMessageAsync(syslogMessage);
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

        protected async Task<bool> SendMessageAsync(SyslogMessage message)
        {
            var result = await SendMessageAsync(message, HostNameOrIp, Port)
                .ConfigureAwait(continueOnCapturedContext: false);
            return result;
        } 
    }
}