namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_delay
    {
        public ma_delay_config config;

        
        public uint cursor;

        
        public uint bufferSizeInFrames;

        public float* pBuffer;
    }
}
