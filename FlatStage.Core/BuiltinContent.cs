namespace FlatStage;
public static class BuiltinContent
{
    public class BuiltinFonts
    {
        public TextureFont Monogram { get; }
        public TextureFont Koubit001 { get; }
        public TextureFont ChikareGo2 { get; }

        internal BuiltinFonts()
        {
            Monogram = Content.Get<TextureFont>("monogram", embeddedAsset: true);
            Koubit001 = Content.Get<TextureFont>("koubit001", embeddedAsset: true);
            ChikareGo2 = Content.Get<TextureFont>("chikareGo2", embeddedAsset: true);
        }
    }

    public class BuiltinTextures
    {
        public Texture Logo { get; }

        internal BuiltinTextures()
        {
            Logo = Content.Get<Texture>("logo", embeddedAsset: true);
        }
    }

    public static BuiltinFonts Fonts { get; private set; } = null!;
    public static BuiltinTextures Textures { get; private set; } = null!;

    internal static void Load()
    {
        Fonts = new BuiltinFonts();
        Textures = new BuiltinTextures();
    }

}
