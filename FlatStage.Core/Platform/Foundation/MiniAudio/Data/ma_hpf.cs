namespace MINIAUDIO
{
    public unsafe partial struct ma_hpf
    {
        public ma_format format;


        public uint channels;


        public uint sampleRate;


        public uint hpf1Count;


        public uint hpf2Count;

        public ma_hpf1* pHPF1;

        public ma_hpf2* pHPF2;

        public void* _pHeap;


        public uint _ownsHeap;
    }
}
