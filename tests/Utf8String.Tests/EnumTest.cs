using Cysharp.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cysharp.Text.Tests
{
    public enum DuplicateEnum
    {
        A = 1,
        B = 2,
        BB = 2,
        C = 3
    }

    public enum StandardEnum
    {
        Abc = 1,
        Def = 2,
        Ghi = 3,
    }

    [Flags]
    public enum FlagsEnum
    {
        None = 0,
        Abc = 1,
        Bcd = 2,
        Efg = 4,
    }

    public class EnumTest
    {
        [Fact]
        public void Duplicate()
        {
            Utf8String.Format($"{DuplicateEnum.A}").Should().Be("A");
            Utf8String.Format($"{DuplicateEnum.B}").Should().Be("B");
            Utf8String.Format($"{DuplicateEnum.BB}").Should().Be("B");
            Utf8String.Format($"{DuplicateEnum.C}").Should().Be("C");
        }

        [Fact]
        public void Standard()
        {
            Utf8String.Format($"{StandardEnum.Abc}").Should().Be("Abc");
            Utf8String.Format($"{StandardEnum.Def}").Should().Be("Def");
            Utf8String.Format($"{StandardEnum.Ghi}").Should().Be("Ghi");
        }

        [Fact]
        public void Flags()
        {
            Utf8String.Format($"{FlagsEnum.Abc | FlagsEnum.Bcd}").Should().Be("Abc, Bcd");
            Utf8String.Format($"{FlagsEnum.None}").Should().Be("None");
            Utf8String.Format($"{FlagsEnum.Efg}").Should().Be("Efg");
        }

    }
}
