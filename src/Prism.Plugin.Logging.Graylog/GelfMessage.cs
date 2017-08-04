using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Prism.Logging.Graylog
{
    public class GelfMessage
    {
        [JsonProperty("version")]
        public string Version { get; } = "1.1";

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("short_message")]
        public string ShortMessage { get; set; }

        [JsonProperty("full_message")]
        public string FullMessage { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("level")]
        public Level Level { get; set; }

        [JsonProperty("_facility")]
        public string Facility { get; set; }

        [JsonProperty("_line")]
        public int Line { get; set; }

        [JsonProperty("_file")]
        public string File { get; set; }

        [JsonProperty("_application")]
        public string Application { get; set; }

        public override string ToString() =>
            JsonConvert.SerializeObject(this);
    }
}