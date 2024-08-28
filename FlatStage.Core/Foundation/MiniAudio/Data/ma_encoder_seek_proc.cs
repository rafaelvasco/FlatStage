using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_encoder_seek_proc(ma_encoder* pEncoder,  long offset, ma_seek_origin origin);
}
