using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public partial struct ma_resource_manager_data_source
    {

        public _backend_e__Union backend;


        public uint flags;


        public uint executionCounter;


        public uint executionPointer;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _backend_e__Union
        {
            [FieldOffset(0)]
            public ma_resource_manager_data_buffer buffer;

            [FieldOffset(0)]
            public ma_resource_manager_data_stream stream;
        }
    }
}
