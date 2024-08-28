namespace MINIAUDIO
{
    public unsafe partial struct ma_node_output_bus
    {

        public void* pNode;


        public byte outputBusIndex;


        public byte channels;


        public byte inputNodeInputBusIndex;


        public uint flags;


        public uint refCount;


        public uint isAttached;


        public uint @lock;

        public float volume;

        public ma_node_output_bus* pNext;

        public ma_node_output_bus* pPrev;


        public void* pInputNode;
    }
}
