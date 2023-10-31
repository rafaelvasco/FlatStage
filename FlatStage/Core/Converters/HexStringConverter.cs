using System;
using System.Runtime.CompilerServices;

namespace FlatStage;
public static class HexStringConverter
{
    /// <summary>
    /// Single hex character to integer; ie. 'B' to 11; supports any casing
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int HexCharToInteger(char c)
    {
        var d = (int)c;
        return (d & 0xf) + (d >> 6) + ((d >> 6) << 3);
    }

    public static ulong HexStringToInt(ReadOnlySpan<char> hexString)
    {
        if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hexString = hexString.Slice(2);
        }

        ulong result = 0;

        while (hexString.Length > 0)
        {
            var val = (uint)HexCharToInteger(hexString[0]);
            result = (result << 4) | val;
            hexString = hexString.Slice(1);
        }

        return result;
    }

    public static int ToHex(ulong value, Span<char> destination, int zeroPadToCharLength = 0)
    {
        bool ok = value.TryFormat(destination, out int written, format: "X");

        if (!ok)
        {
            return 0; // Failed
        }

        int pad = zeroPadToCharLength - written;

        if (pad > 0)
        {
            destination.Slice(0, written).CopyTo(destination.Slice(pad));
            destination.Slice(0, pad).Fill('0');
            return zeroPadToCharLength;
        }

        return written;
    }

    public static string ToHex(ulong value, int zeroPadToCharLength = 0)
    {
        Span<char> tmp = stackalloc char[MathUtils.Max(16, zeroPadToCharLength)];
        int written = ToHex(value, tmp, zeroPadToCharLength);
        return tmp[..written].ToString();
    }
}
