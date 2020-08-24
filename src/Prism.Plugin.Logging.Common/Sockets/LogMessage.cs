using System.Text;

namespace Prism.Logging.Sockets
{
    internal class LogMessage : ILogMessage
    {
        public string Message { get; set; }

        public string MessageType { get; set; }

        public byte[] GetBytes() =>
            Encoding.Default.GetBytes(Message);
    }
}