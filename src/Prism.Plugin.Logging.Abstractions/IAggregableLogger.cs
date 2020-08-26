using System.ComponentModel;

namespace Prism.Logging
{
    // This is a marker interface
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IAggregableLogger : ILogger
    {
    }
}