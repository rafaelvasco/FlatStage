namespace MINIAUDIO
{
    public unsafe partial struct ma_spatializer_config
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

        public float minSpatializationChannelGain;


        public uint gainSmoothTimeInFrames;
    }
}
