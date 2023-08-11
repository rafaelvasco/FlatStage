namespace FlatStage;

public class ImageData : AssetData
{
    public byte[]? Data { get; init; }

    public int BytesPerPixel { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }

    public override string ToString()
    {
        return $"Id: {Id}\nData: {Data?.Length}\nBytesPerPixel: {BytesPerPixel}\nWidth: {Width}\nHeight: {Height}";
    }

    public override bool IsValid()
    {
        return Data is { Length: > 0 } && BytesPerPixel > 0 && Width > 0 && Height > 0;
    }
}