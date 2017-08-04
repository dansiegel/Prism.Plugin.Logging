using System.Collections.Generic;

namespace Prism.Logging.Loggly
{
    public interface ILogglyOptions
    {
        string Token { get; }
        string AppName { get; }
        IEnumerable<string> Tags { get; }
    }
}