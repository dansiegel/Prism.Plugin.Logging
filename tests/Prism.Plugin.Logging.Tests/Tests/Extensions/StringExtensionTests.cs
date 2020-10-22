using System.Linq;
using Prism.Logging.Extensions;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Extensions
{
    public class StringExtensionTests
    {
        [Fact]
        public void ChunkifyChunks()
        {
            var str = "Hello World";
            var chunked = str.Chunkify(4);
            Assert.Equal(2, chunked.Count());
        }
    }
}
