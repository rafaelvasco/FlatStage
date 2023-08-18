using Stb;

namespace FlatStage.ContentPipeline;

internal static class OggCompiler
{
    /// <summary>
    /// Builds WAV file data from Ogg File Data
    /// </summary>
    /// <param name="oggData"></param>
    /// <returns></returns>
    public static byte[] Build(byte[] oggData)
    {
        var samples = StbVorbis.decode_vorbis_from_memory(oggData, out var sampleRate, out var channels);

        var wavData = WavBuilder.WriteWave(samples, sampleRate, channels);

        return wavData;
    }
}
