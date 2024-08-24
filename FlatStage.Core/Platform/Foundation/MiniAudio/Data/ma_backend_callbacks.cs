using System;

namespace MINIAUDIO
{
    public partial struct ma_backend_callbacks
    {

        public IntPtr onContextInit;


        public IntPtr onContextUninit;


        public IntPtr onContextEnumerateDevices;


        public IntPtr onContextGetDeviceInfo;


        public IntPtr onDeviceInit;


        public IntPtr onDeviceUninit;


        public IntPtr onDeviceStart;


        public IntPtr onDeviceStop;


        public IntPtr onDeviceRead;


        public IntPtr onDeviceWrite;


        public IntPtr onDeviceDataLoop;


        public IntPtr onDeviceDataLoopWakeup;


        public IntPtr onDeviceGetInfo;
    }
}
