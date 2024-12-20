﻿using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FlatStage;
public class FlatException : Exception
{
    public FlatException() : base() { }

    public FlatException(string message) : base(message) { }

    public FlatException(string message, Exception? inner) : base(message, inner) { }

    /// <summary>
    /// Throws an exception, in DEBUG only, if first parameter is false
    /// </summary>
    /// <param name="isOk"></param>
    /// <param name="message"></param>
    [Conditional("DEBUG")]
    public static void Assert(bool isOk, string message = "")
    {
        if (!isOk)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            Throw(message);
#else

            Throw(message);
#endif
        }
    }

    /// <summary>
    /// Throws an exception, in DEBUG and RELEASE, if first parameter is false
    /// </summary>
    /// <param name="isOk"></param>
    /// <param name="message"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FatalAssert(bool isOk, string message = "")
    {
        if (!isOk)
        {
            Throw(message);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Throw(string message, Exception? inner = null)
    {
#if DEBUG

        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
#endif

        throw new FlatException(message, inner);
    }
}
