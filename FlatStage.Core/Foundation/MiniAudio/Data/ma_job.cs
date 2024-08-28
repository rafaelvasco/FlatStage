using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public partial struct ma_job
    {

        public _toc_e__Union toc;


        public ulong next;


        public uint order;


        public _data_e__Union data;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _toc_e__Union
        {
            [FieldOffset(0)]

            public _breakup_e__Struct breakup;

            [FieldOffset(0)]

            public ulong allocation;

            public partial struct _breakup_e__Struct
            {

                public ushort code;


                public ushort slot;


                public uint refcount;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _data_e__Union
        {
            [FieldOffset(0)]

            public _custom_e__Struct custom;

            [FieldOffset(0)]

            public _resourceManager_e__Union resourceManager;

            [FieldOffset(0)]

            public _device_e__Union device;

            public partial struct _custom_e__Struct
            {

                public IntPtr proc;


                public ulong data0;


                public ulong data1;
            }

            [StructLayout(LayoutKind.Explicit)]
            public partial struct _resourceManager_e__Union
            {
                [FieldOffset(0)]

                public _loadDataBufferNode_e__Struct loadDataBufferNode;

                [FieldOffset(0)]

                public _freeDataBufferNode_e__Struct freeDataBufferNode;

                [FieldOffset(0)]

                public _pageDataBufferNode_e__Struct pageDataBufferNode;

                [FieldOffset(0)]

                public _loadDataBuffer_e__Struct loadDataBuffer;

                [FieldOffset(0)]

                public _freeDataBuffer_e__Struct freeDataBuffer;

                [FieldOffset(0)]

                public _loadDataStream_e__Struct loadDataStream;

                [FieldOffset(0)]

                public _freeDataStream_e__Struct freeDataStream;

                [FieldOffset(0)]

                public _pageDataStream_e__Struct pageDataStream;

                [FieldOffset(0)]

                public _seekDataStream_e__Struct seekDataStream;

                public unsafe partial struct _loadDataBufferNode_e__Struct
                {
                    public void* pResourceManager;

                    public void* pDataBufferNode;


                    public sbyte* pFilePath;


                    public ushort* pFilePathW;


                    public uint flags;


                    public void* pInitNotification;


                    public void* pDoneNotification;

                    public ma_fence* pInitFence;

                    public ma_fence* pDoneFence;
                }

                public unsafe partial struct _freeDataBufferNode_e__Struct
                {
                    public void* pResourceManager;

                    public void* pDataBufferNode;


                    public void* pDoneNotification;

                    public ma_fence* pDoneFence;
                }

                public unsafe partial struct _pageDataBufferNode_e__Struct
                {
                    public void* pResourceManager;

                    public void* pDataBufferNode;

                    public void* pDecoder;


                    public void* pDoneNotification;

                    public ma_fence* pDoneFence;
                }

                public unsafe partial struct _loadDataBuffer_e__Struct
                {
                    public void* pDataBuffer;


                    public void* pInitNotification;


                    public void* pDoneNotification;

                    public ma_fence* pInitFence;

                    public ma_fence* pDoneFence;


                    public ulong rangeBegInPCMFrames;


                    public ulong rangeEndInPCMFrames;


                    public ulong loopPointBegInPCMFrames;


                    public ulong loopPointEndInPCMFrames;


                    public uint isLooping;
                }

                public unsafe partial struct _freeDataBuffer_e__Struct
                {
                    public void* pDataBuffer;


                    public void* pDoneNotification;

                    public ma_fence* pDoneFence;
                }

                public unsafe partial struct _loadDataStream_e__Struct
                {
                    public void* pDataStream;


                    public sbyte* pFilePath;


                    public ushort* pFilePathW;


                    public ulong initialSeekPoint;


                    public void* pInitNotification;

                    public ma_fence* pInitFence;
                }

                public unsafe partial struct _freeDataStream_e__Struct
                {
                    public void* pDataStream;


                    public void* pDoneNotification;

                    public ma_fence* pDoneFence;
                }

                public unsafe partial struct _pageDataStream_e__Struct
                {
                    public void* pDataStream;


                    public uint pageIndex;
                }

                public unsafe partial struct _seekDataStream_e__Struct
                {
                    public void* pDataStream;


                    public ulong frameIndex;
                }
            }

            [StructLayout(LayoutKind.Explicit)]
            public partial struct _device_e__Union
            {
                [FieldOffset(0)]

                public _aaudio_e__Union aaudio;

                [StructLayout(LayoutKind.Explicit)]
                public partial struct _aaudio_e__Union
                {
                    [FieldOffset(0)]

                    public _reroute_e__Struct reroute;

                    public unsafe partial struct _reroute_e__Struct
                    {
                        public void* pDevice;


                        public uint deviceType;
                    }
                }
            }
        }
    }
}
