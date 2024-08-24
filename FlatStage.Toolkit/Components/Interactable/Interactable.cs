namespace FlatStage.Toolkit;


public delegate void MouseLockChangedEventHandler(bool locked);

public delegate void FocusChangedEventHandler(bool focused);

public class Interactable : Component
{
    public bool Hovered { get; internal set; }

    public bool Pressed { get; internal set; }

    public bool Focused { get; internal set; }

    public bool MouseLocked { get; internal set; }

    public bool Draggable { get; set; }

    public bool CaptureMouseOnPress { get; set; }

    public bool Dragging { get; private set; }

    public event MouseButtonEventHandler? OnMouseDown;
    public event MouseButtonEventHandler? OnMouseUp;
    public event MouseMoveEventHandler? OnMouseMove;
    public event DefaultEventHandler? OnMouseEntered;
    public event DefaultEventHandler? OnMouseExited;
    public event FocusChangedEventHandler? OnFocusChanged;
    public event MouseLockChangedEventHandler? OnMouseLock;

    internal void ProcessMouseButton(MouseButton button, bool down)
    {
        Pressed = down;

        if (down)
        {
            OnMouseDown?.Invoke(button);
        }
        else
        {
            OnMouseUp?.Invoke(button);
        }
    }

    internal void ProcessMouseMove(int x, int y)
    {
        OnMouseMove?.Invoke(x, y);
    }

    internal void ProcessMouseEntered()
    {
        OnMouseEntered?.Invoke();
    }

    internal void ProcessMouseExited()
    {
        OnMouseExited?.Invoke();
    }

    internal void ProcessFocusedChanged(bool focused)
    {
        Focused = focused;
        OnFocusChanged?.Invoke(focused);
    }

    internal void ProcessMouseLockedChanged(bool locked)
    {
        MouseLocked = locked;

        OnMouseLock?.Invoke(locked);

        if (locked && Draggable)
        {
            Dragging = true;
        }
        else
        {
            Dragging = false;
        }
    }

    public Interactable(GameObject parent) : base(parent)
    {
    }

    public override void Update(float dt)
    {
    }
}
