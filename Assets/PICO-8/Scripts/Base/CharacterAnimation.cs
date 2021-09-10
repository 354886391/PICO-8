using UnityEngine;
using DG.Tweening;

public class CharacterAnimation : MonoBehaviour
{
    private int idle;
    private int run;
    private int jump;
    private int climb;
    private int lookUp;
    private int lookDown;

    private Tweener jumpTween;

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
        AddCharacterMovementEvent();
    }

    private void AddCharacterMovementEvent()
    {
        CharacterMovement.JumpBeginEvent += JumpBeginBounce;
        CharacterMovement.JumpEndEvent += JumpEndBounce;
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
        // SetColor
        _anim.SetColor(movement.IsDashing ? dashBlue : normalRed);
    }

    private void QJumpBounce(CharacterMovement movement)
    {
        var originScale = transform.localScale;
        var bounceScale = new Vector3(originScale.x, originScale.y * 0.8f, originScale.z);
        if (jumpTween == null)
        {
            jumpTween = transform.DOScale(bounceScale, 0.1f).SetEase(Ease.InOutCirc).SetLoops(2, LoopType.Yoyo).SetAutoKill(false);
        }
        else
        {
            jumpTween.ChangeStartValue(originScale).ChangeEndValue(bounceScale).Restart();
        }
    }

    private void JumpBeginBounce(CharacterMovement movement)
    {
        QJumpBounce(movement);
    }

    private void JumpEndBounce(CharacterMovement movement)
    {
        //QJumpBounce(movement);
    }
}

