namespace FlatStage.Toolkit;

public interface IComponentsMap {}

public class ComponentsMap<T> : IComponentsMap where T : Component
{
    public void AddComponent(GameObject gameObject, T component)
    {
        FlatException.Assert(!_map.ContainsKey(gameObject));
        _map[gameObject] = component;
    }

    public void RemoveComponent(GameObject entity)
    {
        _map.Remove(entity);
    }

    public T GetComponent(GameObject entity)
    {
        FlatException.Assert(_map.ContainsKey(entity));
        return _map[entity];
    }

    public bool IsComponentAttachedToEntity(GameObject entity)
    {
        return _map.ContainsKey(entity);
    }

    private readonly FastDictionary<GameObject, T> _map = new();
}
