using System.Threading.Tasks;
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
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
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

    public static async void Freeze(int milliseconds)
    {
        LogWarning("Freeze " + Time.unscaledTime);
        Time.timeScale = 0;
        await Task.Delay(milliseconds).ContinueWith(task => { Time.timeScale = 1; LogWarning("Freeze " + Time.unscaledTime); }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public static async void DelayCall(int milliseconds, System.Action callback)
    {
        await Task.Delay(milliseconds).ContinueWith(task => callback?.Invoke());
    }

    public static async void DelayCall(int milliseconds, System.Action callback, TaskScheduler scheduler)
    {
        await Task.Delay(milliseconds).ContinueWith(task => callback?.Invoke(), scheduler);
    }
}

