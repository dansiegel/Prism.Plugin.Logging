using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Prism.Logging.Http;

namespace Prism.Logging.Loggly
{
    public class LogglyHttpLogger : ILoggerFacade
    {
        protected const string LogglyUriTemplate = "{0}/{1}/tag/{2}/";

        protected ILogglyOptions _options { get; }

        public LogglyHttpLogger(ILogglyOptions options)
        {
            _options = options;
        }

        public void Log(string message, Category category, Priority priority)
        {
            SendMessage(new
            {
                HostName = Dns.GetHostName(),
                Priority = priority,
                Category = category,
                Message = message
            }).ContinueWith(t => { });
        }

        protected virtual string LogglyBaseUri =>
            "https://logs-01.loggly.com";

        protected virtual string LogglyUri() =>
            string.Format(LogglyUriTemplate, LogglyBaseUri, _options.Token, Tags());

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

        protected Task SendMessage(object payload)
        {
            using(var client = new HttpClient())
            {
                return client.PostAsync(LogglyUri(), new JsonContent(payload));
            }
        }
    }
}