using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_noise
    {
        public ma_data_source_vtable ds;

        public ma_noise_config config;

        public ma_lcg lcg;


        public _state_e__Union state;

        public void* _pHeap;


        public uint _ownsHeap;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _state_e__Union
        {
            [FieldOffset(0)]

            public _pink_e__Struct pink;

            [FieldOffset(0)]

            public _brownian_e__Struct brownian;

            public unsafe partial struct _pink_e__Struct
            {
                public double** bin;

                public double* accumulation;


                public uint* counter;
            }

            public unsafe partial struct _brownian_e__Struct
            {
                public double* accumulation;
            }
        }
    }
}
