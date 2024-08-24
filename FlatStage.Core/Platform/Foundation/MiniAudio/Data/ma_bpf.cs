namespace MINIAUDIO
{
    public unsafe partial struct ma_bpf
    {
        public ma_format format;


        public uint channels;


        public uint bpf2Count;

        public ma_bpf2* pBPF2;

        public void* _pHeap;


        public uint _ownsHeap;
    }
}
