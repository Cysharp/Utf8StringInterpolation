using System.Buffers;
using Utf8String.Internal;

namespace Cysharp.Text;

public struct Utf8StringBuffer : IDisposable
{
    ArrayBufferWriter<byte> innerBuffer;

    internal Utf8StringBuffer(ArrayBufferWriter<byte> bufferWriter)
    {
        this.innerBuffer = bufferWriter;
    }

    public int WrittenCount => innerBuffer.WrittenCount;
    public ReadOnlySpan<byte> WrittenSpan => innerBuffer.WrittenSpan;
    public ReadOnlyMemory<byte> WrittenMemory => innerBuffer.WrittenMemory;

    public byte[] ToArray()
    {
        return innerBuffer.WrittenSpan.ToArray();
    }

    public void CopyTo<TBufferWriter>(TBufferWriter bufferWriter)
        where TBufferWriter : IBufferWriter<byte>
    {
        var written = innerBuffer.WrittenSpan;
        var dest = bufferWriter.GetSpan(written.Length);
        written.CopyTo(dest);
        bufferWriter.Advance(written.Length);
    }

    public void Dispose()
    {
        if (innerBuffer != null)
        {
            ArrayBufferWriterPool.Return(innerBuffer);
        }
        innerBuffer = null!;
    }
}