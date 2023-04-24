using System;
using System.Collections.Generic;
using Prism.Logging.Http;

namespace Prism.Logging.Graylog
{
    public class GelfLogger : HttpLogger, IGelfLogger
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

        protected virtual GelfMessage CreateMessage(string shortMessage = null, string fullMessage = null, Level level = Level.Debug, IDictionary<string, string> properties = null)
        {
            if (properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            if (properties.ContainsKey(nameof(Level)) && Enum.TryParse(properties[nameof(Level)], out Level lvl))
            {
                level = lvl;
                properties.Remove(nameof(Level));
            }
            else if (properties.ContainsKey("Category") && Enum.TryParse(properties["Category"], out lvl))
            {
                level = lvl;
                properties.Remove("Category");
            }

            return new GelfMessage(properties) {
                Host = Environment.MachineName,
                Level = (long)level,
                ShortMessage = shortMessage,
                FullMessage = fullMessage
            };
        }
    }
}