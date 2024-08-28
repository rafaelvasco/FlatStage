using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ma_device_data_proc(ma_device* pDevice, void* pOutput,  uint frameCount);
}
