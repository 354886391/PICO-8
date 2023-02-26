using Assets.Scripts.Normal.Character;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class Player : MonoBehaviour
{
    //public const float MaxFall = 160f;
    //private const float Gravity = 900f;
    //private const float HalfGravThreshold = 40f;
    //private const float FastMaxFall = 240f;
    //private const float FastMaxAccel = 300f;
    //public const float MaxRun = 90f;
    //public const float RunAccel = 1000f;
    //private const float RunReduce = 400f;
    //private const float AirMult = 0.65f;
    //private const float HoldingMaxRun = 70f;
    //private const float HoldMinTime = 0.35f;
    //private const float BounceAutoJumpTime = 0.1f;
    //private const float DuckFriction = 500f;
    //private const int DuckCorrectCheck = 4;
    //private const float DuckCorrectSlide = 50f;
    //private const float DodgeSlideSpeedMult = 1.2f;
    //private const float DuckSuperJumpXMult = 1.25f;
    //private const float DuckSuperJumpYMult = 0.5f;
    //private const float JumpGraceTime = 0.1f;
    //private const float JumpSpeed = -105f;
    //private const float JumpHBoost = 40f;
    //private const float VarJumpTime = 0.2f;
    //private const float CeilingVarJumpGrace = 0.05f;
    //private const int UpwardCornerCorrection = 4;
    //private const int DashingUpwardCornerCorrection = 5;
    //private const float WallSpeedRetentionTime = 0.06f;
    //private const int WallJumpCheckDist = 3;
    //private const int SuperWallJumpCheckDist = 5;
    //private const float WallJumpForceTime = 0.16f;
    //private const float WallJumpHSpeed = 130f;
    //public const float WallSlideStartMax = 20f;
    //private const float WallSlideTime = 1.2f;
    //private const float BounceVarJumpTime = 0.2f;
    //private const float BounceSpeed = -140f;
    //private const float SuperBounceVarJumpTime = 0.2f;
    //private const float SuperBounceSpeed = -185f;
    //private const float SuperJumpSpeed = -105f;
    //private const float SuperJumpH = 260f;
    //private const float SuperWallJumpSpeed = -160f;
    //private const float SuperWallJumpVarTime = 0.25f;
    //private const float SuperWallJumpForceTime = 0.2f;
    //private const float SuperWallJumpH = 170f;
    //private const float DashSpeed = 240f;
    //private const float EndDashSpeed = 160f;
    //private const float EndDashUpMult = 0.75f;
    //private const float DashTime = 0.15f;
    //private const float SuperDashTime = 0.3f;
    //private const float DashCooldown = 0.2f;
    //private const float DashRefillCooldown = 0.1f;
    //private const int DashHJumpThruNudge = 6;
    //private const int DashCornerCorrection = 4;
    //private const int DashVFloorSnapDist = 3;
    //private const float DashAttackTime = 0.3f;
    //private const float BoostMoveSpeed = 80f;
    //public const float BoostTime = 0.25f;
    //private const float DuckWindMult = 0.0f;
    //private const int WindWallDistance = 3;
    //private const float ReboundSpeedX = 120f;
    //private const float ReboundSpeedY = -120f;
    //private const float ReboundVarJumpTime = 0.15f;
    //private const float ReflectBoundSpeed = 220f;
    //private const float DreamDashSpeed = 240f;
    //private const int DreamDashEndWiggle = 5;
    //private const float DreamDashMinTime = 0.1f;
    //public const float ClimbMaxStamina = 110f;
    //private const float ClimbUpCost = 45.4545441f;
    //private const float ClimbStillCost = 10f;
    //private const float ClimbJumpCost = 27.5f;
    //private const int ClimbCheckDist = 2;
    //private const int ClimbUpCheckDist = 2;
    //private const float ClimbNoMoveTime = 0.1f;
    //public const float ClimbTiredThreshold = 20f;
    //private const float ClimbUpSpeed = -45f;
    //private const float ClimbDownSpeed = 80f;
    //private const float ClimbSlipSpeed = 30f;
    //private const float ClimbAccel = 900f;
    //private const float ClimbGrabYMult = 0.2f;
    //private const float ClimbHopY = -120f;
    //private const float ClimbHopX = 100f;
    //private const float ClimbHopForceTime = 0.2f;
    //private const float ClimbJumpBoostTime = 0.2f;
    //private const float ClimbHopNoWindTime = 0.3f;
    //private const float LaunchSpeed = 280f;
    //private const float LaunchCancelThreshold = 220f;
    //private const float LiftYCap = -130f;
    //private const float LiftXCap = 250f;
    //private const float JumpThruAssistSpeed = -40f;
    //private const float FlyPowerFlashTime = 0.5f;
    //private const float ThrowRecoil = 80f;
    //private static readonly Vector2 CarryOffsetTarget = new Vector2(0.0f, -12f);
    //private const float ChaserStateMaxTime = 4f;
    //public const float WalkSpeed = 64f;
    //private const float LowFrictionMult = 0.35f;
    //private const float LowFrictionAirMult = 0.5f;
    //private const float LowFrictionStopTime = 0.15f;
    //private const float HiccupTimeMin = 1.2f;
    //private const float HiccupTimeMax = 1.8f;
    //private const float HiccupDuckMult = 0.5f;
    //private const float HiccupAirBoost = -60f;
    //private const float HiccupAirVarTime = 0.15f;
    //private const float GliderMaxFall = 40f;
    //private const float GliderWindMaxFall = 0.0f;
    //private const float GliderWindUpFall = -32f;
    //public const float GliderFastFall = 120f;
    //private const float GliderSlowFall = 24f;
    //private const float GliderGravMult = 0.5f;
    //private const float GliderMaxRun = 108.000008f;
    //private const float GliderRunMult = 0.5f;
    //private const float GliderUpMinPickupSpeed = -105f;
    //private const float GliderDashMinPickupSpeed = -240f;
    //private const float GliderWallJumpForceTime = 0.26f;
    //private const float DashGliderBoostTime = 0.55f;


    public const int StNormal = 0;
    public const int StClimb = 1;
    public const int StDash = 2;
    public const int StJump = 3;
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
    private const float MaxRun = 9f;
    private const float MaxWalk = 6f;
    private const float RunAccel = 100f;
    private const float RunReduce = 40f;

    private const float DuckFriction = 50f;
    private const float DuckCorrectSlide = 5f;
    private const float DodgeSlideSpeedMult = 1.2f;


    private const float HitSquashNoMoveTime = 0.1f;
    private const float HitSquashFriction = 80f;

    #endregion

    #region GRAVITY

    private const float Gravity = 9f;
    private const float GravThreshold = 4f;

    private const float MaxFall = -16f;
    private const float FastMaxFall = -24f;
    private const float FastMaxAccel = 30f;

    private const float AirMult = 0.65f;
    #endregion

    #region JUMP    // 跳跃
    private const float JumpHBoost = 4f;
    private const float JumpSpeed = 10.5f;

    private const float JumpTime = 0.2f;
    private const float JumpToleranceTime = 0.15f;
    private const float JumpCount = 2;
    private const float JumpThreshold = 4f;
    private const float JumpEndMult = 0.35f;

    private const float WallSpeedRetentionTime = .06f;

    #endregion

    #region SLIDE   //滑落
    private const float GrabWall = 0;
    private const float MaxSlide = -8f;


    public const float WallSlideStartMax = 20f;
    private const float WallSlideTime = 1.2f;
    #endregion

    #region DASH   // 冲刺
    private const float DashHBoost = 4;
    private const float DashSpeed = 24;
    private const float EndDashSpeed = 16;
    private const float EndDashUpMult = .75f;
    private const float DashAccel = 160;
    private const float DashReduce = 120;
    private const float DashTime = 0.15f;
    private const float DashToleranceTime = 0.15f;
    private const float DashCooldownTime = 0.2f;
    private const float DashTotalCount = 0;
    private const float DashThreshold = 3;
    #endregion
    #endregion

    public enum AirborneType
    {
        None = 0,
        Rising,
        Hovering,
        Falling,
    }

    public int moveX;
    public int moveY;
    public float maxFall;
    public Vector2 speed;           // 期望速度
    public Vector2 movement;        // 实际位移

    public bool onGround;
    public bool wasOnGround;

    public bool grabWall;
    public bool facingRight;

    public AirborneType jumpType;

    private float hitSquashNoMoveTimer;

    public bool Ducking;

    private Facings facing;

    #region JUMP
    [TitleGroup("JUMP")]
    public bool canJump;    // 是否可以跳跃, 检测后立即置否   

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
    private float dashRefillCooldownTimer;
    private float wallSlideTimer = WallSlideTime;
    private Vector2 dashBeforeSpeed;      // 闪避前速度
    private Vector2 dashDir;   // dashing 时的方向
    #endregion

    #region OTHER
    private Rigidbody2D rigid2d;
    private BodyCollider2D detection;
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
            if (value != Vector2.zero && LiftSpeedGraceTime > 0.0)
            {
                lastLiftSpeed = value;
                liftSpeedTimer = LiftSpeedGraceTime;
            }
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

    private bool CanJump
    {
        get
        {
            if (canJump && onGround)
            {
                return true;
            }
            return false;
        }
    }

    private bool CanDash
    {
        get;
    }

    private void Awake()
    {
        canJump = true;
        canDash = true;
        Application.targetFrameRate = 60;
        rigid2d = GetComponent<Rigidbody2D>();
        detection = GetComponent<BodyCollider2D>();
    }

    private void Start()
    {
        _machine = new StateMachine(15);
        _machine.SetCallbacks(StNormal, update: NormalUpdate, coroutine: null, begin: NormalBegin, end: NormalEnd);
        _machine.SetCallbacks(StClimb, update: null, coroutine: null, begin: null, end: null);
        _machine.SetCallbacks(StDash, update: DashUpdate, coroutine: DashCoroutine, begin: DashBegin, end: DashEnd);
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

        detection.onCollideH = OnCollideH;
        detection.onCollideV = OnCollideV;
    }

    private void Update()
    {

        //Dashes
        {
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;

            if (dashRefillCooldownTimer > 0)
                dashRefillCooldownTimer -= Time.deltaTime;
        }

        //Facing
        if (moveX != 0)
        {
            facing = (Facings)moveX;
        }
    }



    private float wallSpeedRetentionTimer; // If you hit a wall, start this timer. If coast is clear within this timer, retain h-speed
    private float wallSpeedRetained;

    private void OnCollideH(CollisionData data)
    {
        speed.x = 0;
        if (_machine.State == StRedDash)
        {
            _machine.State = StHitSquash;
        }
    }

    private void OnCollideV(CollisionData data)
    {
        speed.y = 0;
        if (_machine.State == StRedDash)
        {
            _machine.State = StHitSquash;
        }
    }

    private void Falling(float deltaTime)
    {
        float mf = MaxFall;
        float fmf = FastMaxFall;
        //Fast Fall
        if (moveY == -1 && speed.y < mf)
        {
            maxFall = MathEx.Approach(maxFall, fmf, FastMaxAccel * deltaTime);
        }
        else
        {
            maxFall = MathEx.Approach(maxFall, mf, FastMaxAccel * deltaTime);
        }
    }

    private void Graviting(float deltaTime)
    {
        // Gravity
        if (!onGround)
        {
            // 在空中 y轴速度在[-4, 4]之间时, 且处于冲刺结束状态或按住跳跃键时以0.5倍的加速度下落
            var mult = (Math.Abs(speed.y) < GravThreshold && (MInput.Jump.Check /*|| autoJump*/)) ? 0.5f : 1f;
            speed.y = MathEx.Approach(speed.y, maxFall, Gravity * mult * deltaTime);
        }
    }

    private void Running(float deltaTime)
    {
        // 在地面 按下下键以50的加速的减速
        if (Ducking && onGround)
        {
            speed.x = MathEx.Approach(speed.x, 0, DuckFriction * deltaTime);
        }
        else
        {
            var mult = onGround ? 1f : AirMult;
            if (Mathf.Abs(speed.x) > MaxRun && Mathf.Sign(speed.x) == moveX)
            {
                // 速度大于MaxRun, 按住同向方向键以RunReduce的加速度减速到MaxRun
                speed.x = MathEx.Approach(speed.x, MaxRun * moveX, RunReduce * mult * deltaTime);
            }
            else
            {
                // 速度小于MaxRun或速度大于MaxRun时, 按住异向键速度以RunReduce的加速度加速到MaxRun
                speed.x = MathEx.Approach(speed.x, MaxRun * moveX, RunAccel * mult * deltaTime);
            }
        }
    }

    private void NormalBegin()
    {
        maxFall = MaxFall;
    }

    private void NormalEnd()
    {
        wallSpeedRetentionTimer = 0;
    }

    // idle / fall / gravity / run
    private int NormalUpdate()
    {
        // Vertical
        Falling(Time.deltaTime);
        Graviting(Time.deltaTime);
        Running(Time.deltaTime);

        JumpUpdate(Time.deltaTime);

        if (canDash && MInput.Dash.Pressed)
        {
            return StDash;
        }

        return StNormal;
    }

    // 落地起跳 仅当前状态为StRedDash 且 发生碰撞时进入
    private void HitSquashBegin()
    {
        hitSquashNoMoveTimer = HitSquashNoMoveTime;
    }

    private int HitSquashUpdate()
    {
        // 以80的加速度减速到0
        speed.x = MathEx.Approach(speed.x, 0, HitSquashFriction * Time.deltaTime);
        speed.y = MathEx.Approach(speed.y, 0, HitSquashFriction * Time.deltaTime);

        if (MInput.Jump.Pressed)
        {
            if (onGround)
            {
                //Jump(); WallJump(); 

                return StNormal;
            }
        }

        if (canDash)
        {
            //return startDash();
        }
        if (MInput.Grab.Check)
        {
            return StClimb;
        }
        if (hitSquashNoMoveTimer > 0)
        {
            hitSquashNoMoveTimer -= Time.deltaTime;
        }
        else
        {
            return StNormal;
        }

        return StHitSquash;
    }

    #region JUMP METHOD
    private void JumpBegin()
    {
        // 仅在地面起跳
        if (!onGround) return;
        if (canJump && MInput.Jump.Pressed)
        {
            MInput.Jump.ConsumeBuffer();
            canJump = false;
            isJumping = true;

            jumpSteps = 1;
            jumpTimer = JumpTime;

            speed.y = JumpSpeed;
            speed.x += JumpHBoost * moveX;
            GameConsole.Log("JumpBegin: ", new { color = "red", Speed = speed });
        }
    }

    /// <summary>
    /// 当第一次跳跃过程中,在空中撞到墙壁角色会粘到墙上并缓慢下落
    /// 按住方向键, 按下跳跃键向反方向踢墙跳跃
    /// </summary>
    private void WallJumpBegin(CharacterInput input)
    {

    }

    private void Jump2Begin()
    {
        // 仅在空中起跳
        if (!onGround) return;
        if (jumpSteps >= JumpCount) return;
        if (canJump && MInput.Jump.Pressed)
        {
            MInput.Jump.ConsumeBuffer();
            canJump = false;
            isJumping = true;

            jumpSteps += 1;
            jumpTimer = JumpTime;

            speed.y = JumpSpeed;
            speed.x = JumpHBoost * moveX;
            GameConsole.Log("Jump2Begin: ", new { color = "red", Speed = speed });
        }
    }

    private void JumpEnd(float deltaTime)
    {

        canJump = true;
        isJumping = false;

        jumpSteps = 0;
        jumpTimer = 0f;

        speed.y = 0f;
        speed.x *= JumpEndMult; // 落地后速度
        Debug.Log("JumpEnd speedY: " + speed);
    }

    public void JumpUpdate(float deltaTime)
    {
        JumpBegin();
        Jump2Begin();
        if (isJumping)
        {
            if (jumpTimer > 0f && MInput.Jump.Check)    // && !touchCelling
            {
                speed.y = JumpSpeed;
                jumpTimer -= Time.deltaTime;
                jumpType = AirborneType.Rising;
            }
            else if (!onGround)
            {
                jumpType = AirborneType.Falling;
            }
            else
            {
                jumpType = AirborneType.None;
                JumpEnd(deltaTime);
            }

            if (!onGround && Mathf.Abs(speed.y) < JumpThreshold)
            {
                jumpType = AirborneType.Hovering;
            }
        }
    }
    #endregion

    private void DashBegin()
    {
        // Freeze(0.05f);
        dashCooldownTimer = 0.2f;
        dashRefillCooldownTimer = 0.1f;
        wallSlideTimer = 1.2f;
        dashBeforeSpeed = speed;
        dashDir = GetAimVector(facing);
        speed = Vector2.zero;
        if (!onGround && Ducking)
        {
            Ducking = false;
        }
        else if (!Ducking && MInput.MoveY == -1)
        {
            Ducking = true;
        }
    }

    private void DashEnd()
    {

    }

    // Dash状态跳转
    private int DashUpdate()
    {
        if (MInput.Grab.Check)
        {
            return StPickup;
        }

        if (dashDir.y == 0)
        {
            if (MInput.Jump.Pressed)
            {
                //SuperJump();
                return StNormal;
            }
        }
        if (dashDir.x == 0 && dashDir.y == -1)
        {
            if (MInput.Jump.Pressed)
            {
                //if (WallJumpCheck(1))
                //{
                //    //SuperWallJump(-1);
                //    return StNormal;
                //}
                //else if (WallJumpCheck(-1))
                //{
                //    SuperWallJump(1);
                //    return StNormal;
                //}
            }
        }
        else
        {
            if (MInput.Jump.Pressed)
            {
                //if (WallJumpCheck(1))
                //{
                //    WallJump(-1);
                //    return StNormal;
                //}
                //else if (WallJumpCheck(-1))
                //{
                //    WallJump(1);
                //    return StNormal;
                //}
            }
        }
        return StDash;
    }

    // Dash位置更新
    private IEnumerator DashCoroutine()
    {
        yield return null;
        //
        speed = NewDashSpeed(dashBeforeSpeed);
        //Dash Slide
        //如果斜下冲冲刺到地面则会变成蹲姿、然后横向速度变成 1.2 倍.
        if (onGround && dashDir.x != 0 && dashDir.y < 0 && speed.y < 0)
        {
            dashDir.x = Math.Sign(dashDir.x);
            dashDir.y = 0;
            speed.y = 0;
            speed.x *= DodgeSlideSpeedMult;
            Ducking = true; // 蹲姿
        }
        yield return DashTime;

        if (dashDir.y >= 0)
        {
            speed = dashDir * EndDashSpeed;
        }
        if (speed.y > 0)
        {
            speed.y *= EndDashUpMult;
        }
        _machine.State = StNormal;
    }

    private void RedDashBegin()
    {

    }

    private void RedDashEnd()
    {

    }

    private int RedDashUpdate()
    {
        return 0;
    }

    public Vector2 GetAimVector(Facings defaultFacing = Facings.Right)
    {
        var dir = MInput.Move;
        if (dir == Vector2.zero)
        {
            return Vector2.right * (float)defaultFacing;
        }
        else
        {
            return new Vector2(Math.Sign(dir.x), Math.Sign(dir.y)).normalized;
        }
    }

    /// <summary>
    /// 冲刺的速度计算: 以时针 12 点方向为 0 度, 顺时针定角度 radians
    /// </summary>
    /// <param name="originalSpeed"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector2 NewDashSpeed(Vector2 originalSpeed)
    {
        var newSpeed = dashDir * DashSpeed;
        if (originalSpeed.x * newSpeed.x > 0f && Mathf.Abs(originalSpeed.x) > Mathf.Abs(newSpeed.x))
        {
            newSpeed.x = originalSpeed.x;
        }
        return newSpeed;
    }

    private float getRadians(float angle)
    {
        return angle / 180 * Mathf.PI;
    }
}
