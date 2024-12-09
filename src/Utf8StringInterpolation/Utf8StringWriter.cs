#pragma warning disable CA2014 // Do not use stackalloc in loops

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Utf8StringInterpolation.Internal;

#if NET6_0_OR_GREATER
using System.Diagnostics;
using System.Text.Unicode;
#endif

namespace Utf8StringInterpolation;

[InterpolatedStringHandler]
public ref partial struct Utf8StringWriter<TBufferWriter>
    where TBufferWriter : IBufferWriter<byte>
{
    static readonly byte[] NewLineUtf8Bytes = Encoding.UTF8.GetBytes(Environment.NewLine);

    const int DefaultInitialSize = 256;
    const int GuessedLengthPerHole = 11;

    Span<byte> destination;
    int allocatedDestinationSize;
    TBufferWriter bufferWriter;
    int currentWritten;
    IFormatProvider? formatProvider;
    bool calculateStringJustSize;

    public TBufferWriter GetBufferWriter() => bufferWriter;
    public int GetCurrentWritten() => currentWritten;
    public int GetAllocatedDestinationSize() => allocatedDestinationSize;

    // create directly
    public Utf8StringWriter(TBufferWriter bufferWriter, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = bufferWriter;
        this.formatProvider = formatProvider;
        TryGrow(DefaultInitialSize);
    }

    // from interpolated string
    public Utf8StringWriter(int literalLength, int formattedCount, TBufferWriter bufferWriter, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = bufferWriter;
        this.formatProvider = formatProvider;
        var initialSize = literalLength + (formattedCount * GuessedLengthPerHole);
        TryGrow(initialSize);
    }

    //  from byte[] Format, use ThreadStatic ArrayBufferWriter
    public Utf8StringWriter(int literalLength, int formattedCount, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = (TBufferWriter)(object)ArrayBufferWriterPool.GetThreadStaticInstance();
        this.formatProvider = formatProvider;
        var initialSize = literalLength + (formattedCount * GuessedLengthPerHole);
        TryGrow(initialSize);
    }

    //  from bool TryFormat, use ThreadStatic ArrayBufferWriter
    public Utf8StringWriter(int literalLength, int formattedCount, Span<byte> destination, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = (TBufferWriter)(object)ArrayBufferWriterPool.GetThreadStaticInstance();
        this.formatProvider = formatProvider;
        this.destination = destination;
        this.allocatedDestinationSize = destination.Length;
        this.calculateStringJustSize = true;
        this.bufferWriter.GetSpan(destination.Length); // allocate dummy
    }

    // from AppendFormat extension methods.
    public Utf8StringWriter(int literalLength, int formattedCount, scoped ref Utf8StringWriter<TBufferWriter> parent)
    {
        parent.ClearState();
        this.bufferWriter = parent.bufferWriter;
        this.formatProvider = parent.formatProvider;
        var initialSize = literalLength + (formattedCount * GuessedLengthPerHole);
        TryGrow(initialSize);
    }

    public void AppendLiteral(string s)
    {
        AppendString(s.AsSpan());
    }

    public void AppendWhitespace(int count)
    {
        TryGrow(count);
        destination.Slice(0, count).Fill((byte)' ');
        destination = destination.Slice(count);
        currentWritten += count;
    }

    public void Append(string? s)
    {
        if (s == null) return;
        AppendLiteral(s);
    }

    public void Append(char c)
    {
        Span<char> xs = stackalloc char[1];
        xs[0] = c;
        AppendFormatted(xs);
    }

    public void Append(char c, int repeatCount)
    {
        Span<char> xs = stackalloc char[1];
        xs[0] = c;
        Span<byte> ys = stackalloc byte[16];
        var written = Encoding.UTF8.GetBytes(xs, ys);
        if (written == 1 && ys[0] == (byte)c)
        {
            TryGrow(repeatCount);
            destination.Slice(0, repeatCount).Fill((byte)c);
            destination = destination.Slice(repeatCount);
            currentWritten += repeatCount;
        }
        else
        {
            var encodedChar = ys.Slice(0, written);
            var total = repeatCount * written;
            TryGrow(total);
            for (int i = 0; i < repeatCount; i++)
            {
                encodedChar.CopyTo(destination);
                destination = destination.Slice(written);
            }
            currentWritten += total;
        }
    }

    public void AppendUtf8(scoped ReadOnlySpan<byte> utf8String)
    {
        if (utf8String.Length == 0) return;
        TryGrow(utf8String.Length);
        utf8String.CopyTo(destination);
        var bytesWritten = utf8String.Length;
        destination = destination.Slice(bytesWritten);
        currentWritten += bytesWritten;
    }

    public void AppendFormatted(scoped ReadOnlySpan<byte> utf8String)
    {
        AppendUtf8(utf8String);
    }

    public void AppendFormatted(scoped ReadOnlySpan<char> s)
    {
        AppendString(s);
    }

    int AppendString(scoped ReadOnlySpan<char> s)
    {
        if (s.Length == 0) return 0;
        var max = GetStringByteCount(s);
        TryGrow(max);
        var bytesWritten = Encoding.UTF8.GetBytes(s, destination);
        destination = destination.Slice(bytesWritten);
        currentWritten += bytesWritten;
        return bytesWritten;
    }

    public void AppendFormatted(string value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            AppendLiteral(value);
            return;
        }

        // add left whitespace
        if (alignment > 0)
        {
            var max = GetStringByteCount(value.AsSpan());
            var rentArray = ArrayPool<byte>.Shared.Rent(max);
            var buffer = rentArray.AsSpan();
            var bytesWritten = Encoding.UTF8.GetBytes(value.AsSpan(), buffer);

            int charCount;
#if NETSTANDARD2_0
            unsafe
            {
                fixed (byte* ptr = &buffer[0])
                {
                    charCount = Encoding.UTF8.GetCharCount(ptr, bytesWritten);
                }
            }
#else            
            charCount = Encoding.UTF8.GetCharCount(buffer.Slice(0, bytesWritten));
#endif
            var space = alignment - charCount;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            ArrayPool<byte>.Shared.Return(rentArray);
        }
        else
        {
            // add right whitespace
            var max = GetStringByteCount(value.AsSpan());
            TryGrow(max);
            var bytesWritten = Encoding.UTF8.GetBytes(value.AsSpan(), destination);

            int charCount;
#if NETSTANDARD2_0
            unsafe
            {
                fixed (byte* ptr = &destination[0])
                {
                    charCount = Encoding.UTF8.GetCharCount(ptr, bytesWritten);
                }
            }
#else
            charCount = Encoding.UTF8.GetCharCount(destination.Slice(0, bytesWritten));
#endif
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = charCount + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }

    public void AppendFormatted<T>(T? value, int alignment = 0, string? format = null)
        where T : struct
    {
        if (!value.HasValue)
        {
            if (alignment != 0)
            {
                AppendWhitespace(alignment < 0 ? -alignment : alignment);
            }
            return;
        }
        AppendFormatted(value.GetValueOrDefault(), alignment, format);
    }

    void AppendFormattedCore<T>(T value, int alignment = 0, string? format = null)
    {
        // no alignment or add right whitespace
        if (alignment <= 0)
        {
            int bytesWritten;

#if NET8_0_OR_GREATER
            if (typeof(T).IsEnum)
            {
                bytesWritten = AppendEnum(value, format);
                goto WRITE_WHITESPACE;
            }

            // .NET 8
            if (value is IUtf8SpanFormattable)
            {
                // constrained call avoiding boxing for value types
                while (!((IUtf8SpanFormattable)value).TryFormat(destination, out bytesWritten, format, formatProvider))
                {
                    GrowCore(0);
                }
                destination = destination.Slice(bytesWritten);
                currentWritten += bytesWritten;
                goto WRITE_WHITESPACE;
            }
#endif

#if NET6_0_OR_GREATER
            // .NET 6, better than ToString
            if (value is ISpanFormattable)
            {
                bytesWritten = AppendSpanFormattable(value, format);
                goto WRITE_WHITESPACE;
            }
#endif

            // String fallbacks
            string? s;
            if (value is IFormattable)
            {
                s = ((IFormattable)value).ToString(format, formatProvider);
            }
            else
            {
                s = value?.ToString();
            }

            bytesWritten = AppendString(s.AsSpan());
            goto WRITE_WHITESPACE;

        WRITE_WHITESPACE:
            if (alignment != 0)
            {
                var space = bytesWritten + alignment;
                if (space < 0)
                {
                    AppendWhitespace(-space);
                }
            }
        }
        else
        {
            // add left whitespace
            // first, write to temp buffer
            using var buffer = Utf8String.CreateWriter(out var builder);
            builder.AppendFormatted(value, 0, format); // no alignment
            builder.Flush();

            var bytesWritten = buffer.WrittenCount;

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.WrittenSpan.CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
        }
    }

#if NET6_0_OR_GREATER

    int AppendSpanFormattable<T>(T value, string? format)
    {
        Debug.Assert(value is ISpanFormattable);

        Span<char> charDest = stackalloc char[256];
        int charWritten;
        while (!((ISpanFormattable)value).TryFormat(charDest, out charWritten, format, formatProvider))
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

        var slice = charDest.Slice(0, charWritten);
        var count = Encoding.UTF8.GetByteCount(slice);
        TryGrow(count);
        var bytesWritten = Encoding.UTF8.GetBytes(slice, destination);
        destination = destination.Slice(bytesWritten);
        currentWritten += bytesWritten;
        return bytesWritten;
    }

#endif

#if NET8_0_OR_GREATER

    int AppendEnum<T>(T value, string? format)
    {
        // Enum.TryFormat is constrained, TryWriteInterpolatedStringHandler uses unsconstrained version internally.
        Span<byte> dest = stackalloc byte[256];
        var written = 0;
        while (!Utf8.TryWrite(dest, formatProvider, $"{value}", out written))
        {
            if (dest.Length < 512)
            {
                dest = stackalloc byte[dest.Length * 2];
            }
            else
            {
                dest = new byte[dest.Length * 2]; // too large
            }
        }

        AppendUtf8(dest.Slice(0, written));
        return written;
    }

#endif

    public void AppendLine()
    {
        AppendUtf8(NewLineUtf8Bytes);
    }

    public void AppendLine(string s)
    {
        AppendLiteral(s);
        AppendUtf8(NewLineUtf8Bytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TryGrow(int len)
    {
        if (destination.Length < len)
        {
            GrowCore(len);
        }
    }

    void GrowCore(int len)
    {
        if (currentWritten != 0)
        {
            bufferWriter.Advance(currentWritten);
            currentWritten = 0;
        }
        destination = bufferWriter.GetSpan(Math.Max(allocatedDestinationSize * 2, len));
        allocatedDestinationSize = destination.Length;
    }

    public void ClearState()
    {
        Flush();
        destination = default;
        allocatedDestinationSize = 0;
        currentWritten = 0;
    }

    int GetStringByteCount(scoped ReadOnlySpan<char> str)
    {
        return calculateStringJustSize ? Encoding.UTF8.GetByteCount(str) : Encoding.UTF8.GetMaxByteCount(str.Length);
    }

    public void Flush()
    {
        if (currentWritten != 0)
        {
            bufferWriter.Advance(currentWritten);
            currentWritten = 0;
        }
    }

    public void Dispose()
    {
        if (bufferWriter != null && destination.Length != 0)
        {
            Flush();
        }
        bufferWriter = default!;
        destination = default!;
    }
}

public static class Utf8StringExtensions
{
    // hack for use nested InterpolatedStringHandler.

    public static void AppendFormat<TBufferWriter>(
        this ref Utf8StringWriter<TBufferWriter> parent,
        [InterpolatedStringHandlerArgument("parent")] ref Utf8StringWriter<TBufferWriter> format)
        where TBufferWriter : IBufferWriter<byte>
    {
        format.Flush();
    }

    public static void AppendLine<TBufferWriter>(
        this ref Utf8StringWriter<TBufferWriter> parent,
        [InterpolatedStringHandlerArgument("parent")] ref Utf8StringWriter<TBufferWriter> format)
        where TBufferWriter : IBufferWriter<byte>
    {
        format.Flush();
        parent.AppendLine();
    }
}