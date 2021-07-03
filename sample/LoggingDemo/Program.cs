using System;
using System.Collections.Generic;
using Prism.Logging;
using Prism.Logging.Loggly;
using Prism.Logging.Syslog;

namespace LoggingDemo
{
    class Program
    {
        static bool Continue = true;
        const string Generic = "Generic Syslog";
        const string LogglySyslog = "Loggly Syslog";
        const string LogglyHttp = "Loggly Http";

        static void Main(string[] args)
        {
            var logger = GetLogger();
            while (Continue)
            {
                Console.Clear();
                string message = ConsoleUtility.Question("Enter a test message, or type 'quit' to exit:");
                if (message.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                logger.Log(message, new Dictionary<string, string> { { "Category", "Demo" } });
            }

            Console.WriteLine("Thanks for logging");
        }

        private static ILogger GetLogger()
        {
            switch (ConsoleUtility.Option("Which Logger would you like to use?", Generic, LogglySyslog, LogglyHttp, "Quit"))
            {
                case Generic:
                    var genOptions = new Options {
                        HostNameOrIp = ConsoleUtility.Question("What is the Host Name or IP of your Syslog Server?"),
                        Port = ConsoleUtility.Question<int>("What is the port your Syslog Server is listening on?"),
                        AppNameOrTag = "LoggingDemo"
                    };

                    return new SyslogLogger(genOptions);
                case LogglySyslog:
                    return new LogglySyslogLogger(GetLogglyOptions());
                case LogglyHttp:
                    return new LogglyHttpLogger(GetLogglyOptions());
                case "Quit":
                    break;
                default:
                    Console.WriteLine("You selected a bad option");
                    break;
            }

            Continue = false;
            return null;
        }

        private static ILogglyOptions GetLogglyOptions() =>
            new LogglyOptions {
                AppName = "LoggingDemo",
                Token = ConsoleUtility.Question("What is your Loggly Token?")
            };
    }

    public class Options : ISyslogOptions
    {
        public string HostNameOrIp { get; set; }

        public int? Port { get; set; }

        public string AppNameOrTag { get; set; }
    }

    public class LogglyOptions : ILogglyOptions
    {
        public string Token { get; set; }

        public string AppName { get; set; }

        public IEnumerable<string> Tags => new string[]
        {
            "http",
            "test"
        };
    }
}
