using System;
using System.Collections.Generic;
using System.Net;
using Prism.Logging.Http;

namespace Prism.Logging.Loggly
{
    public class LogglyHttpLogger : HttpLogger, ILogger
    {
        protected const string LogglyUriTemplate = "{0}/inputs/{1}/tag/{2}/";

        protected ILogglyOptions _options { get; }

        public LogglyHttpLogger(ILogglyOptions options)
        {
            _options = options;
        }

        public void Log(string message, Category category, Priority priority)
        {
            PostMessageAsync(new
            {
                HostName = Dns.GetHostName(),
                Priority = priority,
                Category = category,
                Message = message
            }, LogglyUri(Tags())).ContinueWith(t => { });
        }

        protected virtual string LogglyBaseUri =>
            "https://logs-01.loggly.com";

        protected virtual Uri LogglyUri(string tags) =>
            new Uri(string.Format(LogglyUriTemplate, LogglyBaseUri, _options.Token, tags));

        protected IList<string> DefaultTags() =>
            new List<string>(_options.Tags)
            {
                _options.AppName
            };

        protected virtual string Tags() => 
            Tags(DefaultTags());

        protected virtual string Tags(IList<string> tags)
        {
            for(var i = 0; i < tags.Count; i++)
            {
                tags[i] = WebUtility.UrlEncode(tags[i]);
            }

            return string.Join(",", tags);
        }

        public void Log(string message, IDictionary<string, string> properties)
        {
            if(properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("HostName", Dns.GetHostName());
            properties.Add("Message", message);

            PostMessageAsync(properties, LogglyUri(Tags())).ContinueWith(t => { });
        }

        public void Report(Exception ex, IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("HostName", Dns.GetHostName());
            properties.Add("Type", ex.GetType().FullName);
            properties.Add("Message", ex.Message);
            properties.Add("StackTrace", ex.StackTrace);

            PostMessageAsync(properties, LogglyUri(Tags())).ContinueWith(t => { });
        }
    }
}