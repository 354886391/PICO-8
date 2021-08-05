using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private int idle;
    private int run;
    private int jump;
    private int fall;
    private int dash;
    private int lookUp;
    private int lookDown;
    [SerializeField]
    private Animator _anim;

    private void Awake()
    {
        idle = Animator.StringToHash("Idle");
        run = Animator.StringToHash("Run");
        jump = Animator.StringToHash("Jump");
        fall = Animator.StringToHash("Fall");
        dash = Animator.StringToHash("Dash");
        lookUp = Animator.StringToHash("LookUp");
        lookDown = Animator.StringToHash("LookDown");
    }


    public void UpdateAnimation(CharacterMovement movement)
    {
        if (movement._onGround)
        {
            if (movement.MoveX == 0 && movement.MoveY == 0)
            {
                _anim.Play(idle);
            }
            if (movement.MoveX != 0)
            {
                _anim.Play(run);
            }
            else if (movement.MoveY > 0)
            {
                _anim.Play(lookUp);
            }
            else if (movement.MoveY < 0)
            {
                _anim.Play(lookDown);
            }
        }
    }
}
