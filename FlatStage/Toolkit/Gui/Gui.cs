using FlatStage.ContentPipeline;
using FlatStage.Graphics;
using FlatStage.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatStage.Toolkit;

public class GuiMouseState
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

internal class ControlDepthComparer : IComparer<GuiControl>
{
    public int Compare(GuiControl? x, GuiControl? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (y is null) return 1;
        if (x is null) return -1;
        return y.Depth.CompareTo(x.Depth);
    }
}

public class Gui
{
    public int Width => _guiViewport.Width;

    public int Height => _guiViewport.Height;

    public GuiSkin Skin
    {
        get => _skin;
        set
        {
            if (value != _skin)
            {
                _skin = value;
                Invalidate();
            }
        }
    }

    public TextureFont Font
    {
        get => _font;
        set
        {
            if (value != _font)
            {
                _font = value;
                InvalidateLayout();
            }
        }
    }

    public Gui()
    {
        _controls = new List<GuiControl>();

        _font = BuiltinContent.Fonts.Monogram;

        _controlsById = new Dictionary<string, int>();

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

        Keyboard.OnKeyDown += (Key key) =>
        {
            ProcessKeyboardEvent(key, true);
        };

        Keyboard.OnKeyUp += (Key key) =>
        {
            ProcessKeyboardEvent(key, false);
        };

        Keyboard.OnTextInput += ProcessTextInputEvent;

        _skin = new GuiSkinFlat(this);

        _controlDepthComparer = new ControlDepthComparer();

        _debugText = new StringBuilder();
    }

    internal void Invalidate()
    {
        _visualInvalidated = true;
    }

    internal void InvalidateLayout()
    {
        _layoutInvalidated = true;
    }

    public T Get<T>(string id) where T : GuiControl
    {
        if (_controlsById.TryGetValue(id, out var control))
        {
            return (control as T)!;
        }

        throw new Exception($"Could not find control with Id: {id}");
    }

    public void Update(float dt)
    {
        foreach (var control in _controls)
        {
            if (!control.ProcessUpdate)
            {
                continue;
            }

            if (control.InternalProcessUpdate(dt))
            {
                Invalidate();
            }
        }
    }

    public void Draw(Canvas canvas)
    {
        if (_layoutInvalidated)
        {
            _layoutInvalidated = false;

            ProcessLayout();

            _visualInvalidated = true;
        }

        if (_visualInvalidated)
        {
            //Console.WriteLine("Redraw GUI");

            canvas.SetViewport(_guiViewport);

            for (var i = _controls.Count - 1; i >= 0; --i)
            {
                var control = _controls[i];

                control.InternalProcessDraw(canvas, Skin);
            }

            _visualInvalidated = false;
        }

        canvas.SetViewport();

        canvas.Draw(_guiViewport.Texture, Vec2.Zero, Color.White);

        _debugText.Clear();
        _debugText.Append("Current Hover: ");
        _debugText.Append(_hoveredControl?.Id ?? "None");

        canvas.DrawText(BuiltinContent.Fonts.Monogram, _debugText, new Vec2(20, 20), Color.Red);
    }

    public void Focus(GuiControl? control)
    {
        ProcessFocused(control);
    }

    internal void Register(GuiControl control)
    {
        _controls.Add(control);

        _controlsById[control.Id] = _controls.Count - 1;

        _controls.Sort(_controlDepthComparer);
    }

    private void ProcessMouseButtonEvent(MouseButton button, bool down)
    {
        _mouseState.LastMouseButton = button;
        _mouseState.MouseButtonDown = down;

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
            for (int i = 0; i < _controls.Count; ++i)
            {
                var control = _controls[i];

                if (control.ContainsPoint(x, y) && _hoveredControl != control)
                {
                    if (_hoveredControl == null || _hoveredControl.Depth <= control.Depth)
                    {
                        ProcessHover(control);
                    }

                    break;
                }
                else if (control == _hoveredControl && !control.ContainsPoint(x, y))
                {
                    ProcessHover(null);
                }
            }
        }
        else
        {
            bool changed = _activeControl.InternalProcessMouseMove(_mouseState);

            if (!_activeControl.TrackInputOutsideArea && !_activeControl.ContainsPoint(x, y))
            {
                ProcessActive(null);
            }

            if (changed)
            {
                Invalidate();
            }
        }

    }

    private void ProcessTextInputEvent(TextInputEventArgs args)
    {
        if (_focusedControl?.ReceiveTextInputEvents == true)
        {
            if (_focusedControl.InternalProcessTextInput(args))
            {
                Invalidate();
            }
        }
    }

    private void ProcessKeyboardEvent(Key key, bool down)
    {
        if (_focusedControl != null)
        {
            _focusedControl.InternalProcessKeyboardKey(key, down);
            Invalidate();
        }
    }

    private void ProcessHover(GuiControl? control)
    {
        _lastHoveredControl = _hoveredControl;

        _hoveredControl = control;

        if (_hoveredControl != _lastHoveredControl)
        {
            if (_lastHoveredControl != null)
            {
                _lastHoveredControl.Hovered = false;
            }

            if (_hoveredControl != null)
            {
                _hoveredControl.Hovered = true;
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
            _activeControl.InternalProcessMouseButton(_mouseState);

            ProcessFocused(_activeControl);

        }
        else if (_lastActiveControl != null)
        {
            _lastActiveControl.Active = false;
            _lastActiveControl.InternalProcessMouseButton(_mouseState);

            if (!_lastActiveControl.ContainsPoint(_mouseState.MouseX, _mouseState.MouseY))
            {
                ProcessHover(null);
            }
        }

        Invalidate();
    }

    private void ProcessFocused(GuiControl? control)
    {
        if (control?.CanGetFocus == true)
        {
            if (control != _focusedControl)
            {
                _focusedControl?.InternalProcessFocusChanged(false);
            }

            if (control?.ReceiveTextInputEvents == true)
            {
                Keyboard.ActivateTextInputEvents(true);
            }

            _focusedControl = control!;
            _focusedControl.InternalProcessFocusChanged(true);
        }
        else
        {
            _focusedControl?.InternalProcessFocusChanged(false);
            _focusedControl = null;
            Keyboard.ActivateTextInputEvents(false);
        }
    }

    private void ProcessLayout()
    {
        for (int i = 0; i < _controls.Count; ++i)
        {
            var control = _controls[i];

            if ((control is GuiLayout layout))
            {
                layout.ProcessLayout();
            }
        }
    }

    private GuiSkin _skin;

    private TextureFont _font;

    private readonly List<GuiControl> _controls;
    private readonly Dictionary<string, int> _controlsById;
    private readonly CanvasViewport _guiViewport;
    private readonly GuiMouseState _mouseState;
    private GuiControl? _hoveredControl;
    private GuiControl? _lastHoveredControl;
    private GuiControl? _activeControl;
    private GuiControl? _lastActiveControl;
    private GuiControl? _focusedControl;
    private bool _layoutInvalidated = true;
    private bool _visualInvalidated = true;

    private readonly ControlDepthComparer _controlDepthComparer;
    private readonly StringBuilder _debugText;

}
