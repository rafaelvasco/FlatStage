namespace MINIAUDIO
{
    public partial struct ma_lpf_config
    {
        public ma_format format;


        public uint channels;


        public uint sampleRate;

        public double cutoffFrequency;


        public uint order;
    }
}