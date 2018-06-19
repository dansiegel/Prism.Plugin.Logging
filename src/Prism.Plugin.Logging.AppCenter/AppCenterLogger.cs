using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Prism.Logging;

namespace Prism.Plugin.Logging.AppCenter
{
    public class AppCenterLogger : ILogger
    {
        public AppCenterLogger()
        {
            IsDebug = Assembly.GetExecutingAssembly().CustomAttributes.Any(a => a is DebuggableAttribute);
        }

        private bool IsDebug { get; }
        private bool analyticsEnabled = false;
        private bool crashesEnabled = false;

        private DateTime? analyticsChecked = null;
        private DateTime? crashesChecked = null;

        public void Log(string message, IDictionary<string, string> properties)
        {
            if(AnalyticsEnabled())
            {
                DebugLog(message, properties);
                return;
            }

            Analytics.TrackEvent(message, properties);
        }

        public void Log(string message, Category category, Priority priority)
        {
            Log(message, new Dictionary<string, string>
            {
                { nameof(Category), $"{category}" },
                { nameof(Priority), $"{priority}" }
            });
        }

        public void Report(Exception ex, IDictionary<string, string> properties)
        {
            if (CrashesEnabled())
            {
                if (properties == null) properties = new Dictionary<string, string>();

                properties.Add("ExceptionType", ex.GetType().FullName);
                properties.Add("StackTrace", ex.StackTrace);
                DebugLog(ex.Message, properties);
                return;
            }

            Crashes.TrackError(ex, properties);
        }

        private void DebugLog(string message, IDictionary<string, string> properties)
        {
            Console.WriteLine(message);
            if(properties != null)
            {
                foreach (var prop in properties)
                {
                    Console.WriteLine($"    {prop.Key}: {prop.Value}");
                }
            }
        }

        private bool AnalyticsEnabled()
        {
            if (analyticsEnabled) return analyticsEnabled;

            if(analyticsChecked.HasValue && analyticsChecked.Value < DateTime.Now.AddSeconds(-30))
            {
                analyticsEnabled = Analytics.IsEnabledAsync().GetAwaiter().GetResult();
                analyticsChecked = DateTime.Now;
            }

            return analyticsEnabled;
        }

        private bool CrashesEnabled()
        {
            if (crashesEnabled) return crashesEnabled;

            if (crashesChecked.HasValue && crashesChecked.Value < DateTime.Now.AddSeconds(-30))
            {
                crashesEnabled = Crashes.IsEnabledAsync().GetAwaiter().GetResult();
                crashesChecked = DateTime.Now;
            }

            return crashesEnabled;
        }
    }
}
