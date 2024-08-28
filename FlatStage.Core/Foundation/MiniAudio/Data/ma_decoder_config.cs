namespace MINIAUDIO
{
    public unsafe partial struct ma_decoder_config
    {
        public ma_format format;


        public uint channels;


        public uint sampleRate;


        public byte* pChannelMap;

        public ma_channel_mix_mode channelMixMode;

        public ma_dither_mode ditherMode;

        public ma_resampler_config resampling;

        public ma_allocation_callbacks allocationCallbacks;

        public ma_encoding_format encodingFormat;


        public uint seekPointCount;

        public ma_decoding_backend_vtable** ppCustomBackendVTables;


        public uint customBackendCount;

        public void* pCustomBackendUserData;
    }
}
