namespace FlatStage;
public readonly struct CharRange
{
    private static readonly Dictionary<int, string> Names = new()
    {
        {0, "BasicLatin" },
        {1, "Latin1Supplement" },
        {2, "LatinExtendedA" },
        {3, "LatinExtendedB" },
        {4, "Cyrillic" },
        {5, "CyrillicSupplement" },
        {6, "Hiragana" },
        {7, "Katakana" },
        {8, "Greek" },
        {9, "CjkSymbolsAndPunctuation" },
        {10, "CjkUnifiedIdeographs" },
        {11, "HangulCompatibilityJamo" },
        {12, "HangulSyllables" },

    };

    public static readonly CharRange BasicLatin = new(0, 32, 127);
    public static readonly CharRange Latin1Supplement = new(1, 160, 255);
    public static readonly CharRange LatinExtendedA = new(2, 256, 383);
    public static readonly CharRange LatinExtendedB = new(3, 384, 591);
    public static readonly CharRange Cyrillic = new(4, 1024, 1279);
    public static readonly CharRange CyrillicSupplement = new(5, 1280, 1327);
    public static readonly CharRange Hiragana = new(6, 12352, 12447);
    public static readonly CharRange Katakana = new(7, 12448, 12543);
    public static readonly CharRange Greek = new(8, 880, 1023);
    public static readonly CharRange CjkSymbolsAndPunctuation = new(9, 12288, 12351);
    public static readonly CharRange CjkUnifiedIdeographs = new(10, 19968, 40959);
    public static readonly CharRange HangulCompatibilityJamo = new(11, 12592, 12687);
    public static readonly CharRange HangulSyllables = new(12, 44032, 55215);

    public int Id { get; }

    public int Start { get; }

    public int End { get; }

    public int Size => End - Start + 1;

    public CharRange(int id, int start, int end)
    {
        Id = id;
        Start = start;
        End = end;
    }

    public static string NameFromCharRange(CharRange range)
    {
        return Names[range.Id];
    }

    public static CharRange? FromName(string name)
    {
        switch (name)
        {
            case "BasicLatin": return BasicLatin;
            case "Latin1Supplement": return Latin1Supplement;
            case "LatinExtendedA": return LatinExtendedA;
            case "LatinExtendedB": return LatinExtendedB;
            case "Cyrillic": return Cyrillic;
            case "CyrillicSupplement": return CyrillicSupplement;
            case "Hiragana": return Hiragana;
            case "Katakana": return Katakana;
            case "Greek": return Greek;
            case "CjkSymbolsAndPunctuation": return CjkSymbolsAndPunctuation;
            case "CjkUnifiedIdeographs": return CjkUnifiedIdeographs;
            case "HangulCompatibilityJamo": return HangulCompatibilityJamo;
            case "HangulSyllables": return HangulSyllables;
            default: return null;
        }
    }

    public CharRange(int id, int single) : this(id, single, single)
    {
    }
}
