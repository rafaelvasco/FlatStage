namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_device_descriptor
    {
        
        public ma_device_id* pDeviceID;

        public ma_share_mode shareMode;

        public ma_format format;

        
        public uint channels;

        
        public uint sampleRate;

        
        public fixed byte channelMap[254];

        
        public uint periodSizeInFrames;

        
        public uint periodSizeInMilliseconds;

        
        public uint periodCount;
    }
}
