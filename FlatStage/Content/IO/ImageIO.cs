using Stb;
using System.IO;

namespace FlatStage.Content;
public static class ImageIO
{
    /// <summary>
    /// Encodes Image Color Data (4 bytes per pixel) to PNG and saves it to a file;
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <param name="path"></param>
    public static void SavePNG(byte[] imageData, int imageWidth, int imageHeight, string path)
    {
        using var stream = File.OpenWrite(path);
        ImageWriter.WritePng(imageData, imageWidth, imageHeight, ColorComponents.RedGreenBlueAlpha, stream);
    }

    /// <summary>
    /// Encodes Image Color Data (4 bytes per pixel) to PNG and returns a byte array with the resulting data;
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <param name="output"></param>
    public static void SavePNGToMem(byte[] imageData, int imageWidth, int imageHeight, out byte[] output)
    {
        ImageWriter.WritePng(imageData, imageWidth, imageHeight, ColorComponents.RedGreenBlueAlpha, out output);
    }

    /// <summary>
    /// Decodes a PNG Image to Color Data (4 bytes per pixel) and returns the data together with the image size;
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static (byte[] Data, int Width, int Height) LoadPNG(string path)
    {
        using var stream = File.OpenRead(path);

        var imageData = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        return (Data: imageData.Data, Width: imageData.Width, Height: imageData.Height);
    }

    /// <summary>
    /// Receives PNG Image Data from memory, decodes it and returns the Image Color Data with the size; (4 bytes per pixel)
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static (byte[] Data, int Width, int Height) LoadPNGFromMem(byte[] data)
    {
        var imageData = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);

        return (Data: imageData.Data, Width: imageData.Width, Height: imageData.Height);
    }

}
