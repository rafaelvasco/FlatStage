namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_fader
    {
        public ma_fader_config config;

        public float volumeBeg;

        public float volumeEnd;

        [NativeTypeName("ma_uint64")]
        public ulong lengthInFrames;

        [NativeTypeName("ma_int64")]
        public long cursorInFrames;
    }
}
