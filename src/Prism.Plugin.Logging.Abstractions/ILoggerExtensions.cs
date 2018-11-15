using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Logging
{
    public static class ILoggerExtensions
    {
        public static void Debug(this ILogger logger, string message, IDictionary<string, string> properties = null)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties[nameof(Category)] = $"{Category.Debug}";

            logger.Log(message, properties);
        }

        public static void Info(this ILogger logger, string message, IDictionary<string, string> properties = null)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties[nameof(Category)] = $"{Category.Info}";

            logger.Log(message, properties);
        }

        public static void Log(this ILogger logger, string message) =>
            logger.Log(message, null);

        public static void Log(this ILogger logger, Exception ex, IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("Type", ex.GetType().FullName);
            properties.Add("StackTrace", ex.StackTrace);

            logger.Log(ex.Message, properties);
        }

        public static void Log(this ILogger logger, Exception ex, Category category, IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add(nameof(Category), $"{category}");
            logger.Log(ex, properties);
        }

        public static void Report(this ILogger logger, Exception ex) =>
            logger.Report(ex, new Dictionary<string, string>());

        public static void Warn(this ILogger logger, string message, IDictionary<string, string> properties = null)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties[nameof(Category)] = $"{Category.Warn}";

            logger.Log(message, properties);
        }
    }
}
