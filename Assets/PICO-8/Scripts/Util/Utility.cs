using DG.Tweening;
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

    public static Tween To(float from, float to, float time, System.Action<float> onUpdate)
    {
        return DOTween.To(() => from, x => from = x, to, time).OnUpdate(() => onUpdate(from));
    }

    public static Tween DelayCall(float delay, System.Action callback)
    {
        return DOVirtual.DelayedCall(delay, () => callback(), true);
    }

    public static Tween MoveTo(Transform transform, Vector3 endValue, float duration, bool snapping = false, Ease ease = Ease.Linear)
    {
        return transform.DOMove(endValue, duration, false).SetEase(ease);
    }

    public static void Animate(this SpriteRenderer renderer, Sprite[] sequence, float fps, Sprite tailSprite = null, System.Action callback = null)
    {
        if (sequence != null && sequence.Length > 0)
        {
            To(0.0f, sequence.Length - 1, sequence.Length / fps, val =>
              {
                  if (renderer == null) return;
                  renderer.sprite = sequence[(int)val];
              }).SetEase(Ease.Linear).OnComplete(() => { renderer.sprite = tailSprite; callback?.Invoke(); }).SetAutoKill(true);
        }
    }
}