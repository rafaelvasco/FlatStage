using System;

namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_sound
    {
        public ma_engine_node engineNode;

        
        public void* pDataSource;

        
        public ulong seekTarget;

        
        public uint atEnd;

        
        public IntPtr endCallback;

        public void* pEndCallbackUserData;

        
        public byte ownsDataSource;

        public ma_resource_manager_data_source* pResourceManagerDataSource;
    }
}
