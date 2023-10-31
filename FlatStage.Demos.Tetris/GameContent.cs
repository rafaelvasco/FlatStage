using FlatStage.Content;
using FlatStage.Graphics;
using FlatStage.Sound;

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
        TexBackground = Assets.Get<Texture>("bg");
        TexObjects = Assets.Get<Texture>("tetris_sheet");
        FntDefault = BuiltinContent.Fonts.Koubit001;
        FntMenu = BuiltinContent.Fonts.Koubit001;
        FntGameTitle = BuiltinContent.Fonts.Koubit001;

        SfxRotate = Assets.Get<Audio>("rotate_sfx");
        SfxLineClear = Assets.Get<Audio>("lineclear_sfx");
        SfxGameOver = Assets.Get<Audio>("gameover_sfx");
        SfxPlaceBlock = Assets.Get<Audio>("placeblock_sfx");
        SfxFullLinesClear = Assets.Get<Audio>("clear_max");
        SfxMenuHover = Assets.Get<Audio>("menu_hover");
        SngTitle = Assets.Get<Audio>("title_song");

        AudioContext.SetVolume(AudioContext.DefaultSongsGroup, 0.05f);
        AudioContext.SetVolume(AudioContext.DefaultEffectsGroup, 0.05f);
    }
}
