namespace MINIAUDIO
{
    public unsafe partial struct ma_audio_buffer_config
    {
        public ma_format format;


        public uint channels;


        public uint sampleRate;


        public ulong sizeInFrames;


        public void* pData;

        public ma_allocation_callbacks allocationCallbacks;
    }
}
