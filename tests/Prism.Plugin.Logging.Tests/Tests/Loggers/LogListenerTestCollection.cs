using Xunit;

namespace Prism.Plugin.Logging.Tests.Loggers
{
    [CollectionDefinition(nameof(LogListener), DisableParallelization = true)]
    public class LogListenerTestCollection : ICollectionFixture<LogListener>
    {
    }
}
