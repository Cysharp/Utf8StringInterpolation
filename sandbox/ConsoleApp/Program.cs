using Cysharp.Text;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

//Console.WriteLine($"Foo{100,5}Bar");



var format = "Prime numbers less than 10: {00 , 01 }, {01  ,02  }, {2,3:D}, {3  ,4: X }";
var expected = string.Format(format, 2, 3, 5, 7);

Console.WriteLine(expected);

//var actual = Utf8String.Format($"Prime numbers less than 10: {2,01}, {3,02}, {5,3:D}, {7,4: X}");
//actual.Should().Be(expected);




// true.TryFormat

// var bytes = Cysharp.Text.Utf8String.Join(" - ", new[] { 1, 10, 100 });



//var bytes = Utf8String.Format($"foo{DateTime.Now:yy,あ:mm,:dd}Bar");

//Console.WriteLine(Encoding.UTF8.GetString(bytes));

//var i = 100;
//Utf8String.Format(writer, $"Today {DateTime.Now:O}.");


// check: DateTimeOffset TryFormat
// check: TimeSpan TryFormat
// ZString.CreateUtf8StringBuilder().



//DateTime.Now.TryFormat(writer.GetSpan(), out var bytesWritten, "yyyy:mm:ss");

//writer.Advance(bytesWritten);

//var builder = Utf8String.CreateBuilder(writer);

//builder.AppendLiteral("Hello, ");
//builder.AppendFormat($"Today is {DateTime.Now}.");

//builder.Flush();

//var span = writer.WrittenSpan;
//Console.WriteLine(Encoding.UTF8.GetString(span));

enum Fruit
{
    Apple, Grape, Orange
}