namespace MINIAUDIO
{
    public unsafe partial struct ma_audio_buffer
    {
        public ma_audio_buffer_ref @ref;

        public ma_allocation_callbacks allocationCallbacks;


        public uint ownsData;


        public fixed byte _pExtraData[1];
    }
}
