namespace MINIAUDIO
{
    public partial struct ma_encoder_config
    {
        public ma_encoding_format encodingFormat;

        public ma_format format;


        public uint channels;


        public uint sampleRate;

        public ma_allocation_callbacks allocationCallbacks;
    }
}
