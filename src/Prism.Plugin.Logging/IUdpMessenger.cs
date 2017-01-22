using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prism.Logging
{
    public interface IUdpMessenger
    {
        Task SendMessage( string message, string hostOrIp, int port );

        Task SendMulticastMessage( string message, int port );
    }
}
