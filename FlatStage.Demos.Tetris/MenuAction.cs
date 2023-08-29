namespace FlatStage.Tetris;

public class MenuAction
{
    public MenuActionId ActionId { get; set; }

    public required string Label { get; set; }

    public Rect Rect { get; set; }
}
