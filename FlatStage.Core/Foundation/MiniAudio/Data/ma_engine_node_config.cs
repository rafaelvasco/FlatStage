namespace MINIAUDIO
{
    public unsafe partial struct ma_engine_node_config
    {
        public ma_engine* pEngine;

        public ma_engine_node_type type;


        public uint channelsIn;


        public uint channelsOut;


        public uint sampleRate;


        public uint volumeSmoothTimeInPCMFrames;

        public ma_mono_expansion_mode monoExpansionMode;


        public byte isPitchDisabled;


        public byte isSpatializationDisabled;


        public byte pinnedListenerIndex;
    }
}
