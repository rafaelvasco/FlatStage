﻿using MemoryPack;
using System.Collections.Generic;
namespace FlatStage.ContentPipeline;

[MemoryPackable]
internal partial class FontData : AssetData
{
    public required ImageData ImageData { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }

    public required Dictionary<int, GlyphInfo> Glyphs { get; init; }

    public override string ToString()
    {
        return $"Id: {Id}\nData: {ImageData.Data.Length}\nWidth: {Width}\nHeight: {Height}\nGlyphs: {Glyphs.Count}";
    }

    public override bool IsValid()
    {
        return ImageData != null && Glyphs != null && Glyphs.Count > 0 && Width > 0 && Height > 0;
    }
}
