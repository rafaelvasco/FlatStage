using FlatStage.Foundation.FreeType;
using FlatStage.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlatStage.Content;
internal unsafe class FontCompiler : IDisposable
{
    private const int MAX_BITMAP_SIZE = 4096;

    private RectPacker _rectPacker = null!;
    private readonly FT_LibraryRec* _ftLibrary;

    internal FontCompiler()
    {
        FT_LibraryRec* lib = (FT_LibraryRec*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FT_LibraryRec)));
        var error = FT.FT_Init_FreeType(&lib);

        if (error != FT_Error.FT_Err_Ok)
        {
            throw new Exception("Error On Initializing FreeType");
        }

        _ftLibrary = lib;
    }

    public FontData Build(string id, byte[] fontData, int initialBitmapSize, int fontSize, CharRange charRange, int padding)
    {

        var glyphTargetRegionsOnBitmap = new List<PackerRect>();

        _rectPacker = new RectPacker(initialBitmapSize, initialBitmapSize);

        if (fontData == null || fontData.Length == 0)
        {
            throw new ArgumentNullException(nameof(fontData));
        }

        if (fontSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fontSize));
        }

        FT_FaceRec* face;

        if (FT.FT_New_Memory_Face(_ftLibrary, (byte*)Unsafe.AsPointer(ref fontData[0]), fontData.Length, 0, &face) != FT_Error.FT_Err_Ok)
        {
            throw new ArgumentException("FontCompiler::Add: Unable to create font face.");
        }

        if (FT.FT_Set_Pixel_Sizes(face, 0, (uint)(fontSize)) != FT_Error.FT_Err_Ok)
        {
            throw new ArgumentException($"FontCompiler::Add: Could not set given fontSize: {fontSize}");
        }

        // Build out glyph target rectangles on bitmap

        for (int i = charRange.Start; i <= charRange.End; ++i)
        {
            var glyphIndex = FT.FT_Get_Char_Index(face, (uint)i);

            if (glyphIndex == 0)
            {
                continue;
            }

            FT.FT_Load_Glyph(face, glyphIndex, FT_Load.FT_LOAD_NO_BITMAP);

            int glyphW = face->glyph->metrics.width / 64;
            int glyphH = face->glyph->metrics.height / 64;

            if (glyphW <= 0 || glyphH <= 0)
            {
                glyphW = (face->glyph->metrics.horiBearingX + face->glyph->metrics.horiAdvance) / 64;
                glyphH = face->glyph->metrics.vertBearingY / 64;
            }

            var packerRect = _rectPacker.PackRect(glyphW + (padding * 2), glyphH + (padding * 2), i);

            while (packerRect == null)
            {
                if (_rectPacker.Width == MAX_BITMAP_SIZE)
                {
                    throw new Exception($"FontCompiler: Can't fit all the fonts characters in the bitmap. Maximum Size is: {MAX_BITMAP_SIZE}");
                }

                var newPacker = new RectPacker(_rectPacker.Width * 2, _rectPacker.Height * 2);

                foreach (var existingRect in glyphTargetRegionsOnBitmap)
                {
                    newPacker.PackRect(existingRect.Width, existingRect.Height, existingRect.Data);
                }

                _rectPacker.Dispose();

                _rectPacker = newPacker;

                packerRect = _rectPacker.PackRect(glyphW + (padding * 2), glyphH + (padding * 2), i);
            }

            glyphTargetRegionsOnBitmap.Add(packerRect.Value);
        }

        // Render Glyphs on Bitmap

        int finalBitmapSize = _rectPacker.Width;

        int bboxYmax = face->bbox.yMax / 64;

        byte[] fontBitmapData = new byte[(finalBitmapSize * finalBitmapSize) * 4];

        var glyphInfos = new Dictionary<int, GlyphInfo>();

        foreach (var rect in glyphTargetRegionsOnBitmap)
        {
            var glyphIndex = FT.FT_Get_Char_Index(face, (uint)rect.Data);
            if (FT.FT_Load_Glyph(face, glyphIndex, FT_Load.FT_LOAD_DEFAULT) != FT_Error.FT_Err_Ok)
            {
                throw new Exception($"Failed To Load Glyph Info: {(char)rect.Data}");
            }

            int glyphWidth = face->glyph->metrics.width / 64;
            int glyphHeight = face->glyph->metrics.height / 64;

            int advance = face->glyph->metrics.horiAdvance / 64;

            int xOffset = (advance - glyphWidth) / 2;
            int yOffset = bboxYmax - (face->glyph->metrics.horiBearingY / 64);

            if (FT.FT_Render_Glyph(face->glyph, FT_Render_Mode.FT_RENDER_MODE_NORMAL) != FT_Error.FT_Err_Ok)
            {
                throw new Exception($"Failed To Render Glyph: {(char)rect.Data}");
            }

            var glyphBmp = face->glyph->bitmap;

            // Render Glyph On fontBitmapData Buffer

            for (var y = 0; y < glyphBmp.rows; ++y)
            {
                for (var x = 0; x < glyphBmp.width; ++x)
                {
                    var glyphColorOnXY = glyphBmp.buffer[(y * glyphBmp.pitch) + x];

                    var targetIndex = (rect.X + x + padding + ((rect.Y + y + padding) * finalBitmapSize)) * 4;

                    fontBitmapData[targetIndex + 0] = glyphColorOnXY;
                    fontBitmapData[targetIndex + 1] = glyphColorOnXY;
                    fontBitmapData[targetIndex + 2] = glyphColorOnXY;
                    fontBitmapData[targetIndex + 3] = (byte)(glyphColorOnXY > 0 ? 255 : 0);
                }
            }

            // Save Glyph Info on Glyphs Dictionary
            var glyphInfo = new GlyphInfo()
            {
                X = rect.X + padding,
                Y = rect.Y + padding,
                Width = rect.Width - (padding * 2),
                Height = rect.Height - (padding * 2),
                XAdvance = advance,
                XOffset = xOffset,
                YOffset = yOffset,
            };

            glyphInfos[rect.Data] = glyphInfo;

        }

        // Offset EveryGlyph By The Minimum YOffset of All Glyphs
        var minOffsetY = int.MaxValue;

        foreach (var (_, glyph) in glyphInfos)
        {
            if (glyph.YOffset < minOffsetY)
            {
                minOffsetY = glyph.YOffset;
            }
        }

        foreach (var (key, _) in glyphInfos)
        {
            var gl = glyphInfos[key];
            gl.YOffset -= minOffsetY;
            glyphInfos[key] = gl;
        }

        var finalFontImage = BuildFontImage(fontBitmapData, finalBitmapSize, finalBitmapSize);

        var resultFontData = new FontData
        {
            Id = id,
            Glyphs = glyphInfos,
            Width = finalBitmapSize,
            Height = finalBitmapSize,
            ImageData = new ImageData { Data = finalFontImage, Id = id + "_ImageData", Width = finalBitmapSize, Height = finalBitmapSize }

        };

        FT.FT_Done_Face(face);

        return resultFontData;
    }

    private static byte[] BuildFontImage(byte[] fontBitmapData, int width, int height)
    {
        // Convert RgbaToBgra
        Blitter.Begin(fontBitmapData, width, height);
        Blitter.ConvertRgbaToBgra(premultiplyAlpha: true);
        Blitter.End();

        ImageIO.SavePNGToMem(fontBitmapData, width, height, out var fontImagePng);

        return fontImagePng;
    }

    public void Dispose()
    {
        FT.FT_Done_FreeType(_ftLibrary);

        Marshal.FreeHGlobal((nint)_ftLibrary);
    }
}
