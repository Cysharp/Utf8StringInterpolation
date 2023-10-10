using FluentAssertions;
using FluentAssertions.Primitives;
using System.Text;

public static class Extensions
{
    public static StringAssertions Should(this byte[] xs)
    {
        return Encoding.UTF8.GetString(xs).Should();
    }
}