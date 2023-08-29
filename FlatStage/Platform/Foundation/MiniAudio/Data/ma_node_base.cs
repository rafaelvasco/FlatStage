using System.Runtime.CompilerServices;

namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_node_base
    {
        public ma_node_graph* pNodeGraph;

        
        public ma_node_vtable* vtable;

        public float* pCachedData;

        
        public ushort cachedDataCapInFramesPerBus;

        
        public ushort cachedFrameCountOut;

        
        public ushort cachedFrameCountIn;

        
        public ushort consumedFrameCountIn;

        public ma_node_state state;

        
        public fixed ulong stateTimes[2];

        
        public ulong localTime;

        
        public uint inputBusCount;

        
        public uint outputBusCount;

        public ma_node_input_bus* pInputBuses;

        public ma_node_output_bus* pOutputBuses;

        
        public __inputBuses_e__FixedBuffer _inputBuses;

        
        public __outputBuses_e__FixedBuffer _outputBuses;

        public void* _pHeap;

        
        public uint _ownsHeap;

        public partial struct __inputBuses_e__FixedBuffer
        {
            public ma_node_input_bus e0;
            public ma_node_input_bus e1;

            public unsafe ref ma_node_input_bus this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    fixed (ma_node_input_bus* pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }

        public partial struct __outputBuses_e__FixedBuffer
        {
            public ma_node_output_bus e0;
            public ma_node_output_bus e1;

            public unsafe ref ma_node_output_bus this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    fixed (ma_node_output_bus* pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }
}
