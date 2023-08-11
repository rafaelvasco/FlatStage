using System;

namespace FlatStage;

/// <summary>
/// Represents the right-handed 4x4 floating point matrix, which can store translation, scale and rotation information.
/// </summary>
public struct Matrix : IEquatable<Matrix>
{
    #region Public Properties

    /// <summary>
    /// The backward vector formed from the third row M31, M32, M33 elements.
    /// </summary>
    public Vec3 Backward
    {
        get => new(M31, M32, M33);
        set
        {
            M31 = value.X;
            M32 = value.Y;
            M33 = value.Z;
        }
    }

    /// <summary>
    /// The down vector formed from the second row -M21, -M22, -M23 elements.
    /// </summary>
    public Vec3 Down
    {
        get => new(-M21, -M22, -M23);
        set
        {
            M21 = -value.X;
            M22 = -value.Y;
            M23 = -value.Z;
        }
    }

    /// <summary>
    /// The forward vector formed from the third row -M31, -M32, -M33 elements.
    /// </summary>
    public Vec3 Forward
    {
        get => new(-M31, -M32, -M33);
        set
        {
            M31 = -value.X;
            M32 = -value.Y;
            M33 = -value.Z;
        }
    }

    /// <summary>
    /// Returns the identity matrix.
    /// </summary>
    public static Matrix Identity => identity;

    /// <summary>
    /// The left vector formed from the first row -M11, -M12, -M13 elements.
    /// </summary>
    public Vec3 Left
    {
        get => new(-M11, -M12, -M13);
        set
        {
            M11 = -value.X;
            M12 = -value.Y;
            M13 = -value.Z;
        }
    }

    /// <summary>
    /// The right vector formed from the first row M11, M12, M13 elements.
    /// </summary>
    public Vec3 Right
    {
        get => new(M11, M12, M13);
        set
        {
            M11 = value.X;
            M12 = value.Y;
            M13 = value.Z;
        }
    }

    /// <summary>
    /// Position stored in this matrix.
    /// </summary>
    public Vec3 Translation
    {
        get => new(M41, M42, M43);
        set
        {
            M41 = value.X;
            M42 = value.Y;
            M43 = value.Z;
        }
    }

    /// <summary>
    /// The upper vector formed from the second row M21, M22, M23 elements.
    /// </summary>
    public Vec3 Up
    {
        get => new(M21, M22, M23);
        set
        {
            M21 = value.X;
            M22 = value.Y;
            M23 = value.Z;
        }
    }

    #endregion

    #region Internal Properties

    internal string DebugDisplayString =>
        string.Concat(
            "( ", M11.ToString(), " ",
            M12.ToString(), " ",
            M13.ToString(), " ",
            M14.ToString(), " ) \r\n",
            "( ", M21.ToString(), " ",
            M22.ToString(), " ",
            M23.ToString(), " ",
            M24.ToString(), " ) \r\n",
            "( ", M31.ToString(), " ",
            M32.ToString(), " ",
            M33.ToString(), " ",
            M34.ToString(), " ) \r\n",
            "( ", M41.ToString(), " ",
            M42.ToString(), " ",
            M43.ToString(), " ",
            M44.ToString(), " )"
        );

    #endregion

    #region Public Fields

    /// <summary>
    /// A first row and first column value.
    /// </summary>
    public float M11;

    /// <summary>
    /// A first row and second column value.
    /// </summary>
    public float M12;

    /// <summary>
    /// A first row and third column value.
    /// </summary>
    public float M13;

    /// <summary>
    /// A first row and fourth column value.
    /// </summary>
    public float M14;

    /// <summary>
    /// A second row and first column value.
    /// </summary>
    public float M21;

    /// <summary>
    /// A second row and second column value.
    /// </summary>
    public float M22;

    /// <summary>
    /// A second row and third column value.
    /// </summary>
    public float M23;

    /// <summary>
    /// A second row and fourth column value.
    /// </summary>
    public float M24;

    /// <summary>
    /// A third row and first column value.
    /// </summary>
    public float M31;

    /// <summary>
    /// A third row and second column value.
    /// </summary>
    public float M32;

    /// <summary>
    /// A third row and third column value.
    /// </summary>
    public float M33;

    /// <summary>
    /// A third row and fourth column value.
    /// </summary>
    public float M34;

    /// <summary>
    /// A fourth row and first column value.
    /// </summary>
    public float M41;

    /// <summary>
    /// A fourth row and second column value.
    /// </summary>
    public float M42;

    /// <summary>
    /// A fourth row and third column value.
    /// </summary>
    public float M43;

    /// <summary>
    /// A fourth row and fourth column value.
    /// </summary>
    public float M44;

    #endregion

    #region Private Static Variables

    private static Matrix identity = new(
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f
    );

    #endregion

    #region Public Constructors

    /// <summary>
    /// Constructs a matrix.
    /// </summary>
    /// <param name="m11">A first row and first column value.</param>
    /// <param name="m12">A first row and second column value.</param>
    /// <param name="m13">A first row and third column value.</param>
    /// <param name="m14">A first row and fourth column value.</param>
    /// <param name="m21">A second row and first column value.</param>
    /// <param name="m22">A second row and second column value.</param>
    /// <param name="m23">A second row and third column value.</param>
    /// <param name="m24">A second row and fourth column value.</param>
    /// <param name="m31">A third row and first column value.</param>
    /// <param name="m32">A third row and second column value.</param>
    /// <param name="m33">A third row and third column value.</param>
    /// <param name="m34">A third row and fourth column value.</param>
    /// <param name="m41">A fourth row and first column value.</param>
    /// <param name="m42">A fourth row and second column value.</param>
    /// <param name="m43">A fourth row and third column value.</param>
    /// <param name="m44">A fourth row and fourth column value.</param>
    public Matrix(
        float m11, float m12, float m13, float m14,
        float m21, float m22, float m23, float m24,
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44
    )
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M14 = m14;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M24 = m24;
        M31 = m31;
        M32 = m32;
        M33 = m33;
        M34 = m34;
        M41 = m41;
        M42 = m42;
        M43 = m43;
        M44 = m44;
    }

    #endregion

    #region Public Methods


    /// <summary>
    /// Returns a determinant of this <see cref="Matrix"/>.
    /// </summary>
    /// <returns>Determinant of this <see cref="Matrix"/></returns>
    /// <remarks>See more about determinant here - http://en.wikipedia.org/wiki/Determinant.
    /// </remarks>
    public float Determinant()
    {
        float num18 = (M33 * M44) - (M34 * M43);
        float num17 = (M32 * M44) - (M34 * M42);
        float num16 = (M32 * M43) - (M33 * M42);
        float num15 = (M31 * M44) - (M34 * M41);
        float num14 = (M31 * M43) - (M33 * M41);
        float num13 = (M31 * M42) - (M32 * M41);
        return (
            (
                (
                    (M11 * (((M22 * num18) - (M23 * num17)) + (M24 * num16))) -
                    (M12 * (((M21 * num18) - (M23 * num15)) + (M24 * num14)))
                ) + (M13 * (((M21 * num17) - (M22 * num15)) + (M24 * num13)))
            ) - (M14 * (((M21 * num16) - (M22 * num14)) + (M23 * num13)))
        );
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Matrix"/> without any tolerance.
    /// </summary>
    /// <param name="other">The <see cref="Matrix"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public bool Equals(Matrix other)
    {
        return M11 == other.M11 &&
               M12 == other.M12 &&
               M13 == other.M13 &&
               M14 == other.M14 &&
               M21 == other.M21 &&
               M22 == other.M22 &&
               M23 == other.M23 &&
               M24 == other.M24 &&
               M31 == other.M31 &&
               M32 == other.M32 &&
               M33 == other.M33 &&
               M34 == other.M34 &&
               M41 == other.M41 &&
               M42 == other.M42 &&
               M43 == other.M43 &&
               M44 == other.M44;
    }

    /// <summary>
    /// Compares whether current instance is equal to specified <see cref="Object"/> without any tolerance.
    /// </summary>
    /// <param name="obj">The <see cref="Object"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public override bool Equals(object? obj)
    {
        return (obj is Matrix matrix) && Equals(matrix);
    }

    /// <summary>
    /// Gets the hash code of this <see cref="Matrix"/>.
    /// </summary>
    /// <returns>Hash code of this <see cref="Matrix"/>.</returns>
    public override int GetHashCode()
    {
        return (
            M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
            M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
            M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
            M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode()
        );
    }

    /// <summary>
    /// Returns a <see cref="String"/> representation of this <see cref="Matrix"/> in the format:
    /// {M11:[<see cref="M11"/>] M12:[<see cref="M12"/>] M13:[<see cref="M13"/>] M14:[<see cref="M14"/>]}
    /// {M21:[<see cref="M21"/>] M12:[<see cref="M22"/>] M13:[<see cref="M23"/>] M14:[<see cref="M24"/>]}
    /// {M31:[<see cref="M31"/>] M32:[<see cref="M32"/>] M33:[<see cref="M33"/>] M34:[<see cref="M34"/>]}
    /// {M41:[<see cref="M41"/>] M42:[<see cref="M42"/>] M43:[<see cref="M43"/>] M44:[<see cref="M44"/>]}
    /// </summary>
    /// <returns>A <see cref="String"/> representation of this <see cref="Matrix"/>.</returns>
    public override string ToString()
    {
        return (
            "{M11:" + M11.ToString() +
            " M12:" + M12.ToString() +
            " M13:" + M13.ToString() +
            " M14:" + M14.ToString() +
            "} {M21:" + M21.ToString() +
            " M22:" + M22.ToString() +
            " M23:" + M23.ToString() +
            " M24:" + M24.ToString() +
            "} {M31:" + M31.ToString() +
            " M32:" + M32.ToString() +
            " M33:" + M33.ToString() +
            " M34:" + M34.ToString() +
            "} {M41:" + M41.ToString() +
            " M42:" + M42.ToString() +
            " M43:" + M43.ToString() +
            " M44:" + M44.ToString() + "}"
        );
    }

    #endregion

    #region Public Static Methods

    /// <summary>
    /// Creates a new <see cref="Matrix"/> which contains sum of two matrixes.
    /// </summary>
    /// <param name="matrix1">The first matrix to add.</param>
    /// <param name="matrix2">The second matrix to add.</param>
    /// <returns>The result of the matrix addition.</returns>
    public static Matrix Add(Matrix matrix1, Matrix matrix2)
    {
        matrix1.M11 += matrix2.M11;
        matrix1.M12 += matrix2.M12;
        matrix1.M13 += matrix2.M13;
        matrix1.M14 += matrix2.M14;
        matrix1.M21 += matrix2.M21;
        matrix1.M22 += matrix2.M22;
        matrix1.M23 += matrix2.M23;
        matrix1.M24 += matrix2.M24;
        matrix1.M31 += matrix2.M31;
        matrix1.M32 += matrix2.M32;
        matrix1.M33 += matrix2.M33;
        matrix1.M34 += matrix2.M34;
        matrix1.M41 += matrix2.M41;
        matrix1.M42 += matrix2.M42;
        matrix1.M43 += matrix2.M43;
        matrix1.M44 += matrix2.M44;
        return matrix1;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> which contains sum of two matrixes.
    /// </summary>
    /// <param name="matrix1">The first matrix to add.</param>
    /// <param name="matrix2">The second matrix to add.</param>
    /// <param name="result">The result of the matrix addition as an output parameter.</param>
    public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
        result.M11 = matrix1.M11 + matrix2.M11;
        result.M12 = matrix1.M12 + matrix2.M12;
        result.M13 = matrix1.M13 + matrix2.M13;
        result.M14 = matrix1.M14 + matrix2.M14;
        result.M21 = matrix1.M21 + matrix2.M21;
        result.M22 = matrix1.M22 + matrix2.M22;
        result.M23 = matrix1.M23 + matrix2.M23;
        result.M24 = matrix1.M24 + matrix2.M24;
        result.M31 = matrix1.M31 + matrix2.M31;
        result.M32 = matrix1.M32 + matrix2.M32;
        result.M33 = matrix1.M33 + matrix2.M33;
        result.M34 = matrix1.M34 + matrix2.M34;
        result.M41 = matrix1.M41 + matrix2.M41;
        result.M42 = matrix1.M42 + matrix2.M42;
        result.M43 = matrix1.M43 + matrix2.M43;
        result.M44 = matrix1.M44 + matrix2.M44;
    }


    /// <summary>
    /// Creates a new viewing <see cref="Matrix"/>.
    /// </summary>
    /// <param name="cameraPosition">Position of the camera.</param>
    /// <param name="cameraTarget">Lookup vector of the camera.</param>
    /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
    /// <returns>The viewing <see cref="Matrix"/>.</returns>
    public static Matrix CreateLookAt(
        Vec3 cameraPosition,
        Vec3 cameraTarget,
        Vec3 cameraUpVector
    )
    {
        CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out Matrix matrix);
        return matrix;
    }

    /// <summary>
    /// Creates a new viewing <see cref="Matrix"/>.
    /// </summary>
    /// <param name="cameraPosition">Position of the camera.</param>
    /// <param name="cameraTarget">Lookup vector of the camera.</param>
    /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
    /// <param name="result">The viewing <see cref="Matrix"/> as an output parameter.</param>
    public static void CreateLookAt(
        ref Vec3 cameraPosition,
        ref Vec3 cameraTarget,
        ref Vec3 cameraUpVector,
        out Matrix result
    )
    {
        Vec3 vectorA = Vec3.Normalize(cameraPosition - cameraTarget);
        Vec3 vectorB = Vec3.Normalize(Vec3.Cross(cameraUpVector, vectorA));
        Vec3 vectorC = Vec3.Cross(vectorA, vectorB);
        result.M11 = vectorB.X;
        result.M12 = vectorC.X;
        result.M13 = vectorA.X;
        result.M14 = 0f;
        result.M21 = vectorB.Y;
        result.M22 = vectorC.Y;
        result.M23 = vectorA.Y;
        result.M24 = 0f;
        result.M31 = vectorB.Z;
        result.M32 = vectorC.Z;
        result.M33 = vectorA.Z;
        result.M34 = 0f;
        result.M41 = -Vec3.Dot(vectorB, cameraPosition);
        result.M42 = -Vec3.Dot(vectorC, cameraPosition);
        result.M43 = -Vec3.Dot(vectorA, cameraPosition);
        result.M44 = 1f;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for orthographic view.
    /// </summary>
    /// <param name="width">Width of the viewing volume.</param>
    /// <param name="height">Height of the viewing volume.</param>
    /// <param name="zNearPlane">Depth of the near plane.</param>
    /// <param name="zFarPlane">Depth of the far plane.</param>
    /// <returns>The new projection <see cref="Matrix"/> for orthographic view.</returns>
    public static Matrix CreateOrthographic(
        float width,
        float height,
        float zNearPlane,
        float zFarPlane
    )
    {
        CreateOrthographic(width, height, zNearPlane, zFarPlane, out Matrix matrix);
        return matrix;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for orthographic view.
    /// </summary>
    /// <param name="width">Width of the viewing volume.</param>
    /// <param name="height">Height of the viewing volume.</param>
    /// <param name="zNearPlane">Depth of the near plane.</param>
    /// <param name="zFarPlane">Depth of the far plane.</param>
    /// <param name="result">The new projection <see cref="Matrix"/> for orthographic view as an output parameter.</param>
    public static void CreateOrthographic(
        float width,
        float height,
        float zNearPlane,
        float zFarPlane,
        out Matrix result
    )
    {
        result.M11 = 2f / width;
        result.M12 = result.M13 = result.M14 = 0f;
        result.M22 = 2f / height;
        result.M21 = result.M23 = result.M24 = 0f;
        result.M33 = 1f / (zNearPlane - zFarPlane);
        result.M31 = result.M32 = result.M34 = 0f;
        result.M41 = result.M42 = 0f;
        result.M43 = zNearPlane / (zNearPlane - zFarPlane);
        result.M44 = 1f;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for customized orthographic view.
    /// </summary>
    /// <param name="left">Lower x-value at the near plane.</param>
    /// <param name="right">Upper x-value at the near plane.</param>
    /// <param name="bottom">Lower y-coordinate at the near plane.</param>
    /// <param name="top">Upper y-value at the near plane.</param>
    /// <param name="zNearPlane">Depth of the near plane.</param>
    /// <param name="zFarPlane">Depth of the far plane.</param>
    /// <returns>The new projection <see cref="Matrix"/> for customized orthographic view.</returns>
    public static Matrix CreateOrthographicOffCenter(
        float left,
        float right,
        float bottom,
        float top,
        float zNearPlane,
        float zFarPlane
    )
    {
        CreateOrthographicOffCenter(
            left,
            right,
            bottom,
            top,
            zNearPlane,
            zFarPlane,
            out Matrix matrix
        );
        return matrix;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for customized orthographic view.
    /// </summary>
    /// <param name="left">Lower x-value at the near plane.</param>
    /// <param name="right">Upper x-value at the near plane.</param>
    /// <param name="bottom">Lower y-coordinate at the near plane.</param>
    /// <param name="top">Upper y-value at the near plane.</param>
    /// <param name="zNearPlane">Depth of the near plane.</param>
    /// <param name="zFarPlane">Depth of the far plane.</param>
    /// <param name="result">The new projection <see cref="Matrix"/> for customized orthographic view as an output parameter.</param>
    public static void CreateOrthographicOffCenter(
        float left,
        float right,
        float bottom,
        float top,
        float zNearPlane,
        float zFarPlane,
        out Matrix result
    )
    {
        result.M11 = (float)(2.0 / (right - (double)left));
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = (float)(2.0 / (top - (double)bottom));
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = (float)(1.0 / (zNearPlane - (double)zFarPlane));
        result.M34 = 0.0f;
        result.M41 = (float)(
            (left + (double)right) /
            (left - (double)right)
        );
        result.M42 = (float)(
            (top + (double)bottom) /
            (bottom - (double)top)
        );
        result.M43 = (float)(
            zNearPlane /
            (zNearPlane - (double)zFarPlane)
        );
        result.M44 = 1.0f;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for perspective view.
    /// </summary>
    /// <param name="width">Width of the viewing volume.</param>
    /// <param name="height">Height of the viewing volume.</param>
    /// <param name="nearPlaneDistance">Distance to the near plane.</param>
    /// <param name="farPlaneDistance">Distance to the far plane.</param>
    /// <returns>The new projection <see cref="Matrix"/> for perspective view.</returns>
    public static Matrix CreatePerspective(
        float width,
        float height,
        float nearPlaneDistance,
        float farPlaneDistance
    )
    {
        CreatePerspective(width, height, nearPlaneDistance, farPlaneDistance, out Matrix matrix);
        return matrix;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for perspective view.
    /// </summary>
    /// <param name="width">Width of the viewing volume.</param>
    /// <param name="height">Height of the viewing volume.</param>
    /// <param name="nearPlaneDistance">Distance to the near plane.</param>
    /// <param name="farPlaneDistance">Distance to the far plane.</param>
    /// <param name="result">The new projection <see cref="Matrix"/> for perspective view as an output parameter.</param>
    public static void CreatePerspective(
        float width,
        float height,
        float nearPlaneDistance,
        float farPlaneDistance,
        out Matrix result
    )
    {
        if (nearPlaneDistance <= 0f)
        {
            throw new ArgumentException("nearPlaneDistance <= 0");
        }
        if (farPlaneDistance <= 0f)
        {
            throw new ArgumentException("farPlaneDistance <= 0");
        }
        if (nearPlaneDistance >= farPlaneDistance)
        {
            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
        }
        result.M11 = (2f * nearPlaneDistance) / width;
        result.M12 = result.M13 = result.M14 = 0f;
        result.M22 = (2f * nearPlaneDistance) / height;
        result.M21 = result.M23 = result.M24 = 0f;
        result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
        result.M31 = result.M32 = 0f;
        result.M34 = -1f;
        result.M41 = result.M42 = result.M44 = 0f;
        result.M43 = (
            (nearPlaneDistance * farPlaneDistance) /
            (nearPlaneDistance - farPlaneDistance)
        );
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for perspective view with field of view.
    /// </summary>
    /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
    /// <param name="aspectRatio">Width divided by height of the viewing volume.</param>
    /// <param name="nearPlaneDistance">Distance to the near plane.</param>
    /// <param name="farPlaneDistance">Distance to the far plane.</param>
    /// <returns>The new projection <see cref="Matrix"/> for perspective view with FOV.</returns>
    public static Matrix CreatePerspectiveFieldOfView(
        float fieldOfView,
        float aspectRatio,
        float nearPlaneDistance,
        float farPlaneDistance
    )
    {
        CreatePerspectiveFieldOfView(
            fieldOfView,
            aspectRatio,
            nearPlaneDistance,
            farPlaneDistance,
            out Matrix result
        );
        return result;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for perspective view with field of view.
    /// </summary>
    /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
    /// <param name="aspectRatio">Width divided by height of the viewing volume.</param>
    /// <param name="nearPlaneDistance">Distance of the near plane.</param>
    /// <param name="farPlaneDistance">Distance of the far plane.</param>
    /// <param name="result">The new projection <see cref="Matrix"/> for perspective view with FOV as an output parameter.</param>
    public static void CreatePerspectiveFieldOfView(
        float fieldOfView,
        float aspectRatio,
        float nearPlaneDistance,
        float farPlaneDistance,
        out Matrix result
    )
    {
        if ((fieldOfView <= 0f) || (fieldOfView >= 3.141593f))
        {
            throw new ArgumentException("fieldOfView <= 0 or >= PI");
        }
        if (nearPlaneDistance <= 0f)
        {
            throw new ArgumentException("nearPlaneDistance <= 0");
        }
        if (farPlaneDistance <= 0f)
        {
            throw new ArgumentException("farPlaneDistance <= 0");
        }
        if (nearPlaneDistance >= farPlaneDistance)
        {
            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
        }
        float num = 1f / ((float)Math.Tan(fieldOfView * 0.5f));
        float num9 = num / aspectRatio;
        result.M11 = num9;
        result.M12 = result.M13 = result.M14 = 0;
        result.M22 = num;
        result.M21 = result.M23 = result.M24 = 0;
        result.M31 = result.M32 = 0f;
        result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
        result.M34 = -1;
        result.M41 = result.M42 = result.M44 = 0;
        result.M43 = (
            (nearPlaneDistance * farPlaneDistance) /
            (nearPlaneDistance - farPlaneDistance)
        );
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for customized perspective view.
    /// </summary>
    /// <param name="left">Lower x-value at the near plane.</param>
    /// <param name="right">Upper x-value at the near plane.</param>
    /// <param name="bottom">Lower y-coordinate at the near plane.</param>
    /// <param name="top">Upper y-value at the near plane.</param>
    /// <param name="nearPlaneDistance">Distance to the near plane.</param>
    /// <param name="farPlaneDistance">Distance to the far plane.</param>
    /// <returns>The new <see cref="Matrix"/> for customized perspective view.</returns>
    public static Matrix CreatePerspectiveOffCenter(
        float left,
        float right,
        float bottom,
        float top,
        float nearPlaneDistance,
        float farPlaneDistance
    )
    {
        CreatePerspectiveOffCenter(
            left,
            right,
            bottom,
            top,
            nearPlaneDistance,
            farPlaneDistance,
            out Matrix result
        );
        return result;
    }

    /// <summary>
    /// Creates a new projection <see cref="Matrix"/> for customized perspective view.
    /// </summary>
    /// <param name="left">Lower x-value at the near plane.</param>
    /// <param name="right">Upper x-value at the near plane.</param>
    /// <param name="bottom">Lower y-coordinate at the near plane.</param>
    /// <param name="top">Upper y-value at the near plane.</param>
    /// <param name="nearPlaneDistance">Distance to the near plane.</param>
    /// <param name="farPlaneDistance">Distance to the far plane.</param>
    /// <param name="result">The new <see cref="Matrix"/> for customized perspective view as an output parameter.</param>
    public static void CreatePerspectiveOffCenter(
        float left,
        float right,
        float bottom,
        float top,
        float nearPlaneDistance,
        float farPlaneDistance,
        out Matrix result
    )
    {
        if (nearPlaneDistance <= 0f)
        {
            throw new ArgumentException("nearPlaneDistance <= 0");
        }
        if (farPlaneDistance <= 0f)
        {
            throw new ArgumentException("farPlaneDistance <= 0");
        }
        if (nearPlaneDistance >= farPlaneDistance)
        {
            throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
        }
        result.M11 = 2f * nearPlaneDistance / (right - left);
        result.M12 = result.M13 = result.M14 = 0;
        result.M22 = 2f * nearPlaneDistance / (top - bottom);
        result.M21 = result.M23 = result.M24 = 0;
        result.M31 = (left + right) / (right - left);
        result.M32 = (top + bottom) / (top - bottom);
        result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
        result.M34 = -1;
        result.M43 = (
            (nearPlaneDistance * farPlaneDistance) /
            (nearPlaneDistance - farPlaneDistance)
        );
        result.M41 = result.M42 = result.M44 = 0;
    }

    /// <summary>
    /// Creates a new rotation <see cref="Matrix"/> around X axis.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <returns>The rotation <see cref="Matrix"/> around X axis.</returns>
    public static Matrix CreateRotationX(float radians)
    {
        CreateRotationX(radians, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new rotation <see cref="Matrix"/> around X axis.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <param name="result">The rotation <see cref="Matrix"/> around X axis as an output parameter.</param>
    public static void CreateRotationX(float radians, out Matrix result)
    {
        result = Matrix.Identity;

        float val1 = (float)Math.Cos(radians);
        float val2 = (float)Math.Sin(radians);

        result.M22 = val1;
        result.M23 = val2;
        result.M32 = -val2;
        result.M33 = val1;
    }

    /// <summary>
    /// Creates a new rotation <see cref="Matrix"/> around Y axis.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <returns>The rotation <see cref="Matrix"/> around Y axis.</returns>
    public static Matrix CreateRotationY(float radians)
    {
        CreateRotationY(radians, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new rotation <see cref="Matrix"/> around Y axis.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <param name="result">The rotation <see cref="Matrix"/> around Y axis as an output parameter.</param>
    public static void CreateRotationY(float radians, out Matrix result)
    {
        result = Matrix.Identity;

        float val1 = (float)Math.Cos(radians);
        float val2 = (float)Math.Sin(radians);

        result.M11 = val1;
        result.M13 = -val2;
        result.M31 = val2;
        result.M33 = val1;
    }

    /// <summary>
    /// Creates a new rotation <see cref="Matrix"/> around Z axis.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <returns>The rotation <see cref="Matrix"/> around Z axis.</returns>
    public static Matrix CreateRotationZ(float radians)
    {
        CreateRotationZ(radians, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new rotation <see cref="Matrix"/> around Z axis.
    /// </summary>
    /// <param name="radians">Angle in radians.</param>
    /// <param name="result">The rotation <see cref="Matrix"/> around Z axis as an output parameter.</param>
    public static void CreateRotationZ(float radians, out Matrix result)
    {
        result = Matrix.Identity;

        float val1 = (float)Math.Cos(radians);
        float val2 = (float)Math.Sin(radians);

        result.M11 = val1;
        result.M12 = val2;
        result.M21 = -val2;
        result.M22 = val1;
    }

    /// <summary>
    /// Creates a new scaling <see cref="Matrix"/>.
    /// </summary>
    /// <param name="scale">Scale value for all three axises.</param>
    /// <returns>The scaling <see cref="Matrix"/>.</returns>
    public static Matrix CreateScale(float scale)
    {
        CreateScale(scale, scale, scale, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new scaling <see cref="Matrix"/>.
    /// </summary>
    /// <param name="scale">Scale value for all three axises.</param>
    /// <param name="result">The scaling <see cref="Matrix"/> as an output parameter.</param>
    public static void CreateScale(float scale, out Matrix result)
    {
        CreateScale(scale, scale, scale, out result);
    }

    /// <summary>
    /// Creates a new scaling <see cref="Matrix"/>.
    /// </summary>
    /// <param name="xScale">Scale value for X axis.</param>
    /// <param name="yScale">Scale value for Y axis.</param>
    /// <param name="zScale">Scale value for Z axis.</param>
    /// <returns>The scaling <see cref="Matrix"/>.</returns>
    public static Matrix CreateScale(float xScale, float yScale, float zScale)
    {
        CreateScale(xScale, yScale, zScale, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new scaling <see cref="Matrix"/>.
    /// </summary>
    /// <param name="xScale">Scale value for X axis.</param>
    /// <param name="yScale">Scale value for Y axis.</param>
    /// <param name="zScale">Scale value for Z axis.</param>
    /// <param name="result">The scaling <see cref="Matrix"/> as an output parameter.</param>
    public static void CreateScale(
        float xScale,
        float yScale,
        float zScale,
        out Matrix result
    )
    {
        result.M11 = xScale;
        result.M12 = 0;
        result.M13 = 0;
        result.M14 = 0;
        result.M21 = 0;
        result.M22 = yScale;
        result.M23 = 0;
        result.M24 = 0;
        result.M31 = 0;
        result.M32 = 0;
        result.M33 = zScale;
        result.M34 = 0;
        result.M41 = 0;
        result.M42 = 0;
        result.M43 = 0;
        result.M44 = 1;
    }

    /// <summary>
    /// Creates a new scaling <see cref="Matrix"/>.
    /// </summary>
    /// <param name="scales"><see cref="Vec3"/> representing x,y and z scale values.</param>
    /// <returns>The scaling <see cref="Matrix"/>.</returns>
    public static Matrix CreateScale(Vec3 scales)
    {
        CreateScale(ref scales, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new scaling <see cref="Matrix"/>.
    /// </summary>
    /// <param name="scales"><see cref="Vec3"/> representing x,y and z scale values.</param>
    /// <param name="result">The scaling <see cref="Matrix"/> as an output parameter.</param>
    public static void CreateScale(ref Vec3 scales, out Matrix result)
    {
        result.M11 = scales.X;
        result.M12 = 0;
        result.M13 = 0;
        result.M14 = 0;
        result.M21 = 0;
        result.M22 = scales.Y;
        result.M23 = 0;
        result.M24 = 0;
        result.M31 = 0;
        result.M32 = 0;
        result.M33 = scales.Z;
        result.M34 = 0;
        result.M41 = 0;
        result.M42 = 0;
        result.M43 = 0;
        result.M44 = 1;
    }

    /// <summary>
    /// Creates a new translation <see cref="Matrix"/>.
    /// </summary>
    /// <param name="xPosition">X coordinate of translation.</param>
    /// <param name="yPosition">Y coordinate of translation.</param>
    /// <param name="zPosition">Z coordinate of translation.</param>
    /// <returns>The translation <see cref="Matrix"/>.</returns>
    public static Matrix CreateTranslation(
        float xPosition,
        float yPosition,
        float zPosition
    )
    {
        CreateTranslation(xPosition, yPosition, zPosition, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new translation <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">X,Y and Z coordinates of translation.</param>
    /// <param name="result">The translation <see cref="Matrix"/> as an output parameter.</param>
    public static void CreateTranslation(ref Vec3 position, out Matrix result)
    {
        result.M11 = 1;
        result.M12 = 0;
        result.M13 = 0;
        result.M14 = 0;
        result.M21 = 0;
        result.M22 = 1;
        result.M23 = 0;
        result.M24 = 0;
        result.M31 = 0;
        result.M32 = 0;
        result.M33 = 1;
        result.M34 = 0;
        result.M41 = position.X;
        result.M42 = position.Y;
        result.M43 = position.Z;
        result.M44 = 1;
    }

    /// <summary>
    /// Creates a new translation <see cref="Matrix"/>.
    /// </summary>
    /// <param name="position">X,Y and Z coordinates of translation.</param>
    /// <returns>The translation <see cref="Matrix"/>.</returns>
    public static Matrix CreateTranslation(Vec3 position)
    {
        CreateTranslation(ref position, out Matrix result);
        return result;
    }

    /// <summary>
    /// Creates a new translation <see cref="Matrix"/>.
    /// </summary>
    /// <param name="xPosition">X coordinate of translation.</param>
    /// <param name="yPosition">Y coordinate of translation.</param>
    /// <param name="zPosition">Z coordinate of translation.</param>
    /// <param name="result">The translation <see cref="Matrix"/> as an output parameter.</param>
    public static void CreateTranslation(
        float xPosition,
        float yPosition,
        float zPosition,
        out Matrix result
    )
    {
        result.M11 = 1;
        result.M12 = 0;
        result.M13 = 0;
        result.M14 = 0;
        result.M21 = 0;
        result.M22 = 1;
        result.M23 = 0;
        result.M24 = 0;
        result.M31 = 0;
        result.M32 = 0;
        result.M33 = 1;
        result.M34 = 0;
        result.M41 = xPosition;
        result.M42 = yPosition;
        result.M43 = zPosition;
        result.M44 = 1;
    }



    /// <summary>
    /// Divides the elements of a <see cref="Matrix"/> by the elements of another matrix.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="matrix2">Divisor <see cref="Matrix"/>.</param>
    /// <returns>The result of dividing the matrix.</returns>
    public static Matrix Divide(Matrix matrix1, Matrix matrix2)
    {
        matrix1.M11 /= matrix2.M11;
        matrix1.M12 /= matrix2.M12;
        matrix1.M13 /= matrix2.M13;
        matrix1.M14 /= matrix2.M14;
        matrix1.M21 /= matrix2.M21;
        matrix1.M22 /= matrix2.M22;
        matrix1.M23 /= matrix2.M23;
        matrix1.M24 /= matrix2.M24;
        matrix1.M31 /= matrix2.M31;
        matrix1.M32 /= matrix2.M32;
        matrix1.M33 /= matrix2.M33;
        matrix1.M34 /= matrix2.M34;
        matrix1.M41 /= matrix2.M41;
        matrix1.M42 /= matrix2.M42;
        matrix1.M43 /= matrix2.M43;
        matrix1.M44 /= matrix2.M44;
        return matrix1;
    }

    /// <summary>
    /// Divides the elements of a <see cref="Matrix"/> by the elements of another matrix.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="matrix2">Divisor <see cref="Matrix"/>.</param>
    /// <param name="result">The result of dividing the matrix as an output parameter.</param>
    public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
        result.M11 = matrix1.M11 / matrix2.M11;
        result.M12 = matrix1.M12 / matrix2.M12;
        result.M13 = matrix1.M13 / matrix2.M13;
        result.M14 = matrix1.M14 / matrix2.M14;
        result.M21 = matrix1.M21 / matrix2.M21;
        result.M22 = matrix1.M22 / matrix2.M22;
        result.M23 = matrix1.M23 / matrix2.M23;
        result.M24 = matrix1.M24 / matrix2.M24;
        result.M31 = matrix1.M31 / matrix2.M31;
        result.M32 = matrix1.M32 / matrix2.M32;
        result.M33 = matrix1.M33 / matrix2.M33;
        result.M34 = matrix1.M34 / matrix2.M34;
        result.M41 = matrix1.M41 / matrix2.M41;
        result.M42 = matrix1.M42 / matrix2.M42;
        result.M43 = matrix1.M43 / matrix2.M43;
        result.M44 = matrix1.M44 / matrix2.M44;
    }

    /// <summary>
    /// Divides the elements of a <see cref="Matrix"/> by a scalar.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <returns>The result of dividing a matrix by a scalar.</returns>
    public static Matrix Divide(Matrix matrix1, float divider)
    {
        float num = 1f / divider;
        matrix1.M11 *= num;
        matrix1.M12 *= num;
        matrix1.M13 *= num;
        matrix1.M14 *= num;
        matrix1.M21 *= num;
        matrix1.M22 *= num;
        matrix1.M23 *= num;
        matrix1.M24 *= num;
        matrix1.M31 *= num;
        matrix1.M32 *= num;
        matrix1.M33 *= num;
        matrix1.M34 *= num;
        matrix1.M41 *= num;
        matrix1.M42 *= num;
        matrix1.M43 *= num;
        matrix1.M44 *= num;
        return matrix1;
    }

    /// <summary>
    /// Divides the elements of a <see cref="Matrix"/> by a scalar.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="divider">Divisor scalar.</param>
    /// <param name="result">The result of dividing a matrix by a scalar as an output parameter.</param>
    public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
    {
        float num = 1f / divider;
        result.M11 = matrix1.M11 * num;
        result.M12 = matrix1.M12 * num;
        result.M13 = matrix1.M13 * num;
        result.M14 = matrix1.M14 * num;
        result.M21 = matrix1.M21 * num;
        result.M22 = matrix1.M22 * num;
        result.M23 = matrix1.M23 * num;
        result.M24 = matrix1.M24 * num;
        result.M31 = matrix1.M31 * num;
        result.M32 = matrix1.M32 * num;
        result.M33 = matrix1.M33 * num;
        result.M34 = matrix1.M34 * num;
        result.M41 = matrix1.M41 * num;
        result.M42 = matrix1.M42 * num;
        result.M43 = matrix1.M43 * num;
        result.M44 = matrix1.M44 * num;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> which contains inversion of the specified matrix.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/>.</param>
    /// <returns>The inverted matrix.</returns>
    public static Matrix Invert(Matrix matrix)
    {
        Invert(ref matrix, out matrix);
        return matrix;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> which contains inversion of the specified matrix.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/>.</param>
    /// <param name="result">The inverted matrix as output parameter.</param>
    public static void Invert(ref Matrix matrix, out Matrix result)
    {
        /*
         * Use Laplace expansion theorem to calculate the inverse of a 4x4 matrix.
         *
         * 1. Calculate the 2x2 determinants needed the 4x4 determinant based on
         *    the 2x2 determinants.
         * 3. Create the adjugate matrix, which satisfies: A * adj(A) = det(A) * I.
         * 4. Divide adjugate matrix with the determinant to find the inverse.
         */

        float num1 = matrix.M11;
        float num2 = matrix.M12;
        float num3 = matrix.M13;
        float num4 = matrix.M14;
        float num5 = matrix.M21;
        float num6 = matrix.M22;
        float num7 = matrix.M23;
        float num8 = matrix.M24;
        float num9 = matrix.M31;
        float num10 = matrix.M32;
        float num11 = matrix.M33;
        float num12 = matrix.M34;
        float num13 = matrix.M41;
        float num14 = matrix.M42;
        float num15 = matrix.M43;
        float num16 = matrix.M44;
        float num17 = (float)(
            num11 * (double)num16 -
            num12 * (double)num15
        );
        float num18 = (float)(
            num10 * (double)num16 -
            num12 * (double)num14
        );
        float num19 = (float)(
            num10 * (double)num15 -
            num11 * (double)num14
        );
        float num20 = (float)(
            num9 * (double)num16 -
            num12 * (double)num13
        );
        float num21 = (float)(
            num9 * (double)num15 -
            num11 * (double)num13
        );
        float num22 = (float)(
            num9 * (double)num14 -
            num10 * (double)num13
        );
        float num23 = (float)(
            num6 * (double)num17 -
            num7 * (double)num18 +
            num8 * (double)num19
        );
        float num24 = (float)-(
            num5 * (double)num17 -
            num7 * (double)num20 +
            num8 * (double)num21
        );
        float num25 = (float)(
            num5 * (double)num18 -
            num6 * (double)num20 +
            num8 * (double)num22
        );
        float num26 = (float)-(
            num5 * (double)num19 -
            num6 * (double)num21 +
            num7 * (double)num22
        );
        float num27 = (float)(
            1.0 / (
                num1 * (double)num23 +
                num2 * (double)num24 +
                num3 * (double)num25 +
                num4 * (double)num26
            )
        );

        result.M11 = num23 * num27;
        result.M21 = num24 * num27;
        result.M31 = num25 * num27;
        result.M41 = num26 * num27;
        result.M12 = (float)(
            -(
                num2 * (double)num17 -
                num3 * (double)num18 +
                num4 * (double)num19
            ) * num27
        );
        result.M22 = (float)(
            (
                num1 * (double)num17 -
                num3 * (double)num20 +
                num4 * (double)num21
            ) * num27
        );
        result.M32 = (float)(
            -(
                num1 * (double)num18 -
                num2 * (double)num20 +
                num4 * (double)num22
            ) * num27
        );
        result.M42 = (float)(
            (
                num1 * (double)num19 -
                num2 * (double)num21 +
                num3 * (double)num22
            ) * num27
        );
        float num28 = (float)(
            num7 * (double)num16 -
            num8 * (double)num15
        );
        float num29 = (float)(
            num6 * (double)num16 -
            num8 * (double)num14
        );
        float num30 = (float)(
            num6 * (double)num15 -
            num7 * (double)num14
        );
        float num31 = (float)(
            num5 * (double)num16 -
            num8 * (double)num13
        );
        float num32 = (float)(
            num5 * (double)num15 -
            num7 * (double)num13
        );
        float num33 = (float)(
            num5 * (double)num14 -
            num6 * (double)num13
        );
        result.M13 = (float)(
            (
                num2 * (double)num28 -
                num3 * (double)num29 +
                num4 * (double)num30
            ) * num27
        );
        result.M23 = (float)(
            -(
                num1 * (double)num28 -
                num3 * (double)num31 +
                num4 * (double)num32
            ) * num27
        );
        result.M33 = (float)(
            (
                num1 * (double)num29 -
                num2 * (double)num31 +
                num4 * (double)num33
            ) * num27
        );
        result.M43 = (float)(
            -(
                num1 * (double)num30 -
                num2 * (double)num32 +
                num3 * (double)num33
            ) * num27
        );
        float num34 = (float)(
            num7 * (double)num12 -
            num8 * (double)num11
        );
        float num35 = (float)(
            num6 * (double)num12 -
            num8 * (double)num10
        );
        float num36 = (float)(
            num6 * (double)num11 -
            num7 * (double)num10
        );
        float num37 = (float)(
            num5 * (double)num12 -
            num8 * (double)num9);
        float num38 = (float)(
            num5 * (double)num11 -
            num7 * (double)num9
        );
        float num39 = (float)(
            num5 * (double)num10 -
            num6 * (double)num9
        );
        result.M14 = (float)(
            -(
                num2 * (double)num34 -
                num3 * (double)num35 +
                num4 * (double)num36
            ) * num27
        );
        result.M24 = (float)(
            (
                num1 * (double)num34 -
                num3 * (double)num37 +
                num4 * (double)num38
            ) * num27
        );
        result.M34 = (float)(
            -(
                num1 * (double)num35 -
                num2 * (double)num37 +
                num4 * (double)num39
            ) * num27
        );
        result.M44 = (float)(
            (
                num1 * (double)num36 -
                num2 * (double)num38 +
                num3 * (double)num39
            ) * num27
        );
    }



    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains a multiplication of two matrix.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/>.</param>
    /// <returns>Result of the matrix multiplication.</returns>
    public static Matrix Multiply(
        Matrix matrix1,
        Matrix matrix2
    )
    {
        float m11 = (
            (matrix1.M11 * matrix2.M11) +
            (matrix1.M12 * matrix2.M21) +
            (matrix1.M13 * matrix2.M31) +
            (matrix1.M14 * matrix2.M41)
        );
        float m12 = (
            (matrix1.M11 * matrix2.M12) +
            (matrix1.M12 * matrix2.M22) +
            (matrix1.M13 * matrix2.M32) +
            (matrix1.M14 * matrix2.M42)
        );
        float m13 = (
            (matrix1.M11 * matrix2.M13) +
            (matrix1.M12 * matrix2.M23) +
            (matrix1.M13 * matrix2.M33) +
            (matrix1.M14 * matrix2.M43)
        );
        float m14 = (
            (matrix1.M11 * matrix2.M14) +
            (matrix1.M12 * matrix2.M24) +
            (matrix1.M13 * matrix2.M34) +
            (matrix1.M14 * matrix2.M44)
        );
        float m21 = (
            (matrix1.M21 * matrix2.M11) +
            (matrix1.M22 * matrix2.M21) +
            (matrix1.M23 * matrix2.M31) +
            (matrix1.M24 * matrix2.M41)
        );
        float m22 = (
            (matrix1.M21 * matrix2.M12) +
            (matrix1.M22 * matrix2.M22) +
            (matrix1.M23 * matrix2.M32) +
            (matrix1.M24 * matrix2.M42)
        );
        float m23 = (
            (matrix1.M21 * matrix2.M13) +
            (matrix1.M22 * matrix2.M23) +
            (matrix1.M23 * matrix2.M33) +
            (matrix1.M24 * matrix2.M43)
        );
        float m24 = (
            (matrix1.M21 * matrix2.M14) +
            (matrix1.M22 * matrix2.M24) +
            (matrix1.M23 * matrix2.M34) +
            (matrix1.M24 * matrix2.M44)
        );
        float m31 = (
            (matrix1.M31 * matrix2.M11) +
            (matrix1.M32 * matrix2.M21) +
            (matrix1.M33 * matrix2.M31) +
            (matrix1.M34 * matrix2.M41)
        );
        float m32 = (
            (matrix1.M31 * matrix2.M12) +
            (matrix1.M32 * matrix2.M22) +
            (matrix1.M33 * matrix2.M32) +
            (matrix1.M34 * matrix2.M42)
        );
        float m33 = (
            (matrix1.M31 * matrix2.M13) +
            (matrix1.M32 * matrix2.M23) +
            (matrix1.M33 * matrix2.M33) +
            (matrix1.M34 * matrix2.M43)
        );
        float m34 = (
            (matrix1.M31 * matrix2.M14) +
            (matrix1.M32 * matrix2.M24) +
            (matrix1.M33 * matrix2.M34) +
            (matrix1.M34 * matrix2.M44)
        );
        float m41 = (
            (matrix1.M41 * matrix2.M11) +
            (matrix1.M42 * matrix2.M21) +
            (matrix1.M43 * matrix2.M31) +
            (matrix1.M44 * matrix2.M41)
        );
        float m42 = (
            (matrix1.M41 * matrix2.M12) +
            (matrix1.M42 * matrix2.M22) +
            (matrix1.M43 * matrix2.M32) +
            (matrix1.M44 * matrix2.M42)
        );
        float m43 = (
            (matrix1.M41 * matrix2.M13) +
            (matrix1.M42 * matrix2.M23) +
            (matrix1.M43 * matrix2.M33) +
            (matrix1.M44 * matrix2.M43)
        );
        float m44 = (
            (matrix1.M41 * matrix2.M14) +
            (matrix1.M42 * matrix2.M24) +
            (matrix1.M43 * matrix2.M34) +
            (matrix1.M44 * matrix2.M44)
        );
        matrix1.M11 = m11;
        matrix1.M12 = m12;
        matrix1.M13 = m13;
        matrix1.M14 = m14;
        matrix1.M21 = m21;
        matrix1.M22 = m22;
        matrix1.M23 = m23;
        matrix1.M24 = m24;
        matrix1.M31 = m31;
        matrix1.M32 = m32;
        matrix1.M33 = m33;
        matrix1.M34 = m34;
        matrix1.M41 = m41;
        matrix1.M42 = m42;
        matrix1.M43 = m43;
        matrix1.M44 = m44;
        return matrix1;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains a multiplication of two matrix.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/>.</param>
    /// <param name="result">Result of the matrix multiplication as an output parameter.</param>
    public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
        float m11 = (
            (matrix1.M11 * matrix2.M11) +
            (matrix1.M12 * matrix2.M21) +
            (matrix1.M13 * matrix2.M31) +
            (matrix1.M14 * matrix2.M41)
        );
        float m12 = (
            (matrix1.M11 * matrix2.M12) +
            (matrix1.M12 * matrix2.M22) +
            (matrix1.M13 * matrix2.M32) +
            (matrix1.M14 * matrix2.M42)
        );
        float m13 = (
            (matrix1.M11 * matrix2.M13) +
            (matrix1.M12 * matrix2.M23) +
            (matrix1.M13 * matrix2.M33) +
            (matrix1.M14 * matrix2.M43)
        );
        float m14 = (
            (matrix1.M11 * matrix2.M14) +
            (matrix1.M12 * matrix2.M24) +
            (matrix1.M13 * matrix2.M34) +
            (matrix1.M14 * matrix2.M44)
        );
        float m21 = (
            (matrix1.M21 * matrix2.M11) +
            (matrix1.M22 * matrix2.M21) +
            (matrix1.M23 * matrix2.M31) +
            (matrix1.M24 * matrix2.M41)
        );
        float m22 = (
            (matrix1.M21 * matrix2.M12) +
            (matrix1.M22 * matrix2.M22) +
            (matrix1.M23 * matrix2.M32) +
            (matrix1.M24 * matrix2.M42)
        );
        float m23 = (
            (matrix1.M21 * matrix2.M13) +
            (matrix1.M22 * matrix2.M23) +
            (matrix1.M23 * matrix2.M33) +
            (matrix1.M24 * matrix2.M43)
            );
        float m24 = (
            (matrix1.M21 * matrix2.M14) +
            (matrix1.M22 * matrix2.M24) +
            (matrix1.M23 * matrix2.M34) +
            (matrix1.M24 * matrix2.M44)
        );
        float m31 = (
            (matrix1.M31 * matrix2.M11) +
            (matrix1.M32 * matrix2.M21) +
            (matrix1.M33 * matrix2.M31) +
            (matrix1.M34 * matrix2.M41)
        );
        float m32 = (
            (matrix1.M31 * matrix2.M12) +
            (matrix1.M32 * matrix2.M22) +
            (matrix1.M33 * matrix2.M32) +
            (matrix1.M34 * matrix2.M42)
        );
        float m33 = (
            (matrix1.M31 * matrix2.M13) +
            (matrix1.M32 * matrix2.M23) +
            (matrix1.M33 * matrix2.M33) +
            (matrix1.M34 * matrix2.M43)
        );
        float m34 = (
            (matrix1.M31 * matrix2.M14) +
            (matrix1.M32 * matrix2.M24) +
            (matrix1.M33 * matrix2.M34) +
            (matrix1.M34 * matrix2.M44)
        );
        float m41 = (
            (matrix1.M41 * matrix2.M11) +
            (matrix1.M42 * matrix2.M21) +
            (matrix1.M43 * matrix2.M31) +
            (matrix1.M44 * matrix2.M41)
        );
        float m42 = (
            (matrix1.M41 * matrix2.M12) +
            (matrix1.M42 * matrix2.M22) +
            (matrix1.M43 * matrix2.M32) +
            (matrix1.M44 * matrix2.M42)
        );
        float m43 = (
            (matrix1.M41 * matrix2.M13) +
            (matrix1.M42 * matrix2.M23) +
            (matrix1.M43 * matrix2.M33) +
            (matrix1.M44 * matrix2.M43)
        );
        float m44 = (
            (matrix1.M41 * matrix2.M14) +
            (matrix1.M42 * matrix2.M24) +
            (matrix1.M43 * matrix2.M34) +
            (matrix1.M44 * matrix2.M44)
        );
        result.M11 = m11;
        result.M12 = m12;
        result.M13 = m13;
        result.M14 = m14;
        result.M21 = m21;
        result.M22 = m22;
        result.M23 = m23;
        result.M24 = m24;
        result.M31 = m31;
        result.M32 = m32;
        result.M33 = m33;
        result.M34 = m34;
        result.M41 = m41;
        result.M42 = m42;
        result.M43 = m43;
        result.M44 = m44;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains a multiplication of <see cref="Matrix"/> and a scalar.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <returns>Result of the matrix multiplication with a scalar.</returns>
    public static Matrix Multiply(Matrix matrix1, float scaleFactor)
    {
        matrix1.M11 *= scaleFactor;
        matrix1.M12 *= scaleFactor;
        matrix1.M13 *= scaleFactor;
        matrix1.M14 *= scaleFactor;
        matrix1.M21 *= scaleFactor;
        matrix1.M22 *= scaleFactor;
        matrix1.M23 *= scaleFactor;
        matrix1.M24 *= scaleFactor;
        matrix1.M31 *= scaleFactor;
        matrix1.M32 *= scaleFactor;
        matrix1.M33 *= scaleFactor;
        matrix1.M34 *= scaleFactor;
        matrix1.M41 *= scaleFactor;
        matrix1.M42 *= scaleFactor;
        matrix1.M43 *= scaleFactor;
        matrix1.M44 *= scaleFactor;
        return matrix1;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains a multiplication of <see cref="Matrix"/> and a scalar.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/>.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="result">Result of the matrix multiplication with a scalar as an output parameter.</param>
    public static void Multiply(ref Matrix matrix1, float scaleFactor, out Matrix result)
    {
        result.M11 = matrix1.M11 * scaleFactor;
        result.M12 = matrix1.M12 * scaleFactor;
        result.M13 = matrix1.M13 * scaleFactor;
        result.M14 = matrix1.M14 * scaleFactor;
        result.M21 = matrix1.M21 * scaleFactor;
        result.M22 = matrix1.M22 * scaleFactor;
        result.M23 = matrix1.M23 * scaleFactor;
        result.M24 = matrix1.M24 * scaleFactor;
        result.M31 = matrix1.M31 * scaleFactor;
        result.M32 = matrix1.M32 * scaleFactor;
        result.M33 = matrix1.M33 * scaleFactor;
        result.M34 = matrix1.M34 * scaleFactor;
        result.M41 = matrix1.M41 * scaleFactor;
        result.M42 = matrix1.M42 * scaleFactor;
        result.M43 = matrix1.M43 * scaleFactor;
        result.M44 = matrix1.M44 * scaleFactor;

    }

    /// <summary>
    /// Returns a matrix with the all values negated.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/>.</param>
    /// <returns>Result of the matrix negation.</returns>
    public static Matrix Negate(Matrix matrix)
    {
        matrix.M11 = -matrix.M11;
        matrix.M12 = -matrix.M12;
        matrix.M13 = -matrix.M13;
        matrix.M14 = -matrix.M14;
        matrix.M21 = -matrix.M21;
        matrix.M22 = -matrix.M22;
        matrix.M23 = -matrix.M23;
        matrix.M24 = -matrix.M24;
        matrix.M31 = -matrix.M31;
        matrix.M32 = -matrix.M32;
        matrix.M33 = -matrix.M33;
        matrix.M34 = -matrix.M34;
        matrix.M41 = -matrix.M41;
        matrix.M42 = -matrix.M42;
        matrix.M43 = -matrix.M43;
        matrix.M44 = -matrix.M44;
        return matrix;
    }

    /// <summary>
    /// Returns a matrix with the all values negated.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/>.</param>
    /// <param name="result">Result of the matrix negation as an output parameter.</param>
    public static void Negate(ref Matrix matrix, out Matrix result)
    {
        result.M11 = -matrix.M11;
        result.M12 = -matrix.M12;
        result.M13 = -matrix.M13;
        result.M14 = -matrix.M14;
        result.M21 = -matrix.M21;
        result.M22 = -matrix.M22;
        result.M23 = -matrix.M23;
        result.M24 = -matrix.M24;
        result.M31 = -matrix.M31;
        result.M32 = -matrix.M32;
        result.M33 = -matrix.M33;
        result.M34 = -matrix.M34;
        result.M41 = -matrix.M41;
        result.M42 = -matrix.M42;
        result.M43 = -matrix.M43;
        result.M44 = -matrix.M44;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains subtraction of one matrix from another.
    /// </summary>
    /// <param name="matrix1">The first <see cref="Matrix"/>.</param>
    /// <param name="matrix2">The second <see cref="Matrix"/>.</param>
    /// <returns>The result of the matrix subtraction.</returns>
    public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
    {
        matrix1.M11 -= matrix2.M11;
        matrix1.M12 -= matrix2.M12;
        matrix1.M13 -= matrix2.M13;
        matrix1.M14 -= matrix2.M14;
        matrix1.M21 -= matrix2.M21;
        matrix1.M22 -= matrix2.M22;
        matrix1.M23 -= matrix2.M23;
        matrix1.M24 -= matrix2.M24;
        matrix1.M31 -= matrix2.M31;
        matrix1.M32 -= matrix2.M32;
        matrix1.M33 -= matrix2.M33;
        matrix1.M34 -= matrix2.M34;
        matrix1.M41 -= matrix2.M41;
        matrix1.M42 -= matrix2.M42;
        matrix1.M43 -= matrix2.M43;
        matrix1.M44 -= matrix2.M44;
        return matrix1;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains subtraction of one matrix from another.
    /// </summary>
    /// <param name="matrix1">The first <see cref="Matrix"/>.</param>
    /// <param name="matrix2">The second <see cref="Matrix"/>.</param>
    /// <param name="result">The result of the matrix subtraction as an output parameter.</param>
    public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
    {
        result.M11 = matrix1.M11 - matrix2.M11;
        result.M12 = matrix1.M12 - matrix2.M12;
        result.M13 = matrix1.M13 - matrix2.M13;
        result.M14 = matrix1.M14 - matrix2.M14;
        result.M21 = matrix1.M21 - matrix2.M21;
        result.M22 = matrix1.M22 - matrix2.M22;
        result.M23 = matrix1.M23 - matrix2.M23;
        result.M24 = matrix1.M24 - matrix2.M24;
        result.M31 = matrix1.M31 - matrix2.M31;
        result.M32 = matrix1.M32 - matrix2.M32;
        result.M33 = matrix1.M33 - matrix2.M33;
        result.M34 = matrix1.M34 - matrix2.M34;
        result.M41 = matrix1.M41 - matrix2.M41;
        result.M42 = matrix1.M42 - matrix2.M42;
        result.M43 = matrix1.M43 - matrix2.M43;
        result.M44 = matrix1.M44 - matrix2.M44;
    }

    /// <summary>
    /// Swap the matrix rows and columns.
    /// </summary>
    /// <param name="matrix">The matrix for transposing operation.</param>
    /// <returns>The new <see cref="Matrix"/> which contains the transposing result.</returns>
    public static Matrix Transpose(Matrix matrix)
    {
        Transpose(ref matrix, out Matrix ret);
        return ret;
    }

    /// <summary>
    /// Swap the matrix rows and columns.
    /// </summary>
    /// <param name="matrix">The matrix for transposing operation.</param>
    /// <param name="result">The new <see cref="Matrix"/> which contains the transposing result as an output parameter.</param>
    public static void Transpose(ref Matrix matrix, out Matrix result)
    {
        Matrix ret;

        ret.M11 = matrix.M11;
        ret.M12 = matrix.M21;
        ret.M13 = matrix.M31;
        ret.M14 = matrix.M41;

        ret.M21 = matrix.M12;
        ret.M22 = matrix.M22;
        ret.M23 = matrix.M32;
        ret.M24 = matrix.M42;

        ret.M31 = matrix.M13;
        ret.M32 = matrix.M23;
        ret.M33 = matrix.M33;
        ret.M34 = matrix.M43;

        ret.M41 = matrix.M14;
        ret.M42 = matrix.M24;
        ret.M43 = matrix.M34;
        ret.M44 = matrix.M44;

        result = ret;
    }

    #endregion

    #region Public Static Operator Overloads

    /// <summary>
    /// Adds two matrixes.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the add sign.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the add sign.</param>
    /// <returns>Sum of the matrixes.</returns>
    public static Matrix operator +(Matrix matrix1, Matrix matrix2)
    {
        return Matrix.Add(matrix1, matrix2);
    }

    /// <summary>
    /// Divides the elements of a <see cref="Matrix"/> by the elements of another <see cref="Matrix"/>.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the div sign.</param>
    /// <param name="matrix2">Divisor <see cref="Matrix"/> on the right of the div sign.</param>
    /// <returns>The result of dividing the matrixes.</returns>
    public static Matrix operator /(Matrix matrix1, Matrix matrix2)
    {
        return Matrix.Divide(matrix1, matrix2);
    }

    /// <summary>
    /// Divides the elements of a <see cref="Matrix"/> by a scalar.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/> on the left of the div sign.</param>
    /// <param name="divider">Divisor scalar on the right of the div sign.</param>
    /// <returns>The result of dividing a matrix by a scalar.</returns>
    public static Matrix operator /(Matrix matrix, float divider)
    {
        return Matrix.Divide(matrix, divider);
    }

    /// <summary>
    /// Compares whether two <see cref="Matrix"/> instances are equal without any tolerance.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the equal sign.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the equal sign.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(Matrix matrix1, Matrix matrix2)
    {
        return matrix1.Equals(matrix2);
    }

    /// <summary>
    /// Compares whether two <see cref="Matrix"/> instances are not equal without any tolerance.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the not equal sign.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the not equal sign.</param>
    /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
    public static bool operator !=(Matrix matrix1, Matrix matrix2)
    {
        return !matrix1.Equals(matrix2);
    }

    /// <summary>
    /// Multiplies two matrixes.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the mul sign.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the mul sign.</param>
    /// <returns>Result of the matrix multiplication.</returns>
    /// <remarks>
    /// Using matrix multiplication algorithm - see http://en.wikipedia.org/wiki/Matrix_multiplication.
    /// </remarks>
    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        return Multiply(matrix1, matrix2);
    }

    /// <summary>
    /// Multiplies the elements of matrix by a scalar.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/> on the left of the mul sign.</param>
    /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
    /// <returns>Result of the matrix multiplication with a scalar.</returns>
    public static Matrix operator *(Matrix matrix, float scaleFactor)
    {
        return Multiply(matrix, scaleFactor);
    }

    /// <summary>
    /// Subtracts the values of one <see cref="Matrix"/> from another <see cref="Matrix"/>.
    /// </summary>
    /// <param name="matrix1">Source <see cref="Matrix"/> on the left of the sub sign.</param>
    /// <param name="matrix2">Source <see cref="Matrix"/> on the right of the sub sign.</param>
    /// <returns>Result of the matrix subtraction.</returns>
    public static Matrix operator -(Matrix matrix1, Matrix matrix2)
    {
        return Subtract(matrix1, matrix2);
    }

    /// <summary>
    /// Inverts values in the specified <see cref="Matrix"/>.
    /// </summary>
    /// <param name="matrix">Source <see cref="Matrix"/> on the right of the sub sign.</param>
    /// <returns>Result of the inversion.</returns>
    public static Matrix operator -(Matrix matrix)
    {
        return Negate(matrix);
    }

    #endregion
}