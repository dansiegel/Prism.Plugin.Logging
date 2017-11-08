using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging.Logger;

namespace Prism.Logging.Sockets
{
    public class SocketLogger : SocketMessenger, ILoggerFacade, ILogger
    {
        private ISocketLoggerOptions _options { get; }

        public SocketLogger(ISocketLoggerOptions options)
        {
            _options = options;
        }

        public void Log(string message, Category category, Priority priority) => LogAsync(message, category, priority);

        public async Task<bool> LogAsync(string message, Category category, Priority priority)
        {
            var prefix = $"{category} {priority}: ";

            // Ensure message is split into manageable chunks
            bool isSuccess = true;
            foreach (string chunk in Chunkify(prefix, message))
            {
                var result=await SendMessageAsync($"{prefix}{message}").ConfigureAwait(continueOnCapturedContext: false);
                if (!result)
                    isSuccess = false;
            }

            return isSuccess;
        }

        public override ProtocolType GetProtocolType() =>
            _options.ProtocolType;

        private void SendMessage(string message) =>
            SendMessage(new BasicMessage { Message = message }, _options.HostOrIp, _options.Port);

        private async Task<bool> SendMessageAsync(string message)
        {
            var result = await SendMessageAsync(new BasicMessage {Message = message}, _options.HostOrIp, _options.Port)
                .ConfigureAwait(continueOnCapturedContext: false);

            return result;
        }

        private class BasicMessage : ILogMessage
        {
            public string Message { get; set; }

            public byte[] GetBytes() =>
                Encoding.ASCII.GetBytes(Message);
        }
    }
}