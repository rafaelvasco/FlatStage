using System;
using System.IO;

namespace FlatStage.ContentPipeline;

internal static class WavBuilder
{
    private const ushort BitsPerSample = 16;
    private const int FormatChunkSize = 16;
    private const ushort AudioFormat = 1;

    public static unsafe byte[] WriteWave(short[] data, int sampleRate, int channels)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        Span<char> riffChunk = stackalloc char[4] { 'R', 'I', 'F', 'F' };
        Span<char> waveChunk = stackalloc char[4] { 'W', 'A', 'V', 'E' };
        Span<char> fmtChunk = stackalloc char[4] { 'f', 'm', 't', ' ' };
        Span<char> dataChunk = stackalloc char[4] { 'd', 'a', 't', 'a' };

        var byteRate = (uint)(sampleRate * channels * BitsPerSample);
        var blockAlign = (ushort)(channels * (BitsPerSample / 8));
        var dataSize = (uint)(data.Length);
        var chunkSize = 36 + dataSize;

        // RIFF
        writer.Write(riffChunk); // ChunkID // WAVE
        writer.Write(chunkSize); // ChunkSize
        writer.Write(waveChunk); // Format

        //FMT
        writer.Write(fmtChunk);  // Subchunk1ID
        writer.Write(FormatChunkSize); // Subchunk1Size
        writer.Write(AudioFormat); // AudioFormat
        writer.Write((ushort)channels); // NumChannels
        writer.Write(sampleRate); // SampleRate
        writer.Write(byteRate); // ByteRate
        writer.Write(blockAlign); // BlockALign
        writer.Write(BitsPerSample); // BitsPerSample

        // DATA
        writer.Write(dataChunk); // Subchunk2ID
        writer.Write(dataSize); // Subchunk2Size

        // Data
        for (int i = 0; i < data.Length; ++i)
        {
            writer.Write(data[i]);
        }

        return stream.ToArray();
    }
}
