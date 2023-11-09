using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public class GuiSlider : GuiControl
{
    internal static readonly int STypeId;

    static GuiSlider()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public float Value
    {
        get => _value;
        set
        {
            if (value != _value)
            {
                _value = value;

                _value = MathUtils.Clamp(_value, _minValue, _maxValue);

                _value = MathUtils.Snap(_value, _step);

                UpdateThumbRect();
            }
        }
    }

    public float MinValue
    {
        get => _minValue;
        set
        {
            if (value != _minValue)
            {
                _minValue = value;

                if (_minValue > _maxValue)
                {
                    throw new Exception("Invalid value for MinValue: Can't be bigger than MaxValue");
                }

                _value = _minValue;

                UpdateThumbRect();
            }
        }
    }

    public float MaxValue
    {
        get => _maxValue;
        set
        {
            if (value != _maxValue)
            {
                _maxValue = value;

                if (_maxValue < _minValue)
                {
                    throw new Exception("Invalid value for MaxValue: Can't be lower than MinValue");
                }

                if (_minValue + _step > _maxValue)
                {
                    throw new Exception("Invalid value for MaxValue: Must be bigger than MinValue + Step");
                }

                _value = _minValue;

                UpdateThumbRect();
            }
        }
    }

    public float Step
    {
        get => _step;
        set
        {
            if (value != _step)
            {
                _step = value;

                if (_step == 0)
                {
                    throw new Exception("Invalid value for Step: Can't be zero");
                }

                if (_minValue + _step > _maxValue)
                {
                    throw new Exception("Invalid value for Step: Can't be larger than MaxValue - MinValue");
                }

                if ((_maxValue - _minValue) % _step != 0)
                {
                    throw new Exception("Invalid value for Step: Must be Multiple of MaxValue-MinValue");
                }

                _value = _minValue;

                UpdateThumbRect();

            }
        }
    }

    public int ThumbWidth
    {
        get => _thumbWidth;
        set
        {
            if (value != _thumbWidth)
            {
                _thumbWidth = value;
                UpdateThumbRect();
            }
        }
    }

    internal Rect ThumbRect => _thumbRect;

    private int TotalTrackWidth => Width - ThumbWidth;

    private int TrackStartOffset => ThumbWidth / 2;

    public GuiSlider(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
        UpdateThumbRect();
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiSliderDef sliderDef)
        {
            Value = sliderDef.Value;
            MinValue = sliderDef.MinValue;
            MaxValue = sliderDef.MaxValue;
            Step = sliderDef.Step;
            ThumbWidth = sliderDef.ThumbWidth;
        }
    }

    public override Size SizeHint => new(250, 40);

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.MouseButtonDown)
        {
            Gui.MouseFocus(this);
        }
        else
        {
            Gui.MouseFocus(null);
        }

        if (mouseState.LastMouseButton == Input.MouseButton.Left && mouseState.MouseButtonDown)
        {
            var (localX, _) = ToLocalPos(mouseState.MouseX, mouseState.MouseY);

            return MoveThumbTo(localX);
        }

        return false;
    }

    protected override bool ProcessMouseMove(GuiMouseState mouseState)
    {
        if (mouseState.LastMouseButton == Input.MouseButton.Left)
        {
            var (localX, _) = ToLocalPos(mouseState.MouseX, mouseState.MouseY);

            return MoveThumbTo(localX);
        }

        return false;
    }

    private bool MoveThumbTo(int position)
    {
        if (SetValueFromPosition(position))
        {
            UpdateThumbRect();
            return true;
        }
        return false;
    }

    private static float ValueToPercent(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    private bool SetValueFromPosition(int position)
    {
        float factor = (float)(position - TrackStartOffset) / TotalTrackWidth;

        float newValue = _minValue + ((_maxValue - _minValue) * factor);

        newValue = MathUtils.Clamp(newValue, _minValue, _maxValue);

        newValue = MathUtils.Snap(newValue, _step);

        if (newValue != _value)
        {
            _value = newValue;
            Console.WriteLine($"Value Changed: {_value}");
            return true;
        }

        return false;
    }

    private void UpdateThumbRect()
    {
        if (_maxValue - _minValue == 0)
        {
            return;
        }

        float factor = ValueToPercent(_value, _minValue, _maxValue);

        float xPos = (TotalTrackWidth * factor) - (ThumbWidth / 2) + TrackStartOffset;

        xPos = MathUtils.Clamp(xPos, 0, Width - ThumbWidth);

        _thumbRect = new Rect((int)(xPos), 0, ThumbWidth, Height);
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawSlider(canvas, this);
    }

    private int _thumbWidth = 30;

    private float _value = 0;
    private float _minValue = 0;
    private float _maxValue = 10;
    private float _step = 1;
    private Rect _thumbRect;

}
