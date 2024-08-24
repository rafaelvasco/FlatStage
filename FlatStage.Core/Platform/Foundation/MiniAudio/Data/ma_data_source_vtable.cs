using System;

namespace MINIAUDIO
{
    public partial struct ma_data_source_vtable
    {

        public IntPtr onRead;


        public IntPtr onSeek;


        public IntPtr onGetDataFormat;


        public IntPtr onGetCursor;


        public IntPtr onGetLength;


        public IntPtr onSetLooping;


        public uint flags;
    }
}
