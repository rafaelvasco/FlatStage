namespace MINIAUDIO
{
    public unsafe partial struct ma_engine_node
    {
        public ma_node_base baseNode;

        public ma_engine* pEngine;


        public uint sampleRate;


        public uint volumeSmoothTimeInPCMFrames;

        public ma_mono_expansion_mode monoExpansionMode;

        public ma_fader fader;

        public ma_linear_resampler resampler;

        public ma_spatializer spatializer;

        public ma_panner panner;

        public ma_gainer volumeGainer;

        public ma_atomic_float volume;

        public float pitch;

        public float oldPitch;

        public float oldDopplerPitch;


        public uint isPitchDisabled;


        public uint isSpatializationDisabled;


        public uint pinnedListenerIndex;


        public _fadeSettings_e__Struct fadeSettings;


        public byte _ownsHeap;

        public void* _pHeap;

        public partial struct _fadeSettings_e__Struct
        {
            public ma_atomic_float volumeBeg;

            public ma_atomic_float volumeEnd;

            public ma_atomic_uint64 fadeLengthInFrames;

            public ma_atomic_uint64 absoluteGlobalTimeInFrames;
        }
    }
}
