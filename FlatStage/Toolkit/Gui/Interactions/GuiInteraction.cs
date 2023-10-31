namespace FlatStage.Toolkit;
public abstract class GuiInteraction
{
    protected GuiControl Parent;

    protected GuiInteraction(GuiControl parent)
    {
        Parent = parent;
    }

    public abstract bool ProcessMouseButton(GuiMouseState mouseState);

    public abstract bool ProcessMouseMove(GuiMouseState mouseState);
}
