using System;
using UnityEngine;

/// <summary>
/// ÊýÑ§À©Õ¹
/// </summary>
public static class MathEx
{
    public const float MIN = 0.0001f;

    public static bool IsMinOf(this float a, float b)
    {
        return a < b;
    }

    public static bool IsMinOf(this Vector2 a, float b)
    {
        return a.x < b && a.y < b;
    }

    /// <summary>
    /// Returns the sign of b, (true return 1, else -1)
    /// </summary>
    public static int Sign(bool b)
    {
        return b ? 1 : -1;
    }

    /// <summary>
    /// Returns the sign of f, (0 return 0)
    /// </summary>
    public static int Sign(float f)
    {
        return (f > 0) ? 1 : (f < 0 ? -1 : 0);
    }

    public static Vector2 Abs(Vector2 v)
    {
        return new Vector2(Math.Abs(v.x), Math.Abs(v.y));
    }

    public static float Radians(this Vector2 vector)
    {
        return (float)Math.Atan2(vector.y, vector.x);
    }

    public static float Clamp(float value, float a, float b)
    {
        return Math.Max(a, Math.Min(b, value));
    }

    public static float Approach(float value, float target, float amount)
    {
        return value > target ? Math.Max(value - amount, target) : Math.Min(value + amount, target);
    }
}