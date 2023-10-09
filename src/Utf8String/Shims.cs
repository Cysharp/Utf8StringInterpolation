#pragma warning disable CA2014 // Do not use stackalloc in loops

using System.Text;

namespace Utf8String
{
#if !NET8_0_OR_GREATER

    internal static class Shims
    {
        public static bool TryFormat(this DateTime value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
            Span<char> charDest = stackalloc char[32];
            var charWritten = 0;
            while (!value.TryFormat(charDest, out charWritten, format, formatProvider))
            {
                if (charDest.Length < 512)
                {
                    charDest = stackalloc char[charDest.Length * 2];
                }
                else
                {
                    charDest = new char[charDest.Length * 2]; // too large
                }
            }

            var count = Encoding.UTF8.GetByteCount(charDest.Slice(0, charWritten));
            if (utf8Destination.Length < count)
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = Encoding.UTF8.GetBytes(charDest, utf8Destination);
            return true;
        }

        public static bool TryFormat(this DateTimeOffset value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
            Span<char> charDest = stackalloc char[32];
            var charWritten = 0;
            while (!value.TryFormat(charDest, out charWritten, format, formatProvider))
            {
                if (charDest.Length < 512)
                {
                    charDest = stackalloc char[charDest.Length * 2];
                }
                else
                {
                    charDest = new char[charDest.Length * 2]; // too large
                }
            }

            var count = Encoding.UTF8.GetByteCount(charDest.Slice(0, charWritten));
            if (utf8Destination.Length < count)
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = Encoding.UTF8.GetBytes(charDest, utf8Destination);
            return true;
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
            Span<char> charDest = stackalloc char[32];
            var charWritten = 0;
            while (!value.TryFormat(charDest, out charWritten, format, formatProvider))
            {
                if (charDest.Length < 512)
                {
                    charDest = stackalloc char[charDest.Length * 2];
                }
                else
                {
                    charDest = new char[charDest.Length * 2]; // too large
                }
            }

            var count = Encoding.UTF8.GetByteCount(charDest.Slice(0, charWritten));
            if (utf8Destination.Length < count)
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = Encoding.UTF8.GetBytes(charDest, utf8Destination);
            return true;
        }
    }

#endif
}
