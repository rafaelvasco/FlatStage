using System;

namespace FlatStage;

/// <summary>
/// Describes a 4D-vector.
/// </summary>
public struct Vec4 : IEquatable<Vec4>
{
    #region Public Static Properties

    /// <summary>
    /// Returns a <see cref="Vec4"/> with components 0, 0, 0, 0.
    /// </summary>
    public static Vec4 Zero => zero;

    /// <summary>
    /// Returns a <see cref="Vec4"/> with components 1, 1, 1, 1.
    /// </summary>
    public static Vec4 One => unit;

    /// <summary>
    /// Returns a <see cref="Vec4"/> with components 1, 0, 0, 0.
    /// </summary>
    public static Vec4 UnitX => unitX;

    /// <summary>
    /// Returns a <see cref="Vec4"/> with components 0, 1, 0, 0.
    /// </summary>
    public static Vec4 UnitY => unitY;

    /// <summary>
    /// Returns a <see cref="Vec4"/> with components 0, 0, 1, 0.
    /// </summary>
    public static Vec4 UnitZ => unitZ;

    /// <summary>
    /// Returns a <see cref="Vec4"/> with components 0, 0, 0, 1.
    /// </summary>
    public static Vec4 UnitW => unitW;

    #endregion

    #region Internal Properties

    internal string DebugDisplayString =>
        string.Concat(
            X.ToString(), " ",
            Y.ToString(), " ",
            Z.ToString(), " ",
            W.ToString()
        );

    #endregion

    #region Public Fields

    /// <summary>
    /// The x coordinate of this <see cref="Vec4"/>.
    /// </summary>
    public float X;

    /// <summary>
    /// The y coordinate of this <see cref="Vec4"/>.
    /// </summary>
    public float Y;

    /// <summary>
    /// The z coordinate of this <see cref="Vec4"/>.
    /// </summary>
    public float Z;

    /// <summary>
    /// The w coordinate of this <see cref="Vec4"/>.
    /// </summary>
    public float W;

    #endregion

    #region Private Static Fields

    private static Vec4 zero = new();
    private static Vec4 unit = new(1f, 1f, 1f, 1f);
    private static Vec4 unitX = new(1f, 0f, 0f, 0f);
    private static Vec4 unitY = new(0f, 1f, 0f, 0f);
    private static Vec4 unitZ = new(0f, 0f, 1f, 0f);
    private static Vec4 unitW = new(0f, 0f, 0f, 1f);

    #endregion

    #region Public Constructors

    /// <summary>
    /// Constructs a 3d vector with X, Y, Z and W from four values.
    /// </summary>
    /// <param name="x">The x coordinate in 4d-space.</param>
    /// <param name="y">The y coordinate in 4d-space.</param>
    /// <param name="z">The z coordinate in 4d-space.</param>
    /// <param name="w">The w coordinate in 4d-space.</param>
    public Vec4(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    /// <summary>
    /// Constructs a 3d vector with X and Z from <see cref="Vec2"/> and Z and W from the scalars.
    /// </summary>
    /// <param name="value">The x and y coordinates in 4d-space.</param>
    /// <param name="z">The z coordinate in 4d-space.</param>
    /// <param name="w">The w coordinate in 4d-space.</param>
    public Vec4(Vec2 value, float z, float w)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = z;
        this.W = w;
    }

    /// <summary>
    /// Constructs a 3d vector with X, Y, Z from <see cref="Vec3"/> and W from a scalar.
    /// </summary>
    /// <param name="value">The x, y and z coordinates in 4d-space.</param>
    /// <param name="w">The w coordinate in 4d-space.</param>
    public Vec4(Vec3 value, float w)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = value.Z;
        this.W = w;
    }

    /// <summary>
    /// Constructs a 4d vector with X, Y, Z and W set to the same value.
    /// </summary>
    /// <param name="value">The x, y, z and w coordinates in 4d-space.</param>
    public Vec4(float value)
    {
        this.X = value;
        this.Y = value;
        this.Z = value;
        this.W = value;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Object"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Object"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj)
    {
        return (obj is Vec4 vec) && Equals(vec);
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Vec4"/>.
    /// </summary>
    /// <param name="other">The <see cref="Vec4"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public readonly bool Equals(Vec4 other)
    {
        return (X == other.X &&
                Y == other.Y &&
                Z == other.Z &&
                W == other.W);
    }

    /// <summary>
    /// Gets the hash code of this <see cref="Vec4"/>.
    /// </summary>
    /// <returns>Hash code of this <see cref="Vec4"/>.</returns>
    public override int GetHashCode()
    {
        return W.GetHashCode() + X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
    }

    /// <summary>
    /// Returns the length of this <see cref="Vec4"/>.
    /// </summary>
    /// <returns>The length of this <see cref="Vec4"/>.</returns>
    public float Length()
    {
        return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
    }

    /// <summary>
    /// Returns the squared length of this <see cref="Vec4"/>.
    /// </summary>
    /// <returns>The squared length of this <see cref="Vec4"/>.</returns>
    public float LengthSquared()
    {
        return (X * X) + (Y * Y) + (Z * Z) + (W * W);
    }

    /// <summary>
    /// Turns this <see cref="Vec4"/> to a unit vector with the same direction.
    /// </summary>
    public void Normalize()
    {
        float factor = 1.0f / (float)Math.Sqrt(
            (X * X) +
            (Y * Y) +
            (Z * Z) +
            (W * W)
        );
        X *= factor;
        Y *= factor;
        Z *= factor;
        W *= factor;
    }

    public override string ToString()
    {
        return (
            "{X:" + X.ToString() +
            " Y:" + Y.ToString() +
            " Z:" + Z.ToString() +
            " W:" + W.ToString() + "}"
        );
    }

    #endregion

    #region Public Static Methods

    /// <summary>
    /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
    /// </summary>
    /// <param name="value1">The first vector to add.</param>
    /// <param name="value2">The second vector to add.</param>
    /// <returns>The result of the vector addition.</returns>
    public static Vec4 Add(Vec4 value1, Vec4 value2)
    {
        value1.W += value2.W;
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        return value1;
    }

    /// <summary>
    /// Performs vector addition on <paramref name="value1"/> and
    /// <paramref name="value2"/>, storing the result of the
    /// addition in <paramref name="result"/>.
    /// </summary>
    /// <param name="value1">The first vector to add.</param>
    /// <param name="value2">The second vector to add.</param>
    /// <param name="result">The result of the vector addition.</param>
    public static void Add(ref Vec4 value1, ref Vec4 value2, out Vec4 result)
    {
        result.W = value1.W + value2.W;
        result.X = value1.X + value2.X;
        result.Y = value1.Y + value2.Y;
        result.Z = value1.Z + value2.Z;
    }

    /// <summary>
    /// Clamps the specified value within a range.
    /// </summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    /// <returns>The clamped value.</returns>
    public static Vec4 Clamp(Vec4 value1, Vec4 min, Vec4 max)
    {
        return new Vec4(
            Calc.Clamp(value1.X, min.X, max.X),
            Calc.Clamp(value1.Y, min.Y, max.Y),
            Calc.Clamp(value1.Z, min.Z, max.Z),
            Calc.Clamp(value1.W, min.W, max.W)
        );
    }

    /// <summary>
    /// Clamps the specified value within a range.
    /// </summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The min value.</param>
    /// <param name="max">The max value.</param>
    /// <param name="result">The clamped value as an output parameter.</param>
    public static void Clamp(
        ref Vec4 value1,
        ref Vec4 min,
        ref Vec4 max,
        out Vec4 result
    )
    {
        result.X = Calc.Clamp(value1.X, min.X, max.X);
        result.Y = Calc.Clamp(value1.Y, min.Y, max.Y);
        result.Z = Calc.Clamp(value1.Z, min.Z, max.Z);
        result.W = Calc.Clamp(value1.W, min.W, max.W);
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4"/> by the components of another <see cref="Vec4"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec4"/>.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec4 Divide(Vec4 value1, Vec4 value2)
    {
        value1.W /= value2.W;
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec4 Divide(Vec4 value1, float divider)
    {
        float factor = 1f / divider;
        value1.W *= factor;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
    public static void Divide(ref Vec4 value1, float divider, out Vec4 result)
    {
        float factor = 1f / divider;
        result.W = value1.W * factor;
        result.X = value1.X * factor;
        result.Y = value1.Y * factor;
        result.Z = value1.Z * factor;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec4"/> by the components of another <see cref="Vec4"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec4"/>.</param>
    /// <param name="result">The result of dividing the vectors as an output parameter.</param>
    public static void Divide(
        ref Vec4 value1,
        ref Vec4 value2,
        out Vec4 result
    )
    {
        result.W = value1.W / value2.W;
        result.X = value1.X / value2.X;
        result.Y = value1.Y / value2.Y;
        result.Z = value1.Z / value2.Z;
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product of two vectors.</returns>
    public static float Dot(Vec4 vector1, Vec4 vector2)
    {
        return (
            vector1.X * vector2.X +
            vector1.Y * vector2.Y +
            vector1.Z * vector2.Z +
            vector1.W * vector2.W
        );
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <param name="result">The dot product of two vectors as an output parameter.</param>
    public static void Dot(ref Vec4 vector1, ref Vec4 vector2, out float result)
    {
        result = (
            (vector1.X * vector2.X) +
            (vector1.Y * vector2.Y) +
            (vector1.Z * vector2.Z) +
            (vector1.W * vector2.W)
        );
    }


    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec4"/> with maximal values from the two vectors.</returns>
    public static Vec4 Max(Vec4 value1, Vec4 value2)
    {
        return new Vec4(
            Calc.Max(value1.X, value2.X),
            Calc.Max(value1.Y, value2.Y),
            Calc.Max(value1.Z, value2.Z),
            Calc.Max(value1.W, value2.W)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec4"/> with maximal values from the two vectors as an output parameter.</param>
    public static void Max(ref Vec4 value1, ref Vec4 value2, out Vec4 result)
    {
        result.X = Calc.Max(value1.X, value2.X);
        result.Y = Calc.Max(value1.Y, value2.Y);
        result.Z = Calc.Max(value1.Z, value2.Z);
        result.W = Calc.Max(value1.W, value2.W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec4"/> with minimal values from the two vectors.</returns>
    public static Vec4 Min(Vec4 value1, Vec4 value2)
    {
        return new Vec4(
            Calc.Min(value1.X, value2.X),
            Calc.Min(value1.Y, value2.Y),
            Calc.Min(value1.Z, value2.Z),
            Calc.Min(value1.W, value2.W)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec4"/> with minimal values from the two vectors as an output parameter.</param>
    public static void Min(ref Vec4 value1, ref Vec4 value2, out Vec4 result)
    {
        result.X = Calc.Min(value1.X, value2.X);
        result.Y = Calc.Min(value1.Y, value2.Y);
        result.Z = Calc.Min(value1.Z, value2.Z);
        result.W = Calc.Min(value1.W, value2.W);
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="value2">Source <see cref="Vec4"/>.</param>
    /// <returns>The result of the vector multiplication.</returns>
    public static Vec4 Multiply(Vec4 value1, Vec4 value2)
    {
        value1.W *= value2.W;
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a multiplication of <see cref="Vec4"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <returns>The result of the vector multiplication with a scalar.</returns>
    public static Vec4 Multiply(Vec4 value1, float scaleFactor)
    {
        value1.W *= scaleFactor;
        value1.X *= scaleFactor;
        value1.Y *= scaleFactor;
        value1.Z *= scaleFactor;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a multiplication of <see cref="Vec4"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
    public static void Multiply(ref Vec4 value1, float scaleFactor, out Vec4 result)
    {
        result.W = value1.W * scaleFactor;
        result.X = value1.X * scaleFactor;
        result.Y = value1.Y * scaleFactor;
        result.Z = value1.Z * scaleFactor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="value2">Source <see cref="Vec4"/>.</param>
    /// <param name="result">The result of the vector multiplication as an output parameter.</param>
    public static void Multiply(ref Vec4 value1, ref Vec4 value2, out Vec4 result)
    {
        result.W = value1.W * value2.W;
        result.X = value1.X * value2.X;
        result.Y = value1.Y * value2.Y;
        result.Z = value1.Z * value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4"/>.</param>
    /// <returns>The result of the vector inversion.</returns>
    public static Vec4 Negate(Vec4 value)
    {
        value = new Vec4(-value.X, -value.Y, -value.Z, -value.W);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec4"/>.</param>
    /// <param name="result">The result of the vector inversion as an output parameter.</param>
    public static void Negate(ref Vec4 value, out Vec4 result)
    {
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
        result.W = -value.W;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="vector">Source <see cref="Vec4"/>.</param>
    /// <returns>Unit vector.</returns>
    public static Vec4 Normalize(Vec4 vector)
    {
        float factor = 1.0f / (float)Math.Sqrt(
            (vector.X * vector.X) +
            (vector.Y * vector.Y) +
            (vector.Z * vector.Z) +
            (vector.W * vector.W)
        );
        return new Vec4(
            vector.X * factor,
            vector.Y * factor,
            vector.Z * factor,
            vector.W * factor
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="vector">Source <see cref="Vec4"/>.</param>
    /// <param name="result">Unit vector as an output parameter.</param>
    public static void Normalize(ref Vec4 vector, out Vec4 result)
    {
        float factor = 1.0f / (float)Math.Sqrt(
            (vector.X * vector.X) +
            (vector.Y * vector.Y) +
            (vector.Z * vector.Z) +
            (vector.W * vector.W)
        );
        result.X = vector.X * factor;
        result.Y = vector.Y * factor;
        result.Z = vector.Z * factor;
        result.W = vector.W * factor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains subtraction of on <see cref="Vec4"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="value2">Source <see cref="Vec4"/>.</param>
    /// <returns>The result of the vector subtraction.</returns>
    public static Vec4 Subtract(Vec4 value1, Vec4 value2)
    {
        value1.W -= value2.W;
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains subtraction of on <see cref="Vec4"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec4"/>.</param>
    /// <param name="value2">Source <see cref="Vec4"/>.</param>
    /// <param name="result">The result of the vector subtraction as an output parameter.</param>
    public static void Subtract(ref Vec4 value1, ref Vec4 value2, out Vec4 result)
    {
        result.W = value1.W - value2.W;
        result.X = value1.X - value2.X;
        result.Y = value1.Y - value2.Y;
        result.Z = value1.Z - value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec2"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <returns>Transformed <see cref="Vec4"/>.</returns>
    public static Vec4 Transform(Vec2 position, Matrix matrix)
    {
        Transform(ref position, ref matrix, out Vec4 result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec3"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <returns>Transformed <see cref="Vec4"/>.</returns>
    public static Vec4 Transform(Vec3 position, Matrix matrix)
    {
        Transform(ref position, ref matrix, out Vec4 result);
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a transformation of 4d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="vector">Source <see cref="Vec4"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <returns>Transformed <see cref="Vec4"/>.</returns>
    public static Vec4 Transform(Vec4 vector, Matrix matrix)
    {
        Transform(ref vector, ref matrix, out vector);
        return vector;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec2"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="result">Transformed <see cref="Vec4"/> as an output parameter.</param>
    public static void Transform(ref Vec2 position, ref Matrix matrix, out Vec4 result)
    {
        result = new Vec4(
            (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
            (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42,
            (position.X * matrix.M13) + (position.Y * matrix.M23) + matrix.M43,
            (position.X * matrix.M14) + (position.Y * matrix.M24) + matrix.M44
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec3"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="result">Transformed <see cref="Vec4"/> as an output parameter.</param>
    public static void Transform(ref Vec3 position, ref Matrix matrix, out Vec4 result)
    {
        float x = (
            (position.X * matrix.M11) +
            (position.Y * matrix.M21) +
            (position.Z * matrix.M31) +
            matrix.M41
        );
        float y = (
            (position.X * matrix.M12) +
            (position.Y * matrix.M22) +
            (position.Z * matrix.M32) +
            matrix.M42
        );
        float z = (
            (position.X * matrix.M13) +
            (position.Y * matrix.M23) +
            (position.Z * matrix.M33) +
            matrix.M43
        );
        float w = (
            (position.X * matrix.M14) +
            (position.Y * matrix.M24) +
            (position.Z * matrix.M34) +
            matrix.M44
        );
        result.X = x;
        result.Y = y;
        result.Z = z;
        result.W = w;
    }

    /// <summary>
    /// Creates a new <see cref="Vec4"/> that contains a transformation of 4d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="vector">Source <see cref="Vec4"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="result">Transformed <see cref="Vec4"/> as an output parameter.</param>
    public static void Transform(ref Vec4 vector, ref Matrix matrix, out Vec4 result)
    {
        float x = (
            (vector.X * matrix.M11) +
            (vector.Y * matrix.M21) +
            (vector.Z * matrix.M31) +
            (vector.W * matrix.M41)
        );
        float y = (
            (vector.X * matrix.M12) +
            (vector.Y * matrix.M22) +
            (vector.Z * matrix.M32) +
            (vector.W * matrix.M42)
        );
        float z = (
            (vector.X * matrix.M13) +
            (vector.Y * matrix.M23) +
            (vector.Z * matrix.M33) +
            (vector.W * matrix.M43)
        );
        float w = (
            (vector.X * matrix.M14) +
            (vector.Y * matrix.M24) +
            (vector.Z * matrix.M34) +
            (vector.W * matrix.M44)
        );
        result.X = x;
        result.Y = y;
        result.Z = z;
        result.W = w;
    }

    /// <summary>
    /// Apply transformation on all vectors within array of <see cref="Vec4"/> by the specified <see cref="Matrix"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void Transform(
        Vec4[] sourceArray,
        ref Matrix matrix,
        Vec4[] destinationArray
    )
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (destinationArray.Length < sourceArray.Length)
        {
            throw new ArgumentException(
                "destinationArray is too small to contain the result."
            );
        }
        for (int i = 0; i < sourceArray.Length; i += 1)
        {
            Transform(
                ref sourceArray[i],
                ref matrix,
                out destinationArray[i]
            );
        }
    }

    /// <summary>
    /// Apply transformation on vectors within array of <see cref="Vec4"/> by the specified <see cref="Matrix"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec4"/> should be written.</param>
    /// <param name="length">The number of vectors to be transformed.</param>
    public static void Transform(
        Vec4[] sourceArray,
        int sourceIndex,
        ref Matrix matrix,
        Vec4[] destinationArray,
        int destinationIndex,
        int length
    )
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (destinationIndex + length > destinationArray.Length)
        {
            throw new ArgumentException(
                "destinationArray is too small to contain the result."
            );
        }
        if (sourceIndex + length > sourceArray.Length)
        {
            throw new ArgumentException(
                "The combination of sourceIndex and length was greater than sourceArray.Length."
            );
        }
        for (int i = 0; i < length; i += 1)
        {
            Transform(
                ref sourceArray[i + sourceIndex],
                ref matrix,
                out destinationArray[i + destinationIndex]
            );
        }
    }

    #endregion

    #region Public Static Operators

    public static Vec4 operator -(Vec4 value)
    {
        return new Vec4(-value.X, -value.Y, -value.Z, -value.W);
    }

    public static bool operator ==(Vec4 value1, Vec4 value2)
    {
        return (value1.X == value2.X &&
                value1.Y == value2.Y &&
                value1.Z == value2.Z &&
                value1.W == value2.W);
    }

    public static bool operator !=(Vec4 value1, Vec4 value2)
    {
        return !(value1 == value2);
    }

    public static Vec4 operator +(Vec4 value1, Vec4 value2)
    {
        value1.W += value2.W;
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        return value1;
    }

    public static Vec4 operator -(Vec4 value1, Vec4 value2)
    {
        value1.W -= value2.W;
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    public static Vec4 operator *(Vec4 value1, Vec4 value2)
    {
        value1.W *= value2.W;
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    public static Vec4 operator *(Vec4 value1, float scaleFactor)
    {
        value1.W *= scaleFactor;
        value1.X *= scaleFactor;
        value1.Y *= scaleFactor;
        value1.Z *= scaleFactor;
        return value1;
    }

    public static Vec4 operator *(float scaleFactor, Vec4 value1)
    {
        value1.W *= scaleFactor;
        value1.X *= scaleFactor;
        value1.Y *= scaleFactor;
        value1.Z *= scaleFactor;
        return value1;
    }

    public static Vec4 operator /(Vec4 value1, Vec4 value2)
    {
        value1.W /= value2.W;
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    public static Vec4 operator /(Vec4 value1, float divider)
    {
        float factor = 1f / divider;
        value1.W *= factor;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

    #endregion
}