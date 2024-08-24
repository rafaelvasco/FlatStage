namespace FlatStage.Toolkit;

public class InteractablesMouseState
{
    public int MouseX;
    public int MouseY;
    public int LastMouseX;
    public int LastMouseY;
    public MouseButton LastMouseButton;
    public bool MouseButtonDown;

    public void UpdatePosition(int x, int y)
    {
        LastMouseX = MouseX;
        LastMouseY = MouseY;
        MouseX = x;
        MouseY = y;
    }

    public bool Moved => MouseX != LastMouseX || MouseY != LastMouseY;
}


public delegate void InteractableEventHandler(GameObject gameObject);

public class InteractablesSystem : System<Interactable>
{
    public event InteractableEventHandler? OnInteractableHover;
    public event InteractableEventHandler? OnInteractableMouseOut;
    public event InteractableEventHandler? OnInteractableMouseDown;
    public event InteractableEventHandler? OnInteractableMouseUp;

    public Interactable? Hovered => _hovered;
    public Interactable? Active => _active;
    public Interactable? Focused => _focused;
    public Interactable? MouseLocked => _mouseLocked;

    public InteractablesMouseState MouseState => _mouseState;

    public InteractablesSystem()
    {
        _mouseState = new InteractablesMouseState();

        Mouse.OnMouseDown += button => ProcessMouseButton(button, true);
        Mouse.OnMouseUp += button => ProcessMouseButton(button, false);
        Mouse.OnMouseMove += ProcessMouseMove;
        Mouse.OnMouseExited += ProcessMouseExitedScreen;
    }

    private void ProcessMouseExitedScreen(int x, int y)
    {
        if (_mouseLocked != null)
        {
            return;
        }

        ProcessHover(null);
    }

    private void ProcessMouseButton(MouseButton button, bool down)
    {
        _mouseState.LastMouseButton = button;
        _mouseState.MouseButtonDown = down;

        if (down && _hovered != null)
        {
            if (_mouseLocked != null && !_hovered.Equals(_mouseLocked))
            {
                MouseLock(null);
            }

            ProcessPressed(_hovered);

            if (_hovered.CaptureMouseOnPress)
            {
                MouseLock(_hovered);
            }
        }
        else
        {
            ProcessPressed(null);
        }
    }

    protected void ProcessMouseMove(int x, int y)
    {
        _mouseState.UpdatePosition(x, y);

        if (_mouseLocked == null)
        {
            var components = _systemComponents.ReadOnlySpan;
            foreach (var component in components)
            {
                var node = component.Parent;

                if (node.Visible && node.GlobalBounds.Contains(x, y))
                {
                    if (_hovered == null || node.TreeDepth >= _hovered.Parent.TreeDepth)
                    {
                        ProcessHover(component);
                    }

                    break;
                }

                if (component.Equals(_hovered))
                {
                    if (_active != null && _active.Equals(_hovered))
                    {
                        ProcessPressed(null);
                    }

                    ProcessHover(null);
                }
            }
        }
        else
        {
            if (_mouseLocked.Draggable)
            {
                int dX = _mouseState.MouseX - _mouseState.LastMouseX;
                int dY = _mouseState.MouseY - _mouseState.LastMouseY;

                if (dX == 0 && dY == 0) return;

                _mouseLocked.Parent.X += dX;
                _mouseLocked.Parent.Y += dY;
            }
            else
            {
                _mouseLocked.ProcessMouseMove(_mouseState.MouseX, _mouseState.MouseY);
            }
        }
    }

    public override Type GetComponentType()
    {
        return typeof(Interactable);
    }

    public override void Update(float dt) { }

    public void Focus(Interactable? interactable)
    {
        ProcessFocused(interactable);
    }

    public override void _RegisterNode(GameObject gameObject)
    {
        base._RegisterNode(gameObject);

        _systemComponents.Sort(
            (interactable, interactable1) =>
                interactable1.Parent.TreeDepth - interactable.Parent.TreeDepth
        );
    }

    public override void _UnRegisterNode(GameObject gameObject)
    {
        var interactable = Behaviors.GetComponent<Interactable>(gameObject);

        if (_hovered == interactable)
        {
            _hovered = null;
            _lastHovered = null;
        }

        if (_mouseLocked == interactable)
        {
            _mouseLocked = null;
        }

        if (_active == interactable)
        {
            _active = null;
            _lastActive = null;
        }

        if (_focused == interactable)
        {
            _focused = null;
        }

        base._UnRegisterNode(gameObject);
    }

    public void MouseLock(Interactable? interactable)
    {
        if (_mouseLocked != null && _mouseLocked.Equals(interactable)) return;

        _mouseLocked?.ProcessMouseLockedChanged(false);

        _mouseLocked = interactable;

        _mouseLocked?.ProcessMouseLockedChanged(true);

    }

    private void ProcessHover(Interactable? interactable)
    {
        _lastHovered = _hovered;

        _hovered = interactable;

        if (!ReferenceEquals(_hovered, _lastHovered))
        {
            if (_lastHovered != null)
            {
                _lastHovered.Hovered = false;
                _lastHovered.ProcessMouseExited();
                OnInteractableMouseOut?.Invoke(_lastHovered.Parent);
            }

            if (_hovered != null)
            {
                _hovered.Hovered = true;
                _hovered.ProcessMouseEntered();
                OnInteractableHover?.Invoke(_hovered.Parent);
            }
        }
    }

    private void ProcessPressed(Interactable? interactable)
    {
        _lastActive = _active;

        if (interactable != null)
        {
            ProcessFocused(interactable);
            interactable.Pressed = true;
            interactable.ProcessMouseButton(_mouseState.LastMouseButton, _mouseState.MouseButtonDown);
            OnInteractableMouseDown?.Invoke(interactable.Parent);
            _active = interactable;

        }
        else if (_lastActive != null)
        {
            _lastActive.ProcessMouseButton(_mouseState.LastMouseButton, _mouseState.MouseButtonDown);
            _lastActive.Pressed = false;
            OnInteractableMouseUp?.Invoke(_lastActive.Parent);

            if (!_lastActive.Parent.GlobalBounds.Contains(_mouseState.MouseX, _mouseState.MouseY))
            {
                ProcessHover(null);
            }

            _active = null;
        }
    }

    private void ProcessFocused(Interactable? node)
    {
        if (_focused != null && !_focused.Equals(node))
        {
            _focused.ProcessFocusedChanged(false);
        }

        _focused = node;
        _focused?.ProcessFocusedChanged(true);

    }

    private readonly InteractablesMouseState _mouseState;

    private Interactable? _mouseLocked;
    private Interactable? _hovered;
    private Interactable? _lastHovered;
    private Interactable? _active;
    private Interactable? _lastActive;
    private Interactable? _focused;
}
