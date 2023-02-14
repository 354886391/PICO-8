using Assets.Scripts.Normal.Character;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

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

    #region JUMP    // ��Ծ
    private const float JumpHBoost = 4f;
    private const float JumpMax = 10.5f;

    private const float JumpTime = 0.2f;
    private const float JumpToleranceTime = 0.15f;
    private const float JumpCount = 2;
    private const float JumpThreshold = 4f;
    private const float JumpEndMult = 0.35f;

    private const float WallSpeedRetentionTime = .06f;

    #endregion

    #region SLIDE   //����
    private const float GrabWall = 0;
    private const float MaxSlide = -8f;
    #endregion

    #region DASH   // ���
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

    public int moveX;
    public int moveY;
    public float maxFall;
    public Vector2 speed;           // �����ٶ�
    public Vector2 movement;        // ʵ��λ��

    public bool onGround;
    public bool wasOnGround;

    public bool grabWall;
    public bool facingRight;

    private float hitSquashNoMoveTimer;


    public bool Ducking;

    #region JUMP

    [TitleGroup("JUMP")]
    public bool canJump;    // �Ƿ������Ծ, ���������÷�   
    public bool isRising;   // ��Ծ�������׶�(�������Ϳյ�ǰ���)  
    public bool isJumping;  // ��������, �Ϳպ��½��׶�      
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
    private Vector2 dashBeforeSpeed;      // ����ǰ�ٶ�
    private Vector2 dashDir;   // dashing ʱ�ķ���
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

        detection.onCollideH = OnCollideH;
        detection.onCollideV = OnCollideV;
    }

    private void Update()
    {

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
            // �ڿ��� y���ٶ���[-4, 4]֮��ʱ, �Ҵ��ڳ�̽���״̬��ס��Ծ��ʱ��0.5���ļ��ٶ�����
            var mult = (Math.Abs(speed.y) < GravThreshold && (MInput.Jump.Check /*|| autoJump*/)) ? 0.5f : 1f;
            speed.y = MathEx.Approach(speed.y, maxFall, Gravity * mult * deltaTime);
        }
    }

    private void Running(float deltaTime)
    {
        // �ڵ��� �����¼���50�ļ��ٵļ���
        if (Ducking && onGround)
        {
            speed.x = MathEx.Approach(speed.x, 0, DuckFriction * deltaTime);
        }
        else
        {
            var mult = onGround ? 1f : AirMult;
            if (Mathf.Abs(speed.x) > MaxRun && Mathf.Sign(speed.x) == moveX)
            {
                // �ٶȴ���MaxRun, ��סͬ�������RunReduce�ļ��ٶȼ��ٵ�MaxRun
                speed.x = MathEx.Approach(speed.x, MaxRun * moveX, RunReduce * mult * deltaTime);
            }
            else
            {
                // �ٶ�С��MaxRun���ٶȴ���MaxRunʱ, ��ס������ٶ���RunReduce�ļ��ٶȼ��ٵ�MaxRun
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

        if (canJump && MInput.Jump.Pressed)
        {
            return StJump;
        }

        if (canDash && MInput.Dash.Pressed)
        {
            return StDash;
        }

        return StNormal;
    }

    // ������� ����ǰ״̬ΪStRedDash �� ������ײʱ����
    private void HitSquashBegin()
    {
        hitSquashNoMoveTimer = HitSquashNoMoveTime;
    }

    private int HitSquashUpdate()
    {
        // ��80�ļ��ٶȼ��ٵ�0
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

    // ƽ����Ծ
    private void JumpBegin()
    {
        MInput.Jump.ConsumeBuffer();
        jumpTimer = JumpTime;
        // ƽ����Ծ D+4, 10.5�ĳ��ٶ�
        speed.x += JumpHBoost * moveX;
        speed.y = JumpMax;
        speed += LiftBoost;
        jumpSpeed = speed.y;
        // LaunchedBoostCheck();
    }

    private void JumpEnd()
    {
        jumpTimer = 0f;
        jumpSpeed = 0f;
    }

    private int JumpUpdate()
    {
        if (jumpTimer > 0f && MInput.Jump.Check)
        {
            Running(Time.deltaTime);
            speed.y = Math.Min(speed.y, jumpSpeed);
            jumpTimer -= Time.deltaTime;
            return StJump;
        }
        else
        {
            jumpTimer = 0f;
        }
        return StNormal;
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

    // Dash״̬��ת
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

    // Dashλ�ø���
    private IEnumerator DashCoroutine()
    {
        yield return null;

        var newSpeed = dashDir * DashSpeed;
        if (Math.Sign(dashBeforeSpeed.x) == Math.Sign(newSpeed.x) && Math.Abs(dashBeforeSpeed.x) > Math.Abs(newSpeed.x))
        {
            newSpeed.x = dashBeforeSpeed.x;
        }
        speed = newSpeed;
        if (dashDir.x != 0)
        {
            //facingRight = Math.Sign(dashDir.x);
        }
        //Dash Slide
        if (onGround && dashDir.x != 0 && dashDir.y < 0 && speed.y <= 0)
        {
            dashDir.x = Math.Sign(dashDir.x);
            dashDir.y = 0;
            speed.y = 0;
            speed.x *= DodgeSlideSpeedMult;
            Ducking = true; // ���ٵĵ�ͷ
        }

        //if (dashDir.x != 0 && MInput.Grab.Check)
        //{
        //    var swapBlock = CollideFirst<SwapBlock>(Position + Vector2.right * Math.Sign(dashDir.x));
        //    if (swapBlock != null && swapBlock.Direction.X == Math.Sign(dashDir.x))
        //    {
        //        _machine.State = StClimb;
        //        speed = Vector3.zero;
        //        yield break;
        //    }
        //}
        yield return DashTime;

        //AutoJump = true;
        //AutoJumpTimer = 0;
        //if (DashDir.Y <= 0)
        //{
        //    Speed = DashDir * EndDashSpeed;
        //    Speed.X *= swapCancel.X;
        //    Speed.Y *= swapCancel.Y;
        //}
        //if (Speed.Y < 0)
        //    Speed.Y *= EndDashUpMult;
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


}
