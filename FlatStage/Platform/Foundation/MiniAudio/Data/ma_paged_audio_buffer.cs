namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_paged_audio_buffer
    {
        public ma_data_source_base ds;

        public ma_paged_audio_buffer_data* pData;

        public ma_paged_audio_buffer_page* pCurrent;

        
        public ulong relativeCursor;

        
        public ulong absoluteCursor;
    }
}
