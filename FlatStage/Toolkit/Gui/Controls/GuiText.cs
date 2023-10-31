using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;

public class GuiText : GuiControl
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }

    public ReadOnlySpan<char> Text => _text.ReadOnlySpan;

    public HorizontalAlignment HorizontalAlign { get; set; } = HorizontalAlignment.Left;

    public VerticalAlignment VerticalAlign { get; set; } = VerticalAlignment.Top;

    public int TextMargin { get; set; } = 10;

    internal int TextDrawX => _textDrawX;

    internal int TextDrawY => _textDrawY;

    public GuiText(string id, Gui gui, GuiContainer? parent) : base(id, gui, parent)
    {
        _text = new StringBuffer();

        Console.Write(this.Parent!.GlobalX);

        SetText("Text");
    }

    public override Size SizeHint => new(0, 0);

    public void SetText(ReadOnlySpan<char> text)
    {
        _text.Clear();
        _text.Append(text);
        UpdateSize();
    }

    public void Append(ReadOnlySpan<char> text)
    {
        _text.Append(text);
        UpdateSize();
    }

    public void AppendLine(ReadOnlySpan<char> text)
    {
        _text.AppendLine(text);
        UpdateSize();
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        UpdateTextDrawPoint();
        skin.DrawText(canvas, this);
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiTextDef textDef)
        {
            SetText(textDef.Text);

            HorizontalAlign = textDef.HorizontalAlign;
            VerticalAlign = textDef.VerticalAlign;
        }
    }

    private void UpdateSize()
    {
        var textSize = Gui.Skin.MeasureText(_text.ReadOnlySpan);

        Resize((int)textSize.X, (int)textSize.Y);
    }

    private void UpdateTextDrawPoint()
    {
        var textSize = Gui.Skin.MeasureText(_text.ReadOnlySpan);

        switch (HorizontalAlign)
        {
            case HorizontalAlignment.Left:

                _textDrawX = GlobalX + TextMargin;

                break;

            case HorizontalAlignment.Center:

                _textDrawX = (int)(GlobalX + (Width / 2) - (textSize.X / 2));

                break;

            case HorizontalAlignment.Right:

                _textDrawX = Width - TextMargin;

                break;
        }

        switch (VerticalAlign)
        {
            case VerticalAlignment.Top:

                _textDrawY = GlobalY + TextMargin;

                break;

            case VerticalAlignment.Bottom:

                _textDrawY = Height - TextMargin;

                break;

            case VerticalAlignment.Center:

                _textDrawY = (int)(GlobalY + (Height / 2) - (textSize.Y / 2));

                break;
        }
    }

    static GuiText()
    {
        STypeId = ++SBTypeId;
    }

    private readonly StringBuffer _text;

    private int _textDrawX;
    private int _textDrawY;

    internal static readonly int STypeId;

    internal override int TypeId => STypeId;
}
