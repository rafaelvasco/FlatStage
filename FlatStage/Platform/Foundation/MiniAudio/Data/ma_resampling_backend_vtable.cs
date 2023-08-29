using System;

namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_resampling_backend_vtable
    {
        
        public IntPtr onGetHeapSize;

        
        public IntPtr onInit;

        
        public IntPtr onUninit;

        
        public IntPtr onProcess;

        
        public IntPtr onSetRate;

        
        public IntPtr onGetInputLatency;

        
        public IntPtr onGetOutputLatency;

        
        public IntPtr onGetRequiredInputFrameCount;

        
        public IntPtr onGetExpectedOutputFrameCount;

        
        public IntPtr onReset;
    }
}
