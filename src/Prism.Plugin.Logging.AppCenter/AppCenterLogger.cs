using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Timers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Prism.Logging.AppCenter
{
    public class AppCenterLogger : ILogger
    {
        private static Assembly StartupAssembly = null;

        public static void Init(object assemblyType) => 
            StartupAssembly = assemblyType.GetType().Assembly;

        public AppCenterLogger()
        {
            IsDebug = IsDebugBuild();

            if(!IsDebug)
            {
                AnalyticsEnabled();
                CrashesEnabled();
                Timer = new Timer(15 * 1000);
                Timer.Elapsed += Timer_Elapsed;
                Timer.Start();
            }
        }

        private bool IsDebugBuild()
        {
            try
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                if(entryAssembly == null)
                {
                    entryAssembly = StartupAssembly;
                }

                return IsDebugAssembly(entryAssembly);
            }
            catch
            {
                return false;
            }
        }

        private bool IsDebugAssembly(Assembly assembly) =>
            assembly?.GetCustomAttributes(false)
                    .OfType<DebuggableAttribute>()
                    .Any(da => da.IsJITTrackingEnabled) ?? false;

        private Timer Timer { get; }
        private bool IsDebug { get; }
        private bool analyticsEnabled = false;
        private bool crashesEnabled = false;

        public void Log(string message, IDictionary<string, string> properties)
        {
            if(IsDebug || !analyticsEnabled)
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
            if (IsDebug || !crashesEnabled)
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


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AnalyticsEnabled();
            CrashesEnabled();
        }

        private bool AnalyticsEnabled()
        {
            return analyticsEnabled = Analytics.IsEnabledAsync().GetAwaiter().GetResult();
        }

        private bool CrashesEnabled()
        {
            return crashesEnabled = Crashes.IsEnabledAsync().GetAwaiter().GetResult();
        }
    }
}
