using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_read_proc(void* pUserData, void* pBufferOut,  ulong* pBytesRead);
}
