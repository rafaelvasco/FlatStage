namespace MINIAUDIO
{
    public unsafe partial struct ma_spatializer
    {

        public uint channelsIn;


        public uint channelsOut;


        public byte* pChannelMapIn;

        public ma_attenuation_model attenuationModel;

        public ma_positioning positioning;

        public ma_handedness handedness;

        public float minGain;

        public float maxGain;

        public float minDistance;

        public float maxDistance;

        public float rolloff;

        public float coneInnerAngleInRadians;

        public float coneOuterAngleInRadians;

        public float coneOuterGain;

        public float dopplerFactor;

        public float directionalAttenuationFactor;


        public uint gainSmoothTimeInFrames;

        public ma_atomic_vec3f position;

        public ma_atomic_vec3f direction;

        public ma_atomic_vec3f velocity;

        public float dopplerPitch;

        public float minSpatializationChannelGain;

        public ma_gainer gainer;

        public float* pNewChannelGainsOut;

        public void* _pHeap;


        public uint _ownsHeap;
    }
}
