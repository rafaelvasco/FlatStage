using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public abstract class GuiControl
{
    public event EventHandler? OnMouseClick;
    public event EventHandler? OnMousePressed;
    public event EventHandler? OnMouseReleased;

    public int X { get; internal set; }
    public int Y { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }

    public bool TrackInputOutsideArea { get; set; } = false;

    public abstract Size SizeHint { get; }

    public int GlobalX => Parent?.GlobalX + X ?? X;

    public int GlobalY => Parent?.GlobalY + Y ?? Y;

    public GuiControlState State
    {
        get
        {
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

    public GuiContainer? Parent { get; }

    protected GuiControl(Gui gui, GuiContainer? parent = null)
    {
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
        if (Width == width && Height == height)
        {
            return;
        }

        Width = width;
        Height = height;

    }

    public virtual bool ContainsPoint(int x, int y)
    {
        return BoundingRect.Contains(x, y);
    }

    internal abstract void ProcessMouseEvent(GuiMouseState mouseState);

    internal virtual void Update(float dt) { }

    internal abstract void Draw(Canvas canvas, GuiSkin skin);

    protected readonly Gui Gui;
    private GuiDocking _docking;
}
