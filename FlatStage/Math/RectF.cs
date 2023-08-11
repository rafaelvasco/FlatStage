using System;

namespace FlatStage;


public struct RectF : IEquatable<RectF>
{
    #region Public Properties

    /// <summary>
    /// Returns the x coordinate of the left edge of this <see cref="RectF"/>.
    /// </summary>
    public readonly float Left => X;

    /// <summary>
    /// Returns the x coordinate of the right edge of this <see cref="RectF"/>.
    /// </summary>
    public readonly float Right => X + Width;


    /// <summary>
    /// Returns the y coordinate of the top edge of this <see cref="RectF"/>.
    /// </summary>
    public readonly float Top => Y;

    /// <summary>
    /// Returns the y coordinate of the bottom edge of this <see cref="RectF"/>.
    /// </summary>
    public readonly float Bottom => Y + Height;

    /// <summary>
    /// The top-left coordinates of this <see cref="RectF"/>.
    /// </summary>
    public Vec2 Location
    {
        readonly get => new(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    /// <summary>
    /// A <see cref="Vec2"/> located in the center of this <see cref="RectF"/>'s bounds.
    /// </summary>
    /// <remarks>
    /// If <see cref="Width"/> or <see cref="Height"/> is an odd number,
    /// the center point will be rounded down.
    /// </remarks>
    public readonly Vec2 Center =>
        new(
            X + Width / 2,
            Y + Height / 2
        );

    /// <summary>
    /// Whether or not this <see cref="RectF"/> has a width and
    /// height of 0, and a position of (0, 0).
    /// </summary>
    public readonly bool IsEmpty =>
        Width == 0 &&
        Height == 0 &&
        X == 0 &&
        Y == 0;

    #endregion

    #region Public Static Properties

    /// <summary>
    /// Returns a <see cref="RectF"/> with X=0, Y=0, Width=0, and Height=0.
    /// </summary>
    public static RectF Empty => EmptyRectangle;

    #endregion

    #region Public Fields

    /// <summary>
    /// The x coordinate of the top-left corner of this <see cref="RectF"/>.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// The y coordinate of the top-left corner of this <see cref="RectF"/>.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// The width of this <see cref="RectF"/>.
    /// </summary>
    public float Width { get; set; }

    /// <summary>
    /// The height of this <see cref="RectF"/>.
    /// </summary>
    public float Height { get; set; }

    #endregion

    #region Private Static Fields

    private static readonly RectF EmptyRectangle = new();

    #endregion

    #region Public Constructors

    /// <summary>
    /// Creates a <see cref="RectF"/> with the specified
    /// position, width, and height.
    /// </summary>
    /// <param name="x">The x coordinate of the top-left corner of the created <see cref="RectF"/>.</param>
    /// <param name="y">The y coordinate of the top-left corner of the created <see cref="RectF"/>.</param>
    /// <param name="width">The width of the created <see cref="RectF"/>.</param>
    /// <param name="height">The height of the created <see cref="RectF"/>.</param>
    public RectF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="RectF"/>.
    /// </summary>
    /// <param name="x">The x coordinate of the point to check for containment.</param>
    /// <param name="y">The y coordinate of the point to check for containment.</param>
    /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RectF"/>. <c>false</c> otherwise.</returns>
    public readonly bool Contains(float x, float y)
    {
        return X <= x &&
               x < X + Width &&
               Y <= y &&
               y < Y + Height;
    }

    /// <summary>
    /// Gets whether or not the provided <see cref="Vec2"/> lies within the bounds of this <see cref="RectF"/>.
    /// </summary>
    /// <param name="value">The coordinates to check for inclusion in this <see cref="RectF"/>.</param>
    /// <returns><c>true</c> if the provided <see cref="Vec2"/> lies inside this <see cref="RectF"/>. <c>false</c> otherwise.</returns>
    public readonly bool Contains(Vec2 value)
    {
        return X <= value.X &&
               value.X < X + Width &&
               Y <= value.Y &&
               value.Y < Y + Height;
    }

    /// <summary>
    /// Gets whether or not the provided <see cref="RectF"/> lies within the bounds of this <see cref="RectF"/>.
    /// </summary>
    /// <param name="value">The <see cref="RectF"/> to check for inclusion in this <see cref="RectF"/>.</param>
    /// <returns><c>true</c> if the provided <see cref="RectF"/>'s bounds lie entirely inside this <see cref="RectF"/>. <c>false</c> otherwise.</returns>
    public readonly bool Contains(RectF value)
    {
        return X <= value.X &&
               value.X + value.Width <= X + Width &&
               Y <= value.Y &&
               value.Y + value.Height <= Y + Height;
    }

    public readonly void Contains(ref Vec2 value, out bool result)
    {
        result = X <= value.X &&
                 value.X < X + Width &&
                 Y <= value.Y &&
                 value.Y < Y + Height;
    }

    public readonly void Contains(ref RectF value, out bool result)
    {
        result = X <= value.X &&
                 value.X + value.Width <= X + Width &&
                 Y <= value.Y &&
                 value.Y + value.Height <= Y + Height;
    }

    /// <summary>
    /// Increments this <see cref="RectF"/>'s <see cref="Location"/> by the
    /// x and y components of the provided <see cref="Vec2"/>.
    /// </summary>
    /// <param name="offset">The x and y components to add to this <see cref="RectF"/>'s <see cref="Location"/>.</param>
    public void Offset(Vec2 offset)
    {
        X += offset.X;
        Y += offset.Y;
    }

    /// <summary>
    /// Increments this <see cref="RectF"/>'s <see cref="Location"/> by the
    /// provided x and y coordinates.
    /// </summary>
    /// <param name="offsetX">The x coordinate to add to this <see cref="RectF"/>'s <see cref="Location"/>.</param>
    /// <param name="offsetY">The y coordinate to add to this <see cref="RectF"/>'s <see cref="Location"/>.</param>
    public void Offset(float offsetX, float offsetY)
    {
        X += offsetX;
        Y += offsetY;
    }

    public void Inflate(float horizontalValue, float verticalValue)
    {
        X -= horizontalValue;
        Y -= verticalValue;
        Width += horizontalValue * 2;
        Height += verticalValue * 2;
    }

    public readonly RectF Inflated(float horizontalValue, float verticalValue)
    {
        var result = this;
        result.Inflate(horizontalValue, verticalValue);
        return result;
    }

    /// <summary>
    /// Checks whether or not this <see cref="RectF"/> is equivalent
    /// to a provided <see cref="RectF"/>.
    /// </summary>
    /// <param name="other">The <see cref="RectF"/> to test for equality.</param>
    /// <returns>
    /// <c>true</c> if this <see cref="RectF"/>'s x coordinate, y coordinate, width, and height
    /// match the values for the provided <see cref="RectF"/>. <c>false</c> otherwise.
    /// </returns>
    public readonly bool Equals(RectF other)
    {
        return this == other;
    }

    /// <summary>
    /// Checks whether or not this <see cref="Rect"/> is equivalent
    /// to a provided object.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to test for equality.</param>
    /// <returns>
    /// <c>true</c> if the provided object is a <see cref="Rect"/>, and this
    /// <see cref="Rect"/>'s x coordinate, y coordinate, width, and height
    /// match the values for the provided <see cref="Rect"/>. <c>false</c> otherwise.
    /// </returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is RectF rectangle && this == rectangle;
    }

    public override readonly string ToString()
    {
        return "{X:" + X +
               " Y:" + Y +
               " Width:" + Width +
               " Height:" + Height +
               "}";
    }

    public override readonly int GetHashCode()
    {
        return (int)X ^ (int)Y ^ (int)Width ^ (int)Height;
    }

    /// <summary>
    /// Gets whether or not the other <see cref="Rect"/> intersects with this rectangle.
    /// </summary>
    /// <param name="value">The other rectangle for testing.</param>
    /// <returns><c>true</c> if other <see cref="Rect"/> intersects with this rectangle; <c>false</c> otherwise.</returns>
    public readonly bool Intersects(RectF value)
    {
        return value.Left < Right &&
               Left < value.Right &&
               value.Top < Bottom &&
               Top < value.Bottom;
    }

    /// <summary>
    /// Gets whether or not the other <see cref="Rect"/> intersects with this rectangle.
    /// </summary>
    /// <param name="value">The other rectangle for testing.</param>
    /// <param name="result"><c>true</c> if other <see cref="Rect"/> intersects with this rectangle; <c>false</c> otherwise. As an output parameter.</param>
    public readonly void Intersects(ref RectF value, out bool result)
    {
        result = value.Left < Right &&
                 Left < value.Right &&
                 value.Top < Bottom &&
                 Top < value.Bottom;
    }

    #endregion

    #region Public Static Methods

    public static bool operator ==(RectF a, RectF b)
    {
        return a.X == b.X &&
               a.Y == b.Y &&
               a.Width == b.Width &&
               a.Height == b.Height;
    }

    public static bool operator !=(RectF a, RectF b)
    {
        return !(a == b);
    }

    public static RectF Intersect(RectF value1, RectF value2)
    {
        Intersect(ref value1, ref value2, out var rectangle);
        return rectangle;
    }

    public static void Intersect(
        ref RectF value1,
        ref RectF value2,
        out RectF result
    )
    {
        if (value1.Intersects(value2))
        {
            float rightSide = Calc.Min(
                value1.X + value1.Width,
                value2.X + value2.Width
            );
            float leftSide = Calc.Max(value1.X, value2.X);
            float topSide = Calc.Max(value1.Y, value2.Y);
            float bottomSide = Calc.Min(
                value1.Y + value1.Height,
                value2.Y + value2.Height
            );
            result = new RectF(
                leftSide,
                topSide,
                rightSide - leftSide,
                bottomSide - topSide
            );
        }
        else
        {
            result = new RectF(0, 0, 0, 0);
        }
    }

    public static RectF Union(RectF value1, RectF value2)
    {
        float x = Calc.Min(value1.X, value2.X);
        float y = Calc.Min(value1.Y, value2.Y);
        return new RectF(
            x,
            y,
            Calc.Max(value1.Right, value2.Right) - x,
            Calc.Max(value1.Bottom, value2.Bottom) - y
        );
    }

    public static void Union(ref RectF value1, ref RectF value2, out RectF result)
    {
        result = default;
        result.X = Calc.Min(value1.X, value2.X);
        result.Y = Calc.Min(value1.Y, value2.Y);
        result.Width = Calc.Max(value1.Right, value2.Right) - result.X;
        result.Height = Calc.Max(value1.Bottom, value2.Bottom) - result.Y;
    }

    #endregion
}