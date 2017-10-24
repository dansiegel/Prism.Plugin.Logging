using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Prism.Logging.Sockets
{
    public interface ISocketMessenger
    {
        bool SendMessage(ILogMessage message, string hostOrIp, int port);
        Task<bool> SendMessageAsync(ILogMessage message, string hostOrIp, int port);
        ProtocolType GetProtocolType();
        SocketType GetSocketType();

        IEnumerable<string> Chunkify(string prefix, string message);
    }
}