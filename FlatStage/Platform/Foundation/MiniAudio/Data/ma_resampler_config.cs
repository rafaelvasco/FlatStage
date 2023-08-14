namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_resampler_config
    {
        public ma_format format;

        [NativeTypeName("ma_uint32")]
        public uint channels;

        [NativeTypeName("ma_uint32")]
        public uint sampleRateIn;

        [NativeTypeName("ma_uint32")]
        public uint sampleRateOut;

        public ma_resample_algorithm algorithm;

        public ma_resampling_backend_vtable* pBackendVTable;

        public void* pBackendUserData;

        [NativeTypeName("__AnonymousRecord_miniaudio_L5350_C5")]
        public _linear_e__Struct linear;

        public partial struct _linear_e__Struct
        {
            [NativeTypeName("ma_uint32")]
            public uint lpfOrder;
        }
    }
}
