using Xunit;

namespace Prism.Plugin.Logging.Tests
{
    [CollectionDefinition(nameof(FileSystem), DisableParallelization = true)]
    public class FileSystemTestCollection : ICollectionFixture<FileSystem>
    {
    }
}
