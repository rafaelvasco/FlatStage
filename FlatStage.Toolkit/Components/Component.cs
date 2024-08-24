namespace FlatStage.Toolkit;

public abstract class Component(GameObject parent)
{
    public GameObject Parent { get; } = parent;

    public abstract void Update(float dt);


}
