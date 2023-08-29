namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_audio_buffer_ref
    {
        public ma_data_source_base ds;

        public ma_format format;

        
        public uint channels;

        
        public uint sampleRate;

        
        public ulong cursor;

        
        public ulong sizeInFrames;

        
        public void* pData;
    }
}
