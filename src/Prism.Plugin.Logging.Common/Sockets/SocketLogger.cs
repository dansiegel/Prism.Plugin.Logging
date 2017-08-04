using System.Net.Sockets;
using System.Text;

namespace Prism.Logging.Sockets
{
    public class SocketLogger : SocketMessenger, ILoggerFacade
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
                SendMessage($"{prefix}{message}", _options.HostOrIp, _options.Port);
            }
        }

        public override ProtocolType GetProtocolType() =>
            _options.ProtocolType;

        private class BasicMessage : ILogMessage
        {
            public string Message { get; set; }

            public byte[] GetBytes() =>
                Encoding.ASCII.GetBytes(Message);
        }
    }
}