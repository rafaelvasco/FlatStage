namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_linear_resampler_config
    {
        public ma_format format;

        
        public uint channels;

        
        public uint sampleRateIn;

        
        public uint sampleRateOut;

        
        public uint lpfOrder;

        public double lpfNyquistFactor;
    }
}
