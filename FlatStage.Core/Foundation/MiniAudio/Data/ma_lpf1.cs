namespace MINIAUDIO
{
    public unsafe partial struct ma_lpf1
    {
        public ma_format format;


        public uint channels;

        public ma_biquad_coefficient a;

        public ma_biquad_coefficient* pR1;

        public void* _pHeap;


        public uint _ownsHeap;
    }
}