﻿namespace Stb;

internal static unsafe class Utility
{
    public static byte* ToBytePointer(this IntPtr ptr)
    {
        return (byte*)ptr.ToPointer();
    }

}
