using System;
using System.Collections.Generic;

namespace Prism.Logging.Loggly
{
    public class LogglyOptions : ILogglyOptions
    {
        public string Token { get; set; }

        private string _appName;
        public string AppName
        {
            get => string.IsNullOrEmpty(_appName) ? "PrismApp" : _appName;
            set => _appName = value;
        }

        private IEnumerable<string> _tags;
        public IEnumerable<string> Tags
        {
            get => _tags ?? Array.Empty<string>();
            set => _tags = value;
        }
    }
}