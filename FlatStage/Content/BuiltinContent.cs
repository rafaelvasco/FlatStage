using FlatStage.Graphics;

namespace FlatStage.Content;
public static class BuiltinContent
{
    public class BuiltinFonts
    {
        public TextureFont Monogram { get; } = null!;
        public TextureFont Koubit001 { get; } = null!;

        public TextureFont ChikareGo2 { get; } = null!;

        internal BuiltinFonts()
        {
            Monogram = Assets.Get<TextureFont>("monogram", embeddedAsset: true);
            Koubit001 = Assets.Get<TextureFont>("koubit001", embeddedAsset: true);
            ChikareGo2 = Assets.Get<TextureFont>("chikareGo2", embeddedAsset: true);
        }
    }

    public class BuiltinTextures
    {
        public Texture Logo { get; } = null!;

        internal BuiltinTextures()
        {
            Logo = Assets.Get<Texture>("logo", embeddedAsset: true);
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
