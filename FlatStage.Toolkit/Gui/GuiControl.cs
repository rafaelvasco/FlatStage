namespace FlatStage.Toolkit;

public abstract class GuiControl : GameObject
{
    protected readonly Gui _gui;

    public abstract Size SizeHint { get; }

    protected GuiControl(string id, Gui gui) : base(id)
    {
        _gui = gui;
        _gui.Register(this);
    }

    public override void InitFromDefinition(GameObjectDef definition)
    {
        base.InitFromDefinition(definition);

        Width = SizeHint.Width;
        Height = SizeHint.Height;

        Origin = Vec2.Zero;
    }
}
