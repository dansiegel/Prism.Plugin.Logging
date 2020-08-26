using System;
using System.Linq;
using Prism.Logging.Extensions;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Extensions
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void SubArraySplitsArray()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var subArray = array.SubArray(5);
            Assert.Equal(4, subArray.Max());
        }

        [Fact]
        public void ToArraySegmentConvertsArray()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var segment = array.ToArraySegment();
            Assert.IsType<ArraySegment<int>>(segment);
            Assert.Equal(array.Length, segment.Count);
        }
    }
}
