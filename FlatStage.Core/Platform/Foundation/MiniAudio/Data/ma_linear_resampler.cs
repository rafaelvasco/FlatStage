using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_linear_resampler
    {
        public ma_linear_resampler_config config;


        public uint inAdvanceInt;


        public uint inAdvanceFrac;


        public uint inTimeInt;


        public uint inTimeFrac;


        public _x0_e__Union x0;


        public _x1_e__Union x1;

        public ma_lpf lpf;

        public void* _pHeap;


        public uint _ownsHeap;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _x0_e__Union
        {
            [FieldOffset(0)]
            public float* f32;

            [FieldOffset(0)]

            public short* s16;
        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _x1_e__Union
        {
            [FieldOffset(0)]
            public float* f32;

            [FieldOffset(0)]

            public short* s16;
        }
    }
}
