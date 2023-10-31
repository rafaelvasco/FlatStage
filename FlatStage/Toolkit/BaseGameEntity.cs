using System;

namespace FlatStage.Toolkit;
public abstract class BaseGameEntity : IEquatable<BaseGameEntity>
{
    internal static int SUid;

    public int UId { get; }

    protected BaseGameEntity()
    {
        UId = ++SUid;
    }

    public bool Equals(BaseGameEntity? other)
    {
        return this.UId.Equals(other?.UId);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseGameEntity);
    }

    public override int GetHashCode()
    {
        return UId.GetHashCode();
    }
}
