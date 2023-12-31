using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate ma_result ma_decoder_seek_proc(ma_decoder* pDecoder,  long byteOffset, ma_seek_origin origin);
}
