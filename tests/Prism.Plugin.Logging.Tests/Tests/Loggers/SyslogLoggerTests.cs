using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Logging.Syslog;
using Xunit;
using Xunit.Abstractions;

namespace Prism.Plugin.Logging.Tests.Loggers
{
    public sealed class SyslogLoggerTests
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private ITestOutputHelper _testOutputHelper { get; }

        public SyslogLoggerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ListenerTest()
        {
            LogListener logListener = null;
            try
            {
                var port = 11000;
                await semaphoreSlim.WaitAsync();
                logListener = new LogListener(port);
                logListener.StartListener();

                Assert.True(logListener.IsListening);

                var client = new UdpClient();
                var testMessage = Encoding.ASCII.GetBytes("Hello World");
                client.Connect(new IPEndPoint(IPAddress.Loopback, port));
                client.Send(testMessage, testMessage.Length);
                client.Close();

                await Task.Delay(500);
                logListener.StopListener();

                Assert.False(logListener.IsListening);
                Assert.Single(logListener.Messages);
                Assert.Contains("Hello World", logListener.Messages);
            }
            finally
            {
                logListener?.Dispose();
                semaphoreSlim.Release();
            }
        }

        [Fact]
        public async Task LogTest()
        {
            LogListener logListener = null;
            try
            {
                int port = 11000;
                await semaphoreSlim.WaitAsync();
                logListener = new LogListener(port);
                logListener.StartListener();
                var logger = CreateLogger(port);
                Assert.Empty(logListener.Messages);
                logger.Log("Syslog Logger Test", Level.Debug, Facility.Syslog);
                await Task.Delay(500);

                foreach (var line in logListener.Messages)
                    _testOutputHelper.WriteLine(line);

                Assert.Single(logListener.Messages);
                var message = logListener.Messages.First();
                Assert.Matches($@"\<47\>[A-Z][a-z][a-z]\s\d\d\s\d\d\:\d\d\:\d\d\s({Dns.GetHostName()})\sUnitTests\:\sSyslog Logger Test", message);
            }
            finally
            {
                logListener?.Dispose();
                semaphoreSlim.Release();
            }
        }

        [Fact]
        public async Task LoggerCachesUnsendableMessages()
        {
            LogListener logListener = null;
            try
            {
                int port = 11000;
                await semaphoreSlim.WaitAsync();
                logListener = new LogListener(port);
                var logger = CreateLogger(port, "baddomain.dansiegel.net");
                logListener.StartListener();
                logger.Log("Listener Not Started", Level.Debug, Facility.Daemon);
                await Task.Delay(500);
                logger = CreateLogger(port);
                Assert.Empty(logListener.Messages);
                logger.Log("Syslog Logger Test", Level.Debug, Facility.Syslog);
                await Task.Delay(500);

                foreach (var line in logListener.Messages)
                    _testOutputHelper.WriteLine(line);

                Assert.Equal(2, logListener.Messages.Count);
                var message = logListener.Messages.First();
                Assert.Matches($@"\<31\>[A-Z][a-z][a-z]\s\d\d\s\d\d\:\d\d\:\d\d\s({Dns.GetHostName()})\sUnitTests\:\sListener Not Started", message);

                message = logListener.Messages.Last();
                Assert.Matches($@"\<47\>[A-Z][a-z][a-z]\s\d\d\s\d\d\:\d\d\:\d\d\s({Dns.GetHostName()})\sUnitTests\:\sSyslog Logger Test", message);
            }
            finally
            {
                logListener?.Dispose();
                semaphoreSlim.Release();
            }
        }

        private ISyslogLogger CreateLogger(int port, string hostOrIp = null)
        {
            var options = new SyslogOptions {
                AppNameOrTag = "UnitTests",
                HostNameOrIp = hostOrIp ?? IPAddress.Loopback.ToString(),
                Port = port
            };
            return new SyslogLogger(options);
        }
    }
}
