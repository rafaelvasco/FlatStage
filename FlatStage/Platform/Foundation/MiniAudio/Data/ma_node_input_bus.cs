namespace FlatStage.Foundation.MiniAudio
{
    public partial struct ma_node_input_bus
    {
        public ma_node_output_bus head;

        
        public uint nextCounter;

        
        public uint @lock;

        
        public byte channels;
    }
}
