using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Prism.Logging.Sockets;

namespace Prism.Logging.Syslog
{
    public class SyslogLogger : SocketMessenger, ISyslogLogger
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

        public virtual void Log(string message, Level level, Facility facility = Facility.Local0)
        {
            var syslogMessage = GetSyslogMessage(null, level, facility);

            // TODO: Fix the Chunkify implementation
            // Ensure message is split into manageable chunks
            //foreach(string chunk in Chunkify(syslogMessage, message))
            {
                syslogMessage.Text = message;
                SendMessage(syslogMessage);
            }
        }

        protected virtual SyslogMessage GetSyslogMessage(string message, Level level, Facility facility) =>
            new SyslogMessage(facility, level, message) {
                AppName = AppNameOrTag
            };

        protected virtual string GetLocalIP()
        {
            if (string.IsNullOrEmpty(LocalHostName))
            {
                var addressTask = Dns.GetHostAddressesAsync(Dns.GetHostName());
                addressTask.Wait();

                foreach (var ip in addressTask.Result)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        LocalHostName = ip.ToString();
                    }
                }
            }

            return LocalHostName;
        }

        private string ValueOrDefault(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return value;
        }

        protected IEnumerable<string> Chunkify(SyslogMessage baseMessage, string text) =>
            Chunkify(baseMessage.ToString(), text);

        protected bool SendMessage(SyslogMessage message) =>
            SendMessage(message, HostNameOrIp, Port);

        public virtual void Log(string message, IDictionary<string, string> properties)
        {
            if (properties is null) properties = new Dictionary<string, string>();

            if (properties.ContainsKey(nameof(Level)) && Enum.TryParse(properties[nameof(Level)], out Level level))
            {
                properties.Remove(nameof(Level));
            }
            else if (properties.ContainsKey("Category") && Enum.TryParse(properties["Category"], out level))
            {
                properties.Remove("Category");
            }
            else
            {
                level = Level.Debug;
            }

            if (properties.ContainsKey(nameof(Facility)) && Enum.TryParse(properties[nameof(Facility)], out Facility facility))
            {
                properties.Remove(nameof(Facility));
            }
            else
            {
                facility = Facility.Daemon;
            }

            var props = properties?.Select(prop => $"\n    {prop.Key} - {prop.Value}") ?? Array.Empty<string>();

            var syslog = new SyslogMessage(facility, level, $"{message}\nProperties{string.Join("", props)}") {
                AppName = AppNameOrTag
            };
            SendMessage(syslog);
        }

        public virtual void Report(Exception ex, IDictionary<string, string> properties)
        {
            if (properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            if (!properties.ContainsKey("Category") && !properties.ContainsKey(nameof(Level)))
            {
                properties.Add(nameof(Level), $"{Level.Error}");
            }

            Log(ex.ToString(), properties);
        }

        public virtual void TrackEvent(string name, IDictionary<string, string> properties)
        {
            if (properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            if (!properties.ContainsKey("Category") && !properties.ContainsKey(nameof(Level)))
            {
                properties.Add(nameof(Level), $"{Level.Information}");
            }

            Log(name, properties);
        }
    }
}