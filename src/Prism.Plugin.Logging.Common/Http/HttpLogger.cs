using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prism.Logging.Http
{
    public class HttpLogger : IDisposable
    {
        private readonly HttpClient _client = new HttpClient();

        protected Task<HttpResponseMessage> PostMessageAsync(object message, Uri requestUri)
        {
            return _client.PostAsync(requestUri, new JsonContent(message));
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
