using FlatStage;


namespace FlatStage.Tetris;
public static class GameContent
{
    public static Texture TexBackground { get; private set; } = null!;
    public static Texture TexObjects { get; private set; } = null!;
    public static TextureFont FntDefault { get; private set; } = null!;
    public static TextureFont FntMenu { get; private set; } = null!;
    public static TextureFont FntGameTitle { get; private set; } = null!;
    public static Audio SfxRotate { get; private set; } = null!;
    public static Audio SfxGameOver { get; private set; } = null!;
    public static Audio SfxLineClear { get; private set; } = null!;
    public static Audio SfxFullLinesClear { get; private set; } = null!;
    public static Audio SfxPlaceBlock { get; private set; } = null!;
    public static Audio SfxMenuHover { get; private set; } = null!;
    public static Audio SngTitle { get; private set; } = null!;

    public static void Load()
    {
        TexBackground = Content.Get<Texture>("bg");
        TexObjects = Content.Get<Texture>("tetris_sheet");
        FntDefault = BuiltinContent.Fonts.Koubit001;
        FntMenu = BuiltinContent.Fonts.Koubit001;
        FntGameTitle = BuiltinContent.Fonts.Koubit001;

        SfxRotate = Content.Get<Audio>("rotate_sfx");
        SfxLineClear = Content.Get<Audio>("lineclear_sfx");
        SfxGameOver = Content.Get<Audio>("gameover_sfx");
        SfxPlaceBlock = Content.Get<Audio>("placeblock_sfx");
        SfxFullLinesClear = Content.Get<Audio>("clear_max");
        SfxMenuHover = Content.Get<Audio>("menu_hover");
        SngTitle = Content.Get<Audio>("title_song");

        AudioContext.SetVolume(AudioContext.DefaultSongsGroup, 0.05f);
        AudioContext.SetVolume(AudioContext.DefaultEffectsGroup, 0.05f);
    }
}
