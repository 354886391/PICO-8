using Assets.Scripts.Normal.Character;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public const int StNormal = 0;
    public const int StClimb = 1;
    public const int StDash = 2;
    public const int StSwim = 3;
    public const int StBoost = 4;
    public const int StRedDash = 5;
    public const int StHitSquash = 6;
    public const int StLaunch = 7;
    public const int StPickup = 8;
    public const int StDreamDash = 9;
    public const int StReflectionFall = 18;
    public const int StStarFly = 19;
    public const int StTempleFall = 20;
    public const int StCassetteFly = 21;
    public const int StAttract = 22;

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
    private const float JumpMax = 10.5f;

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
    private const float DashSpeed = 24;
    private const float DashAccel = 160;
    private const float DashReduce = 120;
    private const float DashTime = 0.15f;
    private const float DashToleranceTime = 0.15f;
    private const float DashCooldownTime = 0.2f;
    private const float DashTotalCount = 0;
    private const float DashThreshold = 3;
    #endregion
    #endregion

    public bool canMove;
    public Vector2 speed;           // 期望速度
    public Vector2 movement;        // 实际位移

    [TitleGroup("MOVE"), ShowInInspector]
    public bool isOnGround => detection ? detection.groundHit.isHit : false;
    [TitleGroup("MOVE"), ShowInInspector]
    public bool isGrapWall => detection ? detection.wallHit.isHit : false;
    public bool isAirborne => false;
    public bool isFacingRight => false;

    #region JUMP

    [TitleGroup("JUMP")]
    public bool canJump;    // 是否可以跳跃, 检测后立即置否   
    public bool isRising;   // 跳跃的上升阶段(不包括滞空的前半段)  
    public bool isJumping;  // 包括上升, 滞空和下降阶段      
    public bool isWallJumping;
    public bool isDoubleJumping;
    public bool isWallSliding;
    public int jumpSteps;
    public float jumpTimer;
    public float jumpSpeed;
    #endregion

    #region DASH
    [TitleGroup("DASH")]
    public bool canDash;
    public bool isDashing;
    public bool isDashRight;
    public int dashSteps;
    public float dashTimer;
    public float dashCooldownTimer;
    private Vector2 dashBeforeSpeed;      // 闪避前速度
    private Vector2 dashDir;   // dashing 时的方向
    #endregion

    #region OTHER
    private Rigidbody2D rigid2d;
    private BodyDetection detection;
    #endregion

    [SerializeField]
    public CharacterInput _input;
    [SerializeField]
    public CharacterMovement _movement;
    [SerializeField]
    public CharacterCamera _cameraFollow;
    [SerializeField]
    public StateMachine _machine;


    private float liftSpeedTimer;
    private float LiftSpeedGraceTime = 0.16f;
    private Vector2 lastLiftSpeed;
    private Vector2 currentLiftSpeed;


    public Vector2 LiftSpeed
    {
        set
        {
            currentLiftSpeed = value;
            if (!(value != Vector2.zero) || LiftSpeedGraceTime <= 0.0)
                return;
            lastLiftSpeed = value;
            liftSpeedTimer = LiftSpeedGraceTime;
        }
        get => currentLiftSpeed == Vector2.zero ? lastLiftSpeed : currentLiftSpeed;
    }

    private Vector2 LiftBoost
    {
        get
        {
            Vector2 liftSpeed = LiftSpeed;
            if ((double)Math.Abs(liftSpeed.x) > 250.0)
                liftSpeed.x = 250f * Math.Sign(liftSpeed.x);
            if (liftSpeed.y > 0.0)
                liftSpeed.y = 0.0f;
            else if (liftSpeed.y < -130.0)
                liftSpeed.y = -130f;
            return liftSpeed;
        }
    }

    private void Awake()
    {
        canMove = true;
        canJump = true;
        canDash = true;
        Application.targetFrameRate = 60;
        rigid2d = GetComponent<Rigidbody2D>();
        detection = GetComponent<BodyDetection>();
    }

    private void Start()
    {
        _machine = new StateMachine(15);
        _machine.SetCallbacks(StNormal, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StClimb, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StDash, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StSwim, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StBoost, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StRedDash, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StHitSquash, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StLaunch, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StPickup, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StDreamDash, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StReflectionFall, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StStarFly, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StTempleFall, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StCassetteFly, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StAttract, update: null, coroutine: null, begin: null, end: null);

    }

    private void Update()
    {
        if (jumpTimer > 0)
        {
            jumpTimer = Math.Max(jumpTimer - Time.deltaTime, 0);
        }
    }

    private void GravityUpdate(float deltaTime)
    {
        if (!isOnGround)
        {
            var mult = Math.Abs(speed.y) > GravThreshold || !MInput.Jump.Check ? 1f : 0.5f;
            speed.y = MathEx.Approach(speed.y, MaxFall, Gravity * mult * deltaTime);
        }
    }

    private void RunUpdate(float deltaTime)
    {
        var mult = isOnGround ? 1f : AirMult;
        if ((Mathf.Abs(speed.x) > MaxRun && Mathf.Sign(speed.x) == MInput.Move.x))
        {
            speed.x = MathEx.Approach(speed.x, MaxRun * MInput.Move.x, RunReduce * mult * deltaTime);
        }
        else
        {
            speed.x = MathEx.Approach(speed.x, MaxRun * MInput.Move.x, RunAccel * mult * deltaTime);
        }
    }


    private void JumpBegin()
    {
        if (canJump && MInput.Jump.Pressed)
        {
            MInput.Jump.ConsumeBuffer();
            jumpTimer = JumpTime;
            speed.x += JumpHBoost * moveX;
            speed.y = JumpMax;
            speed += LiftBoost;
            jumpSpeed = speed.y;
            // LaunchedBoostCheck();
        }
    }

    private void JumpEnd()
    {
        jumpTimer = 0f;
        jumpSpeed = 0f;
    }

    private int JumpUpdate()
    {

        if (jumpTimer > 0f)
        {
            if (MInput.Jump.Check)
            {
                speed.y = Math.Min(speed.y, jumpSpeed);
            }
            else
            {
                jumpTimer = 0f;
            }
        }
        return 0;
    }

    private void DashBegin()
    {
        // Freeze(0.05f);
        dashCooldownTimer = 0.2f;

        dashBeforeSpeed = speed;
        dashDir = Vector2.zero;
        speed = Vector2.zero;
       
    }

    private void DashEnd()
    {

    }

    private int DashUpdate()
    {
        return 0;
    }

}
