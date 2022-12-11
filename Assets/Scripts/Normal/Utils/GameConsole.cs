using System.Reflection;
using UnityEngine;


public static class GameConsole
{

    /// <summary>
    /// 自定义打印颜色
    /// </summary>
    public static void Log(string message, string color, params object[] arry)
    {
        var str = new System.Text.StringBuilder();
        str.Append($"<color={color}>");
        foreach (var item in arry)
        {
            str.Append(item.ToString());
        }
        str.Append("</color>");
        Debug.Log(message + str);
    }

    /// <summary>
    /// 自定义打印的字段值颜色
    /// </summary>
    /// <param name="message">标识</param>
    /// <param name="arry">形如 new {color="red", name=value} 的匿名类</param>
    public static void Log(string message, params object[] arry)
    {
        var str = new System.Text.StringBuilder();
        foreach (var item in arry)
        {
            str.Append(FormatParams(item));
        }
        Debug.Log(message + str);
    }

    public static void Warring()
    {

    }

    public static void Error()
    {

    }

    /// <summary>
    /// 添加打印颜色, 第一项固定为颜色值, 后继内容为键值对
    /// Console.Log("Moving: ",
    ///    new { color = "red", OnGround = isOnGround, HitWall = isHitWall },
    ///    new { color = "green", Facing = facing},
    ///    new { color = "blue", Speed = speed });
    /// </summary>
    /// <param name="obj"> new {color = "red", key1 = value1, key2 = value2, ...} </param>
    /// <returns></returns>
    private static string FormatParams(object obj)
    {
        if (obj != null)
        {
            var builder = new System.Text.StringBuilder();
            var properties = obj.GetType().GetProperties(); // 获取匿名类所有属性
            // 第一个 property 固定为颜色属性
            for (int i = 1; i < properties.Length; i++)     // 循环从 1 开始
            {
                builder.AppendFormat("{0}: ", properties[i].GetName() ); // key
                builder.AppendFormat("<color={0}>{1}</color>. ", properties[0].GetValue(obj), properties[i].GetValue(obj)); // [颜色]value
            }
            return builder.ToString();
        }
        return string.Empty;
    }

    public static string GetName(this PropertyInfo info)
    {
        return info != null ? info.Name : string.Empty;
    }
}
