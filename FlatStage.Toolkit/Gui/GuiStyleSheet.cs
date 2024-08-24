namespace FlatStage.Toolkit;

public class Style
{
    public T GetValue<T>(string propertyId) where T : struct
    {
        if (_values.TryGetValue(propertyId, out var value))
        {
            return (T)value;
        }

        FlatException.Throw($"Style doesn't contain value: {propertyId}");

        return default!;
    }

    public void SetValue<T>(string valueId, T value) where T : struct
    {
        _values[valueId] = value;
    }

    public bool TryGet<T>(string propertyId, out T? value) where T : struct
    {
        if (_values.TryGetValue(propertyId, out var result))
        {
            value = (T)result;
            return true;
        }

        value = default;
        return false;
    }

    private readonly Dictionary<string, object> _values = new();
}

public class GuiStyleSheet
{
    public GuiStyleSheet(Gui gui)
    {
        _gui = gui;
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

    public void SetProperty<T>(string targetId, GuiControlState state, string propertyId, T value) where T : struct
    {
        var id = GetStyleRuleTargetId(targetId);
        SetProperty(id, state, propertyId, value);
    }

    internal void SetProperty<T>(int targetId, GuiControlState state, string propertyId, T value) where T : struct
    {
        if (!_baseStyles.ContainsKey(targetId))
        {
            _baseStyles.Add(targetId, new Dictionary<GuiControlState, Style>());
        }

        if (!_baseStyles[targetId].ContainsKey(state))
        {
            _baseStyles[targetId][state] = new Style();
        }

        _baseStyles[targetId][state].SetValue(propertyId, value);
    }

    public T GetProperty<T>(GuiControl control, string propertyId, GuiControlState? state = null) where T : struct
    {
        var _state = state ?? _gui.GetState(control);

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

        bool TryToFindPropertyForState(GuiControl c, GuiControlState s, out T value)
        {
            // Try to Get Property for specific control

            var style = GetCustom(c, s);

            if (style?.TryGet<T>(propertyId, out var valueCustom) == true)
            {
                value = (T)valueCustom!;
                return true;
            }

            // Try to Get Property for all controls of this type

            var targetIdControlType = GetStyleRuleTargetId(c.GetType().Name);

            style = GetBase(targetIdControlType, s);

            if (style?.TryGet<T>(propertyId, out var valueBaseType) == true)
            {
                value = (T)valueBaseType!;
                return true;
            }

            // Try to Get Property common for all controls (*)

            style = GetBase(GuiStyleProperties.AllElementsTypeId, s);

            if (style?.TryGet<T>(propertyId, out var valueRootStyle) == true)
            {
                value = (T)valueRootStyle!;
                return true;
            }

            value = default;
            return false;
        }
    }



    private Style? GetCustom(GameObject gameObject, GuiControlState state)
    {
        if (_styles.TryGetValue(gameObject.UId, out var styleMapCustom))
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

    internal static int GetStyleRuleTargetId(string ruleTarget)
    {
        if (ruleTarget.Equals("*"))
        {
            return GuiStyleProperties.AllElementsTypeId;
        }

        if (_styleTargetIdsForControlTypes.TryGetValue(ruleTarget, out var targetId))
        {
            return targetId;
        }

        FlatException.Throw($"Could not obtain StyleRuleTargetId for ruleTarget: {ruleTarget}");

        return -1;
    }

    internal static readonly Dictionary<string, int> _styleTargetIdsForControlTypes = new()
    {
        {
            nameof(GuiButton), 1
        }
    };

    private readonly Gui _gui;

    private readonly Dictionary<int, Dictionary<GuiControlState, Style>> _baseStyles = new();
    private readonly Dictionary<int, Dictionary<GuiControlState, Style>> _styles = new();
}
