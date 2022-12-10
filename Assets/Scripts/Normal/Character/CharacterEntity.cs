using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class CharacterEntity : ScriptableObject
{
    #region State
    public bool CanMove;
    public bool Freezing;
    public bool OnGround;
    public bool WasOnGround;    // 上一帧是否在地面
    public bool AgainstWall;    // 当前帧是否贴墙壁

    public States State;                    // 当前状态         
    public Facings Facing;                  // 角色面部朝向
    public Vector2 Speed;       // 移动速度
    public Rigidbody2D Rigidbody;
    public BoxCollider2D BoxCollider;
    public bool IsRising { get { return !OnGround && Speed.y > Constants.MinOffset; } }
    public bool IsFalling { get { return !OnGround && Speed.y < Constants.MinOffset; } }
    #endregion

    #region Raycast
    public float VRaysCount = 5;
    public float HRaysCount = 5;
    public float VRaysInterval;
    public float HRaysInterval;

    public Bounds RayOrigin;       // 角色边框坐标
    public LayerMask GroundMask;    // 单项平台的检测
    public LayerMask PlatformMask;   // 双向平台的检测   
    public RaycastHit2D RaycastGround;
    public RaycastHit2D RaycastWall;
    #endregion

    #region Input
    private bool _jump = true;                 // Press
    private bool _dash = true;
    private bool _climb = true;
    private float _moveX;
    private float _moveY;

    [ShowInInspector] public bool CanJump { get; set; }             // Release    
    [ShowInInspector] public bool CanDash { get; set; }
    [ShowInInspector] public bool CanClimb { get; set; }
    [ShowInInspector] public float DeltaTime { get; set; }
    [ShowInInspector] public float HeldDownTimer { get; set; }          // HoldTime

    [ShowInInspector]
    public bool Jump
    {
        get { return _jump; }
        set
        {
            if (_jump && !value)
            {
                CanJump = true;
                HeldDownTimer = 0.0f;
            }
            _jump = value;
            if (_jump)
            {
                HeldDownTimer += DeltaTime;
            }
        }
    }

    [ShowInInspector]
    public bool Dash
    {
        get { return _dash; }
        set
        {
            if (_dash && !value)
            {
                CanDash = true;
                HeldDownTimer = 0.0f;
            }
            _dash = value;
            if (_dash)
            {
                HeldDownTimer += DeltaTime;
            }
        }
    }

    [ShowInInspector]
    public bool Climb
    {
        get { return _climb; }
        set
        {
            if (_climb && !value)
            {
                CanClimb = true;
                HeldDownTimer = 0.0f;
            }
            _climb = value;
            if (_climb)
            {
                HeldDownTimer += DeltaTime;
            }
        }
    }

    [ShowInInspector]
    public float MoveX
    {
        get => _moveX;
        set => _moveX = value;
    }

    [ShowInInspector]
    public float MoveY
    {
        get => _moveY;
        set => _moveY = value;
    }
    #endregion

    #region Move
    public readonly float MaxRun = 9f;
    public readonly float MaxFall = -16f;
    public readonly float FallAccel = 30f;
    public readonly float RunAccel = 100f;
    public readonly float RunReduce = 40f;
    public readonly float Gravity = 90f;
    public readonly float HalfGravThreshold = 4f;
    public readonly float GroundMult = 1.0f;
    public readonly float AirMult = 0.65f;
    #endregion

    #region Jump
    public readonly float JumpHBoost = 4f;
    public readonly float JumpWBoost = 6f;
    public readonly float JumpSpeed = 10.5f;
    public readonly float JumpTime = 0.2f;
    public readonly float JumpToleranceTime = 0.15f;
    public readonly int JumpMaxMidAir = 1;   // 0 or 1

    public bool IsJumping;
    public bool IsJumpUpdate;
    public bool IsJumpCooldown;

    public int JumpMidAir;
    public float JumpTimer;
    public float JumpHeldDownTimer;

    public Action JumpBeginEvent;
    public Action JumpUpdateEvent;
    public Action JumpEndEvent;
    public Action JumpLandEvent;
    public Action JumpMidAirBeginEvent;
    #endregion

    #region Dash
    public readonly float DashSpeed = 24f;
    public readonly float DashTime = 0.15f;
    public readonly float DashToleranceTime = 0.15f;
    public readonly float DashCooldownTime = 0.2f;

    public bool IsDashing;
    public bool IsDashUpdate;
    public bool IsDashCooldown;
    public bool IsDashFreezing;

    public float DashTimer;
    public float DashHeldDownTimer;
    public float DashCooldownTimer;

    public Vector2 DashDirection;
    public Vector2 DashBeforeSpeed;

    public Action DashBeginEvent;
    public Action DashUpdateEvent;
    public Action DashEndEvent;
    public Action DashLandEvent;
    #endregion

    #region Climb
    public readonly float ClimbUpSpeed = 4.5f;
    public readonly float ClimbDownSpeed = -6f;
    public readonly float ClimbSlipSpeed = -3f;
    public readonly float ClimbTime = 2.0f;
    public readonly float ClimbToleranceTime = 0.15f;
    public readonly float ClimbCooldownTime = 0.2f;
    public readonly float ClimbAccel = 90f;
    public readonly float ClimbSlideAccel = 90f;
    public readonly float ClimbGrabYMult = .2f;


    public bool IsClimbing;
    public bool IsClimbUpdate;
    public bool IsClimbCooldown;

    public float ClimbTimer;
    public float ClimbHeldDownTimer;
    public float ClimbCooldownTimer;
    public float ClimbWallSlideTimer;
    public float ClimbNoMoveTimer;

    public Action ClimbBeginEvent;
    public Action ClimbUpdateEvent;
    public Action ClimbEndEvent;
    public Action ClimbLandEvent;
    #endregion
}
