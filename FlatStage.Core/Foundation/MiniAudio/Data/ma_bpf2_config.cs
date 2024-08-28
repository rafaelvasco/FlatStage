namespace MINIAUDIO
{
    public partial struct ma_bpf2_config
    {
        public ma_format format;


        public uint channels;


        public uint sampleRate;

        public double cutoffFrequency;

        public double q;
    }
}
