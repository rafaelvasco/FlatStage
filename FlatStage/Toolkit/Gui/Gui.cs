using FlatStage.Content;
using FlatStage.Engine.Toolkit.Definitions.Gui;
using FlatStage.Graphics;
using FlatStage.Input;
using System;
using System.Collections.Generic;

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

internal class ControlZIndexComparer : IComparer<GuiControl>
{
    public int Compare(GuiControl? x, GuiControl? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (y is null) return 1;
        if (x is null) return -1;
        return y.ZIndex.CompareTo(x.ZIndex);
    }
}

public class Gui
{
    internal const string DefaultScene = "Default";

    internal const string MainContainerId = "Main";

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

    public GuiContainer Desktop => _desktop;

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
        _font = BuiltinContent.Fonts.Monogram;

        _controlsById = new FastDictionary<string, GuiControl>();

        _controlList = new FastList<GuiControl>();

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

        Mouse.OnMouseExited += ProcessMouseExitedEvent;

        Keyboard.OnKeyDown += (Key key) =>
        {
            ProcessKeyboardEvent(key, true);
        };

        Keyboard.OnKeyUp += (Key key) =>
        {
            ProcessKeyboardEvent(key, false);
        };

        Keyboard.OnTextInput += ProcessTextInputEvent;

        _skin = new DefaultGuiSkin(this);

        _controlZIndexComparer = new ControlZIndexComparer();

        _desktop = new GuiContainer(MainContainerId, this)
        {
            Width = Canvas.Width,
            Height = Canvas.Height,
        };
    }

    public void Invalidate()
    {
        _visualInvalidated = true;
    }

    public void SendToTop(GuiControl control)
    {
        if (control.ZIndex <= _maxZIndex)
        {
            control.ZIndex = ++_maxZIndex;
        }
    }

    public void SendToBottom(GuiControl control)
    {
        if (control.ZIndex >= _minZIndex)
        {
            control.ZIndex = --_minZIndex;
        }
    }

    public void Open(string windowId)
    {
        var window = Get<GuiWindow>(windowId);

        window.Hidden = false;
        SendToTop(window);
        Center(window);

        Invalidate();
    }

    public void Center(GuiControl control)
    {
        if (control.Parent != null)
        {
            control.X = (control.Parent.Width / 2) - (control.Width / 2);
            control.Y = (control.Parent.Height / 2) - (control.Height / 2);
        }
        else
        {
            control.X = (this.Width / 2) - (control.Width / 2);
            control.Y = (this.Height / 2) - (control.Height / 2);
        }

    }

    internal void InvalidateLayout()
    {
        _layoutInvalidated = true;
    }

    internal void InvalidateZIndexes()
    {
        _zIndexesInvalidated = true;
    }

    internal void NormalizeZIndexes()
    {
        for (int i = _controlList.Count - 1; i >= 0; --i)
        {
            var control = _controlList[i];

            if (control.Parent != null && control.ZIndex <= control.Parent.ZIndex)
            {
                control.ZIndex = control.Parent.ZIndex + 1;
            }
        }
    }

    internal void ReorderControlList()
    {
        _controlList.Sort(_controlZIndexComparer);
    }

    public T Get<T>(string id) where T : GuiControl
    {
        if (_controlsById.TryGetValue(id, out var control))
        {
            return (control as T)!;
        }

        throw new Exception($"Could not find control with Id: {id}");
    }

    internal void CreateOrSet<T>(GuiControlDef definition, GuiContainer? parent = null) where T : GuiControl
    {
        if (_controlsById.TryGetValue(definition.Id, out var controlValue))
        {
            var control = (controlValue as T)!;
            control.InitFromDefinition(definition);
            return;
        }

        var newControl = (Activator.CreateInstance(typeof(T), definition.Id, this, parent) as T)!;
        newControl.InitFromDefinition(definition);
        return;
    }

    public void Update(float dt)
    {
        var children = _controlList.ReadOnlySpan;

        if (_zIndexesInvalidated)
        {
            Console.WriteLine("Processing ZIndexes");

            NormalizeZIndexes();
            ReorderControlList();

            _zIndexesInvalidated = false;
        }

        foreach (var child in children)
        {
            if (!child.ProcessUpdate)
            {
                continue;
            }

            if (child.Update(dt))
            {
                Invalidate();
            }
        }
    }

    public void Draw(Canvas canvas)
    {
        if (_desktop == null)
        {
            return;
        }

        if (_layoutInvalidated)
        {
            _layoutInvalidated = false;

            ProcessLayout();

            _visualInvalidated = true;
        }

        if (_visualInvalidated)
        {
            Console.WriteLine("Redraw GUI");

            canvas.SetViewport(_guiViewport);

            _desktop.Draw(canvas, Skin);

            _visualInvalidated = false;

            canvas.SetViewport();
        }

        canvas.Draw(_guiViewport.Texture, Vec2.Zero, Color.White);

        _debugText.Clear();
        _debugText.Append("Hovered: ");
        _debugText.AppendLine(_hoveredControl?.Id ?? "None");
        _debugText.Append("Active: ");
        _debugText.AppendLine(_activeControl?.Id ?? "None");
        _debugText.Append("Mouse Exclusive: ");
        _debugText.AppendLine(_mouseFocusControl?.Id ?? "None");
        _debugText.Append("MousePos: ");
        _debugText.Append(_mouseState.MouseX);
        _debugText.Append(",");
        _debugText.AppendLine(_mouseState.MouseY);

        canvas.DrawText(BuiltinContent.Fonts.Monogram, _debugText.ReadOnlySpan, new Vec2(20, Height - 100), Color.Cyan);
    }

    public void Focus(GuiControl? control)
    {
        ProcessFocused(control);
    }

    public void MouseFocus(GuiControl? control)
    {
        if (_mouseFocusControl != control)
        {
            _mouseFocusControl?.InternalProcessMouseFocusChanged(false);

            _mouseFocusControl = control;

            _mouseFocusControl?.InternalProcessMouseFocusChanged(true);
        }
    }

    internal static void CreateFromDefinition(Gui gui, GuiControlDef definition, GuiContainer? parent = null)
    {
        if (definition is GuiButtonDef buttonDef)
        {
            gui.CreateOrSet<GuiButton>(buttonDef, parent);
        }
        else if (definition is GuiCheckBoxDef checkDef)
        {
            gui.CreateOrSet<GuiCheckbox>(checkDef, parent);
        }
        else if (definition is GuiSliderDef sliderDef)
        {
            gui.CreateOrSet<GuiSlider>(sliderDef, parent);
        }
        else if (definition is GuiTextboxDef textBoxDef)
        {
            gui.CreateOrSet<GuiTextbox>(textBoxDef, parent);
        }
        else if (definition is GuiMenuBarDef menuBarDef)
        {
            gui.CreateOrSet<GuiMenuBar>(menuBarDef, parent);
        }
        else if (definition is GuiWindowDef windowDef)
        {
            gui.CreateOrSet<GuiWindow>(windowDef, parent);
        }
        else if (definition is GuiTextDef textDef)
        {
            gui.CreateOrSet<GuiText>(textDef, parent);
        }
        else if (definition is GuiTreeDef treeDef)
        {
            gui.CreateOrSet<GuiTree>(treeDef, parent);
        }
        else if (definition is GuiLayoutDef layoutDef)
        {
            switch (layoutDef.Direction)
            {
                case GuiLayoutDirection.Vertical:
                    gui.CreateOrSet<GuiVerticalLayout>(layoutDef, parent);
                    break;
                case GuiLayoutDirection.Horizontal:
                    gui.CreateOrSet<GuiHorizontalLayout>(layoutDef, parent);
                    break;
            }
        }
        else if (definition is GuiPanelDef panelDef)
        {
            gui.CreateOrSet<GuiPanel>(panelDef, parent);
        }
        else if (definition is GuiContainerDef containerDef)
        {
            gui.CreateOrSet<GuiContainer>(containerDef, parent);
        }
        else
        {
            FlatException.Throw("Invalid GuiControl Definition");
        }

        return;
    }

    internal void InitFromDefinition(GuiDef definition)
    {
        CreateFromDefinition(this, definition.Main, parent: null);

        ProcessLayout();
    }

    internal void RegisterControl(GuiControl control)
    {
        if (!_controlsById.ContainsKey(control.Id))
        {
            _controlList.Add(control);
            _controlsById.Add(control.Id, control);

            ReorderControlList();

            _maxZIndex = MathUtils.Max(_maxZIndex, control.ZIndex);
        }
    }

    private void ProcessMouseButtonEvent(MouseButton button, bool down)
    {
        _mouseState.LastMouseButton = button;
        _mouseState.MouseButtonDown = down;

        if (down && _hoveredControl != null)
        {
            if (_mouseFocusControl != null && _hoveredControl != _mouseFocusControl)
            {
                MouseFocus(null);
            }

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

        bool changed = false;

        if (_mouseFocusControl == null)
        {
            for (int i = 0; i < _controlList.Count; ++i)
            {
                var control = _controlList[i];

                if (control.Interactive && !control.GlobalHidden && control.ContainsPoint(x, y))
                {
                    if (_hoveredControl == null || control.ZIndex >= _hoveredControl.ZIndex)
                    {
                        ProcessHover(control);
                    }

                    switch (control.MouseMoveEventBehavior)
                    {
                        case GuiMouseMoveEventBehavior.MouseDown:

                            if (control == _activeControl)
                            {
                                changed |= control.InternalProcessMouseMove(_mouseState);
                            }

                            break;

                        case GuiMouseMoveEventBehavior.MouseOver:

                            changed |= control.InternalProcessMouseMove(_mouseState);

                            break;
                    }

                    break;
                }
                else if (control == _hoveredControl)
                {
                    if (_activeControl == _hoveredControl)
                    {
                        ProcessActive(null);
                    }

                    ProcessHover(null);
                }
            }
        }
        else
        {
            changed |= _mouseFocusControl.InternalProcessMouseMove(_mouseState);
        }

        if (changed)
        {
            Invalidate();
        }

    }

    private void ProcessMouseExitedEvent(int x, int y)
    {
        if (_hoveredControl != null && !_hoveredControl.MouseFocused)
        {
            ProcessHover(null);
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
                _lastHoveredControl.InternalProcessMouseExited();
                _lastHoveredControl.Hovered = false;
            }

            if (_hoveredControl != null)
            {
                _hoveredControl.Hovered = true;
                _hoveredControl.InternalProcessMouseEntered();
            }

            Invalidate();
        }
    }

    private void ProcessActive(GuiControl? control)
    {
        _lastActiveControl = _activeControl;

        if (control != null)
        {
            control.Active = true;
            control.InternalProcessMouseButton(_mouseState);

            ProcessFocused(control);

            _activeControl = control;

        }
        else if (_lastActiveControl != null)
        {
            _lastActiveControl.InternalProcessMouseButton(_mouseState);
            _lastActiveControl.Active = false;

            if (!_lastActiveControl.ContainsPoint(_mouseState.MouseX, _mouseState.MouseY))
            {
                ProcessHover(null);
            }

            _activeControl = null;
        }

        Invalidate();
    }

    private void ProcessFocused(GuiControl? control)
    {
        if (control?.CanGetFocus == true)
        {
            if (_focusedControl != null && control != _focusedControl)
            {
                _focusedControl.InternalProcessFocusChanged(false);
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

        Invalidate();
    }

    private void ProcessLayout()
    {
        for (int i = _controlList.Count - 1; i >= 0; --i)
        {
            var control = _controlList[i];

            if (control is GuiContainer container)
            {
                container.ProcessLayout();
            }
        }
    }

    private GuiSkin _skin;

    private TextureFont _font;

    private readonly GuiContainer _desktop = null!;
    private readonly FastList<GuiControl> _controlList;
    private readonly FastDictionary<string, GuiControl> _controlsById;
    private readonly CanvasViewport _guiViewport;
    private readonly GuiMouseState _mouseState;
    private GuiControl? _hoveredControl;
    private GuiControl? _lastHoveredControl;
    private GuiControl? _activeControl;
    private GuiControl? _lastActiveControl;
    private GuiControl? _focusedControl;
    private GuiControl? _mouseFocusControl;
    private bool _layoutInvalidated = true;
    private bool _visualInvalidated = true;
    private bool _zIndexesInvalidated = false;
    private int _maxZIndex = 0;
    private int _minZIndex = 0;

    private readonly ControlZIndexComparer _controlZIndexComparer;

    private readonly StringBuffer _debugText = new();

}
