namespace FlatStage.Toolkit;
public interface IComponentsMap
{
    void RemoveComponent(BaseGameEntity entity);
    bool IsComponentAttachedToEntity(BaseGameEntity entity);
}
