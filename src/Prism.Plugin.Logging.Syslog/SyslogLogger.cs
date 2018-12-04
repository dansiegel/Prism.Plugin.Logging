using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Prism.Logging.Sockets;
using Prism.Logging.Syslog.Extensions;

namespace Prism.Logging.Syslog
{
    public class SyslogLogger : SocketMessenger, ILogger, ISyslogLogger
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

        protected virtual string GetLocalIP()
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

        public virtual void Log(string message, IDictionary<string, string> properties)
        {
            var level = Level.Debug;
            var facility = Facility.Daemon;

            if (properties is null) properties = new Dictionary<string, string>();

            if(properties.ContainsKey(nameof(Category)) && Enum.TryParse(properties[nameof(Category)], out Category category))
            {
                level = category.ToLevel();
            }

            if(properties.ContainsKey(nameof(Facility)) && Enum.TryParse(properties[nameof(Facility)], out Facility fac))
            {
                facility = fac;
            }

            message += "\nProperties";
            foreach(var prop in properties.Where(x => x.Key != nameof(Category) && x.Key != nameof(Facility)))
            {
                message += $"\n    {prop.Key} - {prop.Value}";
            }

            var syslog = new SyslogMessage(facility, level, message)
            {
                AppName = AppNameOrTag
            };
            SendMessage(syslog);
        }

        public virtual void Report(Exception ex, IDictionary<string, string> properties) => 
            Log(ex.ToString(), properties);

        public virtual void TrackEvent(string name, IDictionary<string, string> properties) =>
            Log(name, properties);
    }
}