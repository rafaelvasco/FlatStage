using System;

namespace MINIAUDIO;

public unsafe partial struct ma_allocation_callbacks
{
    public void* pUserData;

    public IntPtr onMalloc;

    public IntPtr onRealloc;

    public IntPtr onFree;
}
