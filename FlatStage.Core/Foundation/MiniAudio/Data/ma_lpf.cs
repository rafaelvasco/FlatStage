namespace MINIAUDIO
{
    public unsafe partial struct ma_lpf
    {
        public ma_format format;


        public uint channels;


        public uint sampleRate;


        public uint lpf1Count;


        public uint lpf2Count;

        public ma_lpf1* pLPF1;

        public ma_lpf2* pLPF2;

        public void* _pHeap;


        public uint _ownsHeap;
    }
}
