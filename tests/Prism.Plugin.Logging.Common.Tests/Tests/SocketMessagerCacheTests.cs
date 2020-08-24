using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Prism.Logging.Sockets;
using Prism.Plugin.Logging.Common.Mocks;
using Xunit;

namespace Prism.Plugin.Logging.Common.Tests
{
    [Collection(nameof(FileSystem))]
    public class SocketMessagerCacheTests
    {
        [Fact]
        public void SavesLogMessages()
        {
            DeleteCache();
            var queue = new Queue<LogMessage>();
            queue.Enqueue(new LogMessage
            {
                Message = "Hello World",
                MessageType = "UnitTest"
            });
            var expectedJson = JsonSerializer.Serialize(queue, new JsonSerializerOptions { WriteIndented = true });
            SocketMessenger.SaveCache(queue);

            var fileJson = File.ReadAllText(SocketMessenger.LogCacheFile);
            Assert.Equal(expectedJson, fileJson);
        }

        [Fact]
        public void GetsCorrectLogMessages()
        {
            DeleteCache();
            var messages = new[]
            {
                new LogMessage
                {
                    Message = "Hello World",
                    MessageType = "UnitTest"
                }
            };
            File.WriteAllText(SocketMessenger.LogCacheFile, JsonSerializer.Serialize(messages));

            var queue = SocketMessenger.GetLogs(null);
            Assert.Single(queue);
            var element = queue.Dequeue();
            Assert.Equal("Hello World", element.Message);
            Assert.Equal("UnitTest", element.MessageType);
        }

        [Fact]
        public void GetLogs_AddsNewMessage()
        {
            DeleteCache();
            var newMessage = new MockLogMessage { Message = "Hello World" };
            var queue = SocketMessenger.GetLogs(newMessage);
            Assert.Single(queue);
            var queuedMessage = queue.Dequeue();
            Assert.Equal(nameof(MockLogMessage), queuedMessage.MessageType);
            Assert.Equal(newMessage.Message, queuedMessage.Message);
        }

        private static void DeleteCache()
        {
            if (File.Exists(SocketMessenger.LogCacheFile))
                File.Delete(SocketMessenger.LogCacheFile);
        }
    }
}
