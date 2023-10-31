namespace FlatStage.Toolkit;
public class GuiDragInteraction : GuiInteraction
{
    public GuiDragInteraction(GuiControl parent) : base(parent)
    {
    }

    public override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.MouseButtonDown)
        {
            if (Parent.BoundingRect.Contains(mouseState.MouseX, mouseState.MouseY))
            {
                _dragging = true;
                return true;
            }
        }
        else
        {
            _dragging = false;
            return true;

        }

        return false;
    }

    public override bool ProcessMouseMove(GuiMouseState mouseState)
    {
        if (_dragging)
        {
            int dX = mouseState.MouseX - mouseState.LastMouseX;
            int dY = mouseState.MouseY - mouseState.LastMouseY;

            if (dX != 0 || dY != 0)
            {
                Parent.X += dX;
                Parent.Y += dY;

                return true;
            }
        }

        return false;
    }

    private bool _dragging = false;
}
