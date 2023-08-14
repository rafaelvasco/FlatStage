using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_job_proc(ma_job* pJob);
}
