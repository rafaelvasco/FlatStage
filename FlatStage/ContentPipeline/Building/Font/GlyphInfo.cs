namespace FlatStage.ContentPipeline;
internal struct GlyphInfo
{
    public int X, Y, Width, Height;
    public int XOffset, YOffset;
    public int XAdvance;

    public override readonly string ToString()
    {
        return $"X: {X}\nY: {Y}\nW: {Width}\nH: {Height}\nXOffset: {XOffset}\nYOffSet: {YOffset}\nXAdvance: {XAdvance}";
    }
}
