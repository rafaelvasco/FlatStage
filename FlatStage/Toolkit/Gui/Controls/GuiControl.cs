using FlatStage.Graphics;
using FlatStage.Input;

namespace FlatStage.Toolkit;

public delegate void MouseButtonEventHandler(MouseButton mouseButton);
public delegate void MouseMoveEventHandler(int x, int y);

public abstract class GuiControl
{
    public event MouseButtonEventHandler? OnMouseDown;
    public event MouseButtonEventHandler? OnMouseUp;
    public event MouseMoveEventHandler? OnMouseMove;

    public string Id { get; private set; }

    public int X { get; internal set; }
    public int Y { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }

    public int Depth { get; internal set; }

    public bool ProcessUpdate { get; set; } = false;

    public bool TrackInputOutsideArea { get; set; } = false;

    public bool ReceiveTextInputEvents { get; set; } = false;

    public bool CanGetFocus { get; set; } = false;

    public abstract Size SizeHint { get; }

    public int GlobalX => Parent?.GlobalX + X ?? X;

    public int GlobalY => Parent?.GlobalY + Y ?? Y;

    public GuiControlState State
    {
        get
        {
            if (Focused)
            {
                return GuiControlState.Focused;
            }

            if (!Hovered && !Active)
            {
                return GuiControlState.Idle;
            }

            if (Hovered && !Active)
            {
                return GuiControlState.Hover;
            }

            return GuiControlState.Active;
        }
    }

    public bool Hovered { get; internal set; }

    public bool Active { get; internal set; }

    public bool Focused { get; internal set; }

    public bool FixedSize { get; set; }

    public Rect BoundingRect => new(GlobalX, GlobalY, Width, Height);

    internal int LayoutWidth { get; set; }

    internal int LayoutHeight { get; set; }

    public GuiDocking Docking
    {
        get => _docking;
        set
        {
            if (value != _docking)
            {
                _docking = value;

            }
        }
    }

    public GuiControl? Parent
    {
        get => _parent;
        set
        {
            if (_parent != value)
            {
                _parent = value;
                _parent?.InternalProcessChildAdded(this);
                RecalculateDepth(this);
            }
        }
    }

    protected GuiControl(string id, Gui gui, GuiControl? parent = null)
    {
        Id = id;
        Gui = gui;
        Parent = parent;
        Width = SizeHint.Width;
        Height = SizeHint.Height;

        Gui.Register(this);
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Resize(int width, int height)
    {
        if (FixedSize && (width > 0 || height > 0))
        {
            return;
        }

        if (Width == width && Height == height)
        {
            return;
        }

        Width = width;
        Height = height;

        OnResize(width, height);
    }

    public virtual bool ContainsPoint(int x, int y)
    {
        return BoundingRect.Contains(x, y);
    }

    protected (int X, int Y) ToLocalPos(int x, int y)
    {
        return (x - GlobalX, y - GlobalY);
    }

    internal bool InternalProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.MouseButtonDown)
        {
            OnMouseDown?.Invoke(mouseState.LastMouseButton);
        }
        else
        {
            OnMouseUp?.Invoke(mouseState.LastMouseButton);
        }

        return ProcessMouseButton(mouseState);
    }

    internal bool InternalProcessMouseMove(GuiMouseState mouseState)
    {
        OnMouseMove?.Invoke(mouseState.MouseX, mouseState.MouseY);

        return ProcessMouseMove(mouseState);
    }

    internal void InternalProcessFocusChanged(bool focused)
    {
        Focused = focused;

        ProcessFocusChanged(focused);
    }

    internal bool InternalProcessUpdate(float dt)
    {
        return Update(dt);
    }

    internal void InternalProcessDraw(Canvas canvas, GuiSkin skin)
    {
        Draw(canvas, skin);
    }

    internal void InternalProcessChildAdded(GuiControl control)
    {
        OnChildAdded(control);
    }

    internal bool InternalProcessKeyboardKey(Key key, bool down)
    {
        return ProcessKeyboardKey(key, down);
    }

    internal bool InternalProcessTextInput(TextInputEventArgs args)
    {
        return ProcessTextInput(args);
    }

    protected virtual void OnChildAdded(GuiControl control) { }

    protected virtual bool Update(float dt) { return false; }

    protected virtual void Draw(Canvas canvas, GuiSkin skin) { }

    protected virtual void OnResize(int width, int height) { }

    protected virtual bool ProcessMouseButton(GuiMouseState mouseState) { return false; }

    protected virtual bool ProcessKeyboardKey(Key key, bool down) { return false; }

    protected virtual bool ProcessMouseMove(GuiMouseState mouseState) { return false; }

    protected virtual void ProcessFocusChanged(bool focused) { }

    protected virtual bool ProcessTextInput(TextInputEventArgs args) { return false; }

    private void RecalculateDepth(GuiControl control)
    {
        if (control.Parent == null) return;
        Depth++;
        RecalculateDepth(control.Parent);
    }

    private GuiControl? _parent;
    protected readonly Gui Gui;
    private GuiDocking _docking;
}
