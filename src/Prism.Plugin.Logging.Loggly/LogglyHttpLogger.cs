using System;
using System.Collections.Generic;
using System.Linq;
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

        protected virtual string LogglyBaseUri =>
            "https://logs-01.loggly.com";

        protected virtual Uri LogglyUri(string tags) =>
            new Uri(string.Format(LogglyUriTemplate, LogglyBaseUri, _options.Token, tags));

        protected IList<string> DefaultTags() =>
            new List<string>(_options.Tags)
            {
                _options.AppName
            };

        protected virtual string Tags(string category)
        {
            var tags = DefaultTags();
            if(!string.IsNullOrWhiteSpace(category))
            {
                tags.Add(category);
            }

            return Tags(tags);
        }

        protected virtual string Tags(IList<string> tags)
        {
            return string.Join(",", tags.Select(x => WebUtility.UrlEncode(x)));
        }

        public virtual void Log(string message, IDictionary<string, string> properties)
        {
            if(properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("Message", message);
            AddDefaultProperties(properties);

            string category = ExtractCategory(properties);

            PostMessageAsync(properties, LogglyUri(Tags(category))).ContinueWith(t => { });
        }

        public virtual void Report(Exception ex, IDictionary<string, string> properties)
        {
            if (properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("Type", ex.GetType().FullName);
            properties.Add("Message", ex.Message);
            properties.Add("StackTrace", ex.StackTrace);
            properties.Add("FullMessage", ex.ToString());

            AddDefaultProperties(properties);
            var category = ExtractCategory(properties, nameof(Report));
            PostMessageAsync(properties, LogglyUri(Tags(category))).ContinueWith(t => { });
        }

        protected string ExtractCategory(IDictionary<string, string> properties, string category = null)
        {
            if (properties.Keys.Contains("Category"))
            {
                category = properties["Category"];
                properties.Remove("Category");
                return category;
            }

            if(properties.Keys.Contains("category"))
            {
                category = properties["category"];
                properties.Remove("category");
            }

            return category;
        }

        public virtual void TrackEvent(string name, IDictionary<string, string> properties)
        {
            if(!properties.Keys.Any(x => x.Equals("Category", StringComparison.InvariantCultureIgnoreCase)))
            {
                properties.Add("Category", "TrackedEvent");
            }

            Log(name, properties);
        }

        protected virtual void AddDefaultProperties(IDictionary<string, string> properties) =>
            properties.Add("HostName", Dns.GetHostName());
    }
}