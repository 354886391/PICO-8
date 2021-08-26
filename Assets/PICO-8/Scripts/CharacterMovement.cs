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
    public const float FastMaxFall = -24f;
    public const float FastMaxAccel = 30f;
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
    private const float ClimbDownSpeed = 8f;
    private const float ClimbSlipSpeed = -3f;
    private const float ClimbTime = 2.0f;
    private const float ClimbToleranceTime = 0.15f;
    private const float ClimbCooldDownTime = 0.2f;
    private const float ClimbAccel = 90f;
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
    [SerializeField] private int _hCollision;
    [SerializeField] private int _vCollision;
    [SerializeField] private bool _isFreezing;
    [SerializeField] private float _maxFall;
    [SerializeField] private Vector2 _speed;

    #region Jump
    [SerializeField] private bool _jump;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canJumpTimer;
    [SerializeField] private float _jumpTimer;
    [SerializeField] private float _jumpHeldDownTimer;
    [SerializeField] private int _midAirJumps;
    #endregion

    #region Dash    
    [SerializeField] private bool _dash;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _canDash;
    [SerializeField] private bool _canDashTimer;
    [SerializeField] private bool _canDashCooldDown;
    [SerializeField] private float _dashTimer;
    [SerializeField] private float _dashHeldDownTimer;
    [SerializeField] private float _dashCooldDownTimer;
    [SerializeField] private Vector2 _dashDir;
    [SerializeField] private Vector2 _beforeDashSpeed;
    #endregion

    #region Climb
    [SerializeField] private bool _climb;
    [SerializeField] private bool _isClimbing;
    [SerializeField] private bool _canClimb;
    [SerializeField] private bool _canClimbTimer;
    [SerializeField] private bool _canClimbCooldDown;
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

    public bool OnGround
    {
        get { return _onGround; }
        set => _onGround = value;
    }

    public bool HitCeiling { get; set; }

    public int AgainstWall
    {
        get { return _hCollision; }
        set { _hCollision = value; }
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
            if (_isClimbing && !_canClimbTimer)
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
        DetectGround(deltaTime);
        ApplyGravity(deltaTime);
        CooldDown(deltaTime);
        ApplyFacing();
        Moving(deltaTime);
        JumpBegin();
        MidAirJumpBegin();
        JumpUpdate(deltaTime);
        DashBegin();
        DashUpdate(deltaTime);
        ClimbBegin();
        ClimbUpdate(deltaTime);
        AgainstWallUpdate(deltaTime);
        CorrectionAndMove(deltaTime);
    }

    private void DetectGround(float deltaTime)
    {
        _onGround = false;
        var origin = _rayOrigin.bottomLeft;
        var direction = Vector2.down;
        var distance = Mathf.Abs(_speed.y * deltaTime) + SkinWidth;
        for (int i = 0; i < HorizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y - SkinWidth);
            var raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.green);
            if (raycastHit /*&& raycastHit.distance < 0.001f + _skinWidth*/)
            {
                _onGround = true; break;
            }
        }
    }

    private void ApplyGravity(float deltaTime)
    {
        //float mf = MaxFall;
        //float fmf = FastMaxFall;
        //if (MoveY == -1 && _speed.y <= mf)
        //{
        //    _maxFall = Mathf.MoveTowards(_maxFall, fmf, FastMaxAccel * deltaTime);
        //}
        //else
        //{
        //    _maxFall = Mathf.MoveTowards(_maxFall, mf, FastMaxAccel * deltaTime);
        //}
        _maxFall = MaxFall;
        if (!_onGround || !_isFreezing)
        {
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && (IsJumping || IsDashing || IsFalling) ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, _maxFall, Gravity * mult * deltaTime);
            //if (Mathf.Abs(_speed.y) > MinOffset) Console.LogFormat("ApplyGravity after speed Y {0:F3}", _speed.y);
        }
    }

    public void ApplyFacing()
    {
        if (MoveX != 0) Facing = (int)MoveX;
        Vector3 scale = transform.localScale;
        if (scale.x == Facing) return;
        transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        Console.LogFormat("scale.x {0},  _facing {1}", scale.x, Facing);
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
        _canJumpTimer = true;
        _speed.x += JumpHBoost * MoveX;
        _speed.y = JumpSpeed;
        Console.LogFormat("Jumping {0}", _speed.y);
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
        _canJumpTimer = true;
        // Apply jump impulse

    }

    private void JumpUpdate(float deltaTime)
    {
        if (!_canJumpTimer) return;
        if (_jump && _jumpTimer < JumpTime)
        {
            _speed.y = JumpSpeed;
            //var jumpProcess = _jumpTimer / JumpVarTime;
            //_speed.y = Mathf.Lerp(JumpSpeed, 0.0f, jumpProcess);
            _jumpTimer = Mathf.Min(_jumpTimer + deltaTime, JumpTime);
            Console.LogFormat("Jumping Timer {0}", _speed.y);
        }
        else
        {
            JumpEnd();
        }
    }

    private void JumpEnd()
    {
        _jumpTimer = 0.0f;
        _canJumpTimer = false;
    }

    private void CooldDown(float deltaTime)
    {
        if (!_onGround) return;
        if (_canDashCooldDown)
        {
            if (_dashCooldDownTimer < DashCooldownTime)
            {
                _dashCooldDownTimer = Mathf.Min(_dashCooldDownTimer + deltaTime, DashCooldownTime);
            }
            else
            {
                _dashCooldDownTimer = 0.0f;
                _canDashCooldDown = false;
            }
        }
        if (_canClimbCooldDown)
        {
            if (_climbCooldownTimer < ClimbCooldDownTime)
            {
                _climbCooldownTimer = Mathf.Min(_climbCooldownTimer + deltaTime, ClimbCooldDownTime);
            }
            else
            {
                _climbCooldownTimer = 0.0f;
                _canClimbCooldDown = false;
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
        if (_canDashCooldDown) return;
        if (!_dash || !_canDash) return;
        if (_dashHeldDownTimer > DashToleranceTime) return;
        _canDash = false;
        _isDashing = true;
        _canDashTimer = true;
        CamputeDashDir();
        //Console.Freeze(50);
        Console.LogFormat("DashBegin {0}", _speed);
    }

    private void DashUpdate(float deltaTime)
    {
        if (!_canDashTimer) return;
        if (_dashTimer < DashTime)
        {
            if (_dashTimer > 0.05f)
            {
                _isFreezing = false;
                _speed = DashSpeed * _dashDir;
                Console.LogFormat("<color=red>Dashing Timer</color>, Speed {0} isDashing {1}", _speed, _isDashing);
            }
            else
            {
                _isFreezing = true;
                _speed = Vector2.zero;
                Console.LogWarningFormat("<color=green>Freezing Timer</color>, Speed {0} isDashing {1}", _speed, _isDashing);
            }
            _dashTimer = Mathf.Min(_dashTimer + deltaTime, DashTime);
        }
        else
        {
            DashEnd();
        }
    }

    private void DashEnd()
    {
        //if (!_onGround) return;
        _dashTimer = 0.0f;
        _canDashTimer = false;
        _canDashCooldDown = true;
        _speed = EndDashSpeed * _dashDir;
        Console.LogFormat("DashEnd {0}", _speed);
    }

    /// <summary>
    /// 设计:
    /// @1撞到墙立即静止
    /// </summary>
    private void ClimbBegin()
    {
        if (_hCollision == 0) return;
        if (_canClimbCooldDown) return;
        if (!_climb || !_canClimb) return;
        if (_climbHeldDownTimer > ClimbToleranceTime) return;
        _canClimb = false;
        _isClimbing = true;
        _canClimbTimer = true;
        Console.LogFormat("ClimbBegin {0}", _speed);
    }

    private void ClimbUpdate(float deltaTime)
    {
        if (!_canClimbTimer) return;
        if (_climb && _climbTimer < ClimbTime)
        {
            _speed.x = 0.0f;
            _speed.y = MoveY * (MoveY > 0 ? ClimbUpSpeed : (MoveY < 0 ? ClimbDownSpeed : 0.0f));
            _climbTimer = Mathf.Min(_climbTimer + deltaTime, ClimbTime);
        }
        else
        {
            ClimbEnd();
        }
    }

    private void ClimbEnd()
    {
        _climbTimer = 0.0f;
        _canClimbTimer = false;
        _canClimbCooldDown = true;
        Console.LogFormat("ClimbEnd {0}", _speed);
    }

    private void AgainstWallUpdate(float deltaTime)
    {
        if (_hCollision == 0) return;
        if (Climb || Dash) return;
        if (IsJumping || IsDashing) return;
        if (MoveX == Facing)
        {
            _speed.y = Mathf.MoveTowards(_speed.y, ClimbSlipSpeed, ClimbAccel * deltaTime);
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
        MoveToPosition(deltaMovement);
    }

    /// <summary>
    /// 水平修正
    /// </summary>
    private RaycastHit2D FixedHorizontally(ref Vector2 deltaMovement)
    {
        _hCollision = 0;
        var isGoingRight = deltaMovement.x > MinOffset;
        var origin = isGoingRight ? _rayOrigin.bottomRight : _rayOrigin.bottomLeft;
        var direction = isGoingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        RaycastHit2D raycastHit = new RaycastHit2D();
        for (int i = 0; i < VerticalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _verticalRaysInterval * i);
            raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.blue);
            if (raycastHit)
            {

                if (isGoingRight)
                {
                    _hCollision = 1;
                    deltaMovement.x = raycastHit.distance - SkinWidth;   // 右方
                }
                else
                {
                    _hCollision = -1;
                    deltaMovement.x = SkinWidth - raycastHit.distance;   // 左方
                }
                if (Mathf.Abs(deltaMovement.x) < MinOffset) return raycastHit;
            }
        }
        return raycastHit;
    }

    /// <summary>
    /// 竖直修正
    /// </summary>
    private RaycastHit2D FixedVertically(ref Vector2 deltaMovement)
    {
        _vCollision = 0;
        var isGoingUp = deltaMovement.y > MinOffset;
        var origin = isGoingUp ? _rayOrigin.topLeft : _rayOrigin.bottomLeft;
        var direction = isGoingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        RaycastHit2D raycastHit = new RaycastHit2D();
        for (int i = 0; i < HorizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y);
            raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (raycastHit)
            {
                if (isGoingUp)
                {
                    _vCollision = 1;
                    deltaMovement.y = raycastHit.distance - SkinWidth;   // 上方
                }
                else
                {
                    _vCollision = -1;
                    deltaMovement.y = SkinWidth - raycastHit.distance;   // 下方
                }
                if (Mathf.Abs(deltaMovement.y) < MinOffset) return raycastHit;
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
