using FlatStage;


namespace FlatStage.Tetris;
public static class GameContent
{
    public static Texture TexBackground { get; private set; } = null!;
    public static Texture TexObjects { get; private set; } = null!;
    public static TextureFont FntDefault { get; private set; } = null!;
    public static TextureFont FntMenu { get; private set; } = null!;
    public static TextureFont FntGameTitle { get; private set; } = null!;
    public static Sound SfxRotate { get; private set; } = null!;
    public static Sound SfxGameOver { get; private set; } = null!;
    public static Sound SfxLineClear { get; private set; } = null!;
    public static Sound SfxFullLinesClear { get; private set; } = null!;
    public static Sound SfxPlaceBlock { get; private set; } = null!;
    public static Sound SfxMenuHover { get; private set; } = null!;
    public static Sound SngTitle { get; private set; } = null!;

    public static void Load()
    {
        TexBackground = Content.Get<Texture>("bg");
        TexObjects = Content.Get<Texture>("tetris_sheet");
        FntDefault = BuiltinContent.Fonts.Koubit001;
        FntMenu = BuiltinContent.Fonts.Koubit001;
        FntGameTitle = BuiltinContent.Fonts.Koubit001;

        SfxRotate = Content.Get<Sound>("rotate_sfx");
        SfxLineClear = Content.Get<Sound>("lineclear_sfx");
        SfxGameOver = Content.Get<Sound>("gameover_sfx");
        SfxPlaceBlock = Content.Get<Sound>("placeblock_sfx");
        SfxFullLinesClear = Content.Get<Sound>("clear_max");
        SfxMenuHover = Content.Get<Sound>("menu_hover");
        SngTitle = Content.Get<Sound>("title_song");

        Audio.SetGlobalVolume(0.05f);
    }
}
