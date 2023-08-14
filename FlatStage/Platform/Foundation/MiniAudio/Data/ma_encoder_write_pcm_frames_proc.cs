using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_encoder_write_pcm_frames_proc(ma_encoder* pEncoder, [NativeTypeName("const void *")] void* pFramesIn, [NativeTypeName("ma_uint64")] ulong frameCount, [NativeTypeName("ma_uint64 *")] ulong* pFramesWritten);
}
