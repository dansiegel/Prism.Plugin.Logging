using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Logging
{
    public static class ILoggerExtensions
    {
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
            logger.Report(ex, null);
    }
}
