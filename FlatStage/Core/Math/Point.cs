using System;
using System.Runtime.InteropServices;

namespace FlatStage;


[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Point : IEquatable<Point>
{
    public static readonly Point Zero = new(0, 0);

    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point(int val) : this(val, val)
    {
    }

    public override bool Equals(object? obj)
    {
        return obj is Point point2 && Equals(point2);
    }

    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X;
            hash = hash * 23 + Y;
            return hash;
        }
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }

    public static int Cross(Point a, Point b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    public static implicit operator Vec2(Point p)
    {
        return new Vec2(p.X, p.Y);
    }

    public static explicit operator Point(Vec2 p)
    {
        return new Point((int)p.X, (int)p.Y);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Point a, Point b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    public static Point operator +(Point a, Point b)
    {
        a.X += b.X;
        a.Y += b.Y;
        return a;
    }

    public static Point operator -(Point a, Point b)
    {
        a.X -= b.X;
        a.Y -= b.Y;
        return a;
    }

    public static Point operator *(Point a, Point b)
    {
        a.X *= b.X;
        a.Y *= b.Y;
        return a;
    }

    public static Point operator *(Point p, int n)
    {
        p.X *= n;
        p.Y *= n;
        return p;
    }

    public static Point operator *(int n, Point p)
    {
        p.X *= n;
        p.Y *= n;
        return p;
    }

    public static Point operator /(Point a, Point b)
    {
        a.X /= b.X;
        a.Y /= b.Y;
        return a;
    }

    public static Point operator /(Point p, int n)
    {
        p.X /= n;
        p.Y /= n;
        return p;
    }

    public static Point operator /(int n, Point p)
    {
        p.X /= n;
        p.Y /= n;
        return p;
    }
}