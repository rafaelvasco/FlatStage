using System;
using System.Runtime.CompilerServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_engine
    {
        public ma_node_graph nodeGraph;

        public ma_resource_manager* pResourceManager;

        public ma_device* pDevice;

        public ma_log* pLog;


        public uint sampleRate;


        public uint listenerCount;


        public _listeners_e__FixedBuffer listeners;

        public ma_allocation_callbacks allocationCallbacks;


        public byte ownsResourceManager;


        public byte ownsDevice;


        public uint inlinedSoundLock;

        public ma_sound_inlined* pInlinedSoundHead;


        public uint inlinedSoundCount;


        public uint gainSmoothTimeInFrames;


        public uint defaultVolumeSmoothTimeInPCMFrames;

        public ma_mono_expansion_mode monoExpansionMode;


        public IntPtr onProcess;

        public void* pProcessUserData;

        public partial struct _listeners_e__FixedBuffer
        {
            public ma_spatializer_listener e0;
            public ma_spatializer_listener e1;
            public ma_spatializer_listener e2;
            public ma_spatializer_listener e3;

            public unsafe ref ma_spatializer_listener this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    fixed (ma_spatializer_listener* pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }
}
