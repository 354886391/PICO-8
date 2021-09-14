using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    #region Structs
    [System.Serializable]
    public struct RayOrigin
    {
        public Vector2 topLeft;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    #endregion

    #region Constants
    public const float MaxRun = 9f;
    public const float MaxFall = -16f;
    public const float FallAccel = 30f;
    public const float RunAccel = 100f;
    public const float RunReduce = 40f;

    public const float Gravity = 90f;
    public const float HalfGravThreshold = 4f;
    public const float AirMult = 0.65f;

    #region Jump
    public const float JumpHBoost = 4f;
    public const float JumpSpeed = 10.5f;
    public const float JumpTime = 0.2f;
    public const float JumpToleranceTime = 0.15f;

    public const int MaxMidAirJump = 0;   // 0 or 1
    #endregion

    #region Dash  
    public const float DashSpeed = 24f;
    public const float EndDashSpeed = 16f;
    public const float DashTime = 0.15f;
    public const float DashToleranceTime = 0.15f;
    public const float DashCooldownTime = 0.2f;
    #endregion

    #region Climb
    private const float ClimbUpSpeed = 4.5f;
    private const float ClimbDownSpeed = -6f;
    private const float ClimbSlipSpeed = -3f;
    private const float ClimbTime = 2.0f;
    private const float ClimbToleranceTime = 0.15f;
    private const float ClimbCooldownTime = 0.2f;
    private const float ClimbAccel = 90f;
    private const float SlideAccel = 90f;
    private const float ClimbGrabYMult = .2f;
    private const float ClimbNoMoveTime = .1f;
    private const float ClimbTiredThreshold = 2f;
    private const float ClimbMaxStamina = 110;
    private const float ClimbUpCost = 100 / 2.2f;
    private const float ClimbStillCost = 100 / 10f;
    private const float ClimbJumpCost = 110 / 4f;
    #endregion

    public const float SkinWidth = 0.02f;
    public const float MinOffset = 0.0001f;
    public const float VerticalRaysCount = 5;
    public const float HorizontalRaysCount = 5;
    #endregion

    #region Vars
    [SerializeField] private bool _onGround;
    [SerializeField] private bool _wasOnGround;
    [SerializeField] private bool _againstWall;
    [SerializeField] private bool _isFreezing;
    [SerializeField] private float _maxFall;
    [SerializeField] private Vector2 _speed;

    #region Jump
    [SerializeField] private bool _jump;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canJumpUpdate;
    [SerializeField] private float _jumpTimer;
    [SerializeField] private float _jumpHeldDownTimer;
    [SerializeField] private int _midAirJumps;
    #endregion

    #region Dash    
    [SerializeField] private bool _dash;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _canDash;
    [SerializeField] private bool _canDashUpdate;
    [SerializeField] private bool _isDashCooldown;
    [SerializeField] private float _dashTimer;
    [SerializeField] private float _dashHeldDownTimer;
    [SerializeField] private float _dashCooldownTimer;
    [SerializeField] private Vector2 _dashDir;
    [SerializeField] private Vector2 _beforeDashSpeed;
    #endregion

    #region Climb
    [SerializeField] private bool _climb;
    [SerializeField] private bool _isClimbing;
    [SerializeField] private bool _canClimb;
    [SerializeField] private bool _canClimbUpdate;
    [SerializeField] private bool _isClimbCooldown;
    [SerializeField] private float _climbTimer;
    [SerializeField] private float _climbHeldDownTimer;
    [SerializeField] private float _climbCooldownTimer;
    [SerializeField] private float _wallSlideTimer;
    [SerializeField] private float _climbNoMoveTimer;
    #endregion

    private float _verticalRaysInterval;
    private float _horizontalRaysInterval;

    [SerializeField] private RayOrigin _rayOrigin;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;

    public static event Action<CharacterMovement> JumpBeginEvent;
    public static event Action<CharacterMovement> MidAirJumpBeginEvent;
    public static event Action<CharacterMovement> JumpUpdateEvent;
    public static event Action<CharacterMovement> JumpEndEvent;

    public static event Action<CharacterMovement> DashBeginEvent;
    public static event Action<CharacterMovement> DashUpdateEvent;
    public static event Action<CharacterMovement> DashEndEvent;

    public static event Action<CharacterMovement> ClimbBeginEvent;
    public static event Action<CharacterMovement> ClimbupdateEvent;
    public static event Action<CharacterMovement> ClimbEndEvent;

    public static event Action<CharacterMovement> LandingEvent;

    #endregion

    public float MoveX { get; set; }

    public float MoveY { get; set; }

    public int Facing { get; set; }

    public float Stamina { get; set; }

    public Vector2 Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    /// <summary>
    /// 当前帧是否在地面
    /// </summary>
    public bool OnGround
    {
        get { return _onGround; }
        set => _onGround = value;
    }

    /// <summary>
    /// 上一帧是否在地面
    /// </summary>
    public bool WasOnGround
    {
        get { return _wasOnGround; }
        set => _wasOnGround = value;
    }

    public bool HitCeiling { get; set; }

    public bool AgainstWall
    {
        get { return _againstWall; }
        set { _againstWall = value; }
    }

    /// <summary>
    /// 按键响应
    /// </summary>
    public bool Jump
    {
        get { return _jump; }
        set
        {
            if (_jump && !value)
            {
                _canJump = true;
                _jumpHeldDownTimer = 0.0f;
            }
            _jump = value;
            if (_jump)
            {
                _jumpHeldDownTimer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 仅上升阶段
    /// </summary>
    public bool IsJumping
    {
        get
        {
            if (_isJumping && _speed.y < MinOffset)
            {
                _isJumping = false;
            }
            return _isJumping;
        }
    }

    /// <summary>
    /// 按键响应
    /// </summary>
    public bool Dash
    {
        get { return _dash; }
        set
        {
            if (_dash && !value)
            {
                _canDash = true;
                _dashHeldDownTimer = 0.0f;
            }
            _dash = value;
            if (_dash)
            {
                _dashHeldDownTimer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 仅上升阶段
    /// </summary>
    public bool IsDashing
    {
        get
        {
            if (_isDashing && !_isFreezing && _speed.y < MinOffset)
            {
                _isDashing = false;
            }
            return _isDashing;
        }
    }

    /// <summary>
    /// 按键响应
    /// </summary>
    public bool Climb
    {
        get { return _climb; }
        set
        {
            if (_climb && !value)
            {
                _canClimb = true;
                _climbHeldDownTimer = 0.0f;
            }
            _climb = value;
            if (_climb)
            {
                _climbHeldDownTimer += Time.deltaTime;
            }
        }
    }

    public bool IsClimbing
    {
        get
        {
            if (_isClimbing && !_canClimbUpdate)
            {
                _isClimbing = false;
            }
            return _isClimbing;
        }
    }

    /// <summary>
    /// 仅下降阶段
    /// </summary>
    public bool IsFalling
    {
        get { return !_onGround && _speed.y < MinOffset; }
    }

    public void Move(float deltaTime)
    {
        ComputeRayOrigin();
        ApplyFacing();
        DetectGround(deltaTime);
        DetectWall(deltaTime);
        ApplyGravity(deltaTime);
        CooldownUpdate(deltaTime);

        Moving(deltaTime);
        JumpBegin();
        MidAirJumpBegin();
        JumpUpdate(deltaTime);
        DashBegin();
        DashUpdate(deltaTime);
        ClimbBegin();
        ClimbUpdate(deltaTime);
        LandingUpdate();
        CorrectionAndMove(deltaTime);
        _wasOnGround = _onGround;
    }

    /// <summary>
    /// 检测到地面时, 此时仍然距离地面有[0, skinWidth]的距离
    /// </summary>
    /// <param name="deltaTime"></param>
    private void DetectGround(float deltaTime)
    {
        _onGround = false;
        var origin = _rayOrigin.bottomLeft;
        var direction = Vector2.down;
        var distance = Mathf.Abs(_speed.y * deltaTime) + SkinWidth * 2f;
        for (int i = 0; i < HorizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y);
            var raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (raycastHit) { _onGround = true; break; }
        }
    }

    private void DetectWall(float deltaTime)
    {
        _againstWall = false;
        var origin = Facing < 0 ? _rayOrigin.bottomLeft : _rayOrigin.bottomRight;
        var direction = Vector2.right * Facing;
        var distance = Mathf.Abs(_speed.x * deltaTime) + SkinWidth * 2f;
        for (int i = 0; i < VerticalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _verticalRaysInterval * i);
            var raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (raycastHit) { _againstWall = true; break; }
        }
    }

    private void ApplyGravity(float deltaTime)
    {
        // Slide if it is against a wall and moving towards it
        if (_againstWall && MoveX == Facing)
        {
            _maxFall = Mathf.MoveTowards(_maxFall, ClimbSlipSpeed, SlideAccel * deltaTime);
        }
        else
        {
            _maxFall = Mathf.MoveTowards(_maxFall, MaxFall, FallAccel * deltaTime);
        }
        if (!_onGround)
        {
            if (_isFreezing) return;
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && (IsJumping || IsDashing || IsFalling) ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, _maxFall, Gravity * mult * deltaTime);
            //if (Mathf.Abs(_speed.y) > MinOffset) Console.LogFormat("ApplyGravity after speed Y {0:F3} OnGround {1}", _speed.y, _onGround);
        }
    }

    public void ApplyFacing()
    {
        if (MoveX != 0) Facing = (int)MoveX;
        Vector3 scale = transform.localScale;
        if (scale.x == Facing) return;
        transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        //Console.LogFormat("scale.x {0},  _facing {1}", scale.x, Facing);
    }

    /// <summary>
    /// 参考 Celeste 机制研究(https://www.cnblogs.com/tmzbot/p/12318561.html) 调整游戏数值
    /// </summary>
    /// <param name="deltaTime"></param>
    private void Moving(float deltaTime)
    {
        float mult = _onGround ? 1.0f : 0.65f;
        if (Mathf.Abs(_speed.x) > MaxRun && Mathf.Sign(_speed.x) == MoveX)
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunReduce * mult * deltaTime);  //Reduce back from beyond the max speed
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunAccel * mult * deltaTime);   //Approach the max speed
        }
        //Console.LogFormat("Move speed X {0:F3}", _speed.x);
    }

    private void JumpBegin()
    {
        if (!_onGround) return;
        if (!_jump || !_canJump) return;
        if (_jumpHeldDownTimer > JumpToleranceTime) return;
        _canJump = false;
        _isJumping = true;
        _canJumpUpdate = true;
        _speed.x += JumpHBoost * MoveX;
        _speed.y = JumpSpeed;
        //事件回调
        JumpBeginEvent?.Invoke(this);
        //Console.LogFormat("JumpBegin {0}", _speed.y);
    }

    private void MidAirJumpBegin()
    {
        if (_midAirJumps > 0 && _onGround)
            _midAirJumps = 0;
        if (_onGround) return;
        if (!_jump || !_canJump) return;
        if (_midAirJumps >= MaxMidAirJump) return;
        _midAirJumps++;
        _canJump = false;
        _isJumping = true;
        _canJumpUpdate = true;
        MidAirJumpBeginEvent?.Invoke(this);
        //Console.LogFormat("MidAirJumpBegin {0}", _speed.y);
    }

    private void JumpUpdate(float deltaTime)
    {
        if (!_canJumpUpdate) return;
        if (_jump && _jumpTimer < JumpTime)
        {
            _speed.y = JumpSpeed;
            //var jumpProcess = _jumpTimer / JumpVarTime;
            //_speed.y = Mathf.Lerp(JumpSpeed, 0.0f, jumpProcess);
            _jumpTimer = Mathf.Min(_jumpTimer + deltaTime, JumpTime);
            JumpUpdateEvent?.Invoke(this);
            //Console.LogFormat("Jumping Timer {0}", _speed.y);
        }
        else
        {
            JumpEnd();
        }
    }

    private void JumpEnd()
    {
        _jumpTimer = 0.0f;
        _canJumpUpdate = false;
        JumpEndEvent?.Invoke(this);
        //Console.LogFormat("JumpEnd {0}", _speed);
    }

    /// <summary>
    /// 仅在地面时更新冷却时间
    /// </summary>
    private void CooldownUpdate(float deltaTime)
    {
        if (!_onGround) return;
        if (_isDashCooldown)
        {
            if (_dashCooldownTimer < DashCooldownTime)
            {
                _dashCooldownTimer = Mathf.Min(_dashCooldownTimer + deltaTime, DashCooldownTime);
            }
            else
            {
                _dashCooldownTimer = 0.0f;
                _isDashCooldown = false;
            }
        }
        if (_isClimbCooldown)
        {
            if (_climbCooldownTimer < ClimbCooldownTime)
            {
                _climbCooldownTimer = Mathf.Min(_climbCooldownTimer + deltaTime, ClimbCooldownTime);
            }
            else
            {
                _climbCooldownTimer = 0.0f;
                _isClimbCooldown = false;
            }
        }
    }

    /// <summary>
    /// Idle状态: Dash方向---Facing
    /// MoveX输入状态(MoveY为0): Dash方向---MoveX
    /// MoveY输入状态(MoveX为0): Dash方向---MoveY 1 SuperJump, -1 FastMaxFall
    /// MoveX, MoveY同时输入状态: Dash方向: Angle(MoveX, MoveY)
    /// </summary>
    private void CamputeDashDir()
    {
        if (MoveX == 0 && MoveY == 0)
        {
            _dashDir = new Vector2(Facing, 0);
        }
        else if (MoveX != 0 && MoveY == 0)
        {
            _dashDir = new Vector2(MoveX, 0);
        }
        else if (MoveX == 0 && MoveY != 0)
        {
            _dashDir = new Vector2(0, MoveY);
        }
        else
        {
            _dashDir = new Vector2(MoveX, MoveY).normalized;
        }
    }

    private void DashBegin()
    {
        if (_isDashCooldown) return;
        if (!_dash || !_canDash) return;
        if (_dashHeldDownTimer > DashToleranceTime) return;
        _canDash = false;
        _isDashing = true;
        _canDashUpdate = true;
        CamputeDashDir();
        DashBeginEvent?.Invoke(this);
        //Console.LogFormat("DashBegin {0}", _speed);
    }

    private void DashUpdate(float deltaTime)
    {
        if (!_canDashUpdate) return;
        if (_dashTimer < DashTime)
        {
            if (_dashTimer > 0.05f)
            {
                _isFreezing = false;
                _speed = DashSpeed * _dashDir;
                //Console.LogFormat("<color=red>Dashing Timer</color>, Speed {0} isDashing {1}", _speed, _isDashing);
            }
            else
            {
                _isFreezing = true;
                _speed = Vector2.zero;
                //Console.LogWarningFormat("<color=green>Freezing Timer</color>, Speed {0} isDashing {1}", _speed, _isDashing);
            }
            DashUpdateEvent?.Invoke(this);
            _dashTimer = Mathf.Min(_dashTimer + deltaTime, DashTime);
        }
        else
        {
            DashEnd();
        }
    }

    private void DashEnd()
    {
        _dashTimer = 0.0f;
        _canDashUpdate = false;
        _isDashCooldown = true;
        _speed = EndDashSpeed * _dashDir;
        DashEndEvent?.Invoke(this);
        //Console.LogFormat("DashEnd {0}", _speed);
    }

    private void ClimbBegin()
    {
        if (!_againstWall) return;
        if (_isClimbCooldown) return;
        if (!_climb || !_canClimb) return;
        if (_climbHeldDownTimer > ClimbToleranceTime) return;
        _canClimb = false;
        _isClimbing = true;
        _canClimbUpdate = true;
        _speed.x = 0;
        _speed.y *= ClimbGrabYMult;
        ClimbBeginEvent?.Invoke(this);
        //Console.LogFormat("ClimbBegin {0}", _speed);
    }

    /// <summary>
    /// 在撞到墙时, 按住 Z键即应该转抓住墙壁, 不需同时按住方向键
    /// 抓住墙壁时, 松开Z键开始下落, 若未降到地面再次按下Z键应再次抓住墙壁
    /// Todo: 向上攀爬时, 如果初速度不够, 则攀爬速度过慢
    /// </summary>
    private void ClimbUpdate(float deltaTime)
    {
        if (_canClimbUpdate)
        {
            if (_againstWall)
            {
                if (_climb)
                {
                    float target = 0;
                    if (_climbTimer < ClimbTime)
                    {
                        if (MoveY > 0)
                        {
                            _speed.y = 3.0f;
                            target = ClimbUpSpeed;
                        }
                        else if (MoveY < 0)
                        {
                            _speed.y = -4.0f;
                            target = ClimbDownSpeed;
                        }
                        //_speed.y = target;
                        _speed.y = Mathf.MoveTowards(_speed.y, target, ClimbAccel * deltaTime);
                        _climbTimer = Mathf.Min(_climbTimer + deltaTime, ClimbTime);
                        ClimbupdateEvent?.Invoke(this);
                    }
                    else ClimbEnd();   // Cooldown                 
                }
                else _canClimbUpdate = false;   // 松开Climb按键                    
            }
            else // 离开墙体
            {
                _climbTimer = 0.0f;
                _canClimbUpdate = false;
            }
        }
        if (_onGround && _climbTimer != 0) _climbTimer = 0.0f;
    }

    private void ClimbEnd()
    {
        _climbTimer = 0.0f;
        _canClimbUpdate = false;
        _isClimbCooldown = true;    // 仅在地面时更新
        ClimbEndEvent?.Invoke(this);
        //Console.LogFormat("ClimbEnd {0}", _speed);
    }

    public void LandingUpdate()
    {
        if (!_wasOnGround && _onGround)
        {
            LandingEvent?.Invoke(this);
        }
    }

    private void CorrectionAndMove(float deltaTime)
    {
        var deltaMovement = _speed * deltaTime;
        if (deltaMovement.x != 0.0f)
        {
            FixedHorizontally(ref deltaMovement);
        }
        if (deltaMovement.y != 0.0f)
        {
            FixedVertically(ref deltaMovement);
        }
        _speed = deltaMovement / deltaTime;
        if (Mathf.Abs(_speed.x) < MinOffset) _speed.x = 0.0f;
        if (Mathf.Abs(_speed.y) < MinOffset) _speed.y = 0.0f;
        MoveToPosition(deltaMovement);
    }

    /// <summary>
    /// 水平修正
    /// </summary>
    private RaycastHit2D FixedHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > MinOffset;
        var origin = isGoingRight ? _rayOrigin.bottomRight : _rayOrigin.bottomLeft;
        var direction = isGoingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var raycastHit = new RaycastHit2D();
        for (int i = 0; i < VerticalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _verticalRaysInterval * i);
            raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            //Console.DrawRay(rayOrigin, direction * distance, Color.blue);
            if (raycastHit)
            {

                if (isGoingRight)
                {
                    deltaMovement.x = raycastHit.distance - SkinWidth;   // 右方
                }
                else
                {
                    deltaMovement.x = SkinWidth - raycastHit.distance;   // 左方
                }
                //if (Mathf.Abs(deltaMovement.x) < MinOffset) return raycastHit;
            }
        }
        return raycastHit;
    }

    /// <summary>
    /// 竖直修正
    /// </summary>
    private RaycastHit2D FixedVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > MinOffset;
        var origin = isGoingUp ? _rayOrigin.topLeft : _rayOrigin.bottomLeft;
        var direction = isGoingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var raycastHit = new RaycastHit2D();
        for (int i = 0; i < HorizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y);
            raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            //Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (raycastHit)
            {
                if (isGoingUp)
                {
                    deltaMovement.y = raycastHit.distance - SkinWidth;   // 上方
                }
                else
                {
                    deltaMovement.y = SkinWidth - raycastHit.distance;   // 下方
                }
                //if (Mathf.Abs(deltaMovement.y) < MinOffset) return raycastHit;
            }
        }
        return raycastHit;
    }

    private void MoveToPosition(Vector2 deltaMovement)
    {
        _rigidbody.MovePosition(_rigidbody.position + deltaMovement);
    }

    private void ComputeRayOrigin()
    {
        var bounds = _boxCollider.bounds;
        bounds.Expand(-SkinWidth * 2f);
        _rayOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        _rayOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _rayOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    private void ComputeRaysInterval()
    {
        _horizontalRaysInterval = (_rayOrigin.bottomRight.x - _rayOrigin.bottomLeft.x) / (HorizontalRaysCount - 1);
        _verticalRaysInterval = (_rayOrigin.topLeft.y - _rayOrigin.bottomLeft.y) / (VerticalRaysCount - 1);
    }

    public void UpdateInput()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        MoveY = Input.GetAxisRaw("Vertical");
        Jump = Input.GetKey(KeyCode.C);
        Dash = Input.GetKey(KeyCode.X);
        Climb = Input.GetKey(KeyCode.Z);
        //Console.LogWarning("timeScale " + Time.timeScale + "deltaTime " + Time.deltaTime);
    }

    private void Awake()
    {
        Facing = 1;
        _canJump = true;
        _canDash = true;
        _canClimb = true;
        _groundMask = LayerMask.GetMask("Ground");
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        ComputeRayOrigin();
        ComputeRaysInterval();
    }
}
