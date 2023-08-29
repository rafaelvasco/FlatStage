namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_fader
    {
        public ma_fader_config config;

        public float volumeBeg;

        public float volumeEnd;

        
        public ulong lengthInFrames;

        
        public long cursorInFrames;
    }
}
