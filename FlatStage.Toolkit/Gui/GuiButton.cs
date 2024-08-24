namespace FlatStage.Toolkit;

public class GuiButton : GuiControl
{
    internal GuiButton(string id, Gui gui) : base(id, gui)
    {
        _label = "Button";
    }

    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            ResizeForLabel();
        }

    }

    public override void InitFromDefinition(GameObjectDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiButtonDef buttonDef)
        {
            Label = buttonDef.Label;
        }
    }

    public override void Draw(Canvas canvas)
    {
        base.Draw(canvas);

        _gui.Theme.DrawButton(canvas, this);
    }

    private void ResizeForLabel()
    {
        var labelMeasure = _gui.Theme.MeasureText(_label);

        if (labelMeasure.X > Width)
        {
            Width = labelMeasure.X + 2 * 20;
        }

        if (labelMeasure.Y > Height)
        {
            Height = labelMeasure.Y + 2 * 10;
        }
    }


    public override Size SizeHint => new(100, 40);

    private string _label;
}
