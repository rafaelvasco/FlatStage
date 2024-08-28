namespace MINIAUDIO
{
    public unsafe partial struct ma_log_callback
    {

        public IntPtr onLog;

        public void* pUserData;
    }
}
