using FlatStage.Graphics;
using FlatStage.Input;
using System;
using System.Collections.Generic;

namespace FlatStage.Toolkit;

internal class GuiMouseState
{
    public int MouseX;
    public int MouseY;
    public int LastMouseX;
    public int LastMouseY;
    public bool MouseLeftDown;
    public bool MouseRightDown;
    public bool MouseMiddleDown;

    public void UpdatePosition(int x, int y)
    {
        LastMouseX = MouseX;
        LastMouseY = MouseY;
        MouseX = x;
        MouseY = y;
    }

    public bool Moved => MouseX != LastMouseX || MouseY != LastMouseY;
}

public enum GuiOrientation
{
    Horizontal,
    Vertical
}

public enum GuiDocking
{
    None,
    Top,
    Center,
    Bottom,
    Left,
    Right
}

public class Gui
{
    public int Width => _guiViewport.Width;

    public int Height => _guiViewport.Height;

    public GuiSkin Skin { get; private set; }

    public Gui()
    {
        _controls = new List<GuiControl>();

        _guiViewport = new CanvasViewport(Canvas.Width, Canvas.Height)
        {
            BackgroundColor = Color.Transparent
        };

        _mouseState = new GuiMouseState();

        Mouse.OnMouseDown += (MouseButton button) =>
        {
            ProcessMouseButtonEvent(button, true);
        };

        Mouse.OnMouseUp += (MouseButton button) =>
        {
            ProcessMouseButtonEvent(button, false);
        };

        Mouse.OnMouseMove += ProcessMouseMoveEvent;

        Skin = new GuiSkinFlat();
    }

    public void Invalidate()
    {
        _visualInvalidated = true;
    }

    internal void Register(GuiControl control)
    {
        _controls.Add(control);
    }

    private void ProcessMouseButtonEvent(MouseButton button, bool down)
    {
        _mouseState.MouseLeftDown = button == MouseButton.Left && down;
        _mouseState.MouseRightDown = button == MouseButton.Right && down;
        _mouseState.MouseMiddleDown = button == MouseButton.Middle && down;

        if (down && _hoveredControl != null)
        {
            ProcessActive(_hoveredControl);
        }
        else
        {
            ProcessActive(null);
        }
    }

    private void ProcessMouseMoveEvent(int x, int y)
    {
        _mouseState.UpdatePosition(x, y);

        if (_activeControl == null)
        {
            for (int i = 0; i < _controls.Count; i++)
            {
                var control = _controls[i];

                if (control.ContainsPoint(x, y))
                {
                    ProcessHover(control);
                }
                else
                {
                    ProcessHover(null);
                }
            }
        }
        else
        {
            _activeControl.ProcessMouseEvent(_mouseState);

            if (!_activeControl.TrackInputOutsideArea && !_activeControl.ContainsPoint(x, y))
            {
                ProcessActive(null);
            }
        }

    }

    public void Draw(Canvas canvas)
    {
        if (_visualInvalidated)
        {
            Console.WriteLine("Redraw GUI");

            canvas.SetViewport(_guiViewport);

            for (var i = 0; i < _controls.Count; ++i)
            {
                var control = _controls[i];

                control.Draw(canvas, Skin);
            }

            _visualInvalidated = false;
        }

        canvas.SetViewport();

        canvas.Draw(_guiViewport.Texture, Vec2.Zero, Color.White);
    }

    private void ProcessHover(GuiControl? control)
    {
        _lastHoveredControl = _hoveredControl;

        _hoveredControl = control;

        if (_hoveredControl != _lastHoveredControl)
        {
            if (_hoveredControl != null)
            {
                _hoveredControl.Hovered = true;
            }
            else if (_lastHoveredControl != null)
            {
                _lastHoveredControl.Hovered = false;
            }

            Invalidate();
        }
    }

    private void ProcessActive(GuiControl? control)
    {
        _lastActiveControl = _activeControl;

        _activeControl = control;

        if (_activeControl != null)
        {
            _activeControl.Active = true;
            _activeControl.ProcessMouseEvent(_mouseState);
        }
        else if (_lastActiveControl != null)
        {
            _lastActiveControl.Active = false;
            _lastActiveControl.ProcessMouseEvent(_mouseState);

            if (!_lastActiveControl.ContainsPoint(_mouseState.MouseX, _mouseState.MouseY))
            {
                ProcessHover(null);
            }
        }

        Invalidate();
    }

    internal void Process(float dt) { }

    private List<GuiControl> _controls;
    private CanvasViewport _guiViewport;
    private GuiMouseState _mouseState;
    private GuiControl? _hoveredControl;
    private GuiControl? _lastHoveredControl;
    private GuiControl? _activeControl;
    private GuiControl? _lastActiveControl;
    private bool _layoutInvalidated = true;
    private bool _visualInvalidated = true;
}
