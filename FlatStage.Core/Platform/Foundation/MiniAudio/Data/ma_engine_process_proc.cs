using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ma_engine_process_proc(void* pUserData, float* pFramesOut,  ulong frameCount);
}
