using System.Net.Sockets;

namespace Prism.Logging.Sockets
{
    public class SocketLoggerOptions : ISocketLoggerOptions
    {
        public string HostOrIp { get; set; }
        public int Port { get; set; } = 8080;
        public ProtocolType ProtocolType { get; set; } = ProtocolType.Udp;
    }
}