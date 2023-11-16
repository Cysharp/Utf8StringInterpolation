#if NETSTANDARD2_0

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Utf8StringInterpolation
{
	internal static partial class Shims
	{
		public static string GetString(this Encoding encoding, scoped ReadOnlySpan<byte> bytes)
		{
			if (bytes.IsEmpty) return string.Empty;

			unsafe
			{
				fixed (byte* pB = &MemoryMarshal.GetReference(bytes))
				{
					return encoding.GetString(pB, bytes.Length);
				}
			}
		}

		public static int GetByteCount(this Encoding encoding, scoped ReadOnlySpan<char> chars)
		{
			unsafe
			{
				fixed (char* charsPtr = &MemoryMarshal.GetReference(chars))
				{
					return encoding.GetByteCount(charsPtr, chars.Length);
				}
			}
		}

		public static int GetBytes(this Encoding encoding, scoped ReadOnlySpan<char> chars, scoped Span<byte> bytes)
		{
			unsafe
			{
				fixed (char* charsPtr = &MemoryMarshal.GetReference(chars))
				fixed (byte* bytesPtr = &MemoryMarshal.GetReference(bytes))
				{
					return encoding.GetBytes(charsPtr, chars.Length, bytesPtr, bytes.Length);
				}
			}
		}

		private static bool TryFormat(this DateTime value, scoped Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider)
		{
			string s = value.ToString(format, formatProvider);
			if (s.Length > destination.Length)
			{
				charsWritten = 0;
				return false;
			}

			s.AsSpan().CopyTo(destination);
			charsWritten = s.Length;
			return true;
		}

		private static bool TryFormat(this DateTimeOffset value, scoped Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider)
		{
			string s = value.ToString(format, formatProvider);
			if (s.Length > destination.Length)
			{
				charsWritten = 0;
				return false;
			}

			s.AsSpan().CopyTo(destination);
			charsWritten = s.Length;
			return true;
		}

		private static bool TryFormat(this TimeSpan value, scoped Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider)
		{
			string s = value.ToString(format, formatProvider);
			if (s.Length > destination.Length)
			{
				charsWritten = 0;
				return false;
			}

			s.AsSpan().CopyTo(destination);
			charsWritten = s.Length;
			return true;
		}
	}
}

#endif