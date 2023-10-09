using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cysharp.Text;

[InterpolatedStringHandler]
public ref partial struct Utf8StringBuilder<TBufferWriter>
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

    internal TBufferWriter GetBufferWriter() => bufferWriter;
    internal int GetAllocatedDestinationSize() => allocatedDestinationSize;

    // create directly
    public Utf8StringBuilder(TBufferWriter bufferWriter, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = bufferWriter;
        this.formatProvider = formatProvider;
        TryGrow(DefaultInitialSize);
    }

    // from interpolated string
    public Utf8StringBuilder(int literalLength, int formattedCount, TBufferWriter bufferWriter, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = bufferWriter;
        this.formatProvider = formatProvider;
        var initialSize = literalLength + (formattedCount * GuessedLengthPerHole);
        TryGrow(initialSize);
    }

    //  from byte[] Format, use ThreadStatic ArrayBufferWriter
    public Utf8StringBuilder(int literalLength, int formattedCount, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = (TBufferWriter)(object)ArrayBufferWriterPool.GetThreadStaticInstance();
        this.formatProvider = formatProvider;
        var initialSize = literalLength + (formattedCount * GuessedLengthPerHole);
        TryGrow(initialSize);
    }

    //  from bool Tryormat, use ThreadStatic ArrayBufferWriter
    public Utf8StringBuilder(int literalLength, int formattedCount, Span<byte> destination, IFormatProvider? formatProvider = default)
    {
        this.bufferWriter = (TBufferWriter)(object)ArrayBufferWriterPool.GetThreadStaticInstance();
        this.formatProvider = formatProvider;
        this.destination = destination;
        this.allocatedDestinationSize = destination.Length;
        this.bufferWriter.GetSpan(destination.Length); // allocate dummy
    }

    // from AppendFormat extension methods.
    public Utf8StringBuilder(int literalLength, int formattedCount, scoped ref Utf8StringBuilder<TBufferWriter> parent)
    {
        parent.ClearState();
        this.bufferWriter = parent.bufferWriter;
        this.formatProvider = parent.formatProvider;
        var initialSize = literalLength + (formattedCount * GuessedLengthPerHole);
        TryGrow(initialSize);
    }

    public void AppendLiteral(string s)
    {
        AppendLiteral(s.AsSpan());
    }

    public void AppendLiteral(scoped ReadOnlySpan<char> s)
    {
        var max = Encoding.UTF8.GetMaxByteCount(s.Length);
        TryGrow(max);
        var bytesWritten = Encoding.UTF8.GetBytes(s, destination);
        destination = destination.Slice(bytesWritten);
        currentWritten += bytesWritten;
    }

    public void AppendWhitespace(int count)
    {
        TryGrow(count);
        destination.Slice(0, count).Fill((byte)' ');
        destination = destination.Slice(count);
        currentWritten += count;
    }

    public void Append(string s)
    {
        AppendLiteral(s);
    }

    public void Append(char c)
    {
        Span<char> xs = stackalloc char[1];
        xs[0] = c;
        AppendLiteral(xs);
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
            var encodedChar = ys.Slice(written);
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
        TryGrow(utf8String.Length);
        utf8String.CopyTo(destination);
        var bytesWritten = utf8String.Length;
        destination = destination.Slice(bytesWritten);
        currentWritten += bytesWritten;
    }

#if NET8_0_OR_GREATER

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null)
        where T : IUtf8SpanFormattable
    {
        var bytesWritten = 0;
        while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
        {
            GrowCore(0);
        }
        destination = destination.Slice(bytesWritten);
        currentWritten += bytesWritten;
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
    void TryGrow(int len)
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

    void ClearState()
    {
        Flush();
        destination = default;
        allocatedDestinationSize = 0;
        currentWritten = 0;
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
        Flush();
    }
}

public static class Utf8StringExtensions
{
    // hack for use nested InterpolatedStringHandler.

    public static void AppendFormat<TBufferWriter>(
        this ref Utf8StringBuilder<TBufferWriter> parent,
        [InterpolatedStringHandlerArgument("parent")] ref Utf8StringBuilder<TBufferWriter> format)
        where TBufferWriter : IBufferWriter<byte>
    {
        format.Flush();
    }

    public static void AppendLine<TBufferWriter>(
        this ref Utf8StringBuilder<TBufferWriter> parent,
        [InterpolatedStringHandlerArgument("parent")] ref Utf8StringBuilder<TBufferWriter> format)
        where TBufferWriter : IBufferWriter<byte>
    {
        format.Flush();
        parent.AppendLine();
    }
}