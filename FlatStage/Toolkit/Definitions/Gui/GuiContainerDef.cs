namespace FlatStage.Toolkit;

public class GuiContainerDef : GuiControlDef
{
    public int Padding { get; init; }

    public GuiControlDef[]? Children { get; init; }

    public override bool IsValid()
    {
        return base.IsValid() && CheckChildrenValid();
    }

    private bool CheckChildrenValid()
    {
        var valid = true;

        if (Children != null)
        {
            foreach (var child in Children)
            {
                valid &= child.IsValid();
            }
        }

        return valid;
    }
}
