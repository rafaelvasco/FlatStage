using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void* ma_data_source_get_next_proc(void* pDataSource);
