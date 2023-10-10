


using System.Buffers;
using System.Text;
using Utf8StringInterpolation;

var a = Utf8String.Format($"Double value is {123.456789:.###}");
var b = Utf8String.Format($"Today is {DateTime.Now:yyyy-MM-dd}");
Console.WriteLine(Encoding.UTF8.GetString(a));
Console.WriteLine(Encoding.UTF8.GetString(b));