# Utf8String
[![GitHub Actions](https://github.com/Cysharp/Utf8String/workflows/Build-Debug/badge.svg)](https://github.com/Cysharp/Utf8String/actions) [![Releases](https://img.shields.io/github/release/Cysharp/Utf8String.svg)](https://github.com/Cysharp/Utf8String/releases)
[![NuGet package](https://img.shields.io/nuget/v/Utf8String.svg)](https://nuget.org/packages/Utf8String)

Successor of [ZString](https://github.com/Cysharp/ZString/); UTF8 based Interpolated Strings and StringBuilder.

[C# 10.0 Improved Interpolated Strings](https://github.com/dotnet/csharplang/blob/main/proposals/csharp-10.0/improved-interpolated-strings.md) gets extremely high performance in string generation by deconstructing format strings at compile time and executing the most optimal calls. However, this applies only to String (UTF16) and cannot be applied to the generation of UTF8 strings. `Utf8String` has achieved the generation of `byte[]` as UTF8 and direct writing to `IBufferWriter<byte>` with zero allocation and at peak performance, thanks to its unique InterpolatedStringHandler optimized for UTF8 writing, all while retaining the user-friendly syntax and compiler support of InterpolatedString.

```csharp
namespace Cysharp.Text;

// Create UTF8 encoded string directly(without encoding).
byte[] utf8 = Utf8String.Format($"Hello, {name}, You id is {id}!");

// write to IBufferWriter<byte>(for example ASP.NET HttpResponse.BodyWriter)
var bufferWriter = new ArrayBufferWriter<byte>();
Utf8String.Format(bufferWriter, $"Today is {DateTime.Now:yyyy-MM-dd}"); // support format

// write to FileStream directly
using var fs = File.OpenWrite("foo.txt");
var pipeWriter = PipeWriter.Create(fs);
Utf8String.Format(pipeWriter, $"Foo: {id,10} {name,-5}"); // support alignment

// like StringBuilder(similar as Writer)
var builder = Utf8String.CreateBuilder(bufferWriter);
builder.Append("My Name...");
builder.AppendFormat($"is...? {name}");
builder.AppendLine();
builder.Flush();

// Join, Concat methods
var seq = Enumerable.Range(1, 10);
byte[] utf8seq = Utf8String.Join(", ", seq);
```

Modern C# treats `ReadOnlySpan<byte>` as Utf8. Additionally, modern C# writes to the output using `IBufferWriter<byte>`. `Utf8String` provides a variety of utilities and writers optimized for use with them. In .NET 8, the new [IUtf8SpanFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iutf8spanformattable) is used to directly write values to UTF8.

Getting Started
---
This library is distributed via NuGet, supporting `.NET Standard 2.1`, `.NET 6(.NET 7)` and `.NET 8` or above.

PM> Install-Package [Utf8String](https://www.nuget.org/packages/Utf8String)

Utf8String methods
---
Entry point is `Cysharp.Text.Utf8String`. Use static methods or create builder.

`ref Utf8StringBuilder`

### Format

```csharp
byte[] Format(ref Utf8StringBuilder format)
void Format<TBufferWriter>(TBufferWriter bufferWriter, ref Utf8StringBuilder format)
    where TBufferWriter : IBufferWriter<byte>
bool TryFormat(Span<byte> destination, ref Utf8StringBuilder format)
```

### Concat, Join

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



Builder API
---

```csharp
Utf8StringBuilder<TBufferWriter> CreateBuilder<TBufferWriter>(TBufferWriter bufferWriter, IFormatProvider? formatProvider = null)
    where TBufferWriter : IBufferWriter<byte>

Utf8StringBuffer CreateBuilder(out Utf8StringBuilder<ArrayBufferWriter<byte>> builder, IFormatProvider? formatProvider = null)
```

### Utf8StringBuilder


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
void AppendFormat(ref Utf8StringBuilder format)
void AppendLine(ref Utf8StringBuilder format)
void AppendLine()
void AppendLine(string s)
void Flush()
void Dispose()
```

### Utf8StringBuffer



```csharp
int WrittenCount { get; }
ReadOnlySpan<byte> WrittenSpan { get; }
ReadOnlyMemory<byte> WrittenMemory { get; }
byte[] ToArray()
void CopyTo<TBufferWriter>(TBufferWriter bufferWriter)
    where TBufferWriter : IBufferWriter<byte>
Dispose()
```


Format strings
---



* https://learn.microsoft.com/en-us/dotnet/api/system.buffers.text.utf8formatter.tryformat
* https://learn.microsoft.com/en-us/dotnet/standard/base-types/formatting-types


Unity
---
This library requires C# 10.0, however currently Unity C# version is 9.0. Therefore, it is not supported at this time and will be considered when the version of Unity's C# is updated.

License
---
This library is licensed under the MIT License.