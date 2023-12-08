using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Utf8StringInterpolation.Internal;

namespace Utf8StringInterpolation;

public static class Utf8String
{
    // Format API

    public static byte[] Format(ref Utf8StringWriter<ArrayBufferWriter<byte>> format)
    {
        format.Flush();
        var writer = format.GetBufferWriter();
        return writer.WrittenSpan.ToArray();
    }

    public static void Format<TBufferWriter>(
         TBufferWriter bufferWriter,
        [InterpolatedStringHandlerArgument("bufferWriter")] ref Utf8StringWriter<TBufferWriter> format)
        where TBufferWriter : IBufferWriter<byte>
    {
        format.Flush();
    }

    public static bool TryFormat(
         Span<byte> destination,
         out int bytesWritten,
        [InterpolatedStringHandlerArgument("destination")] ref Utf8StringWriter<ArrayBufferWriter<byte>> format)
    {
        var written = format.GetCurrentWritten();
        format.Flush();
        if (destination.Length != format.GetAllocatedDestinationSize())
        {
            bytesWritten = 0;
            return false;
        }
        bytesWritten = written;
        return true;
    }

    // Writer API

    public static Utf8StringWriter<TBufferWriter> CreateWriter<TBufferWriter>(TBufferWriter bufferWriter, IFormatProvider? formatProvider = null)
        where TBufferWriter : IBufferWriter<byte>
    {
        return new Utf8StringWriter<TBufferWriter>(0, 0, bufferWriter, formatProvider);
    }

    public static Utf8StringBuffer CreateWriter(out Utf8StringWriter<ArrayBufferWriter<byte>> stringWriter, IFormatProvider? formatProvider = null)
    {
        var writer = ArrayBufferWriterPool.Rent();
        stringWriter = new Utf8StringWriter<ArrayBufferWriter<byte>>(0, 0, writer, formatProvider);
        return new Utf8StringBuffer(writer);
    }

    // Concat, byte[], TBufferWriter

    public static byte[] Concat(params string?[] values)
    {
        using var buffer = CreateWriter(out var builder);
        foreach (var item in values)
        {
            if (item == null) continue;
            builder.AppendLiteral(item);
        }
        builder.Flush();
        return buffer.ToArray();
    }

    public static void Concat<TBufferWriter>(TBufferWriter bufferWriter, params string?[] values)
        where TBufferWriter : IBufferWriter<byte>
    {
        using var builder = CreateWriter(bufferWriter); // Dispose calls Flush
        foreach (var item in values)
        {
            if (item == null) continue;
            builder.AppendLiteral(item);
        }
    }

    public static byte[] Concat<T>(IEnumerable<T> values)
    {
        using var buffer = CreateWriter(out var builder);
        foreach (var item in values)
        {
            builder.AppendFormatted(item);
        }
        builder.Flush();
        return buffer.ToArray();
    }

    public static void Concat<TBufferWriter, T>(TBufferWriter bufferWriter, IEnumerable<T> values)
        where TBufferWriter : IBufferWriter<byte>
    {
        using var builder = CreateWriter(bufferWriter); // Dispose calls Flush
        foreach (var item in values)
        {
            builder.AppendFormatted(item);
        }
    }

    // Join, byte[], TBufferWriter

    public static byte[] Join(string separator, params string?[] values)
    {
        using var buffer = CreateWriter(out var builder);

        var separatorBufferSize = Encoding.UTF8.GetMaxByteCount(separator.Length);
        var separatorBuffer = ArrayPool<byte>.Shared.Rent(separatorBufferSize);
        var utf8SeparatorLength = Encoding.UTF8.GetBytes(separator.AsSpan(), separatorBuffer);
        var utf8Separator = separatorBuffer.AsSpan(0, utf8SeparatorLength);

        var first = true;
        foreach (var item in values)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                builder.AppendUtf8(utf8Separator);
            }
            builder.Append(item);
        }

        builder.Flush();
        ArrayPool<byte>.Shared.Return(separatorBuffer);
        return buffer.ToArray();
    }

    public static void Join<TBufferWriter>(TBufferWriter bufferWriter, string separator, params string?[] values)
        where TBufferWriter : IBufferWriter<byte>
    {
        using var builder = CreateWriter(bufferWriter); // Dispose calls Flush

        var separatorBufferSize = Encoding.UTF8.GetMaxByteCount(separator.Length);
        var separatorBuffer = ArrayPool<byte>.Shared.Rent(separatorBufferSize);
        var utf8SeparatorLength = Encoding.UTF8.GetBytes(separator.AsSpan(), separatorBuffer);
        var utf8Separator = separatorBuffer.AsSpan(0, utf8SeparatorLength);

        var first = true;
        foreach (var item in values)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                builder.AppendUtf8(utf8Separator);
            }
            builder.Append(item);
        }
        
        ArrayPool<byte>.Shared.Return(separatorBuffer);
    }

    public static byte[] Join<T>(string separator, IEnumerable<T> values)
    {
        using var buffer = CreateWriter(out var builder);

        var separatorBufferSize = Encoding.UTF8.GetMaxByteCount(separator.Length);
        var separatorBuffer = ArrayPool<byte>.Shared.Rent(separatorBufferSize);
        var utf8SeparatorLength = Encoding.UTF8.GetBytes(separator.AsSpan(), separatorBuffer);
        var utf8Separator = separatorBuffer.AsSpan(0, utf8SeparatorLength);

        var first = true;
        foreach (var item in values)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                builder.AppendUtf8(utf8Separator);
            }
            builder.AppendFormatted(item);
        }
        builder.Flush();
        
        ArrayPool<byte>.Shared.Return(separatorBuffer);
        return buffer.ToArray();
    }

    public static void Join<TBufferWriter, T>(TBufferWriter bufferWriter, string separator, IEnumerable<T> values)
        where TBufferWriter : IBufferWriter<byte>
    {
        using var builder = CreateWriter(bufferWriter); // Dispose calls Flush

        var separatorBufferSize = Encoding.UTF8.GetMaxByteCount(separator.Length);
        var separatorBuffer = ArrayPool<byte>.Shared.Rent(separatorBufferSize);
        var utf8SeparatorLength = Encoding.UTF8.GetBytes(separator.AsSpan(), separatorBuffer);
        var utf8Separator = separatorBuffer.AsSpan(0, utf8SeparatorLength); 

        var first = true;
        foreach (var item in values)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                builder.AppendUtf8(utf8Separator);
            }
            builder.AppendFormatted(item);
        }
        
        ArrayPool<byte>.Shared.Return(separatorBuffer);
    }
}
