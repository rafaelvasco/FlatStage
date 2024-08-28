using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_encoder
    {
        public ma_encoder_config config;


        public IntPtr onWrite;


        public IntPtr onSeek;


        public IntPtr onInit;


        public IntPtr onUninit;


        public IntPtr onWritePCMFrames;

        public void* pUserData;

        public void* pInternalEncoder;


        public _data_e__Union data;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _data_e__Union
        {
            [FieldOffset(0)]

            public _vfs_e__Struct vfs;

            public unsafe partial struct _vfs_e__Struct
            {

                public void* pVFS;


                public void* file;
            }
        }
    }
}
