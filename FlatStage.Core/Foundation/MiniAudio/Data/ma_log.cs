using System.Runtime.CompilerServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_log
    {

        public _callbacks_e__FixedBuffer callbacks;


        public uint callbackCount;

        public ma_allocation_callbacks allocationCallbacks;


        public void* @lock;

        public partial struct _callbacks_e__FixedBuffer
        {
            public ma_log_callback e0;
            public ma_log_callback e1;
            public ma_log_callback e2;
            public ma_log_callback e3;

            public unsafe ref ma_log_callback this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    fixed (ma_log_callback* pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }
}
