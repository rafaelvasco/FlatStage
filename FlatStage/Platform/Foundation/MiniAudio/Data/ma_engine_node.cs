namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_engine_node
    {
        public ma_node_base baseNode;

        public ma_engine* pEngine;

        [NativeTypeName("ma_uint32")]
        public uint sampleRate;

        [NativeTypeName("ma_uint32")]
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

        [NativeTypeName("ma_bool32")]
        public uint isPitchDisabled;

        [NativeTypeName("ma_bool32")]
        public uint isSpatializationDisabled;

        [NativeTypeName("ma_uint32")]
        public uint pinnedListenerIndex;

        [NativeTypeName("__AnonymousRecord_miniaudio_L11078_C5")]
        public _fadeSettings_e__Struct fadeSettings;

        [NativeTypeName("ma_bool8")]
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
