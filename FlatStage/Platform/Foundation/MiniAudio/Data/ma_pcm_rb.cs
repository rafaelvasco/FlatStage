namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_pcm_rb
    {
        public ma_data_source_base ds;

        public ma_rb rb;

        public ma_format format;

        [NativeTypeName("ma_uint32")]
        public uint channels;

        [NativeTypeName("ma_uint32")]
        public uint sampleRate;
    }
}
