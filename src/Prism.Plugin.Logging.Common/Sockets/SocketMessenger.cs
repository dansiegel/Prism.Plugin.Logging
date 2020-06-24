using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging.Extensions;
#if !NETSTANDARD
using Xamarin.Essentials;
#endif

namespace Prism.Logging.Sockets
{
    public class SocketMessenger : ISocketMessenger
    {
#if NETSTANDARD
        private static readonly string LogCacheFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Prism", "Logging", "Cache", "log");
#else
        private static readonly string LogCacheFile = Path.Combine(FileSystem.AppDataDirectory, "Prism", "Logging", "Cache", "log");
#endif
        internal const int MaxBufferSize = 65500;

        public async Task<bool> SendMessageAsync(ILogMessage message, string hostOrIp, int port)
        {
            try
            {
                return await SendMessageInternalAsync(message, hostOrIp, port);
            }
            catch(SocketException se)
            {
                Console.WriteLine(se);
                Console.WriteLine(message);
                return false;
            }
#if DEBUG
            catch(System.Exception e)
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

            while(logs.Any())
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

                while(logs.Any())
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
            catch(SocketException se)
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
            return IPAddress.TryParse(hostOrIp, out IPAddress address) ?
                    (EndPoint)new IPEndPoint(address, port) :
                    (EndPoint)new DnsEndPoint(hostOrIp, port);
        }

        protected byte[] EncodeMessage(string message) => 
            Encoding.UTF8.GetBytes(message);

        protected int GetEncodedSize(string message) => 
            EncodeMessage(message)?.Length ?? 0;

        protected IEnumerable<string> Chunkify(string prefix, string message)
        {
            if(GetEncodedSize($"{prefix}{message}") <= MaxBufferSize)
            {
                return new string[] { $"{prefix}{message}"};
            }

            var prefixSize = GetEncodedSize(prefix);
            var messageSize = GetEncodedSize(message);
            int chunkSize = (int)(messageSize/((double)MaxBufferSize - prefixSize));

            return message.Chunkify(chunkSize);
        }

        private static void SaveCache(Queue<ILogMessage> queue)
        {
            var cacheFile = new FileInfo(LogCacheFile);
            var formatter = new BinaryFormatter();
            using var stream = cacheFile.Exists ? cacheFile.OpenWrite() : cacheFile.Create();
            formatter.Serialize(stream, queue);
            stream.Close();
        }
        private static Queue<ILogMessage> ReadCache()
        {
            var cacheFile = new FileInfo(LogCacheFile);
            var formatter = new BinaryFormatter();
            using var stream = cacheFile.Exists ? cacheFile.OpenWrite() : cacheFile.Create();
            var cache = (Queue<ILogMessage>)formatter.Deserialize(stream);
            stream.Close();
            return cache;
        }

        private static Queue<ILogMessage> GetLogs(ILogMessage currentMessage)
        {
            var cacheFile = new FileInfo(LogCacheFile);
            Queue<ILogMessage> cache = cacheFile.Exists ? ReadCache() : new Queue<ILogMessage>();
            cache.Enqueue(currentMessage);
            SaveCache(cache);
            return cache;
        }
    }
}