namespace MINIAUDIO
{
    public unsafe partial struct ma_resource_manager_data_source_config
    {

        public sbyte* pFilePath;


        public ushort* pFilePathW;


        public ma_resource_manager_pipeline_notifications* pNotifications;


        public ulong initialSeekPointInPCMFrames;


        public ulong rangeBegInPCMFrames;


        public ulong rangeEndInPCMFrames;


        public ulong loopPointBegInPCMFrames;


        public ulong loopPointEndInPCMFrames;


        public uint isLooping;


        public uint flags;
    }
}
