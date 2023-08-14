using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ma_device_data_proc(ma_device* pDevice, void* pOutput, [NativeTypeName("const void *")] void* pInput, [NativeTypeName("ma_uint32")] uint frameCount);
}
