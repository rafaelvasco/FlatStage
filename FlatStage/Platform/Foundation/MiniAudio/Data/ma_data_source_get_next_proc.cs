using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ma_data_source *")]
    public unsafe delegate void* ma_data_source_get_next_proc([NativeTypeName("ma_data_source *")] void* pDataSource);
}
