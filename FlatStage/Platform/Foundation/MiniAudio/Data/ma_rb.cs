namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_rb
    {
        public void* pBuffer;

        
        public uint subbufferSizeInBytes;

        
        public uint subbufferCount;

        
        public uint subbufferStrideInBytes;

        
        public uint encodedReadOffset;

        
        public uint encodedWriteOffset;

        
        public byte ownsBuffer;

        
        public byte clearOnWriteAcquire;

        public ma_allocation_callbacks allocationCallbacks;
    }
}
