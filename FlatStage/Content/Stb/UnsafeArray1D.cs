using System;
using System.Runtime.InteropServices;

namespace Stb;

internal unsafe class UnsafeArray1D<T> where T : struct
{
    private readonly T[] _data;
    private readonly GCHandle _pinHandle;

    internal GCHandle PinHandle => _pinHandle;

    public T this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }

    public UnsafeArray1D(int size)
    {
        if (size < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        _data = new T[size];
        _pinHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
    }


    ~UnsafeArray1D()
    {
        _pinHandle.Free();
    }

    private void* ToPointer()
    {
        return _pinHandle.AddrOfPinnedObject().ToPointer();
    }

    public static implicit operator void*(UnsafeArray1D<T> array)
    {
        return array.ToPointer();
    }

    public static void* operator +(UnsafeArray1D<T> array, int delta)
    {
        return array.ToPointer();
    }
}