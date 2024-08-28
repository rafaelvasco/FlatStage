namespace MINIAUDIO
{
    public unsafe partial struct ma_resource_manager_config
    {
        public ma_allocation_callbacks allocationCallbacks;

        public ma_log* pLog;

        public ma_format decodedFormat;


        public uint decodedChannels;


        public uint decodedSampleRate;


        public uint jobThreadCount;


        public ulong jobThreadStackSize;


        public uint jobQueueCapacity;


        public uint flags;


        public void* pVFS;

        public ma_decoding_backend_vtable** ppCustomDecodingBackendVTables;


        public uint customDecodingBackendCount;

        public void* pCustomDecodingBackendUserData;
    }
}
