using System;
using System.Diagnostics;
using System.Text;

namespace FlatStage;

/// <summary>
/// Describes a 3D-vector.
/// </summary>
public struct Vec3 : IEquatable<Vec3>
{
    #region Public Static Properties

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, 0, 0.
    /// </summary>
    public static Vec3 Zero => zero;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 1, 1, 1.
    /// </summary>
    public static Vec3 One => one;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 1, 0, 0.
    /// </summary>
    public static Vec3 UnitX => unitX;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, 1, 0.
    /// </summary>
    public static Vec3 UnitY => unitY;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, 0, 1.
    /// </summary>
    public static Vec3 UnitZ => unitZ;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, 1, 0.
    /// </summary>
    public static Vec3 Up => up;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, -1, 0.
    /// </summary>
    public static Vec3 Down => down;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 1, 0, 0.
    /// </summary>
    public static Vec3 Right => right;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components -1, 0, 0.
    /// </summary>
    public static Vec3 Left => left;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, 0, -1.
    /// </summary>
    public static Vec3 Forward => forward;

    /// <summary>
    /// Returns a <see cref="Vec3"/> with components 0, 0, 1.
    /// </summary>
    public static Vec3 Backward => backward;

    #endregion

    #region Internal Properties

    internal string DebugDisplayString =>
        string.Concat(
            X.ToString(), " ",
            Y.ToString(), " ",
            Z.ToString()
        );

    #endregion

    #region Private Static Fields

    private static Vec3 zero = new(0f, 0f, 0f);
    private static Vec3 one = new(1f, 1f, 1f);
    private static Vec3 unitX = new(1f, 0f, 0f);
    private static Vec3 unitY = new(0f, 1f, 0f);
    private static Vec3 unitZ = new(0f, 0f, 1f);
    private static Vec3 up = new(0f, 1f, 0f);
    private static Vec3 down = new(0f, -1f, 0f);
    private static Vec3 right = new(1f, 0f, 0f);
    private static Vec3 left = new(-1f, 0f, 0f);
    private static Vec3 forward = new(0f, 0f, -1f);
    private static Vec3 backward = new(0f, 0f, 1f);

    #endregion

    #region Public Fields

    /// <summary>
    /// The x coordinate of this <see cref="Vec3"/>.
    /// </summary>
    public float X;

    /// <summary>
    /// The y coordinate of this <see cref="Vec3"/>.
    /// </summary>
    public float Y;

    /// <summary>
    /// The z coordinate of this <see cref="Vec3"/>.
    /// </summary>
    public float Z;

    #endregion

    #region Public Constructors

    /// <summary>
    /// Constructs a 3d vector with X, Y and Z from three values.
    /// </summary>
    /// <param name="x">The x coordinate in 3d-space.</param>
    /// <param name="y">The y coordinate in 3d-space.</param>
    /// <param name="z">The z coordinate in 3d-space.</param>
    public Vec3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Constructs a 3d vector with X, Y and Z set to the same value.
    /// </summary>
    /// <param name="value">The x, y and z coordinates in 3d-space.</param>
    public Vec3(float value)
    {
        this.X = value;
        this.Y = value;
        this.Z = value;
    }

    /// <summary>
    /// Constructs a 3d vector with X, Y from <see cref="Vec2"/> and Z from a scalar.
    /// </summary>
    /// <param name="value">The x and y coordinates in 3d-space.</param>
    /// <param name="z">The z coordinate in 3d-space.</param>
    public Vec3(Vec2 value, float z)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = z;
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
        return (obj is Vec3 vector) && Equals(vector);
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Vec3"/>.
    /// </summary>
    /// <param name="other">The <see cref="Vec3"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public bool Equals(Vec3 other)
    {
        return (X == other.X &&
                Y == other.Y &&
                Z == other.Z);
    }

    /// <summary>
    /// Gets the hash code of this <see cref="Vec3"/>.
    /// </summary>
    /// <returns>Hash code of this <see cref="Vec3"/>.</returns>
    public override int GetHashCode()
    {
        return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
    }

    /// <summary>
    /// Returns the length of this <see cref="Vec3"/>.
    /// </summary>
    /// <returns>The length of this <see cref="Vec3"/>.</returns>
    public float Length()
    {
        return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    /// <summary>
    /// Returns the squared length of this <see cref="Vec3"/>.
    /// </summary>
    /// <returns>The squared length of this <see cref="Vec3"/>.</returns>
    public float LengthSquared()
    {
        return (X * X) + (Y * Y) + (Z * Z);
    }

    /// <summary>
    /// Turns this <see cref="Vec3"/> to a unit vector with the same direction.
    /// </summary>
    public void Normalize()
    {
        float factor = 1.0f / (float)Math.Sqrt(
            (X * X) +
            (Y * Y) +
            (Z * Z)
        );
        X *= factor;
        Y *= factor;
        Z *= factor;
    }

    /// <summary>
    /// Returns a <see cref="String"/> representation of this <see cref="Vec3"/> in the format:
    /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Z:[<see cref="Z"/>]}
    /// </summary>
    /// <returns>A <see cref="String"/> representation of this <see cref="Vec3"/>.</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(32);
        sb.Append("{X:");
        sb.Append(this.X);
        sb.Append(" Y:");
        sb.Append(this.Y);
        sb.Append(" Z:");
        sb.Append(this.Z);
        sb.Append("}");
        return sb.ToString();
    }

    #endregion

    #region Public Static Methods

    /// <summary>
    /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
    /// </summary>
    /// <param name="value1">The first vector to add.</param>
    /// <param name="value2">The second vector to add.</param>
    /// <returns>The result of the vector addition.</returns>
    public static Vec3 Add(Vec3 value1, Vec3 value2)
    {
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
    public static void Add(ref Vec3 value1, ref Vec3 value2, out Vec3 result)
    {
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
    public static Vec3 Clamp(Vec3 value1, Vec3 min, Vec3 max)
    {
        return new Vec3(
            MathUtils.Clamp(value1.X, min.X, max.X),
            MathUtils.Clamp(value1.Y, min.Y, max.Y),
            MathUtils.Clamp(value1.Z, min.Z, max.Z)
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
        ref Vec3 value1,
        ref Vec3 min,
        ref Vec3 max,
        out Vec3 result
    )
    {
        result.X = MathUtils.Clamp(value1.X, min.X, max.X);
        result.Y = MathUtils.Clamp(value1.Y, min.Y, max.Y);
        result.Z = MathUtils.Clamp(value1.Z, min.Z, max.Z);
    }

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The cross product of two vectors.</returns>
    public static Vec3 Cross(Vec3 vector1, Vec3 vector2)
    {
        Cross(ref vector1, ref vector2, out vector1);
        return vector1;
    }

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <param name="result">The cross product of two vectors as an output parameter.</param>
    public static void Cross(ref Vec3 vector1, ref Vec3 vector2, out Vec3 result)
    {
        float x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
        float y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
        float z = vector1.X * vector2.Y - vector2.X * vector1.Y;
        result.X = x;
        result.Y = y;
        result.Z = z;
    }



    /// <summary>
    /// Divides the components of a <see cref="Vec3"/> by the components of another <see cref="Vec3"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec3"/>.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec3 Divide(Vec3 value1, Vec3 value2)
    {
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3"/> by the components of another <see cref="Vec3"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Divisor <see cref="Vec3"/>.</param>
    /// <param name="result">The result of dividing the vectors as an output parameter.</param>
    public static void Divide(ref Vec3 value1, ref Vec3 value2, out Vec3 result)
    {
        result.X = value1.X / value2.X;
        result.Y = value1.Y / value2.Y;
        result.Z = value1.Z / value2.Z;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Divisor scalar.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec3 Divide(Vec3 value1, float value2)
    {
        float factor = 1 / value2;
        value1.X *= factor;
        value1.Y *= factor;
        value1.Z *= factor;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3"/> by a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Divisor scalar.</param>
    /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
    public static void Divide(ref Vec3 value1, float value2, out Vec3 result)
    {
        float factor = 1 / value2;
        result.X = value1.X * factor;
        result.Y = value1.Y * factor;
        result.Z = value1.Z * factor;
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product of two vectors.</returns>
    public static float Dot(Vec3 vector1, Vec3 vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
    }

    /// <summary>
    /// Returns a dot product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <param name="result">The dot product of two vectors as an output parameter.</param>
    public static void Dot(ref Vec3 vector1, ref Vec3 vector2, out float result)
    {
        result = (
            (vector1.X * vector2.X) +
            (vector1.Y * vector2.Y) +
            (vector1.Z * vector2.Z)
        );
    }


    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec3"/> with maximal values from the two vectors.</returns>
    public static Vec3 Max(Vec3 value1, Vec3 value2)
    {
        return new Vec3(
            MathUtils.Max(value1.X, value2.X),
            MathUtils.Max(value1.Y, value2.Y),
            MathUtils.Max(value1.Z, value2.Z)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a maximal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec3"/> with maximal values from the two vectors as an output parameter.</param>
    public static void Max(ref Vec3 value1, ref Vec3 value2, out Vec3 result)
    {
        result.X = MathUtils.Max(value1.X, value2.X);
        result.Y = MathUtils.Max(value1.Y, value2.Y);
        result.Z = MathUtils.Max(value1.Z, value2.Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The <see cref="Vec3"/> with minimal values from the two vectors.</returns>
    public static Vec3 Min(Vec3 value1, Vec3 value2)
    {
        return new Vec3(
            MathUtils.Min(value1.X, value2.X),
            MathUtils.Min(value1.Y, value2.Y),
            MathUtils.Min(value1.Z, value2.Z)
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a minimal values from the two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The <see cref="Vec3"/> with minimal values from the two vectors as an output parameter.</param>
    public static void Min(ref Vec3 value1, ref Vec3 value2, out Vec3 result)
    {
        result.X = MathUtils.Min(value1.X, value2.X);
        result.Y = MathUtils.Min(value1.Y, value2.Y);
        result.Z = MathUtils.Min(value1.Z, value2.Z);
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Source <see cref="Vec3"/>.</param>
    /// <returns>The result of the vector multiplication.</returns>
    public static Vec3 Multiply(Vec3 value1, Vec3 value2)
    {
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a multiplication of <see cref="Vec3"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <returns>The result of the vector multiplication with a scalar.</returns>
    public static Vec3 Multiply(Vec3 value1, float scaleFactor)
    {
        value1.X *= scaleFactor;
        value1.Y *= scaleFactor;
        value1.Z *= scaleFactor;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a multiplication of <see cref="Vec3"/> and a scalar.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
    public static void Multiply(ref Vec3 value1, float scaleFactor, out Vec3 result)
    {
        result.X = value1.X * scaleFactor;
        result.Y = value1.Y * scaleFactor;
        result.Z = value1.Z * scaleFactor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a multiplication of two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Source <see cref="Vec3"/>.</param>
    /// <param name="result">The result of the vector multiplication as an output parameter.</param>
    public static void Multiply(ref Vec3 value1, ref Vec3 value2, out Vec3 result)
    {
        result.X = value1.X * value2.X;
        result.Y = value1.Y * value2.Y;
        result.Z = value1.Z * value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/>.</param>
    /// <returns>The result of the vector inversion.</returns>
    public static Vec3 Negate(Vec3 value)
    {
        value = new Vec3(-value.X, -value.Y, -value.Z);
        return value;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains the specified vector inversion.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/>.</param>
    /// <param name="result">The result of the vector inversion as an output parameter.</param>
    public static void Negate(ref Vec3 value, out Vec3 result)
    {
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/>.</param>
    /// <returns>Unit vector.</returns>
    public static Vec3 Normalize(Vec3 value)
    {
        float factor = 1.0f / (float)Math.Sqrt(
            (value.X * value.X) +
            (value.Y * value.Y) +
            (value.Z * value.Z)
        );
        return new Vec3(
            value.X * factor,
            value.Y * factor,
            value.Z * factor
        );
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a normalized values from another vector.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/>.</param>
    /// <param name="result">Unit vector as an output parameter.</param>
    public static void Normalize(ref Vec3 value, out Vec3 result)
    {
        float factor = 1.0f / (float)Math.Sqrt(
            (value.X * value.X) +
            (value.Y * value.Y) +
            (value.Z * value.Z)
        );
        result.X = value.X * factor;
        result.Y = value.Y * factor;
        result.Z = value.Z * factor;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains subtraction of on <see cref="Vec3"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Source <see cref="Vec3"/>.</param>
    /// <returns>The result of the vector subtraction.</returns>
    public static Vec3 Subtract(Vec3 value1, Vec3 value2)
    {
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains subtraction of on <see cref="Vec3"/> from a another.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/>.</param>
    /// <param name="value2">Source <see cref="Vec3"/>.</param>
    /// <param name="result">The result of the vector subtraction as an output parameter.</param>
    public static void Subtract(ref Vec3 value1, ref Vec3 value2, out Vec3 result)
    {
        result.X = value1.X - value2.X;
        result.Y = value1.Y - value2.Y;
        result.Z = value1.Z - value2.Z;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec3"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <returns>Transformed <see cref="Vec3"/>.</returns>
    public static Vec3 Transform(Vec3 position, Matrix matrix)
    {
        Transform(ref position, ref matrix, out position);
        return position;
    }

    /// <summary>
    /// Creates a new <see cref="Vec3"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">Source <see cref="Vec3"/>.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="result">Transformed <see cref="Vec3"/> as an output parameter.</param>
    public static void Transform(
        ref Vec3 position,
        ref Matrix matrix,
        out Vec3 result
    )
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
        result.X = x;
        result.Y = y;
        result.Z = z;
    }

    /// <summary>
    /// Apply transformation on all vectors within array of <see cref="Vec3"/> by the specified <see cref="Matrix"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    public static void Transform(
        Vec3[] sourceArray,
        ref Matrix matrix,
        Vec3[] destinationArray
    )
    {
        Debug.Assert(
            destinationArray.Length >= sourceArray.Length,
            "The destination array is smaller than the source array."
        );

        /* TODO: Are there options on some platforms to implement
         * a vectorized version of this?
         */

        for (int i = 0; i < sourceArray.Length; i += 1)
        {
            Vec3 position = sourceArray[i];
            destinationArray[i] = new Vec3(
                (position.X * matrix.M11) + (position.Y * matrix.M21) +
                    (position.Z * matrix.M31) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) +
                    (position.Z * matrix.M32) + matrix.M42,
                (position.X * matrix.M13) + (position.Y * matrix.M23) +
                    (position.Z * matrix.M33) + matrix.M43
            );
        }
    }

    /// <summary>
    /// Apply transformation on vectors within array of <see cref="Vec3"/> by the specified <see cref="Matrix"/> and places the results in an another array.
    /// </summary>
    /// <param name="sourceArray">Source array.</param>
    /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
    /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
    /// <param name="destinationArray">Destination array.</param>
    /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vec3"/> should be written.</param>
    /// <param name="length">The number of vectors to be transformed.</param>
    public static void Transform(
        Vec3[] sourceArray,
        int sourceIndex,
        ref Matrix matrix,
        Vec3[] destinationArray,
        int destinationIndex,
        int length
    )
    {
        Debug.Assert(
            sourceArray.Length - sourceIndex >= length,
            "The source array is too small for the given sourceIndex and length."
        );
        Debug.Assert(
            destinationArray.Length - destinationIndex >= length,
            "The destination array is too small for " +
            "the given destinationIndex and length."
        );

        /* TODO: Are there options on some platforms to implement a
         * vectorized version of this?
         */

        for (int i = 0; i < length; i += 1)
        {
            Vec3 position = sourceArray[sourceIndex + i];
            destinationArray[destinationIndex + i] = new Vec3(
                (position.X * matrix.M11) + (position.Y * matrix.M21) +
                    (position.Z * matrix.M31) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) +
                    (position.Z * matrix.M32) + matrix.M42,
                (position.X * matrix.M13) + (position.Y * matrix.M23) +
                    (position.Z * matrix.M33) + matrix.M43
            );
        }
    }

    #endregion

    #region Public Static Operators

    /// <summary>
    /// Compares whether two <see cref="Vec3"/> instances are equal.
    /// </summary>
    /// <param name="value1"><see cref="Vec3"/> instance on the left of the equal sign.</param>
    /// <param name="value2"><see cref="Vec3"/> instance on the right of the equal sign.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(Vec3 value1, Vec3 value2)
    {
        return (value1.X == value2.X &&
                value1.Y == value2.Y &&
                value1.Z == value2.Z);
    }

    /// <summary>
    /// Compares whether two <see cref="Vec3"/> instances are not equal.
    /// </summary>
    /// <param name="value1"><see cref="Vec3"/> instance on the left of the not equal sign.</param>
    /// <param name="value2"><see cref="Vec3"/> instance on the right of the not equal sign.</param>
    /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
    public static bool operator !=(Vec3 value1, Vec3 value2)
    {
        return !(value1 == value2);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/> on the left of the add sign.</param>
    /// <param name="value2">Source <see cref="Vec3"/> on the right of the add sign.</param>
    /// <returns>Sum of the vectors.</returns>
    public static Vec3 operator +(Vec3 value1, Vec3 value2)
    {
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        return value1;
    }

    /// <summary>
    /// Inverts values in the specified <see cref="Vec3"/>.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/> on the right of the sub sign.</param>
    /// <returns>Result of the inversion.</returns>
    public static Vec3 operator -(Vec3 value)
    {
        value = new Vec3(-value.X, -value.Y, -value.Z);
        return value;
    }

    /// <summary>
    /// Subtracts a <see cref="Vec3"/> from a <see cref="Vec3"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/> on the left of the sub sign.</param>
    /// <param name="value2">Source <see cref="Vec3"/> on the right of the sub sign.</param>
    /// <returns>Result of the vector subtraction.</returns>
    public static Vec3 operator -(Vec3 value1, Vec3 value2)
    {
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }

    /// <summary>
    /// Multiplies the components of two vectors by each other.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/> on the left of the mul sign.</param>
    /// <param name="value2">Source <see cref="Vec3"/> on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication.</returns>
    public static Vec3 operator *(Vec3 value1, Vec3 value2)
    {
        value1.X *= value2.X;
        value1.Y *= value2.Y;
        value1.Z *= value2.Z;
        return value1;
    }

    /// <summary>
    /// Multiplies the components of vector by a scalar.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/> on the left of the mul sign.</param>
    /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication with a scalar.</returns>
    public static Vec3 operator *(Vec3 value, float scaleFactor)
    {
        value.X *= scaleFactor;
        value.Y *= scaleFactor;
        value.Z *= scaleFactor;
        return value;
    }

    /// <summary>
    /// Multiplies the components of vector by a scalar.
    /// </summary>
    /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
    /// <param name="value">Source <see cref="Vec3"/> on the right of the mul sign.</param>
    /// <returns>Result of the vector multiplication with a scalar.</returns>
    public static Vec3 operator *(float scaleFactor, Vec3 value)
    {
        value.X *= scaleFactor;
        value.Y *= scaleFactor;
        value.Z *= scaleFactor;
        return value;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3"/> by the components of another <see cref="Vec3"/>.
    /// </summary>
    /// <param name="value1">Source <see cref="Vec3"/> on the left of the div sign.</param>
    /// <param name="value2">Divisor <see cref="Vec3"/> on the right of the div sign.</param>
    /// <returns>The result of dividing the vectors.</returns>
    public static Vec3 operator /(Vec3 value1, Vec3 value2)
    {
        value1.X /= value2.X;
        value1.Y /= value2.Y;
        value1.Z /= value2.Z;
        return value1;
    }

    /// <summary>
    /// Divides the components of a <see cref="Vec3"/> by a scalar.
    /// </summary>
    /// <param name="value">Source <see cref="Vec3"/> on the left of the div sign.</param>
    /// <param name="divider">Divisor scalar on the right of the div sign.</param>
    /// <returns>The result of dividing a vector by a scalar.</returns>
    public static Vec3 operator /(Vec3 value, float divider)
    {
        float factor = 1 / divider;
        value.X *= factor;
        value.Y *= factor;
        value.Z *= factor;
        return value;
    }

    #endregion
}