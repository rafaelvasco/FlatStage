namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_job_queue
    {
        
        public uint flags;

        
        public uint capacity;

        
        public ulong head;

        
        public ulong tail;

        
        public void* sem;

        public ma_slot_allocator allocator;

        public ma_job* pJobs;

        
        public uint @lock;

        public void* _pHeap;

        
        public uint _ownsHeap;
    }
}
