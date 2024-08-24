using System;

namespace MINIAUDIO
{
    public unsafe partial struct ma_data_source_base
    {

        public ma_data_source_vtable* vtable;


        public ulong rangeBegInFrames;


        public ulong rangeEndInFrames;


        public ulong loopBegInFrames;


        public ulong loopEndInFrames;


        public void* pCurrent;


        public void* pNext;


        public IntPtr onGetNext;


        public uint isLooping;
    }
}
