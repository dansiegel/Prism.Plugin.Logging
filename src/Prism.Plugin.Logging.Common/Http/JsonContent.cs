using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Prism.Logging.Http
{
    public class JsonContent : ByteArrayContent
    {
        public JsonContent(JObject message, Encoding encoding = null)
            : base(GetByteArray(message.ToString(), encoding))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = (encoding ?? Encoding.UTF8).WebName
            };
        }

        public JsonContent(object message, Encoding encoding = null) 
            : base(GetByteArray(message, encoding))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = (encoding ?? Encoding.UTF8).WebName
            };
        }

        static byte[] GetByteArray(object content, Encoding encoding = null)
        {
            return GetByteArray(JsonConvert.SerializeObject(content));
        }

        static byte[] GetByteArray(string content, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(content);
        }
    }
}