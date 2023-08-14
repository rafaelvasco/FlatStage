using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ma_sound_end_proc(void* pUserData, ma_sound* pSound);
}
