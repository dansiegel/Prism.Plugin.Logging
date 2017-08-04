using System;
using System.Net.Http;
using System.Threading.Tasks;
using Prism.Logging.Sockets;

namespace Prism.Logging.Http
{
    public class HttpLogger
    {
        protected Task<HttpResponseMessage> PostMessageAsync(object message, Uri requestUri)
        {
            using(var client = new HttpClient())
            {
                return client.PostAsync(requestUri, new JsonContent(message));
            }
        }
    }
}
