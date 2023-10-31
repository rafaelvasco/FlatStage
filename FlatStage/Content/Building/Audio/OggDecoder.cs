using Stb;

namespace FlatStage.Content;

internal static class OggDecoder
{
    /// <summary>
    /// Builds WAV file data from Ogg File Data
    /// </summary>
    /// <param name="oggData"></param>
    /// <returns></returns>
    public static byte[] Decode(byte[] oggData)
    {
        var samples = StbVorbis.decode_vorbis_from_memory(oggData, out var sampleRate, out var channels);

        var wavData = WavBuilder.WriteWave(samples, sampleRate, channels);

        return wavData;
    }
}
