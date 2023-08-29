namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_resampler_config
    {
        public ma_format format;

        
        public uint channels;

        
        public uint sampleRateIn;

        
        public uint sampleRateOut;

        public ma_resample_algorithm algorithm;

        public ma_resampling_backend_vtable* pBackendVTable;

        public void* pBackendUserData;

        
        public _linear_e__Struct linear;

        public partial struct _linear_e__Struct
        {
            
            public uint lpfOrder;
        }
    }
}
