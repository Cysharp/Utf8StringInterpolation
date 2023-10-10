using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utf8StringInterpolation.Tests
{
    public class Utf8StringTest
    {
        [Fact]
        public void TryFormat()
        {
            Span<byte> bytes = stackalloc byte[10];
            Utf8String.TryFormat(bytes, out var written, $"aaaa{10}").Should().BeTrue();
            bytes.Slice(0, written).ToArray().Should().Be("aaaa10");
            Utf8String.TryFormat(bytes, out _, $"aaaa{10}bbbb{4}ccccc{5}").Should().BeFalse();
        }
    }
}
