using System.Runtime.InteropServices;

namespace MINIAUDIO;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate uint ma_enum_devices_callback_proc(ma_context* pContext, ma_device_type deviceType, ma_device_info* pInfo, void* pUserData);
