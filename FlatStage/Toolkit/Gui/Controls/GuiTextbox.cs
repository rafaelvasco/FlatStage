using FlatStage.Graphics;
using FlatStage.Input;
using System;

namespace FlatStage.Toolkit;
public class GuiTextbox : GuiControl
{
    internal static readonly int STypeId;

    static GuiTextbox()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public int CaretDelay
    {
        get => _tickDelay;
        set
        {
            _tickDelay = value;

            if (_tickDelay <= 0)
            {
                _tickDelay = 1;
            }
        }
    }

    internal ReadOnlySpan<char> InternalText => _text.ReadOnlySpan;
    internal bool ShowCursor => _showCursor && SelectionStartIndex == SelectionEndIndex;

    internal int CaretOffset => _caretOffset;

    internal int SelectionStartIndex => _tempSelectionStartIndex != -1 ? MathUtils.Min(_tempSelectionStartIndex, _tempSelectionEndIndex) : _selectionStartIndex;

    internal int SelectionEndIndex => _tempSelectionEndIndex != -1 ? MathUtils.Max(_tempSelectionStartIndex, _tempSelectionEndIndex) : _selectionEndIndex;

    internal bool SelectionEmpty => _selectionStartIndex == _selectionEndIndex;

    public GuiTextbox(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
        _text = new StringBuffer();

        CanGetFocus = true;

        ProcessUpdate = true;

        ReceiveTextInputEvents = true;
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiTextboxDef textBoxDef)
        {
            CaretDelay = textBoxDef.CaretDelay;
        }
    }

    public override Size SizeHint => new(250, 40);

    protected override bool ProcessKeyboardKey(Key key, bool down)
    {
        _movingCaretWithKeys = down && key != Key.LeftControl && key != Key.A;

        switch (key)
        {
            case Key.Left when down:
                Select();
                _caretOffset = MathUtils.Max(--_caretOffset, 0);
                return true;

            case Key.Right when down:
                Select();
                _caretOffset = MathUtils.Min(++_caretOffset, _text!.Length);
                return true;

            case Key.Home when down:
                _caretOffset = 0;
                return true;

            case Key.End when down:
                _caretOffset = _text.Length;
                return true;

            case Key.Delete when down:
                Delete();
                return true;

            case Key.Back when down:
                Backspace();
                return true;

            case Key.LeftControl:
                _ctrlDown = down;
                return false;

            case Key.A when down:
                if (_ctrlDown)
                {
                    _selectionStartIndex = 0;
                    _selectionEndIndex = _text.Length;
                }

                return true;
            default: return false;
        }
    }

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (_text.Length == 0)
        {
            return false;
        }

        if (mouseState.MouseButtonDown)
        {
            Gui.MouseFocus(this);
        }
        else
        {
            Gui.MouseFocus(null);
        }

        if (mouseState.MouseButtonDown)
        {
            _selecting = true;

            _showCursor = true;

            Select();

            var moved = PutCaretFromMousePos(mouseState);

            _tempSelectionStartIndex = _caretOffset;
            _tempSelectionEndIndex = _caretOffset;

            return moved;
        }
        else
        {
            if (_tempSelectionStartIndex != _tempSelectionEndIndex)
            {
                Select(_tempSelectionStartIndex, _tempSelectionEndIndex);
            }

            _tempSelectionStartIndex = -1;
            _tempSelectionEndIndex = -1;

            _selecting = false;
            return true;
        }
    }

    private bool PutCaretFromMousePos(GuiMouseState mouseState)
    {
        var (localX, _) = ToLocalPos(mouseState.MouseX, mouseState.MouseY);

        var caretPadding = Gui.Skin.StyleSheet.GetProperty<int>(this, DefaultStyleProperties.TextboxCaretPadding);

        var textLocalX = localX - caretPadding;

        var textSize = Gui.Skin.MeasureText(_text.ReadOnlySpan, _text.Length);

        float percentage = textLocalX / textSize.X;

        var newCaretOffset = (int)(_text.Length * percentage);

        if (newCaretOffset != _caretOffset)
        {
            _caretOffset = newCaretOffset;

            _caretOffset = MathUtils.Clamp(_caretOffset, 0, _text.Length);

            return true;
        }

        return false;
    }

    protected override bool ProcessMouseMove(GuiMouseState mouseState)
    {
        if (_selecting)
        {
            bool moved = PutCaretFromMousePos(mouseState);

            _tempSelectionEndIndex = _caretOffset;

            return moved;
        }

        return false;
    }

    protected override bool ProcessTextInput(TextInputEventArgs args)
    {
        switch (args.Key)
        {
            case Key.Enter or Key.Escape:
                Gui.Focus(null);
                break;
            case Key.Delete or Key.Home or Key.Back:
                break;
            default:

                if (!SelectionEmpty)
                {
                    DeleteSelection();
                }

                if (_caretOffset == _text.Length)
                {
                    _text.Append(args.Character);
                }

                else if (_caretOffset < _text.Length)
                {
                    _text.Insert(_caretOffset, new ReadOnlySpan<char>(args.Character));
                }

                _caretOffset++;
                break;
        }

        return true;
    }

    internal override bool Update(float dt)
    {
        if (!Focused || !SelectionEmpty)
        {
            return false;
        }

        if (_movingCaretWithKeys)
        {
            _showCursor = true;

            return false;
        }

        _ticks++;

        if (_ticks > _tickDelay)
        {
            _ticks = 0;

            _showCursor = !_showCursor;

            return true;
        }

        return false;
    }

    protected override void ProcessFocusChanged(bool focused)
    {
        _showCursor = focused;

        if (!focused)
        {
            Select();
            _caretOffset = 0;
        }
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawTextbox(canvas, this);
    }

    private void Select(int start = 0, int end = 0)
    {
        _selectionStartIndex = MathUtils.Min(start, end);
        _selectionEndIndex = MathUtils.Max(start, end);

        _selectionStartIndex = MathUtils.Max(_selectionStartIndex, 0);
        _selectionEndIndex = MathUtils.Min(_selectionEndIndex, _text.Length);
    }

    private void DeleteSelection()
    {
        _text.Remove(_selectionStartIndex, _selectionEndIndex - _selectionStartIndex);
        _caretOffset = _selectionStartIndex;
        Select();
    }

    private void Backspace()
    {
        if (_selectionEndIndex > _selectionStartIndex)
        {
            DeleteSelection();
        }
        else if (_caretOffset > 0)
        {
            _text.Remove(_caretOffset - 1, 1);
            _caretOffset--;
        }
    }

    private void Delete()
    {
        if (_selectionEndIndex > _selectionStartIndex)
        {
            DeleteSelection();
        }
        else if (_text!.Length > 0 && _caretOffset >= 0 && _caretOffset < _text.Length)
        {
            _text.Remove(_caretOffset, 1);
        }
    }

    private readonly StringBuffer _text;

    private bool _showCursor;
    private int _ticks;
    private int _tickDelay = 50;
    private int _caretOffset;
    private int _selectionStartIndex;
    private int _selectionEndIndex;
    private int _tempSelectionStartIndex = -1;
    private int _tempSelectionEndIndex = -1;
    private bool _ctrlDown;
    private bool _movingCaretWithKeys;
    private bool _selecting;
}
