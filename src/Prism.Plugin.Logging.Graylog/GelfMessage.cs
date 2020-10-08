using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Prism.Logging.Extensions;
using Prism.Logging.Sockets;

namespace Prism.Logging.Graylog
{
    public class GelfMessage : Dictionary<string, object>, ILogMessage
    {
        private const string FacilityKey = "facility";
        private const string FileKey = "file";
        private const string FullMessageKey = "full_message";
        private const string HostKey = "host";
        private const string LevelKey = "level";
        private const string LineKey = "line";
        private const string ShortMessageKey = "short_message";
        private const string VersionKey = "version";
        private const string TimeStampKey = "timestamp";

        public GelfMessage() { }

        public GelfMessage(IDictionary<string, string> properties)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    Add(prop.Key, prop.Value);
                }
            }
        }

        public string Facility
        {
            get => PullStringValue(FacilityKey);
            set => StoreValue(FacilityKey, value);
        }

        public string File
        {
            get => PullStringValue(FileKey);
            set => StoreValue(FileKey, value);
        }

        public string FullMessage
        {
            get => PullStringValue(FullMessageKey);
            set => StoreValue(FullMessageKey, value);
        }

        public string Host
        {
            get => PullStringValue(HostKey);
            set => StoreValue(HostKey, value);
        }

        public long Level
        {
            get => !ContainsKey(LevelKey) ? int.MinValue : (long)this[LevelKey];
            set => StoreValue(LevelKey, value);
        }

        public string Line
        {
            get => PullStringValue(LineKey);
            set => StoreValue(LineKey, value);
        }

        public string ShortMessage
        {
            get => PullStringValue(ShortMessageKey);
            set => StoreValue(ShortMessageKey, value);
        }

        public DateTimeOffset TimeStamp
        {
            get
            {
                if (!this.ContainsKey(TimeStampKey))
                    return DateTime.MinValue;

                var val = this[TimeStampKey];
                var parsed = double.TryParse(val as string, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);
                return parsed ? value.FromUnixTimestamp() : DateTime.MinValue;
            }
            set => StoreValue(TimeStampKey, value.ToUnixTimestamp().ToString(CultureInfo.InvariantCulture));
        }

        public string Version
        {
            get => PullStringValue(VersionKey);
            set => StoreValue(VersionKey, value);
        }

        public byte[] GetBytes() =>
            Encoding.ASCII.GetBytes(ToString());

        private string PullStringValue(string key) =>
            ContainsKey(key) ? this[key].ToString() : string.Empty;

        private void StoreValue(string key, object value)
        {
            if (value is null) return;

            if (ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }
}