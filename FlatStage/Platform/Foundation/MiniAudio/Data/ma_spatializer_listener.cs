namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_spatializer_listener
    {
        public ma_spatializer_listener_config config;

        public ma_atomic_vec3f position;

        public ma_atomic_vec3f direction;

        public ma_atomic_vec3f velocity;

        [NativeTypeName("ma_bool32")]
        public uint isEnabled;

        [NativeTypeName("ma_bool32")]
        public uint _ownsHeap;

        public void* _pHeap;
    }
}
