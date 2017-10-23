using System;
using System.Net.Http;
using System.Threading.Tasks;
using Prism.Logging.Sockets;

namespace Prism.Logging.Http
{
    public class HttpLogger
    {
        private static HttpClient _client = new HttpClient();

        protected Task<HttpResponseMessage> PostMessageAsync(object message, Uri requestUri)
        {
            return _client.PostAsync(requestUri, new JsonContent(message));
        }
    }
}
