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
            var gelf = new GelfMessage()
            {
                FullMessage = message,
                Level = (long)level
            };

            Log(gelf);
        }

        public void Log(GelfMessage message) =>
            PostMessageAsync(message, new Uri(_options.Host, "gelf")).ContinueWith(t => { });

        public void Log(string message, Category category, Priority priority)
        {
            Log(message, category.ToLevel());
        }
    }
}