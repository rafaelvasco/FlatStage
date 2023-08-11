using System;
using System.Runtime.InteropServices;

namespace Stb;

internal static unsafe class CRuntime
{
    private const long DBL_EXP_MASK = 0x7ff0000000000000L;
    private const int DBL_MANT_BITS = 52;
    private const long DBL_SGN_MASK = -1 - 0x7fffffffffffffffL;
    private const long DBL_MANT_MASK = 0x000fffffffffffffL;
    private const long DBL_EXP_CLR_MASK = DBL_SGN_MASK | DBL_MANT_MASK;

    private const string NUMBERS = "0123456789";

    public static void* Malloc(ulong size)
    {
        return Malloc((long)size);
    }

    private static void* Malloc(long size)
    {
        var ptr = Marshal.AllocHGlobal((int)size);

        MemoryStats.Allocated();

        return ptr.ToPointer();
    }

    public static void Free(void* a)
    {
        if (a == null)
            return;

        var ptr = new IntPtr(a);
        Marshal.FreeHGlobal(ptr);
        MemoryStats.Freed();
    }

    private static void Memcpy(void* a, void* b, long size)
    {
        var ap = (byte*)a;
        var bp = (byte*)b;
        for (long i = 0; i < size; ++i)
            *ap++ = *bp++;
    }

    public static void Memcpy(void* a, void* b, ulong size)
    {
        Memcpy(a, b, (long)size);
    }

    private static void Memmove(void* a, void* b, long size)
    {
        void* temp = null;

        try
        {
            temp = Malloc(size);
            Memcpy(temp, b, size);
            Memcpy(a, temp, size);
        }

        finally
        {
            if (temp != null)
                Free(temp);
        }
    }

    public static void Memmove(void* a, void* b, ulong size)
    {
        Memmove(a, b, (long)size);
    }

    private static int Memcmp(void* a, void* b, long size)
    {
        var result = 0;
        var ap = (byte*)a;
        var bp = (byte*)b;
        for (long i = 0; i < size; ++i)
        {
            if (*ap != *bp)
                result += 1;

            ap++;
            bp++;
        }

        return result;
    }

    public static int Memcmp(void* a, void* b, ulong size)
    {
        return Memcmp(a, b, (long)size);
    }

    public static int Memcmp(byte* a, byte[] b, ulong size)
    {
        fixed (void* bptr = b)
        {
            return Memcmp(a, bptr, (long)size);
        }
    }

    private static void Memset(void* ptr, int value, long size)
    {
        var bptr = (byte*)ptr;
        var bval = (byte)value;
        for (long i = 0; i < size; ++i)
            *bptr++ = bval;
    }

    public static void Memset(void* ptr, int value, ulong size)
    {
        Memset(ptr, value, (long)size);
    }

    public static uint _lrotl(uint x, int y)
    {
        return (x << y) | (x >> (32 - y));
    }

    private static void* Realloc(void* a, long newSize)
    {
        if (a == null)
            return Malloc(newSize);

        var ptr = new IntPtr(a);
        var result = Marshal.ReAllocHGlobal(ptr, new IntPtr(newSize));

        return result.ToPointer();
    }

    public static void* Realloc(void* a, ulong newSize)
    {
        return Realloc(a, (long)newSize);
    }

    public static int Abs(int v)
    {
        return System.Math.Abs(v);
    }

    public static double Pow(double a, double b)
    {
        return System.Math.Pow(a, b);
    }

    public static void SetArray<T>(T[] data, T value)
    {
        for (var i = 0; i < data.Length; ++i)
            data[i] = value;
    }

    public static double Ldexp(double number, int exponent)
    {
        return number * System.Math.Pow(2, exponent);
    }

    public static int Strcmp(sbyte* src, string token)
    {
        var result = 0;

        for (var i = 0; i < token.Length; ++i)
        {
            if (src[i] != token[i])
            {
                ++result;
            }
        }

        return result;
    }

    public static int Strncmp(sbyte* src, string token, ulong size)
    {
        var result = 0;

        for (var i = 0; i < System.Math.Min(token.Length, (int)size); ++i)
        {
            if (src[i] != token[i])
            {
                ++result;
            }
        }

        return result;
    }

    public static long Strtol(sbyte* start, sbyte** end, int radix)
    {
        // First step - determine length
        var length = 0;
        sbyte* ptr = start;
        while (NUMBERS.IndexOf((char)*ptr) != -1)
        {
            ++ptr;
            ++length;
        }

        long result = 0;

        // Now build up the number
        ptr = start;
        while (length > 0)
        {
            long num = NUMBERS.IndexOf((char)*ptr);
            long pow = (long)System.Math.Pow(10, length - 1);
            result += num * pow;

            ++ptr;
            --length;
        }

        if (end != null)
        {
            *end = ptr;
        }

        return result;
    }

    /// <summary>
    /// This code had been borrowed from here: https://github.com/MachineCognitis/C.math.NET
    /// </summary>
    /// <param name="number"></param>
    /// <param name="exponent"></param>
    /// <returns></returns>
    public static double Frexp(double number, int* exponent)
    {
        var bits = BitConverter.DoubleToInt64Bits(number);
        var exp = (int)((bits & DBL_EXP_MASK) >> DBL_MANT_BITS);
        *exponent = 0;

        if (exp == 0x7ff || number == 0D)
            number += number;
        else
        {
            // Not zero and finite.
            *exponent = exp - 1022;
            if (exp == 0)
            {
                // Subnormal, scale number so that it is in [1, 2).
                number *= BitConverter.Int64BitsToDouble(0x4350000000000000L); // 2^54
                bits = BitConverter.DoubleToInt64Bits(number);
                exp = (int)((bits & DBL_EXP_MASK) >> DBL_MANT_BITS);
                *exponent = exp - 1022 - 54;
            }

            // Set exponent to -1 so that number is in [0.5, 1).
            number = BitConverter.Int64BitsToDouble((bits & DBL_EXP_CLR_MASK) | 0x3fe0000000000000L);
        }

        return number;
    }

    public static double Acos(double value)
    {
        return System.Math.Acos(value);
    }

    public static double Sin(double value)
    {
        return System.Math.Sin(value);
    }

    public static double Sqrt(double val)
    {
        return System.Math.Sqrt(val);
    }

    public static double Fmod(double x, double y)
    {
        return x % y;
    }

    public static float Fabs(double a)
    {
        return (float)System.Math.Abs(a);
    }

    public static double Ceil(double a)
    {
        return System.Math.Ceiling(a);
    }

    public static double Floor(double a)
    {
        return System.Math.Floor(a);
    }

    public static double Cos(double value)
    {
        return System.Math.Cos(value);
    }

    public static ulong Strlen(sbyte* str)
    {
        var ptr = str;

        while (*ptr != '\0')
            ptr++;

        return (ulong)ptr - (ulong)str - 1;
    }
}