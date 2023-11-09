using FlatStage.Graphics;
using FlatStage.Input;
using System;

namespace FlatStage.Toolkit;

public delegate void MouseButtonEventHandler(MouseButton mouseButton);
public delegate void MouseMoveEventHandler(int x, int y);

public abstract class GuiControl : BaseGameEntity
{
    internal static int AllControlsTypeId = 0;

    protected static int SBTypeId = 0;

    internal abstract int TypeId { get; }

    public event MouseButtonEventHandler? OnMouseDown;
    public event MouseButtonEventHandler? OnMouseUp;
    public event MouseMoveEventHandler? OnMouseMove;
    public event EventHandler? OnMouseEntered;
    public event EventHandler? OnMouseExited;

    public string Id { get; private set; }

    public int X { get; internal set; }
    public int Y { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }

    public int ZIndex
    {
        get => _zIndex;
        set
        {
            if (_zIndex != value)
            {
                _zIndex = value;

                Gui.InvalidateZIndexes();
            }
        }
    }

    public bool ProcessUpdate { get; set; } = false;

    public GuiMouseMoveEventBehavior MouseMoveEventBehavior { get; set; } = GuiMouseMoveEventBehavior.MouseDown;

    public bool ReceiveTextInputEvents { get; set; } = false;

    public bool CanGetFocus { get; set; } = false;

    public abstract Size SizeHint { get; }

    public int GlobalX => Parent?.GlobalX + X ?? X;

    public int GlobalY => Parent?.GlobalY + Y ?? Y;

    public virtual GuiControlState State
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

    public bool MouseFocused { get; internal set; }

    public bool FixedSize { get; set; }

    public bool Hidden { get; set; } = false;

    internal bool GlobalHidden => (Parent?.GlobalHidden ?? false) | Hidden;

    public bool Interactive { get; set; } = true;

    public Rect BoundingRect => new(GlobalX, GlobalY, Width, Height);

    public GuiAnchoring Anchor
    {
        get => _anchor;
        set
        {
            if (value != _anchor)
            {
                _anchor = value;
                Gui.InvalidateLayout();
            }
        }
    }

    public GuiContainer? Parent
    {
        get => _parent;
        internal set
        {
            if (_parent != value)
            {
                _parent?.Remove(this);
                _parent = value;
                _parent?.Add(this);
                RecalculateZIndex(this);
            }
        }
    }

    protected GuiControl(string id, Gui gui, GuiContainer? parent)
    {
        Id = id;
        Gui = gui;
        Width = SizeHint.Width;
        Height = SizeHint.Height;

        Parent = parent;

        gui.RegisterControl(this);
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

        Gui.InvalidateLayout();
    }

    public void AddInteraction<T>() where T : GuiInteraction
    {
        _customInteractions ??= new ComponentsRegistry<GuiInteraction>(this);

        var interaction = (Activator.CreateInstance(typeof(T), this) as GuiInteraction)!;

        _customInteractions.AddComponent(interaction);
    }

    public virtual bool ContainsPoint(int x, int y)
    {
        return BoundingRect.Contains(x, y);
    }

    internal virtual void InitFromDefinition(GuiControlDef definition)
    {
        X = definition.X;
        Y = definition.Y;

        if (definition.Width > 0)
        {
            Width = definition.Width;
        }

        if (definition.Height > 0)
        {
            Height = definition.Height;
        }

        Anchor = definition.Anchor;
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

        bool changed = false;

        if (_customInteractions != null)
        {
            var interactions = _customInteractions.AllComponents.ReadOnlySpan;

            foreach (var interaction in interactions)
            {
                changed |= interaction.ProcessMouseButton(mouseState);
            }
        }

        changed |= ProcessMouseButton(mouseState);

        return changed;
    }

    internal bool InternalProcessMouseMove(GuiMouseState mouseState)
    {
        OnMouseMove?.Invoke(mouseState.MouseX, mouseState.MouseY);

        bool changed = false;

        if (_customInteractions != null)
        {
            var interactions = _customInteractions.AllComponents.ReadOnlySpan;

            foreach (var interaction in interactions)
            {
                changed |= interaction.ProcessMouseMove(mouseState);
            }
        }

        changed |= ProcessMouseMove(mouseState);

        return changed;
    }

    internal void InternalProcessMouseEntered()
    {
        OnMouseEntered?.Invoke(this, EventArgs.Empty);

        ProcessMouseEntered();
    }

    internal void InternalProcessMouseExited()
    {
        OnMouseExited?.Invoke(this, EventArgs.Empty);

        ProcessMouseExited();
    }

    internal void InternalProcessFocusChanged(bool focused)
    {
        Focused = focused;

        ProcessFocusChanged(focused);
    }

    internal void InternalProcessMouseFocusChanged(bool focused)
    {
        MouseFocused = focused;
        ProcessMouseFocusChanged(focused);
    }

    internal bool InternalProcessKeyboardKey(Key key, bool down)
    {
        return ProcessKeyboardKey(key, down);
    }

    internal bool InternalProcessTextInput(TextInputEventArgs args)
    {
        return ProcessTextInput(args);
    }

    internal virtual bool Update(float dt) { return false; }

    internal virtual void Draw(Canvas canvas, GuiSkin skin) { }

    protected (int X, int Y) ToLocalPos(int x, int y)
    {
        return (x - GlobalX, y - GlobalY);
    }

    protected virtual void OnResize(int width, int height) { }

    protected virtual bool ProcessMouseButton(GuiMouseState mouseState) { return false; }

    protected virtual bool ProcessKeyboardKey(Key key, bool down) { return false; }

    protected virtual bool ProcessMouseMove(GuiMouseState mouseState) { return false; }

    protected virtual void ProcessMouseEntered() { }

    protected virtual void ProcessMouseExited() { }

    protected virtual void ProcessFocusChanged(bool focused) { }

    protected virtual void ProcessMouseFocusChanged(bool focused) { }

    protected virtual bool ProcessTextInput(TextInputEventArgs args) { return false; }

    private void RecalculateZIndex(GuiControl control)
    {
        if (control.Parent == null) return;
        _zIndex++;
        RecalculateZIndex(control.Parent);
    }

    private ComponentsRegistry<GuiInteraction>? _customInteractions;
    private int _zIndex;
    private GuiContainer? _parent;
    protected readonly Gui Gui;
    private GuiAnchoring _anchor;
}
