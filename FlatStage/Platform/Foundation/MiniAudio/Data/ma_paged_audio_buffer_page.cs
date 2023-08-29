namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_paged_audio_buffer_page
    {
        public ma_paged_audio_buffer_page* pNext;

        
        public ulong sizeInFrames;

        
        public fixed byte pAudioData[1];
    }
}
