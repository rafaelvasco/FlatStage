using System.Runtime.InteropServices;

namespace MINIAUDIO;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void* ma_data_source_get_next_proc(void* pDataSource);
