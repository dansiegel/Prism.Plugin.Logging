using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Prism.Logging.Sockets
{
    class SocketLoggerMessenger:SocketMessenger
    {
        private ISocketLoggerOptions _options;

        public SocketLoggerMessenger(ISocketLoggerOptions options)
        {
            _options = options;
        }

        public override ProtocolType GetProtocolType() =>
            _options.ProtocolType;
    }
}
