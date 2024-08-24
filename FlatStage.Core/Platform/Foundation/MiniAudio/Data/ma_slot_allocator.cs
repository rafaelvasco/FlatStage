namespace MINIAUDIO
{
    public unsafe partial struct ma_slot_allocator
    {
        public ma_slot_allocator_group* pGroups;


        public uint* pSlots;


        public uint count;


        public uint capacity;


        public uint _ownsHeap;

        public void* _pHeap;
    }
}
