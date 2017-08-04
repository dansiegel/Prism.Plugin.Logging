using System;

namespace Prism.Logging.Graylog
{
    public interface IGelfOptions
    {
        Uri Host { get; }
    }
}