using System.Collections.Generic;

namespace FlatStage.Toolkit;
public class ComponentsMap<T> : IComponentsMap
{
    private readonly Dictionary<BaseGameEntity, T> _registry = new();

    public void AddComponent(BaseGameEntity entity, T component)
    {
        FlatException.Assert(!_registry.ContainsKey(entity));
        _registry[entity] = component;
    }

    public void RemoveComponent(BaseGameEntity entity)
    {
        _registry.Remove(entity);
    }

    public T GetComponent(BaseGameEntity entity)
    {
        FlatException.Assert(_registry.ContainsKey(entity));
        return _registry[entity];
    }

    public bool IsComponentAttachedToEntity(BaseGameEntity entity)
    {
        return _registry.ContainsKey(entity);
    }
}
