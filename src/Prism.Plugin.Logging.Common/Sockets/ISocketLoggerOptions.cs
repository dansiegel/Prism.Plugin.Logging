using System.Net.Sockets;

namespace Prism.Logging.Sockets
{
    public interface ISocketLoggerOptions
    {
        string HostOrIp { get; }

        int Port { get; }

        ProtocolType ProtocolType { get; }
    }
}