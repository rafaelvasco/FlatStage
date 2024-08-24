namespace MINIAUDIO
{
    public unsafe partial struct ma_resource_manager_data_buffer_node
    {

        public uint hashedName32;


        public uint refCount;

        public ma_result result;


        public uint executionCounter;


        public uint executionPointer;


        public uint isDataOwnedByResourceManager;

        public ma_resource_manager_data_supply data;

        public ma_resource_manager_data_buffer_node* pParent;

        public ma_resource_manager_data_buffer_node* pChildLo;

        public ma_resource_manager_data_buffer_node* pChildHi;
    }
}
