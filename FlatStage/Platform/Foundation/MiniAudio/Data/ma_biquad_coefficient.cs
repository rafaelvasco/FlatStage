using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ma_biquad_coefficient
    {
        [FieldOffset(0)]
        public float f32;

        [FieldOffset(0)]
        
        public int s32;
    }
}
