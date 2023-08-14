using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_channel_converter
    {
        public ma_format format;

        [NativeTypeName("ma_uint32")]
        public uint channelsIn;

        [NativeTypeName("ma_uint32")]
        public uint channelsOut;

        public ma_channel_mix_mode mixingMode;

        public ma_channel_conversion_path conversionPath;

        [NativeTypeName("ma_channel *")]
        public byte* pChannelMapIn;

        [NativeTypeName("ma_channel *")]
        public byte* pChannelMapOut;

        [NativeTypeName("ma_uint8 *")]
        public byte* pShuffleTable;

        [NativeTypeName("__AnonymousRecord_miniaudio_L5505_C5")]
        public _weights_e__Union weights;

        public void* _pHeap;

        [NativeTypeName("ma_bool32")]
        public uint _ownsHeap;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _weights_e__Union
        {
            [FieldOffset(0)]
            public float** f32;

            [FieldOffset(0)]
            [NativeTypeName("ma_int32 **")]
            public int** s16;
        }
    }
}
