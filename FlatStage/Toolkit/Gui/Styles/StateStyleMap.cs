using System.Collections.Generic;

namespace FlatStage.Toolkit;

public class StateStyleMap
{
    private readonly Dictionary<GuiControlState, Style> _styles;

    public StateStyleMap(Dictionary<GuiControlState, Style> styles)
    {
        _styles = styles;
    }

    public Style Get(GuiControlState state)
    {
        if (_styles.TryGetValue(state, out var style))
        {
            return style;
        }

        return _styles[GuiControlState.Idle];
    }
}
