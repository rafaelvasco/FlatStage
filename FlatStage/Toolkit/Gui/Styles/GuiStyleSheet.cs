using System.Collections.Generic;

namespace FlatStage.Toolkit;

public class GuiStyleSheet
{
    public GuiStyleSheet()
    {
        _styles = new Dictionary<int, Dictionary<GuiControlState, Style>>();
        _baseStyles = new Dictionary<int, Dictionary<GuiControlState, Style>>();
    }

    public void SetProperty<T>(GuiControl control, GuiControlState state, string propertyId, T value) where T : struct
    {
        if (!_styles.ContainsKey(control.UId))
        {
            _styles.Add(control.UId, new Dictionary<GuiControlState, Style>());
        }

        if (!_styles[control.UId].ContainsKey(state))
        {
            _styles[control.UId][state] = new Style();
        }

        _styles[control.UId][state].SetValue(propertyId, value);
    }

    public void SetProperty<T>(int typeId, GuiControlState state, string propertyId, T value) where T : struct
    {
        if (!_baseStyles.ContainsKey(typeId))
        {
            _baseStyles.Add(typeId, new Dictionary<GuiControlState, Style>());
        }

        if (!_baseStyles[typeId].ContainsKey(state))
        {
            _baseStyles[typeId][state] = new Style();
        }

        _baseStyles[typeId][state].SetValue(propertyId, value);
    }

    public T GetProperty<T>(GuiControl control, string propertyId, GuiControlState? state = null) where T : struct
    {
        bool TryToFindPropertyForState(GuiControl control, GuiControlState state, out T value)
        {
            var style = GetCustom(control, state);

            if (style?.TryGet<T>(propertyId, out var valueCustom) == true)
            {
                value = (T)valueCustom!;
                return true;
            }

            style = GetBase(control.TypeId, state);

            if (style?.TryGet<T>(propertyId, out var valueBaseType) == true)
            {
                value = (T)valueBaseType!;
                return true;
            }

            style = GetBase(GuiControl.AllControlsTypeId, state);

            if (style?.TryGet<T>(propertyId, out var valueRootStyle) == true)
            {
                value = (T)valueRootStyle!;
                return true;
            }

            value = default;
            return false;
        }

        var _state = state ?? control.State;

        if (TryToFindPropertyForState(control, _state, out var valueOnDefinedState))
        {
            return valueOnDefinedState;
        }

        if (TryToFindPropertyForState(control, GuiControlState.Idle, out var valueOnIdleState))
        {
            return valueOnIdleState;
        }

        FlatException.Throw("Could not find Property in StyleSheet");

        return default;
    }

    private Style? GetCustom(GuiControl control, GuiControlState state)
    {
        if (_styles.TryGetValue(control.UId, out var styleMapCustom))
        {
            if (styleMapCustom.TryGetValue(state, out var styleOfState))
            {
                return styleOfState;
            }
        }

        return null;
    }

    private Style? GetBase(int typeId, GuiControlState state)
    {
        if (_baseStyles.TryGetValue(typeId, out var styleMapBase))
        {
            if (styleMapBase.TryGetValue(state, out var styleOfState))
            {
                return styleOfState;
            }
        }

        return null;
    }

    private readonly Dictionary<int, Dictionary<GuiControlState, Style>> _baseStyles;
    private readonly Dictionary<int, Dictionary<GuiControlState, Style>> _styles;

}
