using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace Utf8StringInterpolation;

public ref partial struct Utf8StringWriter<TBufferWriter>
{
# if true

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(bool? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(bool value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(bool value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(byte? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(byte value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(byte value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Decimal? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Decimal value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Decimal value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Double? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Double value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Double value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Guid? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Guid value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Guid value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Int16? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Int16 value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Int16 value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Int32? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Int32 value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Int32 value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Int64? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Int64 value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Int64 value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(SByte? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(SByte value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(SByte value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Single? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(Single value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(Single value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(UInt16? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(UInt16 value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(UInt16 value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(UInt32? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(UInt32 value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(UInt32 value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(UInt64? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(UInt64 value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(UInt64 value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!Utf8Formatter.TryFormat(value, buffer, out bytesWritten, StandardFormat.Parse(format)))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!Utf8Formatter.TryFormat(value, destination, out bytesWritten, StandardFormat.Parse(format)))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(DateTime? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(DateTime value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(DateTime value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!value.TryFormat(buffer, out bytesWritten, format, formatProvider))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(DateTimeOffset? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(DateTimeOffset value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(DateTimeOffset value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!value.TryFormat(buffer, out bytesWritten, format, formatProvider))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

#if !NET8_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(TimeSpan? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(TimeSpan value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(TimeSpan value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!value.TryFormat(buffer, out bytesWritten, format, formatProvider))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

# if true

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(char? value, int alignment = 0, string? format = null)
    {
        if (value == null) return;
        AppendFormatted(value.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(char value, int alignment = 0, string? format = null)
    {
        if (alignment == 0)
        {
            var bytesWritten = 0;
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }

        AppendFormattedAlignment(value, alignment, format);
    }

    void AppendFormattedAlignment(char value, int alignment, string? format)
    {
        var bytesWritten = 0;

        // add left whitespace
        if (alignment > 0)
        {
            Span<byte> buffer = stackalloc byte[32];
            while (!value.TryFormat(buffer, out bytesWritten, format, formatProvider))
            {
                if (buffer.Length < 512)
                {
#pragma warning disable CA2014 // Do not use stackalloc in loops
                    buffer = stackalloc byte[buffer.Length * 2];
#pragma warning restore CA2014 // Do not use stackalloc in loops
                }
                else
                {
                    buffer = new byte[buffer.Length * 2]; // too large
                }
            }

            var space = alignment - bytesWritten;
            if (space > 0)
            {
                AppendWhitespace(space);
            }

            TryGrow(bytesWritten);
            buffer.Slice(0, bytesWritten).CopyTo(destination);
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;
            return;
        }
        else
        {
            // add right whitespace
            while (!value.TryFormat(destination, out bytesWritten, format, formatProvider))
            {
                GrowCore(0);
            }
            destination = destination.Slice(bytesWritten);
            currentWritten += bytesWritten;

            var space = bytesWritten + alignment;
            if (space < 0)
            {
                AppendWhitespace(-space);
            }
        }
    }
#endif

}