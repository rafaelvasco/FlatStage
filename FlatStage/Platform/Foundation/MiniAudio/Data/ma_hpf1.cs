namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_hpf1
    {
        public ma_format format;

        
        public uint channels;

        public ma_biquad_coefficient a;

        public ma_biquad_coefficient* pR1;

        public void* _pHeap;

        
        public uint _ownsHeap;
    }
}
