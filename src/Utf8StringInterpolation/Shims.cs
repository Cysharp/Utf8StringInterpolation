#pragma warning disable CA2014 // Do not use stackalloc in loops
using System;
using System.Buffers.Text;
using System.Text;

#if !NET6_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Indicates the attributed type is to be used as an interpolated string handler.
    /// </summary>
    [global::System.AttributeUsage(
        global::System.AttributeTargets.Class |
        global::System.AttributeTargets.Struct,
        AllowMultiple = false, Inherited = false)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class InterpolatedStringHandlerAttribute : global::System.Attribute
    {
    }
    
    /// <summary>
    /// Indicates which arguments to a method involving an interpolated string handler should be passed to that handler.
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class InterpolatedStringHandlerArgumentAttribute : global::System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="global::System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute"/> class.
        /// </summary>
        /// <param name="argument">The name of the argument that should be passed to the handler.</param>
        /// <remarks><see langword="null"/> may be used as the name of the receiver in an instance method.</remarks>
        public InterpolatedStringHandlerArgumentAttribute(string argument)
        {
            Arguments = new string[] { argument };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="global::System.Runtime.CompilerServices.InterpolatedStringHandlerArgumentAttribute"/> class.
        /// </summary>
        /// <param name="arguments">The names of the arguments that should be passed to the handler.</param>
        /// <remarks><see langword="null"/> may be used as the name of the receiver in an instance method.</remarks>
        public InterpolatedStringHandlerArgumentAttribute(params string[] arguments)
        {
            Arguments = arguments;
        }

        /// <summary>
        /// Gets the names of the arguments that should be passed to the handler.
        /// </summary>
        /// <remarks><see langword="null"/> may be used as the name of the receiver in an instance method.</remarks>
        public string[] Arguments { get; }
    }    
}
#endif

namespace Utf8StringInterpolation
{
    internal static partial class Shims
    {
        public static bool TryFormat(this char value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
#if NET6_0_OR_GREATER
            return new Rune(value).TryEncodeToUtf8(utf8Destination, out bytesWritten);
#else
            Span<char> xs = stackalloc char[1];
            xs[0] = value;

            var count = Encoding.UTF8.GetByteCount(xs);
            if (utf8Destination.Length < count)
            {
                bytesWritten = 0;
                return false;
            }
            else
            {
                bytesWritten = Encoding.UTF8.GetBytes(xs, utf8Destination);
                return true;
            }
#endif
        }
                
#if !NET8_0_OR_GREATER
        public static bool TryFormat(this DateTime value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
            Span<char> charDest = stackalloc char[256];
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

            bytesWritten = Encoding.UTF8.GetBytes(charDest.Slice(0, charWritten), utf8Destination);
            return true;
        }

        public static bool TryFormat(this DateTimeOffset value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
            Span<char> charDest = stackalloc char[256];
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

            bytesWritten = Encoding.UTF8.GetBytes(charDest.Slice(0, charWritten), utf8Destination);
            return true;
        }

        public static bool TryFormat(this TimeSpan value, Span<byte> utf8Destination, out int bytesWritten, string? format, IFormatProvider? formatProvider)
        {
            Span<char> charDest = stackalloc char[256];
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

            bytesWritten = Encoding.UTF8.GetBytes(charDest.Slice(0, charWritten), utf8Destination);
            return true;
        }
#endif
    }
}
