using FluentAssertions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Utf8StringInterpolation.Tests
{
    public class Utf8StringBuilderTest
    {

        [Fact]
        public void AppendCharRepeat()
        {
            using (var buffer = Utf8String.CreateWriter(out var zsb))
            {
                var text = "foo";
                zsb.AppendLiteral(text);
                var bcl = new StringBuilder(text);

                // ASCII
                zsb.Append('\x7F', 10);
                bcl.Append('\x7F', 10);
                zsb.Flush();
                buffer.ToString().Should().Be(bcl.ToString());

                // Non-ASCII
                zsb.Append('\x80', 10);
                bcl.Append('\x80', 10);
                zsb.Flush();
                buffer.ToString().Should().Be(bcl.ToString());

                zsb.Append('\u9bd6', 10);
                bcl.Append('\u9bd6', 10);
                zsb.Flush();
                buffer.ToString().Should().Be(bcl.ToString());
            }

        }
    }
}
