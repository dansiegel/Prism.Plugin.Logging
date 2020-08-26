using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Plugin.Logging.Tests.Loggers
{
    public sealed class LogListener : IDisposable
    {
        private List<string> _messages = new List<string>();
        private UdpClient _listener;

        public LogListener(int port)
        {
            Port = port;
        }

        public int Port { get; }

        public IReadOnlyList<string> Messages => _messages;

        public bool IsListening { get; private set; }

        public void StartListener()
        {
            _messages = new List<string>();
            if (IsListening)
                StopListener();

            _listener = new UdpClient(Port);
            var groupEP = new IPEndPoint(IPAddress.Any, Port);
            IsListening = true;

            Task.Run(() =>
            {
                try
                {
                    while (_listener != null && (IsListening || _listener.Client.Connected))
                    {
                        if (_listener.Client.LocalEndPoint is IPEndPoint ipEndpoint && ipEndpoint.Port == Port)
                        {
                            var bytes = _listener.Receive(ref groupEP);
                            _messages.Add(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                        }
                    }
                }
                catch(SocketException se)
                {
                    if (se.SocketErrorCode == SocketError.Interrupted)
                        return;

                    else if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();

                    throw;
                }
                catch (System.Exception ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();

                    throw;
                }
            });
        }

        public void StopListener()
        {
            if(_listener.Client.Connected)
                _listener.Close();

            IsListening = false;
            _listener.Dispose();
            _listener = null;
        }

        public void Dispose()
        {
            if (_listener != null)
                StopListener();
            _messages = null;
        }
    }
}
