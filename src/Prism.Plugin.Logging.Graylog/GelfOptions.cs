using System;

namespace Prism.Logging.Graylog
{
    public class GelfOptions : IGelfOptions
    {
        public Uri Host { get; set; }
    }
}