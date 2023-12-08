using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;


namespace Utf8StringInterpolation.Tests;

public class Utf8StringTest
{
    
    [Test]
    public void Sample()
    {
        var name = "foo";
        var id = 123;
        var result = Utf8String.Format($"Hello, {name}, Your id is {id}!");
        var expected = Encoding.UTF8.GetBytes($"Hello, {name}, Your id is {id}!");
        Assert.That(result, Is.EquivalentTo(expected));
    }
}