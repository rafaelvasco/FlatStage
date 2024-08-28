namespace MINIAUDIO
{
    public unsafe partial struct ma_data_converter_config
    {
        public ma_format formatIn;

        public ma_format formatOut;


        public uint channelsIn;


        public uint channelsOut;


        public uint sampleRateIn;


        public uint sampleRateOut;


        public byte* pChannelMapIn;


        public byte* pChannelMapOut;

        public ma_dither_mode ditherMode;

        public ma_channel_mix_mode channelMixMode;


        public uint calculateLFEFromSpatialChannels;

        public float** ppChannelWeights;


        public uint allowDynamicSampleRate;

        public ma_resampler_config resampling;
    }
}
