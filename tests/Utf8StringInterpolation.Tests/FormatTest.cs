using System.Numerics;

namespace Utf8StringInterpolation.Tests
{
    public class FormatTest
    {
        [Fact]
        public void EmptyFormat()
        {
            Utf8String.Format($"").Should().Be($"");
        }

        [Fact]
        public void NoFormat()
        {
            Utf8String.Format($"abcdefg").Should().Be($"abcdefg");
        }

        [Fact]
        public void SingleFormat()
        {
            Utf8String.Format($"{100}").Should().Be($"{100}");
        }

        [Fact]
        public void DoubleFormat()
        {
            Utf8String.Format($"{100}{200}").Should().Be($"{100}{200}");
        }

        [Fact]
        public void Nullable()
        {
            var guid = (Guid?)Guid.NewGuid();
            Utf8String.Format($"abc{(int?)100}def{(int?)1}ghi").Should().Be($"abc{(int?)100}def{(int?)1}ghi");
            Utf8String.Format($"abc{(int?)100:X}def{(int?)1:X}ghi").Should().Be($"abc{(int?)100:X}def{(int?)1:X}ghi");
            Utf8String.Format($"abc{guid}def{(Guid?)null}ghi").Should().Be($"abc{guid}def{(Guid?)null}ghi");
            Utf8String.Format($"abc{(double?)Math.PI:e}def{(double?)null:e}ghi").Should().Be($"abc{(double?)Math.PI:e}def{(double?)null:e}ghi");
        }
        
        [Fact]
        public void NullableWithAlignment()
        {
            var guid = (Guid?)Guid.NewGuid();
            Utf8String.Format($"abc{(int?)100,10}def{(int?)null,10}ghi").Should().Be($"abc{(int?)100,10}def{(int?)null,10}ghi");
            Utf8String.Format($"abc{(int?)100,-5:X}def{(int?)null,-5:X}ghi").Should().Be($"abc{(int?)100,-5:X}def{(int?)null,-5:X}ghi");
            Utf8String.Format($"abc{guid}def{(Guid?)null,10}ghi").Should().Be($"abc{guid}def{(Guid?)null,10}ghi");
            Utf8String.Format($"abc{(double?)null,5}def{(double?)null,5:e}ghi").Should().Be($"abc{(double?)null,5}def{(double?)null,5:e}ghi");
        }

        [Fact]
        public void Comment()
        {
            Utf8String.Format($"abc{{100}}def{200}ghi").Should().Be($"abc{{100}}def{200}ghi");
            Utf8String.Format($"}}{{{123}{{}}{456}{{").Should().Be($"}}{{{123}{{}}{456}{{");
        }

#if NET8_0_OR_GREATER

        [Fact]
        public void FormatArgs()
        {
            Utf8String.Format($"{100:00000000}-{200:00000000}").Should().Be("00000100-00000200");
        }

#endif

        [Fact]
        public void FormattableObject()
        {
            Utf8String.Format($"abc{default(Vector2)}def{new Vector2(MathF.PI):F3}").Should().Be($"abc{default(Vector2)}def{new Vector2(MathF.PI):F3}");
            Utf8String.Format($"abc{new Vector3(float.MinValue, float.NaN, float.MaxValue):E0}def{new Vector3(MathF.PI):N}").Should().Be($"abc{new Vector3(float.MinValue, float.NaN, float.MaxValue):E0}def{new Vector3(MathF.PI):N}");
        }

        [Fact]
        public void Escape()
        {
            TimeSpan span = new TimeSpan(12, 34, 56);
            var reference = string.Format(@"{0:h\,h\:mm\:ss}", span);
            var actual = Utf8String.Format($@"{span:h\,h\:mm\:ss}").Should().Be(reference);
        }

        [Fact]
        public void Unicode()
        {
            Utf8String.Format($"\u30cf\u30fc\u30c8: {"\u2764"}, \u5bb6\u65cf: {"\uD83D\uDC69\u200D\uD83D\uDC69\u200D\uD83D\uDC67\u200D\uD83D\uDC67"}(\u7d75\u6587\u5b57)")
                 .Should().Be($"\u30cf\u30fc\u30c8: {"\u2764"}, \u5bb6\u65cf: {"\uD83D\uDC69\u200D\uD83D\uDC69\u200D\uD83D\uDC67\u200D\uD83D\uDC67"}(\u7d75\u6587\u5b57)");
        }
    }
}