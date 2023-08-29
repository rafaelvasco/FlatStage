namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_resource_manager_data_stream
    {
        public ma_data_source_base ds;

        public ma_resource_manager* pResourceManager;

        
        public uint flags;

        public ma_decoder decoder;

        
        public uint isDecoderInitialized;

        
        public ulong totalLengthInPCMFrames;

        
        public uint relativeCursor;

        
        public ulong absoluteCursor;

        
        public uint currentPageIndex;

        
        public uint executionCounter;

        
        public uint executionPointer;

        
        public uint isLooping;

        public void* pPageData;

        
        public fixed uint pageFrameCount[2];

        public ma_result result;

        
        public uint isDecoderAtEnd;

        
        public fixed uint isPageValid[2];

        
        public uint seekCounter;
    }
}
