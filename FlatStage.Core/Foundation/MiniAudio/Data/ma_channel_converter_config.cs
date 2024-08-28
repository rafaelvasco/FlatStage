namespace MINIAUDIO
{
    public unsafe partial struct ma_channel_converter_config
    {
        public ma_format format;


        public uint channelsIn;


        public uint channelsOut;


        public byte* pChannelMapIn;


        public byte* pChannelMapOut;

        public ma_channel_mix_mode mixingMode;


        public uint calculateLFEFromSpatialChannels;

        public float** ppWeights;
    }
}
