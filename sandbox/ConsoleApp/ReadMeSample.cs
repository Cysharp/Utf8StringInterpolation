
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utf8StringInterpolation;

namespace ConsoleApp;

internal class ReadMeSample
{

    void Sample(int id, string name)
    {
        // Create UTF8 encoded string directly(without encoding).
        byte[] utf8 = Utf8String.Format($"Hello, {name}, You id is {id}!");

        // write to IBufferWriter<byte>(for example ASP.NET HttpResponse.BodyWriter)
        var bufferWriter = new ArrayBufferWriter<byte>();
        Utf8String.Format(bufferWriter, $"Today is {DateTime.Now:yyyy-MM-dd}"); // support format

        // write to FileStream directly
        using var fs = File.OpenWrite("foo.txt");
        var pipeWriter = PipeWriter.Create(fs);
        Utf8String.Format(pipeWriter, $"Foo: {id,10} {name,-5}"); // support alignment

        // like StringBuilder
        var writer = Utf8String.CreateWriter(bufferWriter);
        writer.Append("My Name...");
        writer.AppendFormat($"is...? {name}");
        writer.AppendLine();
        writer.Flush();

        // Join, Concat methods
        var seq = Enumerable.Range(1, 10);
        byte[] utf8seq = Utf8String.Join(", ", seq);



    }

    void GettingStarted(string name, int id)
    {


        // `Format(ref Utf8StringWriter)` accepts string interpolation
        Utf8String.Format($"Hello, {name}, Your id is {id}!");



        // Interpolated string compiles like following.
        var writer = new Utf8StringWriter<ArrayBufferWriter<byte>>(literalLength: 20, formattedCount: 2);
        writer.AppendLiteral("Hello, ");
        writer.AppendFormatted<string>(name);
        writer.AppendLiteral(", You id is ");
        writer.AppendFormatted<int>(id);
        writer.AppendLiteral("!");


    }

    class Dummy
    {
#pragma warning disable CS0282

        [InterpolatedStringHandler]
        public ref partial struct Utf8StringWriter<TBufferWriter>
            where TBufferWriter : IBufferWriter<byte>
        {
            TBufferWriter bufferWriter; // when buffer is full, advance and get more buffer
            Span<byte> buffer;          // current write buffer

            public void AppendFormatted<T>(T value, int alignment = 0, string? format = null)
                where T : IUtf8SpanFormattable
            {
                // write value to Utf8 buffer directly
                var bytesWritten = 0;
                while (!value.TryFormat(buffer, out bytesWritten, format, provider))
                {
                    Grow();
                }
                buffer = buffer.Slice(bytesWritten);
            }
        }


        public ref partial struct Utf8StringWriter<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
        {
            IFormatProvider provider;
            void Grow() { }


            public void AppendLiteral(string value)
            {
                // encode string literal to Utf8 buffer directly
                var bytesWritten = Encoding.UTF8.GetBytes(value, buffer);
                buffer = buffer.Slice(bytesWritten);
            }

        }
    }



    void WriterSample()
    {
        var bufferWriter = new ArrayBufferWriter<byte>();

        var writer = Utf8String.CreateWriter(bufferWriter);

        // call each append methods.
        writer.Append("foo");
        writer.AppendFormat($"bar {Guid.NewGuid()}");

        // finally call Flush(or Dispose)
        writer.Flush();

        // get written utf8
        var writtenData = bufferWriter.WrittenSpan;
    }

    void WriterSample2(IBufferWriter<byte> otherBufferWriter)
    {
        // buffer must Dispose after used(recommend to use using)
        using var buffer = Utf8String.CreateWriter(out var writer);

        // call each append methods.
        writer.Append("foo");
        writer.AppendFormat($"bar {Guid.NewGuid()}");

        // finally call Flush(or Dispose)
        writer.Flush();

        // copy to written data or get byte[] / ReadOnlySpan<byte>
        buffer.CopyTo(otherBufferWriter);
        var bytes = buffer.ToArray();
        var writtenData = buffer.WrittenSpan;
    }

    void Formatting()
    {
        // .NET 8 supports all numeric custom format string but .NET Standard 2.1, .NET 6(.NET 7) does not.
        Utf8String.Format($"Double value is {123.456789:.###}");

        // DateTime, DateTimeOffset, TimeSpan support custom format string on all target plaftorms.
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
        Utf8String.Format($"Today is {DateTime.Now:yyyy-MM-dd}");


    }
}
