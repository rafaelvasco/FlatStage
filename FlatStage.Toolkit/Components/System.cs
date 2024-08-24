namespace FlatStage.Toolkit;

public interface ISystem
{
    void _RegisterNode(GameObject gameObject);

    void _UnRegisterNode(GameObject gameObject);

    Type GetComponentType();

    void Update(float dt);

}

public abstract class System<TypeComponent> : ISystem where TypeComponent : Component
{
    public virtual void _RegisterNode(GameObject gameObject)
    {
        if (!Behaviors.HasComponent<TypeComponent>(gameObject))
        {
            FlatException.Throw("Can't register GameObject as it doesn't have the necessary component");
        }

        var component = Behaviors.GetComponent<TypeComponent>(gameObject);

        if (!_systemComponents.Contains(component))
        {
            _systemComponents.Add(component);
        }
    }

    public virtual void _UnRegisterNode(GameObject gameObject)
    {
        if (Behaviors.HasComponent<TypeComponent>(gameObject))
        {
            var component = Behaviors.GetComponent<TypeComponent>(gameObject);

            _systemComponents.Remove(component);
        }
    }


    public abstract Type GetComponentType();

    public abstract void Update(float dt);

    protected readonly FastList<TypeComponent> _systemComponents = new();

}
