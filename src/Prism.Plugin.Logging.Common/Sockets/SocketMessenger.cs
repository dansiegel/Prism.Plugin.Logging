using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging.Extensions;

namespace Prism.Logging.Sockets
{
    public class SocketMessenger : ISocketMessenger
    {
        
        internal const int MaxBufferSize = 65500;

        public async Task<bool> SendMessageAsync(ILogMessage message, string hostOrIp, int port)
        {
            try
            {
                var endpoint = GetEndPoint(hostOrIp, port);

                if(endpoint == null)
                {
                    return false;
                }

                using(var socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp))
                {
                    await socket.ConnectAsync(endpoint).ConfigureAwait(continueOnCapturedContext: false);
                    if (!socket.Connected)
                    {
                        return false;
                    }

                    var data = message.GetBytes();

                    if(data.Length > socket.SendBufferSize)
                    {
                        Console.WriteLine($"Max Buffer Size {socket.SendBufferSize}. Data packets length is currently: {data.Length}");
                        Console.WriteLine(message);

                        // HACK: To ensure that the data packet fits in the buffer size
                        data = data.SubArray(socket.SendBufferSize);
                    }
                    
                    await socket.SendToAsync(data.ToArraySegment(), SocketFlags.None, endpoint).ConfigureAwait(continueOnCapturedContext:false);
                    return true;
                }

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

        public bool SendMessage(ILogMessage message, string hostOrIp, int port)
        {
            try
            {
                var endpoint = GetEndPoint(hostOrIp, port);
                using(var socket = new Socket(endpoint.AddressFamily, GetSocketType(), GetProtocolType()))
                {
                    var data = message.GetBytes();
                    
                    if(data.Length > socket.SendBufferSize)
                    {
                        Console.WriteLine(message);
                        data = data.SubArray(socket.SendBufferSize);
                    }
                    
                    socket.SendTo(data, endpoint);
                    return true;
                }
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
    }
}