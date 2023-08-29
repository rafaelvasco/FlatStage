using System;

namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_vfs_callbacks
    {
        
        public IntPtr onOpen;

        
        public IntPtr onOpenW;

        
        public IntPtr onClose;

        
        public IntPtr onRead;

        
        public IntPtr onWrite;

        
        public IntPtr onSeek;

        
        public IntPtr onTell;

        
        public IntPtr onInfo;
    }
}
