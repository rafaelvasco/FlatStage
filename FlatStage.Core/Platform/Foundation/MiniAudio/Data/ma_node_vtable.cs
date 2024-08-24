using System;

namespace MINIAUDIO
{
    public partial struct ma_node_vtable
    {

        public IntPtr onProcess;


        public IntPtr onGetRequiredInputFrameCount;


        public byte inputBusCount;


        public byte outputBusCount;


        public uint flags;
    }
}
