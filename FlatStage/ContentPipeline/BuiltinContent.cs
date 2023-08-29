using FlatStage.Graphics;

namespace FlatStage.ContentPipeline;
public static class BuiltinContent
{
    public class BuiltinFonts
    {
        public TextureFont Monogram { get; } = null!;
        public TextureFont ImpactBits { get; } = null!;
        public TextureFont Koubit001 { get; } = null!;
        public TextureFont RobotY { get; } = null!;
        public TextureFont SportyV2 { get; } = null!;

        internal BuiltinFonts()
        {
            Monogram = Content.Get<TextureFont>("monogram", embeddedAsset: true);
            ImpactBits = Content.Get<TextureFont>("impactBits", embeddedAsset: true);
            Koubit001 = Content.Get<TextureFont>("koubit001", embeddedAsset: true);
            RobotY = Content.Get<TextureFont>("robotY", embeddedAsset: true);
            SportyV2 = Content.Get<TextureFont>("sportyV2", embeddedAsset: true);
        }
    }

    public static BuiltinFonts Fonts { get; private set; } = null!;

    internal static void Load()
    {
        Fonts = new BuiltinFonts();
    }
}
