namespace MINIAUDIO
{
    public unsafe partial struct ma_node_config
    {

        public ma_node_vtable* vtable;

        public ma_node_state initialState;


        public uint inputBusCount;


        public uint outputBusCount;


        public uint* pInputChannels;


        public uint* pOutputChannels;
    }
}
