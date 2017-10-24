using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Prism.Logging.Http;

namespace Prism.Logging.Loggly
{
    public class LogglyHttpLogger : HttpLogger, ILoggerFacade
    {
        protected const string LogglyUriTemplate = "{0}/inputs/{1}/tag/{2}/";

        protected ILogglyOptions _options { get; }

        public LogglyHttpLogger(ILogglyOptions options)
        {
            _options = options;
        }

        protected override async Task<bool> LogAsync(string message, Category category, Priority priority)
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

        protected virtual string LogglyBaseUri =>
            "https://logs-01.loggly.com";

        protected virtual Uri LogglyUri() =>
            new Uri(string.Format(LogglyUriTemplate, LogglyBaseUri, _options.Token, Tags()));

        protected virtual string Tags()
        {
            var encoder = UrlEncoder.Default;
            var tags = new List<string>{
                encoder.Encode(_options.AppName)
            };
            foreach(var tag in _options.Tags)
            {
                tags.Add(encoder.Encode(tag));
            }
            return string.Join(",", tags);
        }
    }
}