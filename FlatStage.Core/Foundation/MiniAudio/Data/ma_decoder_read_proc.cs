using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_decoder_read_proc(ma_decoder* pDecoder, void* pBufferOut,  ulong* pBytesRead);
}
