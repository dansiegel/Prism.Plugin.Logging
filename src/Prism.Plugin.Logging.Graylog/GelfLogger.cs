using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Prism.Logging.Graylog.Extensions;
using Prism.Logging.Http;
using Prism.Logging.Sockets;

namespace Prism.Logging.Graylog
{
    public class GelfLogger : HttpLogger, IGelfLogger, ILoggerFacade
    {
        protected IGelfOptions _options { get; }

        public GelfLogger(IGelfOptions options)
        {
            _options = options;
        }

        public void Log(string message, Level level = Level.Debug)
        {
            LogAsync(message, level);
        }

        public void Log(GelfMessage message) =>
            LogAsync(message);

        public async Task<bool> LogAsync(string message, Level level = Level.Debug)
        {
            var gelf = new GelfMessage()
            {
                FullMessage = message,
                Level = (long)level
            };

            var result= await LogAsync(gelf).ConfigureAwait(continueOnCapturedContext: false);

            return result;
        }

        public async Task<bool> LogAsync(GelfMessage message)
        {
            var result=await PostMessageAsync(message, new Uri(_options.Host, "gelf"))
                .ConfigureAwait(continueOnCapturedContext: false);
            return result.IsSuccessStatusCode;
        }

        protected override async Task<bool> LogAsync(string message, Category category, Priority priority)
        {
            var result=await LogAsync(message, category.ToLevel()).ConfigureAwait(continueOnCapturedContext:false);

            return result;
        }
    }
}