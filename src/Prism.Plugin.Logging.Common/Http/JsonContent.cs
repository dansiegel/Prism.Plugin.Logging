using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Prism.Logging.Http
{
    internal class JsonContent : ByteArrayContent
    {
        public JsonContent(object message, Encoding encoding = null)
            : base(GetByteArray(message, encoding))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json") {
                CharSet = (encoding ?? Encoding.UTF8).WebName
            };
        }

        static byte[] GetByteArray(object content, Encoding encoding = null)
        {
            return GetByteArray(JsonSerializer.Serialize(content));
        }

        static byte[] GetByteArray(string content, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(content);
        }
    }
}