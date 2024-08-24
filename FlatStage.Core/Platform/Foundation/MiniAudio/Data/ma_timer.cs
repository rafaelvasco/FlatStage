using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ma_timer
    {
        [FieldOffset(0)]

        public long counter;

        [FieldOffset(0)]
        public double counterD;
    }
}
