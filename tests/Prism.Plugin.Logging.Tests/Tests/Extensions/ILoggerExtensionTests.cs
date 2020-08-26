using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Logging;
using Prism.Plugin.Logging.Mocks;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Extensions
{
    public class ILoggerExtensionTests
    {
        private const string TestMessage = "Hello from ILoggerExtensions Test Fixture";

        [Fact]
        public void DebugSendsTestMessage()
        {
            var logger = new LoggerMock();
            logger.Debug(TestMessage);

            Assert.Equal(TestMessage, logger.Sent.Message);
        }

        [Fact]
        public void DebugHandlesNullProperties()
        {
            var logger = new LoggerMock();
            logger.Debug(TestMessage);

            Assert.NotNull(logger.Sent.Properties);
        }

        [Fact]
        public void DebugAddsCategoryToProperties()
        {
            var logger = new LoggerMock();
            logger.Debug(TestMessage, new Dictionary<string, string> { { "Test", nameof(DebugAddsCategoryToProperties) } });

            Assert.Equal(2, logger.Sent.Properties.Count);
            Assert.True(logger.Sent.Properties.ContainsKey("Category"));
            Assert.Equal("Debug", logger.Sent.Properties["Category"]);
        }

        [Fact]
        public void DebugAddsPropertiesFromTuple()
        {
            var logger = new LoggerMock();
            logger.Debug(TestMessage, ("Test", nameof(DebugAddsPropertiesFromTuple)));

            Assert.Equal(2, logger.Sent.Properties.Count);
            Assert.True(logger.Sent.Properties.ContainsKey("Test"));
            Assert.Equal(nameof(DebugAddsPropertiesFromTuple), logger.Sent.Properties["Test"]);
        }

        [Fact]
        public void InfoSendsTestMessage()
        {
            var logger = new LoggerMock();
            logger.Info(TestMessage);

            Assert.Equal(TestMessage, logger.Sent.Message);
        }

        [Fact]
        public void InfoHandlesNullProperties()
        {
            var logger = new LoggerMock();
            logger.Info(TestMessage);

            Assert.NotNull(logger.Sent.Properties);
        }

        [Fact]
        public void InfoAddsCategoryToProperties()
        {
            var logger = new LoggerMock();
            logger.Info(TestMessage, new Dictionary<string, string> { { "Test", nameof(InfoAddsCategoryToProperties) } });

            Assert.Equal(2, logger.Sent.Properties.Count);
            Assert.True(logger.Sent.Properties.ContainsKey("Category"));
            Assert.Equal("Info", logger.Sent.Properties["Category"]);
        }

        [Fact]
        public void InfoAddsPropertiesFromTuple()
        {
            var logger = new LoggerMock();
            logger.Info(TestMessage, ("Test", nameof(InfoAddsPropertiesFromTuple)));

            Assert.Equal(2, logger.Sent.Properties.Count);
            Assert.True(logger.Sent.Properties.ContainsKey("Test"));
            Assert.Equal(nameof(InfoAddsPropertiesFromTuple), logger.Sent.Properties["Test"]);
        }

        [Fact]
        public void WarnSendsTestMessage()
        {
            var logger = new LoggerMock();
            logger.Warn(TestMessage);

            Assert.Equal(TestMessage, logger.Sent.Message);
        }

        [Fact]
        public void WarnHandlesNullProperties()
        {
            var logger = new LoggerMock();
            logger.Warn(TestMessage);

            Assert.NotNull(logger.Sent.Properties);
        }

        [Fact]
        public void WarnAddsCategoryToProperties()
        {
            var logger = new LoggerMock();
            logger.Warn(TestMessage, new Dictionary<string, string> { { "Test", nameof(WarnAddsCategoryToProperties) } });

            Assert.Equal(2, logger.Sent.Properties.Count);
            Assert.True(logger.Sent.Properties.ContainsKey("Category"));
            Assert.Equal("Warn", logger.Sent.Properties["Category"]);
        }

        [Fact]
        public void WarnAddsPropertiesFromTuple()
        {
            var logger = new LoggerMock();
            logger.Warn(TestMessage, ("Test", nameof(WarnAddsPropertiesFromTuple)));

            Assert.Equal(2, logger.Sent.Properties.Count);
            Assert.True(logger.Sent.Properties.ContainsKey("Test"));
            Assert.Equal(nameof(WarnAddsPropertiesFromTuple), logger.Sent.Properties["Test"]);
        }

        [Fact]
        public void LogInitializesPropertiesWhenNoneProvided()
        {
            var logger = new LoggerMock();
            logger.Log(TestMessage);

            Assert.Equal(TestMessage, logger.Sent.Message);
            Assert.NotNull(logger.Sent.Properties);
            Assert.Empty(logger.Sent.Properties);
        }

        [Fact]
        public void LogSetsPropertiesFromTuple()
        {
            var logger = new LoggerMock();
            logger.Log(TestMessage, ("Test", nameof(LogSetsPropertiesFromTuple)));

            Assert.Equal(TestMessage, logger.Sent.Message);
            Assert.Single(logger.Sent.Properties);
            Assert.Equal(nameof(LogSetsPropertiesFromTuple), logger.Sent.Properties["Test"]);
        }

        [Fact]
        public void ReportWithNoPropertiesProvidesInitializedDictionary()
        {
            var logger = new LoggerMock();
            var ex = new Exception(TestMessage);
            logger.Report(ex);

            Assert.Same(ex, logger.Sent.Exception);
            Assert.NotNull(logger.Sent.Properties);
            Assert.Empty(logger.Sent.Properties);
        }

        [Fact]
        public void ReportWithTuplesSetsProperties()
        {
            var logger = new LoggerMock();
            var ex = new Exception(TestMessage);
            logger.Report(ex, ("Test", nameof(ReportWithTuplesSetsProperties)));

            Assert.Same(ex, logger.Sent.Exception);
            Assert.NotNull(logger.Sent.Properties);
            Assert.Single(logger.Sent.Properties);
            Assert.True(logger.Sent.Properties.ContainsKey("Test"));
            Assert.Equal(nameof(ReportWithTuplesSetsProperties), logger.Sent.Properties["Test"]);
        }

        [Fact]
        public void TrackEventWithNoPropertiesProvidesInitializedDictionary()
        {
            var logger = new LoggerMock();
            logger.TrackEvent(TestMessage);

            Assert.Equal(TestMessage, logger.Sent.Message);
            Assert.NotNull(logger.Sent.Properties);
            Assert.Empty(logger.Sent.Properties);
        }

        [Fact]
        public void TrackEventtWithTuplesSetsProperties()
        {
            var logger = new LoggerMock();
            logger.TrackEvent(TestMessage, ("Test", nameof(TrackEventtWithTuplesSetsProperties)));

            Assert.Equal(TestMessage, logger.Sent.Message);
            Assert.NotNull(logger.Sent.Properties);
            Assert.Single(logger.Sent.Properties);
            Assert.True(logger.Sent.Properties.ContainsKey("Test"));
            Assert.Equal(nameof(TrackEventtWithTuplesSetsProperties), logger.Sent.Properties["Test"]);
        }
    }
}
