using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace FlatStage;

public static class Calc
{
    public const float Pi = 3.14159265358979323846f;
    public const float PiOver2 = 1.57079632679489661923f;
    public const float PiOver4 = 0.785398163397448309616f;
    public const float TwoPi = 6.28318530717959f;

    private const float RadiansToDegreesFactor = 180f / Pi;
    private const float DegreesToRadiansFactor = Pi / 180f;
    private const int SinBits = 13;
    private const int SinMask = ~(-1 << SinBits);
    private const int SinCount = SinMask + 1;
    private const float RadFull = Pi * 2;
    private const float DegFull = 360;
    private const float RadToIndex = SinCount / RadFull;
    private const float DegToIndex = SinCount / DegFull;
    private static readonly float[] SinBuffer = new float[SinCount];
    private static readonly float[] CosBuffer = new float[SinCount];

    static Calc()
    {
        for (int i = 0; i < SinCount; i++)
        {
            float angle = (i + 0.5f) / SinCount * RadFull;
            SinBuffer[i] = (float)Math.Sin(angle);
            CosBuffer[i] = (float)Math.Cos(angle);
        }

        for (int i = 0; i < 360; i += 90)
        {
            SinBuffer[(int)(i * DegToIndex) & SinMask] = (float)Math.Sin(i * DegreesToRadiansFactor);
            CosBuffer[(int)(i * DegToIndex) & SinMask] = (float)Math.Cos(i * DegreesToRadiansFactor);
        }
    }

    public static readonly float MachineEpsilonFloat = GetMachineEpsilonFloat();

    private static float GetMachineEpsilonFloat()
    {
        float machineEpsilon = 1.0f;
        float comparison;

        /* Keep halving the working value of machineEpsilon until we get a number that
         * when added to 1.0f will still evaluate as equal to 1.0f.
         */
        do
        {
            machineEpsilon *= 0.5f;
            comparison = 1.0f + machineEpsilon;
        } while (comparison > 1.0f);

        return machineEpsilon;
    }

    #region Base

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int n)
    {
        var mask = n >> 31;
        return (n ^ mask) - mask;
    }

    [Pure]
    public static float Abs(float n) => Math.Abs(n);

    [Pure]
    public static float Sqrt(float n) => (float)Math.Sqrt(n);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SqrtI(int val)
    {
        if (val == 0)
        {
            return 0;
        }

        int n = (val / 2) + 1;
        int n1 = (n + (val / n)) / 2;

        while (n1 < n)
        {
            n = n1;
            n1 = (n + (val / n)) / 2;
        }

        return n;
    }

    /// <summary>
    /// Returns an approximation of the inverse square root of left number.
    /// </summary>
    /// <param name="x">A number.</param>
    /// <returns>An approximation of the inverse square root of the specified number, with an upper error bound of 0.001.</returns>
    /// <remarks>
    /// This is an improved implementation of the the method known as Carmack's inverse square rootMath
    /// which is found in the Quake III source code. This implementation comes from
    /// http://www.codemaestro.com/reviews/review00000105.html. For the history of this method, see
    /// http://www.beyond3d.com/content/articles/8/.
    /// </remarks>
    [Pure]
    public static float InverseSqrtFast(float x)
    {
        unsafe
        {
            var xhalf = 0.5f * x;
            var i = *(int*)&x; // Read bits as integer.
            i = 0x5f375a86 - (i >> 1); // Make an initial guess for Newton-Raphson approximation
            x = *(float*)&i; // Convert bits back to float
            x *= (1.5f - (xhalf * x * x)); // Perform left single Newton-Raphson step.
            return x;
        }
    }

    [Pure]
    public static float Pow(float x, float y) => (float)Math.Pow(x, y);

    [Pure]
    public static int Sign(int d) => Math.Sign(d);

    [Pure]
    public static int Sign(float d) => Math.Sign(d);

    /// <summary>
    /// checks to see if two values are approximately the same using the value of Epsilon
    /// </summary>
    /// <param name="value1">Value1.</param>
    /// <param name="value2">Value2.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximatelyEqual(float value1, float value2)
    {
        return Math.Abs(value1 - value2) <= MachineEpsilonFloat;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextPowerOfTwo(int value)
    {
        if (value == 0)
        {
            return 1;
        }

        value -= 1;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        return value + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPowerOfTwo(int value) => value != 0 && (value & value - 1) == 0;

    public static float IntBitsToFloat(int value)
    {
        var bytes = BitConverter.GetBytes(value & 0xFEFFFFFF);
        return BitConverter.ToSingle(bytes, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap(ref float v1, ref float v2) => (v1, v2) = (v2, v1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap(ref int v1, ref int v2) => (v1, v2) = (v2, v1);

    #endregion

    #region Angles

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sin(float rad)
    {
        return SinBuffer[(int)(rad * RadToIndex) & SinMask];
    }

    [Pure]
    public static float Asin(float radians) => (float)Math.Asin(radians);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cos(float rad)
    {
        return CosBuffer[(int)(rad * RadToIndex) & SinMask];
    }

    [Pure]
    public static float Acos(float radians) => (float)Math.Acos(radians);

    [Pure]
    public static float Tan(float radians) => (float)Math.Tan(radians);

    [Pure]
    public static float Atan(float radians) => (float)Math.Atan(radians);

    [Pure]
    public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToRadians(float degree)
    {
        return degree * DegreesToRadiansFactor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToDegrees(float radian)
    {
        return radian * RadiansToDegreesFactor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AngleBetweenVectors(Vec2 from, Vec2 to)
    {
        return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
    }

    /// <summary>
    /// Calculates the shortest difference between two given angles in degrees
    /// </summary>
    /// <returns>The angle.</returns>
    /// <param name="current">Current.</param>
    /// <param name="target">Target.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DeltaAngle(float current, float target)
    {
        var num = Repeat(target - current, 360f);
        if (num > 180f)
            num -= 360f;

        return num;
    }

    /// <summary>
    /// Calculates the shortest difference between two given angles given in radians
    /// </summary>
    /// <returns>The angle.</returns>
    /// <param name="current">Current.</param>
    /// <param name="target">Target.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DeltaAngleRadians(float current, float target)
    {
        var num = Repeat(target - current, TwoPi);
        if (num > Pi)
            num -= TwoPi;

        return num;
    }

    /// <summary>
    /// Clamps an angle to the range [0, 360).
    /// </summary>
    /// <param name="angle">The angle to clamp in degrees.</param>
    /// <returns>The clamped angle in the range [0, 360).</returns>
    public static float ClampAngle(float angle)
    {
        // mod angle so it's in the range (-360, 360)
        angle %= 360f;

        // abs angle so it's in the range [0, 360)
        angle = Abs(angle);

        return angle;
    }

    /// <summary>
    /// Clamps an angle to the range [0, 2π).
    /// </summary>
    /// <param name="angle">The angle to clamp in radians.</param>
    /// <returns>The clamped angle in the range [0, 2π).</returns>
    public static float ClampRadians(float angle)
    {
        // mod angle so it's in the range (-2π,2π)
        angle %= 2 * Pi;

        // abs angle so it's in the range [0,2π)
        angle = Abs(angle);

        return angle;
    }

    #endregion

    #region Clamping

    [Pure]
    public static float Round(float value) => (float)Math.Round(value);

    [Pure]
    public static float Ceil(float n) => (float)Math.Ceiling(n);

    [Pure]
    public static int CeilToInt(float value)
    {
        return (int)Math.Ceiling(value);
    }

    /// <summary>
    /// Ceils the float to the nearest int value above y. note that this only works for values in the range of short
    /// </summary>
    /// <returns>The ceil to int.</returns>
    /// <param name="value">F.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FastCeilToInt(float value)
    {
        return 32768 - (int)(32768f - value);
    }

    [Pure]
    public static float Floor(float n) => (float)Math.Floor(n);

    [Pure]
    public static int Min(int a, int b) => Math.Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(int a, int b, int c)
    {
        return a < b ? (a < c ? a : c) : (b < c ? b : c);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(int a, int b, int c, int d)
    {
        return Min(d, Min(a, Min(b, c)));
    }

    [Pure]
    public static float Min(float a, float b) => Math.Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(float a, float b, float c)
    {
        return a < b ? (a < c ? a : c) : (b < c ? b : c);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(float a, float b, float c, float d)
    {
        return Min(d, Min(a, Min(b, c)));
    }

    [Pure]
    public static int Max(int a, int b) => Math.Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int a, int b, int c)
    {
        return a < b ? (b < c ? c : b) : (a < c ? c : b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int a, int b, int c, int d)
    {
        return Max(d, Max(a, Max(b, c)));
    }

    [Pure]
    public static float Max(float a, float b) => Math.Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(float a, float b, float c)
    {
        return a < b ? (b < c ? c : b) : (a < c ? c : b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(float a, float b, float c, float d)
    {
        return Max(d, Max(a, Max(b, c)));
    }

    /// <summary>
    /// Clamps float between 0f and 1f
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Normalize(float value)
    {
        if (value < 0f)
            return 0f;

        return value > 1f ? 1f : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Normalize(float var, float min, float max)
    {
        if (var >= min && var < max) return var;

        if (var < min)
            var = max + ((var - min) % max);
        else
            var = min + var % (max - min);

        return var;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
        return (value > max) ? max : ((value < min) ? min : value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        return (value > max) ? max : ((value < min) ? min : value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ClampB(byte value, byte min, byte max)
    {
        return (value > max) ? max : ((value < min) ? min : value);
    }

    /// <summary>
    /// Restricts a value to be multiple of increment.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="increment"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Snap(float value, float increment)
    {
        return Round(value / increment) * increment;
    }

    public static float CeilSnap(float value, float increment)
    {
        return (Ceil(value / increment) * increment);
    }

    #endregion

    #region Animation and Movement

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float a, float b, float x) => a + (b - a) * x;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LerpAngle(float a, float b, float t)
    {
        float num = Repeat(b - a, 360f);
        if (num > 180f)
            num -= 360f;

        return a + (num * Normalize(t));
    }

    /// <summary>
    /// Interpolates between two values using a cubic equation.
    /// </summary>
    /// <param name="value1">Source value.</param>
    /// <param name="value2">Source value.</param>
    /// <param name="amount">Weighting value.</param>
    /// <returns>Interpolated value.</returns>
    public static float SmoothStep(float value1, float value2, float amount)
    {
        /* It is expected that 0 < amount < 1.
         * If amount < 0, return value1.
         * If amount > 1, return value2.
         */
        float result = Clamp(amount, 0f, 1f);
        result = Hermite(value1, 0f, value2, 0f, result);

        return result;
    }

    /// <summary>
    /// Performs a Hermite spline interpolation.
    /// </summary>
    /// <param name="value1">Source position.</param>
    /// <param name="tangent1">Source tangent.</param>
    /// <param name="value2">Source position.</param>
    /// <param name="tangent2">Source tangent.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>The result of the Hermite spline interpolation.</returns>
    public static float Hermite(
        float value1,
        float tangent1,
        float value2,
        float tangent2,
        float amount
    )
    {
        /* All transformed to double not to lose precision
         * Otherwise, for high numbers of param:amount the result is NaN instead
         * of Infinity.
         */
        double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount;
        double result;
        double sCubed = s * s * s;
        double sSquared = s * s;

        if (ApproximatelyEqual(amount, 0f))
        {
            result = value1;
        }
        else if (ApproximatelyEqual(amount, 1f))
        {
            result = value2;
        }
        else
        {
            result = (
                ((2 * v1 - 2 * v2 + t2 + t1) * sCubed) +
                ((3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared) +
                (t1 * s) +
                v1
            );
        }

        return (float)result;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains linear interpolation of the values in specified matrixes.
    /// </summary>
    /// <param name="matrix1">The first <see cref="Matrix"/>.</param>
    /// <param name="matrix2">The second <see cref="Vec2"/>.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <returns>>The result of linear interpolation of the specified matrixes.</returns>
    public static Matrix Lerp(Matrix matrix1, Matrix matrix2, float amount)
    {
        matrix1.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
        matrix1.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
        matrix1.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
        matrix1.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
        matrix1.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
        matrix1.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
        matrix1.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
        matrix1.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
        matrix1.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
        matrix1.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
        matrix1.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
        matrix1.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
        matrix1.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
        matrix1.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
        matrix1.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
        matrix1.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
        return matrix1;
    }

    /// <summary>
    /// Creates a new <see cref="Matrix"/> that contains linear interpolation of the values in specified matrixes.
    /// </summary>
    /// <param name="matrix1">The first <see cref="Matrix"/>.</param>
    /// <param name="matrix2">The second <see cref="Vec2"/>.</param>
    /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
    /// <param name="result">The result of linear interpolation of the specified matrixes as an output parameter.</param>
    public static void Lerp(
        ref Matrix matrix1,
        ref Matrix matrix2,
        float amount,
        out Matrix result
    )
    {
        result.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
        result.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
        result.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
        result.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
        result.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
        result.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
        result.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
        result.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
        result.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
        result.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
        result.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
        result.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
        result.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
        result.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
        result.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
        result.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
    }

    /// <summary>
    /// Loops t so that it is never larger than length and never smaller than 0
    /// </summary>
    /// <param name="t">T.</param>
    /// <param name="length">Length.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Repeat(float t, float length)
    {
        return t - Floor(t / length) * length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Wrap(float var, float min, float max)
    {
        if (var < min)
        {
            var += max;
        }

        var %= max;

        return var;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Wrap(int var, int min, int max)
    {
        if (var < min)
        {
            var += max;
        }

        var %= max;

        return var;
    }

    /// <summary>
    /// ping-pongs t so that it is never larger than length and never smaller than 0
    /// </summary>
    /// <returns>The pong.</returns>
    /// <param name="t">T.</param>
    /// <param name="length">Length.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PingPong(float t, float length)
    {
        t = Repeat(t, length * 2f);
        return length - Abs(t - length);
    }

    /// <summary>
    /// Moves start towards end by shift amount clamping the result. start can be less than or greater than end.
    /// example: start is 2, end is 10, shift is 4 results in 6
    /// </summary>
    /// <param name="start">Start.</param>
    /// <param name="end">End.</param>
    /// <param name="shift">Shift.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Approach(float start, float end, float shift)
    {
        return start < end ? Math.Min(start + shift, end) : Math.Max(start - shift, end);
    }

    public static float Accelerate(float velocity, float minSpeed, float maxSpeed, float acceleration, float dt)
    {
        float min = minSpeed * dt;
        float max = maxSpeed * dt;

        return Clamp(velocity * dt + 0.5f * acceleration * dt * dt, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 RotateAround(Vec2 point, Vec2 center, float angleRadians)
    {
        var cos = Cos(angleRadians);
        var sin = Sin(angleRadians);
        var rotatedX = cos * (point.X - center.X) - sin * (point.Y - center.Y) + center.X;
        var rotatedY = sin * (point.X - center.X) + cos * (point.Y - center.Y) + center.Y;

        return new Vec2(rotatedX, rotatedY);
    }

    #endregion

    #region Base Geometry

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vec2 vec1, Vec2 vec2)
    {
        return (float)Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y));
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The distance between two vectors.</returns>
    public static float Distance(Vec3 vector1, Vec3 vector2)
    {
        DistanceSquared(ref vector1, ref vector2, out float result);
        return (float)Math.Sqrt(result);
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The distance between two vectors as an output parameter.</param>
    public static void Distance(ref Vec3 value1, ref Vec3 value2, out float result)
    {
        DistanceSquared(ref value1, ref value2, out result);
        result = (float)Math.Sqrt(result);
    }

    /// <summary>
    /// Returns the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between two vectors.</returns>
    public static float DistanceSquared(Vec3 value1, Vec3 value2)
    {
        return (
            (value1.X - value2.X) * (value1.X - value2.X) +
            (value1.Y - value2.Y) * (value1.Y - value2.Y) +
            (value1.Z - value2.Z) * (value1.Z - value2.Z)
        );
    }

    /// <summary>
    /// Returns the squared distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="result">The squared distance between two vectors as an output parameter.</param>
    public static void DistanceSquared(
        ref Vec3 value1,
        ref Vec3 value2,
        out float result
    )
    {
        result = (
            (value1.X - value2.X) * (value1.X - value2.X) +
            (value1.Y - value2.Y) * (value1.Y - value2.Y) +
            (value1.Z - value2.Z) * (value1.Z - value2.Z)
        );
    }

    /// <summary>
    /// Gets a point on the circumference of the circle given its center, radius and angle. 0 degrees is 3 o'clock.
    /// </summary>
    /// <returns>The on circle.</returns>
    /// <param name="circleCenter">Circle center.</param>
    /// <param name="radius">Radius.</param>
    /// <param name="angleInDegrees">Angle in degrees.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 PointOnCircle(Vec2 circleCenter, float radius, float angleInDegrees)
    {
        var radians = ToRadians(angleInDegrees);
        return new Vec2
        {
            X = Cos(radians) * radius + circleCenter.X,
            Y = Sin(radians) * radius + circleCenter.Y
        };
    }

    public static bool PointInRect(Vec2 point, float rx, float ry, float rw, float rh)
    {
        if (point.X <= rx) return false;
        if (point.X >= rx + rw) return false;
        if (point.Y <= ry) return false;
        if (point.Y >= ry + rh) return false;

        return true;
    }

    public static float DistanceRectPoint(Vec2 point, float rx, float ry, float rw, float rh)
    {
        if (point.X >= rx && point.X <= rx + rw)
        {
            if (point.Y >= ry && point.Y <= ry + rh) return 0;

            if (point.Y > ry) return point.Y - (ry + rh);

            return ry - point.Y;
        }

        if (point.Y >= ry && point.Y <= ry + rh)
        {
            if (point.X > rx) return point.X - (rx + rw);

            return rx - point.X;
        }

        if (point.X > rx)
        {
            if (point.Y > ry) return Distance(point, new Vec2(rx + rw, ry + rh));

            return Distance(point, new Vec2(rx + rw, ry));
        }

        return Distance(point, point.Y > ry ? new Vec2(rx, ry + rh) : new Vec2(rx, ry));
    }

    #endregion

    #region Packing

    /// <summary>
    /// Pack 3 ints into one. Each can have a max value of 1023.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static int Pack3(int a, int b, int c)
    {
        if (a > 1023)
        {
            throw new ArgumentOutOfRangeException(nameof(a));
        }

        if (b > 1023)
        {
            throw new ArgumentOutOfRangeException(nameof(b));
        }

        if (c > 1023)
        {
            throw new ArgumentOutOfRangeException(nameof(c));
        }

        return (a << 20) | (b << 10) | c;
    }

    public static (int a, int b, int c) Unpack(int packed)
    {
        return ((packed >> 20) & 0x3FF, (packed >> 10) & 0x3FF, (packed) & 0x3FF);
    }

    #endregion

    public static float[] ConvertByteArrayToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        for (int i = 0; i < floatArr.Length; i++)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array, i * 4, 4);
            floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
        }

        return floatArr;
    }

    public static byte[] ConvertFloatArrayToByte(float[] array)
    {
        byte[] byteArr = new byte[array.Length * 4];
        for (int i = 0; i < array.Length; i++)
        {
            var bytes = BitConverter.GetBytes(array[i] * 0x80000000);
            Array.Copy(bytes, 0, byteArr, i * 4, bytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArr, i * 4, 4);
        }

        return byteArr;
    }
}
