namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_delay_config
    {
        
        public uint channels;

        
        public uint sampleRate;

        
        public uint delayInFrames;

        
        public uint delayStart;

        public float wet;

        public float dry;

        public float decay;
    }
}
