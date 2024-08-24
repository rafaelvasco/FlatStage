using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ma_device_notification_proc( ma_device_notification* pNotification);
}
