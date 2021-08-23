using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private int idle;
    private int run;
    private int jump;
    private int climb;
    private int lookUp;
    private int lookDown;


    private Color normalRed = new Color(1f, 0f, 77 / 255f);
    private Color dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    [SerializeField]
    private Animator _anim;

    private void Awake()
    {
        idle = Animator.StringToHash("Idle");
        run = Animator.StringToHash("Run");
        jump = Animator.StringToHash("Jump");
        lookUp = Animator.StringToHash("LookUp");
        lookDown = Animator.StringToHash("LookDown");
    }

    /// <param name="movement"></param>
    public void UpdateAnimation(CharacterMovement movement)
    {
        var moveX = movement.MoveX;
        var moveY = movement.MoveY;
        if (movement.OnGround)
        {
            if (moveX == 0 && moveY == 0)
            {
                _anim.Play(idle);
            }
            else if (moveX != 0)
            {
                _anim.Play(run);
            }
            else if (moveY != 0)
            {
                _anim.Play(moveY > 0 ? lookUp : lookDown);
            }
        }
        else if (movement.IsJumping || movement.IsDashing || movement.IsFalling)
        {
            _anim.Play(jump);
        }
        if (movement.IsClimbing)
        {
            _anim.Play(climb);
        }
        if (movement.IsDashing)
        {
            _anim.SetColor(dashBlue);
        }
        else
        {
            _anim.SetColor(normalRed);
        }
    }

}

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
}
