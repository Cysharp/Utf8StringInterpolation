using Cysharp.Text;
using FluentAssertions;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ZStringTests
{
#if NET8_0_OR_GREATER

    

    public class JoinTest
    {
        [Fact]
        public void JoinOverloads()
        {
            Utf8String.Join("_,_", new string[0]).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", new[] { 1 }).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", new[] { 1, 2 }).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", new[] { 1, 2, 3 }).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));

            // Utf8String.Join("_,_", new string[] { }.AsEnumerable()).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", new[] { 1 }.AsEnumerable()).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", new[] { 1, 2 }.AsEnumerable()).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", new[] { 1, 2, 3 }.AsEnumerable()).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));

            Utf8String.Join(",", new string[0]).Should().Be(string.Join(",", new string[0]));
            Utf8String.Join(",", new[] { 1 }).Should().Be(string.Join(",", new[] { 1 }));
            Utf8String.Join(",", new[] { 1, 2 }).Should().Be(string.Join(",", new[] { 1, 2 }));
            Utf8String.Join(",", new[] { 1, 2, 3 }).Should().Be(string.Join(",", new[] { 1, 2, 3 }));

            // Utf8String.Join(",", new string[0].AsEnumerable()).Should().Be(string.Join(",", new string[0]));
            Utf8String.Join(",", new[] { 1 }.AsEnumerable()).Should().Be(string.Join(",", new[] { 1 }));
            Utf8String.Join(",", new[] { 1, 2 }.AsEnumerable()).Should().Be(string.Join(",", new[] { 1, 2 }));
            Utf8String.Join(",", new[] { 1, 2, 3 }.AsEnumerable()).Should().Be(string.Join(",", new[] { 1, 2, 3 }));
        }

        [Fact]
        public void JoinOverloads2()
        {
            // Utf8String.Join("_,_", new string[] { }.ToList()).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", new[] { 1 }.ToList()).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", new[] { 1, 2 }.ToList()).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", new[] { 1, 2, 3 }.ToList()).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));

            Utf8String.Join("_,_", (IList<int>)new int[] { }).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", (IList<int>)new[] { 1 }).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", (IList<int>)new[] { 1, 2 }).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", (IList<int>)new[] { 1, 2, 3 }).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));

            Utf8String.Join("_,_", (IReadOnlyList<int>)new int[] { }).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", (IReadOnlyList<int>)new[] { 1 }).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", (IReadOnlyList<int>)new[] { 1, 2 }).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", (IReadOnlyList<int>)new[] { 1, 2, 3 }).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));

            Utf8String.Join("_,_", (ICollection<int>)new int[] { }).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", (ICollection<int>)new[] { 1 }).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", (ICollection<int>)new[] { 1, 2 }).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", (ICollection<int>)new[] { 1, 2, 3 }).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));

            Utf8String.Join("_,_", (IReadOnlyCollection<int>)new int[] { }).Should().Be(string.Join("_,_", new string[0]));
            Utf8String.Join("_,_", (IReadOnlyCollection<int>)new[] { 1 }).Should().Be(string.Join("_,_", new[] { 1 }));
            Utf8String.Join("_,_", (IReadOnlyCollection<int>)new[] { 1, 2 }).Should().Be(string.Join("_,_", new[] { 1, 2 }));
            Utf8String.Join("_,_", (IReadOnlyCollection<int>)new[] { 1, 2, 3 }).Should().Be(string.Join("_,_", new[] { 1, 2, 3 }));
        }

        [Fact]
        public void JoinOverloads3()
        {
            // Utf8String.Join(",", new string[] { }.ToList()).Should().Be(string.Join(',', new string[0]));
            Utf8String.Join(",", new[] { 1 }.ToList()).Should().Be(string.Join(',', new[] { 1 }));
            Utf8String.Join(",", new[] { 1, 2 }.ToList()).Should().Be(string.Join(',', new[] { 1, 2 }));
            Utf8String.Join(",", new[] { 1, 2, 3 }.ToList()).Should().Be(string.Join(',', new[] { 1, 2, 3 }));

            Utf8String.Join(",", (IList<int>)new int[] { }).Should().Be(string.Join(',', new string[0]));
            Utf8String.Join(",", (IList<int>)new[] { 1 }).Should().Be(string.Join(',', new[] { 1 }));
            Utf8String.Join(",", (IList<int>)new[] { 1, 2 }).Should().Be(string.Join(',', new[] { 1, 2 }));
            Utf8String.Join(",", (IList<int>)new[] { 1, 2, 3 }).Should().Be(string.Join(',', new[] { 1, 2, 3 }));

            Utf8String.Join(",", (IReadOnlyList<int>)new int[] { }).Should().Be(string.Join(',', new string[0]));
            Utf8String.Join(",", (IReadOnlyList<int>)new[] { 1 }).Should().Be(string.Join(',', new[] { 1 }));
            Utf8String.Join(",", (IReadOnlyList<int>)new[] { 1, 2 }).Should().Be(string.Join(',', new[] { 1, 2 }));
            Utf8String.Join(",", (IReadOnlyList<int>)new[] { 1, 2, 3 }).Should().Be(string.Join(',', new[] { 1, 2, 3 }));

            Utf8String.Join(",", (ICollection<int>)new int[] { }).Should().Be(string.Join(',', new string[0]));
            Utf8String.Join(",", (ICollection<int>)new[] { 1 }).Should().Be(string.Join(',', new[] { 1 }));
            Utf8String.Join(",", (ICollection<int>)new[] { 1, 2 }).Should().Be(string.Join(',', new[] { 1, 2 }));
            Utf8String.Join(",", (ICollection<int>)new[] { 1, 2, 3 }).Should().Be(string.Join(',', new[] { 1, 2, 3 }));

            Utf8String.Join(",", (IReadOnlyCollection<int>)new int[] { }).Should().Be(string.Join(',', new string[0]));
            Utf8String.Join(",", (IReadOnlyCollection<int>)new[] { 1 }).Should().Be(string.Join(',', new[] { 1 }));
            Utf8String.Join(",", (IReadOnlyCollection<int>)new[] { 1, 2 }).Should().Be(string.Join(',', new[] { 1, 2 }));
            Utf8String.Join(",", (IReadOnlyCollection<int>)new[] { 1, 2, 3 }).Should().Be(string.Join(',', new[] { 1, 2, 3 }));
        }

        [Fact]
        public void CooncatNullTest()
        {
            var str = Utf8String.Concat(default(string), "foo", "bar");
            str.Should().Be(string.Concat(default(string), "foo", "bar"));
        }

        [Fact]
        public void ConcatHugeString()
        {
            var a = new string('a', 10000);
            var b = new string('b', 1000000);

            var actrual = Utf8String.Join(",", new string[] { a, b });
            var expected = string.Join(',', new string[] { a, b });
            actrual.Should().Be(expected);
        }

        [Fact]
        public void JoinStrings()
        {
            var values = new[] { "abc", null, "def" };
            {
                var sep = ",";
                var expected = string.Join(sep, values);
                //Utf8String.Join(sep, new ReadOnlySpan<string>(values)).Should().Be(expected);
                Utf8String.Join(sep, values).Should().Be(expected);
                //Utf8String.Join(sep, values.ToList()).Should().Be(expected);
                //Utf8String.Join(sep, values.AsEnumerable()).Should().Be(expected);
            }

            {
                const string sep = "_,_";
                var expected = string.Join(sep, values);
                //Utf8String.Join(sep, new ReadOnlySpan<string>(values)).Should().Be(expected);
                Utf8String.Join(sep, values).Should().Be(expected);
                //Utf8String.Join(sep, values.ToList()).Should().Be(expected);
                //Utf8String.Join(sep, values.AsEnumerable()).Should().Be(expected);
            }
        }

        [Fact]
        public void ConcatStrings()
        {
            var values = new[] { "abc", null, "def" };

            var expected = string.Concat(values);
            //Utf8String.Concat(new ReadOnlySpan<string>(values)).Should().Be(expected);
            Utf8String.Concat(values).Should().Be(expected);
            //Utf8String.Concat(values.ToList()).Should().Be(expected);
            //Utf8String.Concat(values.AsEnumerable()).Should().Be(expected);
        }
    }

#endif
}
