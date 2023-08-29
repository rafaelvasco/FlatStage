using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_resource_manager_data_supply
    {
        public ma_resource_manager_data_supply_type type;

        
        public _backend_e__Union backend;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _backend_e__Union
        {
            [FieldOffset(0)]
            
            public _encoded_e__Struct encoded;

            [FieldOffset(0)]
            
            public _decoded_e__Struct decoded;

            [FieldOffset(0)]
            
            public _decodedPaged_e__Struct decodedPaged;

            public unsafe partial struct _encoded_e__Struct
            {
                
                public void* pData;

                
                public ulong sizeInBytes;
            }

            public unsafe partial struct _decoded_e__Struct
            {
                
                public void* pData;

                
                public ulong totalFrameCount;

                
                public ulong decodedFrameCount;

                public ma_format format;

                
                public uint channels;

                
                public uint sampleRate;
            }

            public partial struct _decodedPaged_e__Struct
            {
                public ma_paged_audio_buffer_data data;

                
                public ulong decodedFrameCount;

                
                public uint sampleRate;
            }
        }
    }
}
