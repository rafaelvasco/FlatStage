using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_seek_proc(void* pUserData,  long offset, ma_seek_origin origin);
}
