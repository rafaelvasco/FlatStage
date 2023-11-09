using System.Collections.Generic;

namespace FlatStage.Toolkit;

public class Style
{
    public T GetValue<T>(string propertyId) where T : struct
    {
        if (_values.TryGetValue(propertyId, out var value) == true)
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
            value = (T)result!;
            return true;
        }

        value = default;
        return false;
    }

    private readonly Dictionary<string, object> _values = new();
}
