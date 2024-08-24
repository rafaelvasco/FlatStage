namespace FlatStage.Toolkit;


public static class Behaviors
{
    public static void AddComponent<T>(GameObject gameObject) where T : Component
    {
        gameObject.HasBehaviors = true;

        var component = (Activator.CreateInstance(typeof(T), gameObject) as T)!;
        _allComponents.Add(component);

        if (!_componentsRegistryMap.ContainsKey(gameObject))
        {
            _componentsRegistryMap.Add(gameObject, new ComponentsRegistry(gameObject));
        }

        _componentsRegistryMap[gameObject].AddComponent(component);

        RegisterNodeOnSystemIfNeeded<T>(gameObject);
    }

    public static void RemoveComponent<T>(GameObject gameObject) where T : Component
    {
        if (_componentsRegistryMap.TryGetValue(gameObject, out var componentsRegistry))
        {
            componentsRegistry.RemoveComponent<T>();
        }
    }

    public static void UnRegisterNode(GameObject gameObject)
    {
        gameObject.HasBehaviors = false;

        var systems = _allSystems.ReadOnlySpan;

        foreach (var system in systems)
        {
            system._UnRegisterNode(gameObject);
        }

        if (!_componentsRegistryMap.TryGetValue(gameObject, out var componentsRegistry)) return;

        var components = componentsRegistry.AllComponents.ReadOnlySpan;

        foreach (var component in components)
        {
            _allComponents.Remove(component);
        }

        componentsRegistry.Clear();
        _componentsRegistryMap.Remove(gameObject);
    }

    public static T GetComponent<T>(GameObject gameObject) where T : Component
    {
        if (_componentsRegistryMap.TryGetValue(gameObject, out var componentsRegistry))
        {
            return componentsRegistry.GetComponent<T>();
        }

        AddComponent<T>(gameObject);

        return _componentsRegistryMap[gameObject].GetComponent<T>();
    }

    public static bool HasComponent<T>(GameObject gameObject) where T : Component
    {
        return _componentsRegistryMap.TryGetValue(gameObject, out var registry) && registry.HasComponent<T>();
    }

    public static TSystem ActivateSystem<TSystem>() where TSystem : class, ISystem
    {
        if (_systemsRegistryMap.TryGetValue(typeof(TSystem), out var existingSystem))
        {
            return (existingSystem as TSystem)!;
        }

        var system = Activator.CreateInstance<TSystem>();

        _allSystems.Add(system);
        _systemsRegistryMap.Add(typeof(TSystem), system);
        _systemsForComponentTypeMap.Add(system.GetComponentType(), system);

        return system;
    }

    internal static void Update(float dt)
    {
        var components = _allComponents.ReadOnlySpan;
        foreach (var component in components)
        {
            component.Update(dt);
        }

        var systems = _allSystems.ReadOnlySpan;

        foreach (var system in systems)
        {
            system.Update(dt);
        }
    }

    private static void RegisterNodeOnSystemIfNeeded<T>(GameObject gameObject) where T : Component
    {
        if (_systemsForComponentTypeMap.TryGetValue(typeof(T), out var system))
        {
            system._RegisterNode(gameObject);
        }
    }

    private static readonly FastList<Component> _allComponents = new();
    private static readonly FastDictionary<GameObject, ComponentsRegistry> _componentsRegistryMap = new();
    private static readonly FastList<ISystem> _allSystems = new();
    private static readonly Dictionary<Type, ISystem> _systemsRegistryMap = new();
    private static readonly Dictionary<Type, ISystem> _systemsForComponentTypeMap = new();

}
