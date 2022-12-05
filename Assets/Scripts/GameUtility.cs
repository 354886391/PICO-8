using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class GameUtility
{
    #region File
    private static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private static bool ExistFile(string path, string name)
    {
        return File.Exists(Path.Combine(path, name));
    }

    public static T LoadFromJson<T>(string path, string name)
    {
        if (ExistFile(path, name))
        {
            var str = File.ReadAllText(Path.Combine(path, name));   // 读取文件
            Debug.Log("load: " + str);
            return JsonConvert.DeserializeObject<T>(str);
        }
        throw new Exception($"LoadFromJson: Not found <{name}>");
    }

    public static void SaveToJson<T>(string path, string name, T data)
    {
        CreateDirectory(path);
        var str = JsonConvert.SerializeObject(data);
        File.WriteAllText(Path.Combine(path, name), str);   // 写入文件
        Debug.Log("save: " + str);
    }
    #endregion

    #region DRAWRAY
    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void DebugRay(Vector2 start, Vector2 direction, Color color)
    {
        Debug.DrawRay(start, direction, color);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void DebugLine(Vector2 start, Vector2 end, Color color)
    {
        Debug.DrawLine(start, end, color);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void DebugCube(Vector3 min, Vector3 max, Color color)
    {
        var tLeft = new Vector3(min.x, max.y);
        var bRight = new Vector3(max.x, min.y);
        Debug.DrawLine(tLeft, (Vector3)max, color);
        Debug.DrawLine((Vector3)max, bRight, color);
        Debug.DrawLine(bRight, (Vector3)min, color);
        Debug.DrawLine((Vector3)min, tLeft, color);
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    public static void DebugCube(Vector2 center, Vector2 size, Color color)
    {
        Vector3 min = new Vector3(center.x - size.x / 2, center.y - size.y / 2);
        Vector3 max = new Vector3(center.x + size.x / 2, center.y + size.y / 2);
        DebugCube(min, max, color);
    }
    #endregion
}
