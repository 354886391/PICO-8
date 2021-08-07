using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private int idle;
    private int run;
    private int jump;
    private int lookUp;
    private int lookDown;
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
        if (movement._onGround)
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
}
