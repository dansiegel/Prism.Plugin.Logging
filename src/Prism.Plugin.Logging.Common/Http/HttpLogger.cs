using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prism.Logging.Http
{
    public class HttpLogger : IDisposable
    {
        private HttpClient _client = new HttpClient();

        protected Task<HttpResponseMessage> PostMessageAsync(object message, Uri requestUri)
        {
            return _client.PostAsync(requestUri, new JsonContent(message));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                    _client = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


    }
}
