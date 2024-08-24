using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_encoder_write_proc(ma_encoder* pEncoder,  ulong* pBytesWritten);
}
