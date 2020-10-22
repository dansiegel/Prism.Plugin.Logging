using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Prism.Logging.Extensions;
#if !NETSTANDARD
using Xamarin.Essentials;
#endif

namespace Prism.Logging.Sockets
{
    public class SocketMessenger : ISocketMessenger
    {
        private static readonly object lockObject = new object();
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions {
            WriteIndented = true
        };

#if NETSTANDARD
        internal static readonly string LogCacheFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Prism", "Logging", "Cache", "log");
#else
        internal static readonly string LogCacheFile = Path.Combine(FileSystem.AppDataDirectory, "Prism", "Logging", "Cache", "log");
#endif
        internal const int MaxBufferSize = 65500;

        public async Task<bool> SendMessageAsync(ILogMessage message, string hostOrIp, int port)
        {
            try
            {
                return await SendMessageInternalAsync(message, hostOrIp, port);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
                Console.WriteLine(message);
                return false;
            }
#if DEBUG
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
#endif
        }

        private async Task<bool> SendMessageInternalAsync(ILogMessage message, string hostOrIp, int port)
        {
            var endpoint = GetEndPoint(hostOrIp, port);

            if (endpoint == null)
            {

                return false;
            }

            var logs = GetLogs(message);
            using var socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            await socket.ConnectAsync(endpoint);
            if (!socket.Connected)
            {
                return false;
            }

            while (logs.Any())
            {
                var log = logs.Dequeue();
                var data = log.GetBytes();

                if (data.Length > socket.SendBufferSize)
                {
                    Console.WriteLine($"Max Buffer Size {socket.SendBufferSize}. Data packets length is currently: {data.Length}");
                    Console.WriteLine(message);

                    // HACK: To ensure that the data packet fits in the buffer size
                    data = data.SubArray(socket.SendBufferSize);
                }

                await socket.SendToAsync(data.ToArraySegment(), SocketFlags.None, endpoint);
                SaveCache(logs);
            }

            return true;
        }

        public bool SendMessage(ILogMessage message, string hostOrIp, int port)
        {
            try
            {
                var endpoint = GetEndPoint(hostOrIp, port);
                using var socket = new Socket(endpoint.AddressFamily, GetSocketType(), GetProtocolType());
                var logs = GetLogs(message);
                socket.Connect(endpoint);
                if (!socket.Connected)
                    return false;

                while (logs.Any())
                {
                    var log = logs.Dequeue();
                    var data = log.GetBytes();

                    if (data.Length > socket.SendBufferSize)
                    {
                        Console.WriteLine(log);
                        data = data.SubArray(socket.SendBufferSize);
                    }

                    socket.SendTo(data, endpoint);
                    SaveCache(logs);
                }
                return true;
            }
            catch (NotSupportedException nse)
            {
                Console.WriteLine(nse);
                Console.WriteLine(message);
                return false;
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
                Console.WriteLine(message);
                return false;
            }
        }

        public virtual ProtocolType GetProtocolType() => ProtocolType.Udp;

        public virtual SocketType GetSocketType() => SocketType.Dgram;

        protected virtual EndPoint GetEndPoint(string hostOrIp, int port)
        {
            if (hostOrIp.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                hostOrIp = "127.0.0.1";
            }

            return IPAddress.TryParse(hostOrIp, out IPAddress address) ?
                    (EndPoint)new IPEndPoint(address, port) :
                    (EndPoint)new DnsEndPoint(hostOrIp, port);
        }

        protected static byte[] EncodeMessage(string message) =>
            Encoding.UTF8.GetBytes(message);

        protected static int GetEncodedSize(string message) =>
            EncodeMessage(message)?.Length ?? 0;

        internal protected static IEnumerable<string> Chunkify(string prefix, string message)
        {
            if (GetEncodedSize($"{prefix}{message}") <= MaxBufferSize)
            {
                return new string[] { $"{prefix}{message}" };
            }

            var prefixSize = GetEncodedSize(prefix);
            var messageSize = GetEncodedSize(message);
            int chunkSize = (int)(messageSize / ((double)MaxBufferSize - prefixSize));

            return message.Chunkify(chunkSize);
        }

        internal static void SaveCache(Queue<LogMessage> queue)
        {
            lock (lockObject)
            {
                var cacheFile = new FileInfo(LogCacheFile);

                if (!cacheFile.Directory.Exists)
                    cacheFile.Directory.Create();

                var json = JsonSerializer.Serialize(queue, Options);
                File.WriteAllText(cacheFile.FullName, json);
            }
        }

        internal static Queue<LogMessage> ReadCache()
        {
            lock (lockObject)
            {
                var cacheFile = new FileInfo(LogCacheFile);
                if (!cacheFile.Exists)
                    return new Queue<LogMessage>();

                using var fs = cacheFile.OpenText();
                string s = null;
                var builder = new StringBuilder();
                while ((s = fs.ReadLine()) != null)
                {
                    builder.AppendLine(s);
                }

                fs.Close();

                var json = builder.ToString();
                var cache = JsonSerializer.Deserialize<Queue<LogMessage>>(json);
                return cache;
            }
        }

        internal static Queue<LogMessage> GetLogs(ILogMessage currentMessage)
        {
            var cacheFile = new FileInfo(LogCacheFile);
            Queue<LogMessage> cache = cacheFile.Exists ? ReadCache() : new Queue<LogMessage>();

            if (currentMessage != null)
            {
                if (!(currentMessage is LogMessage currentLogMessage))
                {
                    currentLogMessage = new LogMessage {
                        Message = Encoding.Default.GetString(currentMessage.GetBytes()),
                        MessageType = currentMessage.GetType().Name
                    };
                }

                cache.Enqueue(currentLogMessage);
            }

            SaveCache(cache);
            return cache;
        }
    }
}