using System;

namespace FlatStage.Graphics;

public struct GraphicsChanges : IEquatable<GraphicsChanges>
{
    public int BackbufferWidth;
    public int BackbufferHeight;
    public bool VsyncEnabled;

    public bool Equals(GraphicsChanges other)
    {
        return GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BackbufferWidth, BackbufferHeight, VsyncEnabled);
    }

    public override bool Equals(object? obj)
    {
        return obj is GraphicsChanges changes && Equals(changes);
    }

    public static bool operator ==(GraphicsChanges left, GraphicsChanges right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GraphicsChanges left, GraphicsChanges right)
    {
        return !(left == right);
    }
}