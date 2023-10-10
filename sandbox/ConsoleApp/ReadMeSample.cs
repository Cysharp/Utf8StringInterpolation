using Cysharp.Text;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var builder = Utf8String.CreateBuilder(bufferWriter);
        builder.Append("My Name...");
        builder.AppendFormat($"is...? {name}");
        builder.AppendLine();
        builder.Flush();

        // Join, Concat methods
        var seq = Enumerable.Range(1, 10);
        byte[] utf8seq = Utf8String.Join(", ", seq);

    }

}
