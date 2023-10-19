using System;

namespace Utf8StringInterpolation.Tests
{
	internal static class Shims
	{
#if NETFRAMEWORK
		public static string Join<T>(char separator, IEnumerable<T> values) => string.Join(separator.ToString(), values);
#else
		public static string Join<T>(char separator, IEnumerable<T> values) => string.Join(separator, values);
#endif
	}
}