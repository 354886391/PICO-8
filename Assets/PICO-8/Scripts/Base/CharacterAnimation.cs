#define NO_ENABLE_DEBUG
using DG.Tweening;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private int born;
    private int idle;
    private int run;
    private int jump;
    private int climb;
    private int lookUp;
    private int lookDown;

    private Tweener jumpTween;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _groupTransform;
    [SerializeField]
    private Sprite[] _particleSprites;

    private void Start()
    {
        born = Animator.StringToHash("Born");
        idle = Animator.StringToHash("Idle");
        run = Animator.StringToHash("Run");
        jump = Animator.StringToHash("Jump");
        lookUp = Animator.StringToHash("LookUp");
        lookDown = Animator.StringToHash("LookDown");
        AddMovementEvent();
    }

    private void AddMovementEvent()
    {
        CharacterMovement.JumpBeginEvent += JumpBeginHandler;
        CharacterMovement.DashBeginEvent += DashBeginHandler;
        CharacterMovement.LandingEvent += LandingHandler;
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
                _animator.Play(idle);
            }
            else if (moveX != 0)
            {
                _animator.Play(run);
            }
            else if (moveY != 0)
            {
                _animator.Play(moveY > 0 ? lookUp : lookDown);
            }
        }
        else if (movement.IsJumping || movement.IsDashing || movement.IsFalling)
        {
            _animator.Play(jump);
        }
        if (movement.IsClimbing)
        {
            _animator.Play(climb);
        }

    }

    public void AnimateBorn()
    {
        _animator.Play(born);
    }

    [ContextMenu("TakeoffBounce")]
    private void TakeOffBounce()
    {
        var yPercentage = 0.7f;  //Todo: 根据Speed动态调整
        var originScale = _groupTransform.localScale;
        var bounceScale = new Vector3(originScale.x, originScale.y * yPercentage, originScale.z);
        if (jumpTween == null)
        {
            jumpTween = _groupTransform.DOScaleY(bounceScale.y, 0.15f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => { _groupTransform.DOScaleY(originScale.y, 0.05f).SetEase(Ease.Linear); })
                .SetAutoKill(false);
        }
        else
        {
            jumpTween.ChangeStartValue(originScale).ChangeEndValue(bounceScale).Restart();
        }
    }

    /// <summary>
    /// 落地瞬间不能更改角色scale, 否则会多次触发地面检测机制
    /// </summary>
    [ContextMenu("LandingBounce")]
    private void LandingBounce()
    {
        var xPercentage = 0.9f;
        var yPercentage = 0.9f;  //Todo: 根据Speed动态调整
        var originScale = _groupTransform.localScale;
        var bounceScale = new Vector3(originScale.x * xPercentage, originScale.y * yPercentage, originScale.z);
        if (jumpTween == null)
        {
            jumpTween = _groupTransform.DOScale(bounceScale, 0.1f)
                .SetEase(Ease.InBack)
                .OnComplete(() => { _groupTransform.DOScale(originScale, 0.05f).SetEase(Ease.Linear); })
                .SetAutoKill(false);
        }
        else
        {
            jumpTween.ChangeStartValue(originScale).ChangeEndValue(bounceScale).Restart();
        }
    }

    [ContextMenu("JumpParticle")]
    private void JumpParticle(CharacterMovement movement)
    {
        // 动画 应该跟随角色, 还是在地面起跳点?
        var go = new GameObject("JumpParticle");
        go.transform.position = transform.position;
        go.transform.localScale = new Vector3(movement.Facing, 1.0f, 1.0f);
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.Animate(_particleSprites, 12f, null, () => Destroy(go));
    }

    private void JumpBeginHandler(CharacterMovement movement)
    {
        TakeOffBounce();
        JumpParticle(movement);
    }

    private void DashBeginHandler(CharacterMovement movement)
    {
        if (movement.OnGround) TakeOffBounce();   // 在空中Dash时轨迹曲折: Line1曲折, Line2无影响                
        JumpParticle(movement);

    }

    private void LandingHandler(CharacterMovement movement)
    {
        LandingBounce();
        //Console.LogFormat("Landing speed {0}", movement.Speed);
    }

}

