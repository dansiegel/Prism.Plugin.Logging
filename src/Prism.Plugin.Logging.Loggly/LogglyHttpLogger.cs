using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Prism.Logging.Http;
using Prism.Logging.Logger;

namespace Prism.Logging.Loggly
{
    public class LogglyHttpLogger : HttpLogger, ILoggerFacade, ILogger
    {
        protected const string LogglyUriTemplate = "{0}/inputs/{1}/tag/{2}/";

        protected ILogglyOptions _options { get; }

        public LogglyHttpLogger(ILogglyOptions options)
        {
            _options = options;
        }

        public void Log(string message, Category category, Priority priority) => LogAsync(message, category, priority);

        public async Task<bool> LogAsync(string message, Category category, Priority priority)
        {
            try
            {
                var result = await PostMessageAsync(new
                {
                    HostName = Dns.GetHostName(),
                    Priority = priority,
                    Category = category,
                    Message = message
                }, LogglyUri()).ConfigureAwait(continueOnCapturedContext: false);

                return result.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected virtual string LogglyBaseUri =>
            "https://logs-01.loggly.com";

        protected virtual Uri LogglyUri() =>
            new Uri(string.Format(LogglyUriTemplate, LogglyBaseUri, _options.Token, Tags()));

        protected virtual string Tags()
        {
            var tags = new List<string>{
                WebUtility.UrlEncode(_options.AppName)
            };
            foreach (var tag in _options.Tags)
            {
                tags.Add(WebUtility.UrlEncode(tag));
            }
            return string.Join(",", tags);
        }
    }
}