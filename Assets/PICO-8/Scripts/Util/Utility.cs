using UnityEngine;

public static class Utility
{
    public static bool IsPlaying(this Animator animator, int stateNameHash)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash == stateNameHash)
        {
            return true;
        }
        return false;
    }

    public static void SetColor(this Animator anim, Color color)
    {
        anim.GetComponentInChildren<SpriteRenderer>().color = color;
    }

    public static void BounceTo(this Transform trans, Vector3 from, Vector3 to, float duration)
    {

    }
}