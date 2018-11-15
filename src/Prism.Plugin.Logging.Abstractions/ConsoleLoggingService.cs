using System;
using System.Collections.Generic;

namespace Prism.Logging
{
    public class ConsoleLoggingService : ILogger, IAnalyticsService
    {
        public void Log(string message, IDictionary<string, string> properties)
        {
            Console.WriteLine("Logged Message");
            LogInternal(message, properties);
        }

        public void Log(string message, Category category, Priority priority)
        {
            Console.WriteLine("Prism Logged Message:");
            Console.WriteLine($"{category} - {priority}: {message}");
        }

        public void Report(Exception ex, IDictionary<string, string> properties)
        {
            Console.WriteLine("Reported Exception:");
            LogInternal(ex, properties);
        }

        public void TrackEvent(string name, IDictionary<string, string> properties)
        {
            Console.WriteLine("Tracked Event:");
            LogInternal(name, properties);
        }

        private void LogInternal(object message, IDictionary<string, string> properties)
        {
            Console.WriteLine(message);

            foreach(var prop in properties ?? new Dictionary<string, string>())
            {
                Console.WriteLine($"    {prop.Key}: {prop.Value}");
            }
        }
    }
}
