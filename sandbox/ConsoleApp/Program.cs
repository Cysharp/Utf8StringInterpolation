using Cysharp.Text;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

//Console.WriteLine($"Foo{100,5}Bar");




// true.TryFormat

// var bytes = Cysharp.Text.Utf8String.Join(" - ", new[] { 1, 10, 100 });



var bytes = Utf8String.Format($"foo{'あ',-10}Bar");

Console.WriteLine(Encoding.UTF8.GetString(bytes));

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