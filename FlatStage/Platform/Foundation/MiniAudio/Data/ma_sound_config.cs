using System;

namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_sound_config
    {
        
        public sbyte* pFilePath;

        
        public ushort* pFilePathW;

        
        public void* pDataSource;

        
        public void* pInitialAttachment;

        
        public uint initialAttachmentInputBusIndex;

        
        public uint channelsIn;

        
        public uint channelsOut;

        public ma_mono_expansion_mode monoExpansionMode;

        
        public uint flags;

        
        public uint volumeSmoothTimeInPCMFrames;

        
        public ulong initialSeekPointInPCMFrames;

        
        public ulong rangeBegInPCMFrames;

        
        public ulong rangeEndInPCMFrames;

        
        public ulong loopPointBegInPCMFrames;

        
        public ulong loopPointEndInPCMFrames;

        
        public uint isLooping;

        
        public IntPtr endCallback;

        public void* pEndCallbackUserData;

        public ma_resource_manager_pipeline_notifications initNotifications;

        public ma_fence* pDoneFence;
    }
}
