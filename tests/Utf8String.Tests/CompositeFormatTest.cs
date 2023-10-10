using Cysharp.Text;
using FluentAssertions;
using System;
using System.Buffers;
using System.Text;
using Xunit;
using static FluentAssertions.FluentActions;

using static Cysharp.Text.Tests.FormatTest;
namespace Cysharp.Text.Tests
{
    public class CompositeFormatTest
    {
        [Fact]
        public void AlignmentComponentInt()
        {
            Utf8String.Format($"{long.MaxValue,-1}{long.MinValue,1}").Should().Be($"{long.MaxValue,-1}{long.MinValue,1}");
            Utf8String.Format($"{1,1}{1,-1}").Should().Be($"{1,1}{1,-1}");
            Utf8String.Format($"{1,10}{1,-10}").Should().Be($"{1,10}{1,-10}");
        }

        [Fact]
        public void AlignmentComponentString()
        {
            Utf8String.Format($"{"left",0}{"right",0}").Should().Be($"{"left",0}{"right",0}");
            Utf8String.Format($"{"Foo",3}{"Foo",-3}").Should().Be($"{"Foo",3}{"Foo",-3}");
            Utf8String.Format($"{"Foo",4}{"Foo",-4}").Should().Be($"{"Foo",4}{"Foo",-4}");
        }

        [Fact]
        public void AlignmentComponent()
        {
            Utf8String.Format($"{long.MaxValue,-1000}{"",1000}").Should().Be($"{long.MaxValue,-1000}{"",1000}");

#if NET8_0_OR_GREATER
            var guid = Guid.NewGuid();
            var neg = DateTime.Now.TimeOfDay.Negate();
            Utf8String.Format($"{guid,10:X}{guid}{neg,-10:c}").Should().Be($"{guid,10:X}{guid}{neg,-10:c}");
#endif

            string[] names = { "Adam", "Bridgette", "Carla", "Daniel", "Ebenezer", "Francine", "George" };
            decimal[] hours = { 40, 6.667m, 40.39m, 82, 40.333m, 80, 16.75m };

            for (int ctr = 0; ctr < names.Length; ctr++)
            {
                Utf8String.Format($"{names[ctr],-20} {hours[ctr],5:f}").Should().Be($"{names[ctr],-20} {hours[ctr],5:f}");
            }

        }


#if NET8_0_OR_GREATER

        // fail on .NET 6(because StandardFormat does not equal of format)
        [Fact]
        public void Spaces()
        {
            // var format = "Prime numbers less than 10: {00 , 01 }, {01  ,02  }, {2 ,3 :D }, {3  ,4: X }";
            var expected = $"Prime numbers less than 10: {2,01}, {3,02}, {5,3:D}, {7,4: X}";
            var actual = Utf8String.Format($"Prime numbers less than 10: {2,01}, {3,02}, {5,3:D}, {7,4: X}");
            actual.Should().Be(expected);
        }

#endif

        [Fact]
        public void CompsiteFormats()
        {
            Utf8String.Format($"Name = {"Fred"}, {500_0000_0000_0000m:f}({500_0000_0000_0000m:E})").Should().Be($"Name = {"Fred"}, {500_0000_0000_0000m:f}({500_0000_0000_0000m:E})");
        }

    }
}