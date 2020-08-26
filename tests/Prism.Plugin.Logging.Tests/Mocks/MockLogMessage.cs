using System.Text;
using Prism.Logging.Sockets;

namespace Prism.Plugin.Logging.Mocks
{
    public class MockLogMessage : ILogMessage
    {
        public string Message { get; set; }

        public byte[] GetBytes() => Encoding.Default.GetBytes(Message);
    }
}
