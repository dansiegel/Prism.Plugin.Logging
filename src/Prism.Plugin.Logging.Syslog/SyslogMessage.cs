using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Prism.Logging.Sockets;

namespace Prism.Logging.Syslog
{
    public class SyslogMessage : ILogMessage
    {
        public SyslogMessage(Facility facility, Level level, string text)
        {
            Facility = facility;
            Level = level;
            Text = text;
            Timestamp = DateTimeOffset.Now;
        }
        
        public DateTimeOffset Timestamp { get; set; }

        public int MessageId { get; set; }

        public Facility Facility { get; set; }

        public Level Level { get; set; }

        public string Text { get; set; }

        public string AppName { get; set; }

        public IEnumerable<string> Tags { get; set; }

        protected int Priority => (((int) Facility)*8) + ((int) Level);

        public byte[] GetBytes() =>
            Encoding.ASCII.GetBytes(ToString());

        public override string ToString() =>
            $"<{Priority}>{DateTimeOffset.Now.ToString("MMM dd HH:mm:ss")} {Dns.GetHostName()} {GetTags()}: {Text}";

        protected virtual string GetTags()
        {
            var tags = AppName;
            foreach(var tag in Tags ?? new string[] {})
            {
                tags += $" {tag}";
            }
            return tags;
        }
    }
}