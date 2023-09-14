using FlatStage.Graphics;
using FlatStage.Input;
using System.Text;

namespace FlatStage.Toolkit;
public class GuiTextbox : GuiControl
{
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

    internal StringBuilder InternalText => _text;
    internal bool ShowCursor => _showCursor && SelectionStartIndex == SelectionEndIndex;

    internal int CaretOffset => _caretOffset;

    internal int SelectionStartIndex => _tempSelectionStartIndex != -1 ? Calc.Min(_tempSelectionStartIndex, _tempSelectionEndIndex) : _selectionStartIndex;

    internal int SelectionEndIndex => _tempSelectionEndIndex != -1 ? Calc.Max(_tempSelectionStartIndex, _tempSelectionEndIndex) : _selectionEndIndex;

    internal bool SelectionEmpty => _selectionStartIndex == _selectionEndIndex;

    internal const int CaretWidth = 15;
    internal const int CaretHeight = 30;
    internal const int Padding = 10;

    public GuiTextbox(string id, Gui gui, GuiControl? parent = null) : base(id, gui, parent)
    {
        _text = new StringBuilder();

        CanGetFocus = true;

        ProcessUpdate = true;

        ReceiveTextInputEvents = true;

        TrackInputOutsideArea = true;
    }

    public override Size SizeHint => new(250, 40);

    protected override bool ProcessKeyboardKey(Key key, bool down)
    {
        _movingCaretWithKeys = down && key != Key.LeftControl && key != Key.A;

        switch (key)
        {
            case Key.Left when down:
                Select();
                _caretOffset = Calc.Max(--_caretOffset, 0);
                return true;

            case Key.Right when down:
                Select();
                _caretOffset = Calc.Min(++_caretOffset, _text!.Length);
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

        var textLocalX = localX - Padding;

        var textSize = Gui.Skin.MeasureText(_text, _text.Length);

        float percentage = textLocalX / textSize.X;

        var newCaretOffset = (int)(_text.Length * percentage);

        if (newCaretOffset != _caretOffset)
        {
            _caretOffset = newCaretOffset;

            _caretOffset = Calc.Clamp(_caretOffset, 0, _text.Length);

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
                    _text.Insert(_caretOffset, args.Character);
                }

                _caretOffset++;
                break;
        }

        return true;
    }

    protected override bool Update(float dt)
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

    protected override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawTextbox(canvas, this);
    }

    private void Select(int start = 0, int end = 0)
    {
        _selectionStartIndex = Calc.Min(start, end);
        _selectionEndIndex = Calc.Max(start, end);

        _selectionStartIndex = Calc.Max(_selectionStartIndex, 0);
        _selectionEndIndex = Calc.Min(_selectionEndIndex, _text.Length);
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

    private readonly StringBuilder _text;

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
