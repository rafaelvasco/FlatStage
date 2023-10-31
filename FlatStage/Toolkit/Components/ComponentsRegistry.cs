using System;
using System.Collections.Generic;

namespace FlatStage.Toolkit;
public class ComponentsRegistry<ComponentBaseType>
{
    public FastList<ComponentBaseType> AllComponents => _allComponents;

    public ComponentsRegistry(BaseGameEntity entity)
    {
        _parent = entity;
        _allComponents = new FastList<ComponentBaseType>();
    }

    private ComponentsMap<T> GetComponentsMap<T>() where T : ComponentBaseType
    {
        FlatException.Assert(_componentsRegistry.ContainsKey(typeof(T)), $"Cannot get component cache: {typeof(T).Name} is not registered");
        return (ComponentsMap<T>)_componentsRegistry[typeof(T)];
    }

    public void AddComponent<T>(T component) where T : ComponentBaseType
    {
        if (!_componentsRegistry.ContainsKey(typeof(T)))
        {
            var componentMapForType = new ComponentsMap<T>();
            _componentsRegistry.Add(typeof(T), componentMapForType);
        }

        GetComponentsMap<T>().AddComponent(_parent, component);
        _allComponents.Add(component);
    }

    public T GetComponent<T>() where T : ComponentBaseType
    {
        return GetComponentsMap<T>().GetComponent(_parent);
    }

    public void RemoveComponent<T>() where T : ComponentBaseType
    {
        GetComponentsMap<T>().RemoveComponent(_parent);
    }

    private readonly BaseGameEntity _parent;
    private readonly Dictionary<Type, IComponentsMap> _componentsRegistry = new();
    private readonly FastList<ComponentBaseType> _allComponents;
}
