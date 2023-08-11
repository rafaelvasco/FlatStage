using System;

namespace FlatStage.Foundation.NVorbis.Contracts.Ogg;

interface IPacketReader
{
    Memory<byte> GetPacketData(int pagePacketIndex);

    void InvalidatePacketCache(IPacket packet);
}