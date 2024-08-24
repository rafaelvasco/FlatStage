using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_resource_manager_data_buffer
    {
        public ma_data_source_base ds;

        public ma_resource_manager* pResourceManager;

        public ma_resource_manager_data_buffer_node* pNode;


        public uint flags;


        public uint executionCounter;


        public uint executionPointer;


        public ulong seekTargetInPCMFrames;


        public uint seekToCursorOnNextRead;

        public ma_result result;


        public uint isLooping;

        public ma_atomic_bool32 isConnectorInitialized;


        public _connector_e__Union connector;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _connector_e__Union
        {
            [FieldOffset(0)]
            public ma_decoder decoder;

            [FieldOffset(0)]
            public ma_audio_buffer buffer;

            [FieldOffset(0)]
            public ma_paged_audio_buffer pagedBuffer;
        }
    }
}
