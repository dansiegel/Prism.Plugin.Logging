using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.ApplicationInsights;

namespace Prism.Logging.AppInsights
{
    public class AppInsightsLogger : ILogger
    {
        private static Assembly StartupAssembly = null;

        public static void Init(object assemblyType) =>
            StartupAssembly = assemblyType.GetType().Assembly;

        private TelemetryClient _telemetry;
        private CancellationTokenSource source;
        private CancellationToken token;
        private Thread _heartbeatThread;
        private IApplicationInsightsOptions _options { get; }

        public AppInsightsLogger(IApplicationInsightsOptions options)
        {
            if(!IsDebugBuild())
            {
                _options = options;
                Startup();
            }
            
        }

        private bool IsDebugBuild()
        {
            try
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null)
                {
                    entryAssembly = StartupAssembly;
                }

                return IsDebugAssembly(entryAssembly);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool IsDebugAssembly(Assembly assembly) =>
            assembly?.GetCustomAttributes(false)
                    .OfType<DebuggableAttribute>()
                    .Any(da => da.IsJITTrackingEnabled) ?? false;

        internal void Startup()
        {
            if (_telemetry != null)
            {
                throw new InvalidOperationException("The analytics service has already been setup!");
            }

            if (string.IsNullOrWhiteSpace(_options.InstrumentationKey))
            {
                throw new NullReferenceException("A value could not be found for the Instrumentation Key");
            }

            _telemetry = new TelemetryClient
            {
                InstrumentationKey = _options.InstrumentationKey
            };
            _telemetry.Context.Device.OperatingSystem = GetPlatform();
            _telemetry.Flush();
        }

        internal void ConfigureUserTraits()
        {
            try
            {
                var user = $"{Environment.UserName}@{Environment.MachineName}";

                _telemetry.Context.User.Id = user;
                if (string.IsNullOrEmpty(_telemetry.Context.Session.Id))
                {
                    _telemetry.Context.Session.Id = Guid.NewGuid().ToString();
                }

                _telemetry.Context.Properties["language"] = Thread.CurrentThread.CurrentCulture.Name;
                _telemetry.Context.Properties["user"] = user;
                
                if(_options.UserTraits != null)
                {
                    foreach(var trait in _options.UserTraits)
                    {
                        _telemetry.Context.Properties[trait.Key] = trait.Value;
                    }
                }

                _telemetry.Flush();
                StartHeartbeat();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Debugger.Break();
            }
        }

        public string GetPlatform()
        {
            return Environment.OSVersion.Platform.ToString();
        }


        private void Heartbeat(object obj)
        {
            const int SleepTimeMilliseconds = 15000;
            var elapsed = new TimeSpan(0, 30, 0);
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(SleepTimeMilliseconds);
                elapsed += TimeSpan.FromMilliseconds(SleepTimeMilliseconds);
                if (elapsed.TotalMinutes > 20)
                {
                    elapsed = new TimeSpan(0, 0, 0);
                    this.Info("User Active");
                    _telemetry.Flush();
                }
            }
        }

        private void StartHeartbeat()
        {
            source = new CancellationTokenSource();
            token = source.Token;
            _heartbeatThread = new Thread(Heartbeat);
            _heartbeatThread.Start();
        }

        private void StopHeartbeat()
        {
            source.Cancel();
            _heartbeatThread.Abort();
        }

        private void DebugLog(string message, IDictionary<string, string> properties)
        {
            Console.WriteLine(message);
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    Console.WriteLine($"    {prop.Key}: {prop.Value}");
                }
            }
        }

        public void Log(string message, IDictionary<string, string> properties)
        {
            if(_telemetry == null)
            {
                DebugLog(message, properties);
                return;
            }

            _telemetry.TrackEvent(message, properties);
        }

        public void Log(string message, Category category, Priority priority)
        {
            var properties = new Dictionary<string, string>
            {
                { "category", $"{category}" },
                { "priority", $"{priority}" }
            };
            Log(message, properties);
        }

        public void Report(Exception ex, IDictionary<string, string> properties)
        {
            if (_telemetry == null)
            {
                DebugLog(ex.ToString(), properties);
                return;
            }

            _telemetry.TrackException(ex, properties);
        }
    }
}
