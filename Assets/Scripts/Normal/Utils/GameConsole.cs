using System.Reflection;
using UnityEngine;


public static class GameConsole
{

    /// <summary>
    /// �Զ����ӡ��ɫ
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
    /// �Զ����ӡ���ֶ�ֵ��ɫ
    /// </summary>
    /// <param name="message">��ʶ</param>
    /// <param name="arry">���� new {color="red", name=value} ��������</param>
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
    /// ��Ӵ�ӡ��ɫ, ��һ��̶�Ϊ��ɫֵ, �������Ϊ��ֵ��
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
            var properties = obj.GetType().GetProperties(); // ��ȡ��������������
            // ��һ�� property �̶�Ϊ��ɫ����
            for (int i = 1; i < properties.Length; i++)     // ѭ���� 1 ��ʼ
            {
                builder.AppendFormat("{0}: ", properties[i].GetName() ); // key
                builder.AppendFormat("<color={0}>{1}</color>. ", properties[0].GetValue(obj), properties[i].GetValue(obj)); // [��ɫ]value
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
