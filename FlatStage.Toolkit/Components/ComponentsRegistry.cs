namespace FlatStage.Toolkit;

public class ComponentsRegistry(GameObject gameObject)
{
    public FastList<Component> AllComponents { get; } = new();

    public void AddComponent<T>(T component) where T : Component
    {
        if (!_registry.ContainsKey(typeof(T)))
        {
            var componentMapForType = new ComponentsMap<T>();
            _registry.Add(typeof(T), componentMapForType);
        }

        GetComponentsMap<T>().AddComponent(gameObject, component);
        AllComponents.Add(component);
    }

    public T GetComponent<T>() where T : Component
    {
        return GetComponentsMap<T>().GetComponent(gameObject);
    }

    public bool HasComponent<T>() where T : Component
    {
        return GetComponentsMap<T>().IsComponentAttachedToEntity(gameObject);
    }

    public void RemoveComponent<T>() where T : Component
    {
        GetComponentsMap<T>().RemoveComponent(gameObject);
    }

    private ComponentsMap<T> GetComponentsMap<T>() where T : Component
    {
        FlatException.Assert(_registry.ContainsKey(typeof(T)), $"Cannot get component cache: {typeof(T).Name} is not registered");
        return (ComponentsMap<T>)_registry[typeof(T)];
    }

    public void Clear()
    {
        _registry.Clear();
        AllComponents.Clear();
    }

    private readonly Dictionary<Type, IComponentsMap> _registry = new();
}
