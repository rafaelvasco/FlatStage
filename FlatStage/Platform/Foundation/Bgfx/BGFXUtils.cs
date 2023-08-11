using System;
using System.Runtime.CompilerServices;
using FlatStage.Foundation.BGFX;


namespace FlatStage;

internal class BgfxUtils
{
    public static readonly Bgfx.FrameBufferHandle FrameBufferNone = new() { idx = ushort.MaxValue };

    public static unsafe Bgfx.Memory* MakeRef<T>(T[] data) where T : struct
    {
        return Bgfx.make_ref(new Memory<T>(data).Pin().Pointer, (uint)((uint)data.Length * Unsafe.SizeOf<T>()));
    }

    public static unsafe Bgfx.Memory* MakeRef<T>(Memory<T> data) where T : struct
    {
        return Bgfx.make_ref(data.Pin().Pointer, (uint)((uint)data.Length * Unsafe.SizeOf<T>()));
    }

    public static unsafe Bgfx.Memory* MakeRef<T>(Span<T> data) where T : struct
    {
        return Bgfx.make_ref(Unsafe.AsPointer(ref data[0]), (uint)((uint)data.Length * Unsafe.SizeOf<T>()));
    }

    public static unsafe Bgfx.Memory* MakeCopy<T>(Memory<T> data) where T : struct
    {
        return Bgfx.copy(data.Pin().Pointer, (uint)((uint)data.Length * Unsafe.SizeOf<T>()));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bgfx.StateFlags STATE_BLEND_FUNC(Bgfx.StateFlags src, Bgfx.StateFlags dst)
    {
        return STATE_BLEND_FUNC_SEPARATE(src, dst, src, dst);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bgfx.StateFlags STATE_BLEND_FUNC_SEPARATE(Bgfx.StateFlags srcRgb, Bgfx.StateFlags dstRgb,
        Bgfx.StateFlags srcA, Bgfx.StateFlags dstA)
    {
        return (Bgfx.StateFlags)((ulong)srcRgb | ((ulong)dstRgb << 4) | (((ulong)srcA | ((ulong)dstA << 4)) << 8));
    }

    public static Bgfx.StateFlags BGFX_STATE_BLEND_EQUATION_SEPARATE(Bgfx.StateFlags equationRgb,
        Bgfx.StateFlags equationAlpha)
    {
        return (Bgfx.StateFlags)((ulong)equationRgb | ((ulong)equationAlpha << 3));
    }
}