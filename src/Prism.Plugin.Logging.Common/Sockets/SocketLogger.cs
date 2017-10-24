using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging.Logger;

namespace Prism.Logging.Sockets
{
    public class SocketLogger : CommonLogger, ILoggerFacade
    {
        private readonly ISocketMessenger _messenger;

        private ISocketLoggerOptions _options { get; }

        public SocketLogger(ISocketLoggerOptions options)
        {
            _options = options;
            _messenger = new SocketLoggerMessenger(_options);
        }

        protected override async Task<bool> LogAsync(string message, Category category, Priority priority)
        {
            var prefix = $"{category} {priority}: ";

            // Ensure message is split into manageable chunks
            bool isSuccess = true;
            foreach (string chunk in _messenger.Chunkify(prefix, message))
            {
                var result=await SendMessageAsync($"{prefix}{message}").ConfigureAwait(continueOnCapturedContext: false);
                if (!result)
                    isSuccess = false;
            }

            return isSuccess;
        }

        private void SendMessage(string message) =>
            _messenger.SendMessage(new BasicMessage { Message = message }, _options.HostOrIp, _options.Port);

        private async Task<bool> SendMessageAsync(string message)
        {
            var result = await _messenger.SendMessageAsync(new BasicMessage {Message = message}, _options.HostOrIp, _options.Port)
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