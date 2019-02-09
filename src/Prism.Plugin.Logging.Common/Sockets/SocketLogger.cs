using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Prism.Logging.Sockets
{
    public class SocketLogger : SocketMessenger, ILogger
    {
        private ISocketLoggerOptions _options { get; }

        public SocketLogger(ISocketLoggerOptions options)
        {
            _options = options;
        }

        public void Log(string message, Category category, Priority priority)
        {
            var prefix = $"{category} {priority}: ";

            // Ensure message is split into manageable chunks
            foreach(string chunk in Chunkify(prefix, message))
            {
                SendMessage($"{prefix}{message}");
            }
        }

        public override ProtocolType GetProtocolType() =>
            _options.ProtocolType;

        private void SendMessage(string message) =>
            SendMessage(new BasicMessage { Message = message }, _options.HostOrIp, _options.Port);

        public void Log(string message, IDictionary<string, string> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine(message);
            AppendProperties(builder, properties);
            SendMessage(builder.ToString());
        }

        public void TrackEvent(string name, IDictionary<string, string> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Tracked Event: {name}");
            AppendProperties(builder, properties);
            SendMessage(builder.ToString());
        }

        public void Report(Exception ex, IDictionary<string, string> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Reported Exception:");
            builder.AppendLine(ex.ToString());
            AppendProperties(builder, properties);
            SendMessage(builder.ToString());
        }

        private void AppendProperties(StringBuilder builder, IDictionary<string, string> properties)
        {
            if (properties != null || !properties.Any()) return;

            builder.AppendLine("  Properties:");
            foreach(var prop in properties)
            {
                builder.AppendLine($"    {prop.Key}: {prop.Value}");
            }
        }

        private class BasicMessage : ILogMessage
        {
            public string Message { get; set; }

            public byte[] GetBytes() =>
                Encoding.ASCII.GetBytes(Message);
        }
    }
}