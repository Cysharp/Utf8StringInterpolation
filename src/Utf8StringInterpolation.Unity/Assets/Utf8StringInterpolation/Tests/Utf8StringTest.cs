using System.Buffers;
using System.Text;
using NUnit.Framework;

namespace Utf8StringInterpolation.Tests;

public class Utf8StringTest
{
    [Test]
    public void Format()
    {
        const string name = "foo";
        const int id = 123;
        
        var result = Utf8String.Format($"Hello, {name}, Your id is {id}!");
        var resultString = Encoding.UTF8.GetString(result);
        Assert.That(resultString, Is.EqualTo("Hello, foo, Your id is 123!"));
    }

    [Test]
    public void Writer()
    {
        const string name = "foo";
        
        var bufferWriter = new ArrayBufferWriter<byte>();
        var writer = Utf8String.CreateWriter(bufferWriter);
        writer.Append("My Name...");
        writer.AppendFormat($"is...? {name}");
        writer.AppendLine();
        writer.Flush();
        
        var resultString = Encoding.UTF8.GetString(bufferWriter.WrittenSpan);
        Assert.That(resultString, Is.EqualTo("My Name...is...? foo\n"));
    }
}
