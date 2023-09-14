using FlatStage.ContentPipeline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace FlatStage.Graphics;

public class TextureFont : Asset
{
    public Glyph[] Glyphs => _glyphs;

    public ReadOnlyCollection<char> Characters { get; private set; }

    public Texture Texture => _texture;

    public char? DefaultCharacter
    {
        get => _defaultCharacter;
        set
        {
            if (value.HasValue)
            {
                if (!TryGetGlyphIndex(value.Value, out _defaultGlyphIndex))
                {
                    throw new ArgumentException(Errors.UnresolvableCharacter);
                }
            }
            else
            {
                _defaultGlyphIndex = -1;
            }

            _defaultCharacter = value;
        }
    }

    public int LineSpacing { get; set; }

    public float Spacing { get; set; }

    private readonly Glyph[] _glyphs;
    private readonly CharacterRegion[] _regions;
    private char? _defaultCharacter;
    private int _defaultGlyphIndex = -1;
    private Texture _texture;

    class CharComparer : IEqualityComparer<char>
    {
        public bool Equals(char x, char y)
        {
            return x == y;
        }

        public int GetHashCode(char b)
        {
            return (b);
        }

        public static readonly CharComparer Default = new CharComparer();
    }

    internal TextureFont
        (
            string id,
            Texture texture,
            List<Rect> glyphBounds,
            List<Rect> cropping,
            List<char> characters,
            int lineSpacing,
            float spacing,
            List<Vec3> kerning,
            char? defaultCharacter

        ) : base(id)
    {
        Characters = new ReadOnlyCollection<char>(characters);
        _texture = texture;
        LineSpacing = lineSpacing;
        Spacing = spacing;

        _glyphs = new Glyph[characters.Count];
        var regions = new Stack<CharacterRegion>();

        for (int i = 0; i < characters.Count; ++i)
        {
            _glyphs[i] = new Glyph()
            {
                BoundsInTexture = glyphBounds[i],
                Cropping = cropping[i],
                Character = characters[i],

                LeftSideBearing = kerning[i].X,
                Width = kerning[i].Y,
                RightSideBearing = kerning[i].Z,

                WidthIncludingBearings = kerning[i].X + kerning[i].Y + kerning[i].Z
            };

            if (regions.Count == 0 || characters[i] > (regions.Peek().End + 1))
            {
                regions.Push(new CharacterRegion(characters[i], i));
            }
            else if (characters[i] == (regions.Peek().End + 1))
            {
                var currentRegion = regions.Pop();

                currentRegion.End++;

                regions.Push(currentRegion);
            }
            else
            {
                throw new InvalidOperationException("Invalid TextureFont. Character map must be in ascending order.");
            }
        }

        _regions = regions.ToArray();
        Array.Reverse(_regions);

        DefaultCharacter = defaultCharacter;
    }

    /// <summary>
    /// Returns the size of the contents of a string when
    /// rendered in this font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>The size, in pixels, of 'text' when rendered in
    /// this font.</returns>
    public Vec2 MeasureString(string text, float scaleX = 1.0f, float scaleY = 1.0f)
    {
        var charSource = new CharSource(text);
        MeasureString(in charSource, text.Length, 0, scaleX, scaleY, out var size);
        return size;
    }

    /// <summary>
    /// Returns the size of the contents of a string when
    /// rendered in this font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>The size, in pixels, of 'text' when rendered in
    /// this font.</returns>
    public Vec2 MeasureString(string text, int length, int startIndex, float scaleX = 1.0f, float scaleY = 1.0f)
    {
        var charSource = new CharSource(text);
        MeasureString(in charSource, length, startIndex, scaleX, scaleY, out var size);
        return size;
    }

    /// <summary>
    /// Returns the size of the contents of a StringBuilder when
    /// rendered in this font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>The size, in pixels, of 'text' when rendered in
    /// this font.</returns>
    public Vec2 MeasureString(StringBuilder text, float scaleX = 1.0f, float scaleY = 1.0f)
    {
        var charSource = new CharSource(text);
        MeasureString(in charSource, text.Length, 0, scaleX, scaleY, out var size);
        return size;
    }

    /// <summary>
    /// Returns the size of the contents of a StringBuilder when
    /// rendered in this font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>The size, in pixels, of 'text' when rendered in
    /// this font.</returns>
    public Vec2 MeasureString(StringBuilder text, int length, int startIndex = 0, float scaleX = 1.0f, float scaleY = 1.0f)
    {
        var charSource = new CharSource(text);
        MeasureString(in charSource, length, startIndex, scaleX, scaleY, out var size);
        return size;
    }

    internal unsafe void MeasureString(in CharSource text, int length, int startIndex, float scaleX, float scaleY, out Vec2 size)
    {
        if (text.Length == 0 || length == 0)
        {
            size = Vec2.Zero;
            return;
        }

        if (length > text.Length)
        {
            length = text.Length;
        }

        var width = 0.0f;
        var height = 0.0f;

        var xOffset = 0.0f;
        var firstGlyphOfLine = true;

        fixed (Glyph* glyphsPtr = Glyphs)
        {
            for (var i = startIndex; i < startIndex + length; ++i)
            {
                var c = text[i];

                if (c == '\r')
                    continue;

                if (c == '\n')
                {
                    xOffset = 0;
                    height += LineSpacing;
                    firstGlyphOfLine = true;
                    continue;
                }

                var currentGlyphIndex = GetGlyphIndexOrDefault(c);
                Debug.Assert(currentGlyphIndex >= 0 && currentGlyphIndex < Glyphs.Length, "currentGlyphIndex was outside the bounds of the array.");
                var pCurrentGlyph = glyphsPtr + currentGlyphIndex;

                // The first character on a line might have a negative left side bearing.
                // In this scenario, SpriteBatch/SpriteFont normally offset the text to the right,
                //  so that text does not hang off the left side of its rectangle.
                if (firstGlyphOfLine)
                {
                    xOffset = Math.Max(pCurrentGlyph->LeftSideBearing, 0);
                    firstGlyphOfLine = false;
                }
                else
                {
                    xOffset += Spacing + pCurrentGlyph->LeftSideBearing;
                }

                xOffset += pCurrentGlyph->Width + pCurrentGlyph->RightSideBearing;

                var proposedWidth = xOffset;
                if (proposedWidth > width)
                    width = proposedWidth;

                if (pCurrentGlyph->Cropping.Height > height)
                    height = (pCurrentGlyph->Cropping.Height);
            }
        }

        size.X = width * scaleX;
        size.Y = height * scaleY;
    }

    internal unsafe bool TryGetGlyphIndex(char c, out int index)
    {
        fixed (CharacterRegion* regionsPtr = _regions)
        {
            // Get region Index 
            int regionIdx = -1;
            var l = 0;
            var r = _regions.Length - 1;

            while (l <= r)
            {
                var m = (l + r) >> 1;
                Debug.Assert(m >= 0 && m < _regions.Length, "Index was outside the bounds of the array.");
                if (regionsPtr[m].End < c)
                {
                    l = m + 1;
                }
                else if (regionsPtr[m].Start > c)
                {
                    r = m - 1;
                }
                else
                {
                    regionIdx = m;
                    break;
                }
            }

            if (regionIdx == -1)
            {
                index = -1;
                return false;
            }

            index = regionsPtr[regionIdx].StartIndex + (c - regionsPtr[regionIdx].Start);
        }

        return true;
    }

    internal int GetGlyphIndexOrDefault(char c)
    {
        int glyphIdx;
        if (!TryGetGlyphIndex(c, out glyphIdx))
        {
            if (_defaultGlyphIndex == -1)
                throw new ArgumentException(Errors.TextContainsUnresolvableCharacters, "text");

            return _defaultGlyphIndex;
        }
        else
            return glyphIdx;
    }

    protected override void Free()
    {
        Texture.Dispose();
    }

    private struct CharacterRegion
    {
        public char Start;
        public char End;
        public int StartIndex;

        public CharacterRegion(char start, int startIndex)
        {
            this.Start = start;
            this.End = start;
            this.StartIndex = startIndex;
        }
    }

    /// <summary>
    /// Struct that defines the spacing, Kerning, and bounds of a character.
    /// </summary>
    public struct Glyph
    {
        /// <summary>
        /// The char associated with this glyph.
        /// </summary>
        public char Character;
        /// <summary>
        /// Rectangle in the font texture where this letter exists.
        /// </summary>
        public Rect BoundsInTexture;
        /// <summary>
        /// Cropping applied to the BoundsInTexture to calculate the bounds of the actual character.
        /// </summary>
        public Rect Cropping;
        /// <summary>
        /// The amount of space between the left side of the character and its first pixel in the X dimension.
        /// </summary>
        public float LeftSideBearing;
        /// <summary>
        /// The amount of space between the right side of the character and its last pixel in the X dimension.
        /// </summary>
        public float RightSideBearing;
        /// <summary>
        /// Width of the character before kerning is applied. 
        /// </summary>
        public float Width;
        /// <summary>
        /// Width of the character before kerning is applied. 
        /// </summary>
        public float WidthIncludingBearings;

        public static readonly Glyph Empty = new Glyph();

        public override string ToString()
        {
            return "CharacterIndex=" + Character + ", Glyph=" + BoundsInTexture + ", Cropping=" + Cropping + ", Kerning=" + LeftSideBearing + "," + Width + "," + RightSideBearing;
        }
    }
}
