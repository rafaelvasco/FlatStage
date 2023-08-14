namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_pulsewave_config
    {
        public ma_format format;

        [NativeTypeName("ma_uint32")]
        public uint channels;

        [NativeTypeName("ma_uint32")]
        public uint sampleRate;

        public double dutyCycle;

        public double amplitude;

        public double frequency;
    }
}
