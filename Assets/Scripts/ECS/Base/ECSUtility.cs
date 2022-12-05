using System;
using UnityEngine;

public class ECSUtility
{
    public float Clamp(float value, float a, float b)
    {
        return Math.Max(a, Math.Min(b, value));
    }

    public static float Approach(float value, float target, float amount)
    {
        return value > target ? Math.Max(value - amount, target) : Math.Min(value + amount, target);
    }

    public int Sign(float value)
    {
        return value > 0 ? 1 : value < 0 ? -1 : 0;
    }

    public bool Maybe()
    {
        return (new System.Random().Next() % 10) < 5;
    }
}
