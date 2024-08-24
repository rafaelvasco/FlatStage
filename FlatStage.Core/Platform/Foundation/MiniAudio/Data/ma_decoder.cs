using System;
using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_decoder
    {
        public ma_data_source_base ds;


        public void* pBackend;


        public ma_decoding_backend_vtable* pBackendVTable;

        public void* pBackendUserData;


        public IntPtr onRead;


        public IntPtr onSeek;


        public IntPtr onTell;

        public void* pUserData;


        public ulong readPointerInPCMFrames;

        public ma_format outputFormat;


        public uint outputChannels;


        public uint outputSampleRate;

        public ma_data_converter converter;

        public void* pInputCache;


        public ulong inputCacheCap;


        public ulong inputCacheConsumed;


        public ulong inputCacheRemaining;

        public ma_allocation_callbacks allocationCallbacks;


        public _data_e__Union data;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _data_e__Union
        {
            [FieldOffset(0)]

            public _vfs_e__Struct vfs;

            [FieldOffset(0)]

            public _memory_e__Struct memory;

            public unsafe partial struct _vfs_e__Struct
            {

                public void* pVFS;


                public void* file;
            }

            public unsafe partial struct _memory_e__Struct
            {

                public byte* pData;


                public ulong dataSize;


                public ulong currentReadPos;
            }
        }
    }
}
