namespace MINIAUDIO
{
    public unsafe partial struct ma_spatializer_listener_config
    {

        public uint channelsOut;


        public byte* pChannelMapOut;

        public ma_handedness handedness;

        public float coneInnerAngleInRadians;

        public float coneOuterAngleInRadians;

        public float coneOuterGain;

        public float speedOfSound;

        public ma_vec3f worldUp;
    }
}
