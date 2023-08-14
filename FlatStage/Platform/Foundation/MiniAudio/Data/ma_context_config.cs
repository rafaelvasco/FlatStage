namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_context_config
    {
        public ma_log* pLog;

        public ma_thread_priority threadPriority;

        [NativeTypeName("size_t")]
        public ulong threadStackSize;

        public void* pUserData;

        public ma_allocation_callbacks allocationCallbacks;

        [NativeTypeName("__AnonymousRecord_miniaudio_L7242_C5")]
        public _alsa_e__Struct alsa;

        [NativeTypeName("__AnonymousRecord_miniaudio_L7246_C5")]
        public _pulse_e__Struct pulse;

        [NativeTypeName("__AnonymousRecord_miniaudio_L7252_C5")]
        public _coreaudio_e__Struct coreaudio;

        [NativeTypeName("__AnonymousRecord_miniaudio_L7259_C5")]
        public _jack_e__Struct jack;

        public ma_backend_callbacks custom;

        public partial struct _alsa_e__Struct
        {
            [NativeTypeName("ma_bool32")]
            public uint useVerboseDeviceEnumeration;
        }

        public unsafe partial struct _pulse_e__Struct
        {
            [NativeTypeName("const char *")]
            public sbyte* pApplicationName;

            [NativeTypeName("const char *")]
            public sbyte* pServerName;

            [NativeTypeName("ma_bool32")]
            public uint tryAutoSpawn;
        }

        public partial struct _coreaudio_e__Struct
        {
            public ma_ios_session_category sessionCategory;

            [NativeTypeName("ma_uint32")]
            public uint sessionCategoryOptions;

            [NativeTypeName("ma_bool32")]
            public uint noAudioSessionActivate;

            [NativeTypeName("ma_bool32")]
            public uint noAudioSessionDeactivate;
        }

        public unsafe partial struct _jack_e__Struct
        {
            [NativeTypeName("const char *")]
            public sbyte* pClientName;

            [NativeTypeName("ma_bool32")]
            public uint tryStartServer;
        }
    }
}
