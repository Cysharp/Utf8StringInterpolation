# Utf8StringInterpolation
[![GitHub Actions](https://github.com/Cysharp/Utf8StringInterpolation/workflows/Build-Debug/badge.svg)](https://github.com/Cysharp/Utf8StringInterpolation/actions) [![Releases](https://img.shields.io/github/release/Cysharp/Utf8StringInterpolation.svg)](https://github.com/Cysharp/Utf8StringInterpolation/releases)
[![NuGet package](https://img.shields.io/nuget/v/Utf8StringInterpolation.svg)](https://nuget.org/packages/Utf8StringInterpolation)

Successor of [ZString](https://github.com/Cysharp/ZString/); UTF8 based zero allocation high-peformance String Interpolation and StringBuilder.

[C# 10.0 Improved Interpolated Strings](https://github.com/dotnet/csharplang/blob/main/proposals/csharp-10.0/improved-interpolated-strings.md) gets extremely high performance in string generation by deconstructing format strings at compile time and executing the most optimal calls. However, this applies only to String (UTF16) and cannot be applied to the generation of UTF8 strings. `Utf8StringInterpolation` has achieved the generation of `byte[]` as UTF8 and direct writing to `IBufferWriter<byte>` with zero allocation and at peak performance, thanks to its **compiler support custom InterpolatedStringHandler** optimized for UTF8 writing, all while retaining the user-friendly syntax and compiler support of InterpolatedString. With the addition of **IUtf8SpanFormattable in .NET 8**, we have achieved optimized Utf8 writing for numerous value types.

```csharp
using Utf8StringInterpolation;

// Create UTF8 encoded string directly(without encoding).
byte[] utf8 = Utf8String.Format($"Hello, {name}, Your id is {id}!");

// write to IBufferWriter<byte>(for example ASP.NET HttpResponse.BodyWriter)
var bufferWriter = new ArrayBufferWriter<byte>();
Utf8String.Format(bufferWriter, $"Today is {DateTime.Now:yyyy-MM-dd}"); // support format

// write to FileStream directly
using var fs = File.OpenWrite("foo.txt");
var pipeWriter = PipeWriter.Create(fs);
Utf8String.Format(pipeWriter, $"Foo: {id,10} {name,-5}"); // support alignment

// like a StringBuilder
var writer = Utf8String.CreateWriter(bufferWriter);
writer.Append("My Name...");
writer.AppendFormat($"is...? {name}");
writer.AppendLine();
writer.Flush();

// Join, Concat methods
var seq = Enumerable.Range(1, 10);
byte[] utf8seq = Utf8String.Join(", ", seq);
```

Modern C# treats `ReadOnlySpan<byte>` as Utf8. Additionally, modern C# writes to the output using `IBufferWriter<byte>`. `Utf8StringInterpolation` provides a variety of utilities and writers optimized for use with them. In .NET 8, the new [IUtf8SpanFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iutf8spanformattable) is used to directly write values to UTF8.

## Getting Started

This library is distributed via NuGet, supporting `.NET Standard 2.0`, `.NET Standard 2.1`, `.NET 6(.NET 7)` and `.NET 8` or above.

PM> Install-Package [Utf8StringInterpolation](https://www.nuget.org/packages/Utf8StringInterpolation)

```csharp
// `Format(ref Utf8StringWriter)` accepts string interpolation
Utf8String.Format($"Hello, {name}, Your id is {id}!");

// Interpolated string compiles like following.
var writer = new Utf8StringWriter(literalLength: 20, formattedCount: 2);
writer.AppendLiteral("Hello, ");
writer.AppendFormatted<T>(name);
writer.AppendLiteral(", You id is ");
writer.AppendFormatted<T>(id);
writer.AppendLiteral("!");

// internal struct writer write value to utf8 directly without boxing.
[InterpolatedStringHandler]
public ref struct Utf8StringWriter<TBufferWriter> where TBufferWriter : IBufferWriter<byte>
{
    TBufferWriter bufferWriter; // when buffer is full, advance and get more buffer
    Span<byte> buffer;          // current write buffer

    public void AppendLiteral(string value)
    {
        // encode string literal to Utf8 buffer directly
        var bytesWritten = Encoding.UTF8.GetBytes(value, buffer);
        buffer = buffer.Slice(bytesWritten);
    }

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null)
        where T : IUtf8SpanFormattable
    {
        // write value to Utf8 buffer directly
        while (!value.TryFormat(buffer, out bytesWritten, format))
        {
            Grow();
        }
        buffer = buffer.Slice(bytesWritten);
    }
}
```

The actual Utf8StringWriter accepts various types, uses the Utf8Formatter in .NET 6 environments that do not support IUtf8SpanFormattable, and is designed with optimizations such as more efficient buffer usage.

## Utf8String methods

Entry point is `Utf8StringInterpolation.Utf8String`. You can use static methods or create writer.

When the argument is `ref Utf8StringWriter`, you can pass a string interpolation expression like `$"{...}"`.

### Utf8String.Format

Format is a one-shot method. The Format that takes an `IBufferWriter<byte>` performs writing based on the bufferWriter and finally flushes (Advance). The Format that returns a `byte[]` writes using its internally pooled `ArrayBufferWriter` and generates a final `byte[]`. TryFormat writes to the specified `Span<byte>`, and if the length is insufficient, it returns false.

```csharp
byte[] Format(ref Utf8StringWriter format)
void Format<TBufferWriter>(TBufferWriter bufferWriter, ref Utf8StringWriter format)
    where TBufferWriter : IBufferWriter<byte>
bool TryFormat(Span<byte> destination, out int bytesWritten, ref Utf8StringWriter format)
```

### Utf8String.Concat, Join

`Utf8String.Concat` and `Utf8String.Join` are similar to `String.Concat` and `String.Join`, but they write everything in UTF8. Like Format, there are two overloads: one that writes to `IBufferWriter<byte>` and another that writes to the internally pooled `ArrayBufferWriter` and then returns a `byte[]`.

```csharp
// Concat overloads, return byte[] or receive IBufferWriter<byte>.
byte[] Concat(params string?[] values)
void Concat<TBufferWriter>(TBufferWriter bufferWriter, params string?[] values)
    where TBufferWriter : IBufferWriter<byte>
byte[] Concat<T>(IEnumerable<T> values)
void Concat<TBufferWriter, T>(TBufferWriter bufferWriter, IEnumerable<T> values)
    where TBufferWriter : IBufferWriter<byte>
```

```csharp
// Join overloads, return byte[] or receive IBufferWriter<byte>.
byte[] Join(string separator, params string?[] values)
void Join<TBufferWriter>(TBufferWriter bufferWriter, string separator, params string?[] values)
    where TBufferWriter : IBufferWriter<byte>
byte[] Join<T>(string separator, IEnumerable<T> values)
void Join<TBufferWriter, T>(TBufferWriter bufferWriter, string separator, IEnumerable<T> values)
    where TBufferWriter : IBufferWriter<byte>
```

## Utf8String.CreateWriter

`Utf8String.CreateWriter` allows you to obtain a `Utf8StringWriter` that can write continuously, similar to `StringBuilder`. Each `Append***` method can be used in a manner similar to `StringBuilder`. However, unlike `StringBuilder`, the buffer is managed by the provided BufferWriter, which is why it's named Writer. For performance reasons, the Append methods don't always flush (Advance) to the internal BufferWriter. By manually calling Flush, you ensure that Advance is invoked. Additionally, Dispose also calls Flush.

```csharp
var writer = Utf8String.CreateWriter(bufferWriter);

// call each append methods.
writer.Append("foo");
writer.AppendFormat($"bar {Guid.NewGuid}");

// finally call Flush(or Dispose)
writer.Flush();
```

When writing larger messages, it's advisable to periodically call Flush. At those times, for instance, if using a `PipeWriter`, you can invoke `pipeWriter.FlushAsync()` to stream the write operations without holding onto a large buffer internally.

`CreateWriter` has two overloads. One takes an `IBufferWriter<byte>`, and the other uses the internally pooled buffer.

```csharp
Utf8StringWriter<TBufferWriter> CreateWriter<TBufferWriter>(TBufferWriter bufferWriter, IFormatProvider? formatProvider = null)
    where TBufferWriter : IBufferWriter<byte>

Utf8StringBuffer CreateWriter(out Utf8StringWriter<ArrayBufferWriter<byte>> stringWriter, IFormatProvider? formatProvider = null)
```

`Utf8StringBuffer` is convenient because it uses an internally pooled buffer. Therefore, when you just want to obtain a `byte[]`, for example, there's no need to separately prepare or manage an `IBufferWriter<byte>`.

```csharp
// buffer must Dispose after used(recommend to use using)
using var buffer = Utf8String.CreateWriter(out var writer);

// call each append methods.
writer.Append("foo");
writer.AppendFormat($"bar {Guid.NewGuid}");

// finally call Flush(no need to call Dispose for writer)
writer.Flush();

// copy to written byte[]
var bytes = buffer.ToArray();

// or copy to other IBufferWriter<byte>, get ReadOnlySpan<byte>
buffer.CopyTo(otherBufferWriter);
var writtenData = buffer.WrittenSpan;
```

### Utf8StringWriter

`AppendLiteral`, `AppendFormatted` is called from compiler generated code.

```csharp
void AppendLiteral(string s)
void AppendWhitespace(int count)
void Append(string? s)
void Append(char c)
void Append(char c, int repeatCount)
void AppendUtf8(scoped ReadOnlySpan<byte> utf8String)
void AppendFormatted(scoped ReadOnlySpan<byte> utf8String)
void AppendFormatted(scoped ReadOnlySpan<char> s)
void AppendFormatted(string value, int alignment = 0, string? format = null)
void AppendFormatted<T>(T value, int alignment = 0, string? format = null)
void AppendFormat(ref Utf8StringWriter format) // extension method
void AppendLine(ref Utf8StringWriter format)   // extension method
void AppendLine()
void AppendLine(string s)
void Flush()
void Dispose() // call Flush and dereference buffer and bufferwriter
```

### Utf8StringBuffer

`Utf8StringBuffer` can obtain from `Utf8String.CreateWriter` overload. Since it holds an internally pooled buffer, it's necessary to call `Dispose` to release the buffer once obtained.

```csharp
int WrittenCount { get; }
ReadOnlySpan<byte> WrittenSpan { get; }
ReadOnlyMemory<byte> WrittenMemory { get; }
byte[] ToArray()
void CopyTo<TBufferWriter>(TBufferWriter bufferWriter)
    where TBufferWriter : IBufferWriter<byte>
Dispose()
```

## Format strings

The formatting in string interpolation can use alignment and format just like regular .NET. In .NET 8, all formatting follows the standard format. For more details, please refer to the .NET documentation on [formatting-types](https://learn.microsoft.com/en-us/dotnet/standard/base-types/formatting-types).

However, this is only the case for .NET 8 and above where `IUtf8SpanFormattable` is implemented. In .NET Standard 2.1, .NET 6 (and .NET 7), UTF8 writing of values is performed using [Utf8Formatter.TryFormat](https://learn.microsoft.com/en-us/dotnet/api/system.buffers.text.utf8formatter.tryformat). This requires a specific format called [StandardFormat](https://learn.microsoft.com/en-us/dotnet/api/system.buffers.standardformat), which might not be compatible with standard formats in some cases. The supported format strings are illustrated in the Remarks section of the TryFormat documentation. The types in focus are `bool, byte, Decimal, Double, Guid, Int16, Int32, Int64, SByte, Single, UInt16, UInt32, UInt64`.

Exceptionally, `DateTime`, `DateTimeOffset`, and `TimeSpan` can use regular format specifiers even in .NET Standard 2.1 and .NET 6 (and .NET 7). This special accommodation was made because `StandardFormat` only allows for four patterns, which was found to be too limiting.

```csharp
// .NET 8 supports all numeric custom format string but .NET Standard 2.1, .NET 6(.NET 7) does not.
Utf8String.Format($"Double value is {123.456789:.###}");

// DateTime, DateTimeOffset, TimeSpan support custom format string on all target plaftorms.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
Utf8String.Format($"Today is {DateTime.Now:yyyy-MM-dd}");
```

Unity
---
This library requires C# 10.0, however currently Unity C# version is 9.0. Therefore, it is not supported at this time and will be considered when the version of Unity's C# is updated.

License
---
This library is licensed under the MIT License.
