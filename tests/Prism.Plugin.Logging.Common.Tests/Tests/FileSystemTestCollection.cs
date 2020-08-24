using Xunit;

namespace Prism.Plugin.Logging.Common.Tests
{
    [CollectionDefinition(nameof(FileSystem), DisableParallelization = true)]
    public class FileSystemTestCollection : ICollectionFixture<FileSystem>
    {
    }
}
