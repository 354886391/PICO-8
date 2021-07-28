
using UnityEngine;

public static class Console
{
    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void DrawRay(Vector2 start, Vector2 end, Color color)
    {
        UnityEngine.Debug.DrawRay(start, end, color);
    }
}

