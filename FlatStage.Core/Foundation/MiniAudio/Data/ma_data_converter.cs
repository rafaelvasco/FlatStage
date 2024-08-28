namespace MINIAUDIO
{
    public unsafe partial struct ma_data_converter
    {
        public ma_format formatIn;

        public ma_format formatOut;


        public uint channelsIn;


        public uint channelsOut;


        public uint sampleRateIn;


        public uint sampleRateOut;

        public ma_dither_mode ditherMode;

        public ma_data_converter_execution_path executionPath;

        public ma_channel_converter channelConverter;

        public ma_resampler resampler;


        public byte hasPreFormatConversion;


        public byte hasPostFormatConversion;


        public byte hasChannelConverter;


        public byte hasResampler;


        public byte isPassthrough;


        public byte _ownsHeap;

        public void* _pHeap;
    }
}
