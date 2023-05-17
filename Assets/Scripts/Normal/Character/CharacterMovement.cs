#define ENABLE_DEBUG
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

/// <summary>
/// 发呆, 移动, 跳, 冲刺, 爬墙
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    #region CONSTANT
    #region RUN
    private const float MaxRun = 8.3f;
    private const float MaxWalk = 6;
    private const float RunAccel = 100;
    private const float RunReduce = 40;
    private const float AirMult = 0.65f;
    #endregion

    #region GRAVITY
    private const float MaxFall = -16;
    private const float Gravity = 79;
    private const float GravThreshold = 4;
    #endregion

    #region JUMP    // 跳跃
    private const float JumpHBoost = 4;
    private const float MaxJump = 16.65f;
    private const float JumpTime = 0.2f;
    private const float JumpToleranceTime = 0.15f;
    private const float JumpCount = 2;
    private const float JumpThreshold = 4;
    private const float JumpEndMult = 0.35f;
    #endregion

    #region SLIDE   //滑落
    private const float GrabWall = 0;
    private const float MaxSlide = -8f;
    #endregion

    #region DASH   // 冲刺
    private const float DashHBoost = 4;
    private const float MaxDash = 24;
    private const float DashAccel = 160;
    private const float DashReduce = 120;
    private const float DashTime = 0.15f;
    private const float DashToleranceTime = 0.15f;
    private const float DashCooldownTime = 0.2f;
    private const float DashTotalCount = 0;
    private const float DashThreshold = 3;
    #endregion
    #endregion

    #region VAR


    [TitleGroup("MOVE")]
    public bool canMove;
    [TitleGroup("MOVE")]
    public Vector2 speed;           // 期望速度
    [TitleGroup("MOVE")]
    public Vector2 movement;        // 实际位移

    // 在地面
    [TitleGroup("MOVE"), ShowInInspector]
    public bool isOnGround => detection ? detection.vertical.isTouch : false;

    // 碰到墙
    [TitleGroup("MOVE"), ShowInInspector]
    public bool isGraspWall => detection ? detection.horizontal.isTouch : false;

    [TitleGroup("MOVE"), ShowInInspector]
    public bool isAirborne => false;

    [TitleGroup("MOVE"), ShowInInspector]
    public bool isFacingRight => false;

    #region JUMP
    [ReadOnly]
    [TitleGroup("JUMP")]
    public bool canJump;    // 是否可以跳跃, 检测后立即置否
    [ReadOnly]
    public bool isRising;   // 跳跃的上升阶段(不包括滞空的前半段)
    [ReadOnly]
    public bool isJumping;  // 包括上升, 滞空和下降阶段   
    [ReadOnly]
    public bool isWallJumping;
    [ReadOnly]
    public bool isDoubleJumping;
    [ReadOnly]
    public bool isWallSliding;
    [ReadOnly]
    public int jumpSteps;
    [ReadOnly]
    public float jumpTimer;
    #endregion

    #region DASH
    [ReadOnly]
    [TitleGroup("DASH")]
    public bool canDash;
    [ReadOnly]
    public bool isDashing;
    [ReadOnly]
    public bool isDashRight;
    [ReadOnly]
    public int dashSteps;
    [ReadOnly]
    public float dashTimer;

    private Vector2 dashBefore;      // 闪避前速度
    private Vector2 dashDirection;   // dashing 时的方向
    #endregion

    #endregion

    #region OTHER
    private Rigidbody2D rigid2d;
    private BodyCollider2D detection;
    #endregion

    private void Awake()
    {
        canMove = true;
        canJump = true;
        canDash = true;

        rigid2d = GetComponent<Rigidbody2D>();
        detection = GetComponent<BodyCollider2D>();
    }

    private void Start()
    {

    }

    public void Move(CharacterInput input, float deltaTime)
    {
        // grounded
        GravityUpdate(input, deltaTime);
        DashUpdate(input, deltaTime);
        JumpUpdate(input, deltaTime);
        RunUpdate(input, deltaTime);

        // detect Ground/Wall
        DetectCollision(deltaTime);
        ApplyMovement();
    }

    public void DetectCollision(float deltaTime)
    {
        movement = speed * deltaTime;
        detection.DetectRaycast(ref movement);
        speed = movement / deltaTime;
    }

    private void ApplyMovement()
    {
        if (canMove)
        {
            rigid2d.MovePosition(rigid2d.position + movement);
        }
    }

    private void GravityUpdate(CharacterInput input, float deltaTime)
    {
        if (!isOnGround)
        {
            var mult = (Mathf.Abs(speed.y) < GravThreshold) && (input.jumpPressed) ? 0.5f : 1f;
            speed.y = MathEx.Approach(speed.y, MaxFall, Gravity * mult * deltaTime);
        }
    }

    public void RunUpdate(CharacterInput input, float deltaTime)
    {
        if (isDashing) return;
        //Reduce back from beyond the max speed
        //Approach the max speed
        var mult = isOnGround ? 1 : AirMult;
        var accel = (Mathf.Abs(speed.x) > MaxRun && Mathf.Sign(speed.x) == input.move.x) ? RunReduce : RunAccel;
        speed.x = MathEx.Approach(speed.x, MaxRun * input.move.x, accel * mult * deltaTime);
    }

    #region JUMP METHOD
    public void JumpUpdate(CharacterInput input, float deltaTime)
    {
        JumpBegin(input);
        WallJumpBegin(input);
        DoubleJumpBegin(input);
        if (isJumping || isWallJumping || isDoubleJumping)
        {
            if (isRising)                       // Rise
            {
                JumpRising(input, deltaTime);
            }
            else if (isOnGround)                // Fall/Slide
            {
                JumpEnd(deltaTime);             // 起跳第一帧仍然在地面上(应当忽略)
            }
            else                                //Land
            {
                JumpFalling(deltaTime);
            }

            if (!isOnGround && !isWallSliding && Mathf.Abs(speed.y) < JumpThreshold)  // Hover [-4, 4] 滞空状态 ^                
            {
                //Console.Log("Hover: ", new { color = "red", Speed = speed });
            }
            //Console.Log("JumpUpdate: ",
            //    new { color = "red", OnGround = onGround },
            //    new { color = "orange", _Jump = isJumping },
            //    new { color = "yellow", _WallJump = isWallJumping },
            //    new { color = "green", DoubleJumpState = jumpSteps > 1 },
            //    new { color = "blue", Speed = speed });

            //Player.StButton.jumping = isJumping;
            //Player.StButton.wallJumping = isWallJumping;
            //Player.StButton.doubleJumping = isDoubleJumping;
        }

        //Player.StButton.rising = isRising && isAirborne;
        //Player.StButton.falling = !isRising && isAirborne;
        //Player.StButton.wallSliding = isWallSliding && isAirborne;
    }

    private void JumpBegin(CharacterInput input)
    {
        // 仅在地面起跳
        if (isAirborne) return;
        if (canJump && input.jumpPressed)
        {
            if (input.jumpPressTimer < JumpToleranceTime)
            {
                jumpSteps = 1;
                canJump = false;
                isRising = true;

                isJumping = true;
                isWallJumping = false;
                isDoubleJumping = false;

                speed.y = MaxJump;
                speed.x += JumpHBoost * input.move.x;
                GameConsole.Log("JumpBegin: ", new { color = "red", Speed = speed });
            }
        }
    }

    /// <summary>
    /// 当第一次跳跃过程中,在空中撞到墙壁角色会粘到墙上并缓慢下落
    /// 按住方向键, 按下跳跃键向反方向踢墙跳跃
    /// </summary>
    /// <param name="input"></param>
    private void WallJumpBegin(CharacterInput input)
    {
        // 仅在空中起跳
        if (!isAirborne) return;
        if (!isGraspWall) return;
        if (canJump && input.jumpPressed)
        {
            if (input.jumpPressTimer < JumpToleranceTime)
            {
                jumpSteps = 1;
                canJump = false;
                isRising = true;

                isJumping = false;
                isDoubleJumping = false;
                isWallJumping = true;
                speed.y = MaxJump;
                speed.x = JumpHBoost * -input.move.x;   // 踢墙跳方向与X轴输入方向相反
                // todo 停顿 4 帧(优化踢墙跳手感)
                //Game.Freeze(0.03f);
                GameConsole.Log("WallJumpBegin: ", new { color = "red", Speed = speed });
            }
        }
    }

    private void DoubleJumpBegin(CharacterInput input)
    {
        // 仅在空中起跳
        if (!isAirborne) return;
        if (isGraspWall) return;
        if (jumpSteps >= JumpCount) return;
        if (canJump && input.jumpPressed)
        {
            if (input.jumpPressTimer < JumpToleranceTime)
            {
                jumpSteps += 1;
                canJump = false;
                isRising = true;

                isJumping = false;
                isDoubleJumping = true;
                isWallJumping = false;
                speed.y = MaxJump;
                speed.x = JumpHBoost * input.move.x;
                // todo 停顿 4 帧(模仿蓄力起跳效果)
                //Game.Freeze(0.03f);
                GameConsole.Log("Jump2Begin: ", new { color = "red", Speed = speed });
            }
        }
    }

    private void JumpRising(CharacterInput input, float deltaTime)
    {
        if (input.jumpPressed && jumpTimer < JumpTime)
        {
            speed.y = MaxJump;
            speed.x = isWallJumping ? MaxRun * -input.move.x : speed.x;
            jumpTimer = Mathf.Min(jumpTimer + deltaTime, JumpTime);
            //GameConsole.Log("Rise: ", new { color = "yellow", _WallJump = isWallJumping }, new { color = "red", Speed = speed });
        }
        else
        {
            isRising = false;
        }
    }

    private void JumpFalling(float deltaTime)
    {
        if (isGraspWall && isAirborne) // WallSlide
        {
            //isJumping = false;
            //isDoubleJumping = false;
            //isWallJumping = false;
            isWallSliding = true;
            WallSlide(deltaTime);
        }
        else                //Fall
        {
            isWallSliding = false;
        }
    }

    private void WallSlide(float deltaTime)
    {
        speed.y = MathEx.Approach(speed.y, MaxSlide, Gravity * deltaTime);
        //Console.Log("WallSlide: ", new { color = "yellow", WallSliding = isWallSliding }, new { color = "red", SpeedY = speed.y });
    }

    private void JumpEnd(float deltaTime)
    {
        jumpSteps = 0;
        jumpTimer = 0;
        canJump = true;
        isRising = false;
        isJumping = false;
        isWallJumping = false;
        isDoubleJumping = false;
        isWallSliding = false;
        speed.x *= JumpEndMult; // 落地后速度
        //Game.Freeze(0.02f);
        Debug.Log("JumpEnd speedY: " + speed);
    }
    #endregion

    #region DASH METHOD
    public void DashUpdate(CharacterInput input, float deltaTime)
    {
        DashBegin(input);
        if (isDashing)
        {
            // 冲刺
            if (dashTimer < DashTime)
            {
                speed.y = -MathEx.MIN;
                speed.x = MathEx.Approach(speed.x, MaxDash * dashDirection.x, DashAccel * deltaTime);
                dashTimer = Mathf.Min(dashTimer + deltaTime, DashTime);
                //Debug.Log("DashUpdate 冲刺: " + speed);
            }
            else
            {
                // 减速
                speed.y = -MathEx.MIN;
                speed.x = MathEx.Approach(speed.x, dashBefore.x, DashReduce * deltaTime);
                //Debug.Log("DashUpdate 减速: " + speed);
            }

            // 碰墙或者速度减速到闪避前速度停止闪避
            if (isGraspWall || Mathf.Abs(speed.x - dashBefore.x) < MathEx.MIN)
            {
                DashEnd();
            }
        }
    }

    private void DashBegin(CharacterInput input)
    {
        if (canDash && input.dashPressed)
        {
            if (input.dashPressTimer < DashToleranceTime)
            {

                dashSteps += 1;
                canDash = false;
                isRising = false;
                isDashing = true;

                isJumping = false;
                isDoubleJumping = false;
                isWallJumping = false;

                dashBefore = speed;
                dashDirection = new Vector2(MathEx.Sign(isFacingRight), 0);

                speed.x = DashHBoost * input.move.x;
            }
        }
    }

    private void DashEnd()
    {
        dashTimer = 0;
        canDash = true;
        isDashing = false;
        dashBefore = Vector2.zero;
        dashDirection = Vector2.zero;
        Debug.Log("DashEnd speedX: " + speed);
    }
    #endregion
}
