using System;
using System.Globalization;
using Prism.Logging.Extensions;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Extensions
{
    public class DateTimeExtensionTests
    {
        private const double UnixTimestamp = 1562767200;
        private static readonly DateTimeOffset DTO = new DateTimeOffset(2019, 7, 10, 14, 0, 0, 0, new GregorianCalendar(), TimeSpan.FromSeconds(0));

        [Fact]
        public void ConvertsToUnixTimeFromDateTimeOffset()
        {
            Assert.Equal(UnixTimestamp, DTO.ToUnixTimestamp());
        }

        [Fact]
        public void ConvertsToDateTimeOffsetFromUnixstamp()
        {
            Assert.Equal(DTO, UnixTimestamp.FromUnixTimestamp());
        }
    }
}
