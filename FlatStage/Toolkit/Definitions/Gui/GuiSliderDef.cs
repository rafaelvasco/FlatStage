namespace FlatStage.Toolkit;
public class GuiSliderDef : GuiControlDef
{
    public float Value { get; init; }

    public float MinValue { get; init; }

    public float MaxValue { get; init; }

    public float Step { get; init; }

    public int ThumbWidth { get; init; } = 30;
}
