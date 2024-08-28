using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_channel_converter
    {
        public ma_format format;


        public uint channelsIn;


        public uint channelsOut;

        public ma_channel_mix_mode mixingMode;

        public ma_channel_conversion_path conversionPath;


        public byte* pChannelMapIn;


        public byte* pChannelMapOut;


        public byte* pShuffleTable;


        public _weights_e__Union weights;

        public void* _pHeap;


        public uint _ownsHeap;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _weights_e__Union
        {
            [FieldOffset(0)]
            public float** f32;

            [FieldOffset(0)]

            public int** s16;
        }
    }
}
