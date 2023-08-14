using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ma_encoder_uninit_proc(ma_encoder* pEncoder);
}
