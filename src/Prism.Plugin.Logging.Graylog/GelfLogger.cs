using System;
using System.Collections.Generic;
using Prism.Logging.Graylog.Extensions;
using Prism.Logging.Http;

namespace Prism.Logging.Graylog
{
    public class GelfLogger : HttpLogger, IGelfLogger, ILogger
    {
        protected IGelfOptions _options { get; }

        public GelfLogger(IGelfOptions options)
        {
            _options = options;
        }

        public virtual void Log(string message, Level level = Level.Debug)
        {
            var gelf = CreateMessage(fullMessage: message, level: level);
            Log(gelf);
        }

        public void Log(GelfMessage message) =>
            PostMessageAsync(message, new Uri(_options.Host, "gelf")).ContinueWith(t => { });

        public void Log(string message, Category category, Priority priority)
        {
            var gelf = CreateMessage(fullMessage: message, category: category, priority: priority);
            Log(gelf);
        }

        public virtual void Log(string message, IDictionary<string, string> properties)
        {
            var gelf = CreateMessage(fullMessage: message, properties: properties);
            Log(gelf);
        }

        public virtual void Report(Exception ex, IDictionary<string, string> properties)
        {
            var gelf = CreateMessage(ex.Message, ex.ToString(), properties: properties);
            Log(gelf);
        }

        public virtual void TrackEvent(string name, IDictionary<string, string> properties) =>
            Log(name, properties);

        protected virtual GelfMessage CreateMessage(string shortMessage = null, string fullMessage = null, Category? category = null, Priority? priority = null, Level level = Level.Debug, IDictionary<string, string> properties = null)
        {
            if(properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            if(priority.HasValue)
            {
                properties.Add(nameof(Priority), $"{priority.Value}");
            }

            if(properties.ContainsKey(nameof(Category)) && Enum.TryParse(properties[nameof(Category)], out Category cat))
            {
                category = cat;
                properties.Remove(nameof(Category));
            }

            if(category.HasValue)
            {
                level = category.Value.ToLevel();
            }

            return new GelfMessage(properties)
            {
                Host = Environment.MachineName,
                Level = (long)category.ToLevel(),
                ShortMessage = shortMessage,
                FullMessage = fullMessage
            };
        }
    }
}