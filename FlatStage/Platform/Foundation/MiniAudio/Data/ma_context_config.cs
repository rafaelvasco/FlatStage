namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_context_config
    {
        public ma_log* pLog;

        public ma_thread_priority threadPriority;

        
        public ulong threadStackSize;

        public void* pUserData;

        public ma_allocation_callbacks allocationCallbacks;

        
        public _alsa_e__Struct alsa;

        
        public _pulse_e__Struct pulse;

        
        public _coreaudio_e__Struct coreaudio;

        
        public _jack_e__Struct jack;

        public ma_backend_callbacks custom;

        public partial struct _alsa_e__Struct
        {
            
            public uint useVerboseDeviceEnumeration;
        }

        public unsafe partial struct _pulse_e__Struct
        {
            
            public sbyte* pApplicationName;

            
            public sbyte* pServerName;

            
            public uint tryAutoSpawn;
        }

        public partial struct _coreaudio_e__Struct
        {
            public ma_ios_session_category sessionCategory;

            
            public uint sessionCategoryOptions;

            
            public uint noAudioSessionActivate;

            
            public uint noAudioSessionDeactivate;
        }

        public unsafe partial struct _jack_e__Struct
        {
            
            public sbyte* pClientName;

            
            public uint tryStartServer;
        }
    }
}
