namespace MINIAUDIO
{
    public partial struct ma_noise_config
    {
        public ma_format format;


        public uint channels;

        public ma_noise_type type;


        public int seed;

        public double amplitude;


        public uint duplicateChannels;
    }
}
