namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_gainer
    {
        public ma_gainer_config config;

        
        public uint t;

        public float masterVolume;

        public float* pOldGains;

        public float* pNewGains;

        public void* _pHeap;

        
        public uint _ownsHeap;
    }
}
