using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public const float JumpVarTime = 0.2f;
    public const float JumpForceTime = 0.15f;

    private const float WallJumpHSpeed = 13f;
    private const float WallJumpForceTime = 0.16f;

    public const float SuperJumpH = 26f;
    public const float SuperJumpSpeed = 10.5f;

    public const float SuperWallJumpH = 17f;
    public const float SuperWallJumpSpeed = 16f;
    public const float SuperWallJumpVarTime = 0.25f;
    public const float SuperWallJumpForceTime = 0.2f;
    #endregion

    #region Dash  
    public const float DashSpeed = 18f;
    public const float EndDashSpeed = 9f;
    public const float EndDashUpMult = 0.75f;
    public const float DashTime = 0.2f;
    public const float DashCooldownTime = 0.2f;
    #endregion

    #region Climb

    private const float ClimbUpSpeed = 4.5f;
    private const float ClimbDownSpeed = 8f;
    private const float ClimbSlipSpeed = 3f;
    private const float ClimbAccel = 90f;
    private const float ClimbGrabYMult = .2f;
    private const float ClimbNoMoveTime = .1f;
    private const float ClimbVarTime = 2f;
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

    [SerializeField] private float _maxFall;
    [SerializeField] private Vector2 _speed;

    #region Jump

    [SerializeField] private bool _jump;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canJumpTimer = false;
    [SerializeField] private float _jumpTimer;
    [SerializeField] private float _jumpHeldDownTimer;
    [SerializeField] private int _midAirJump;
    [SerializeField] private int _midAirJumpCount = 0;   // 0 or 1
    #endregion

    #region Dash    
    [SerializeField] private bool _dash;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _isFreezing;
    [SerializeField] private bool _canDash = true;
    [SerializeField] private bool _canDashTimer = false;
    [SerializeField] private float _dashTimer;
    [SerializeField] private float _dashCooldownTimer;
    [SerializeField] private Vector2 _dashDir;
    [SerializeField] private Vector2 _beforeDashSpeed;
    #endregion

    #region Climb
    [SerializeField] private bool _climb;
    [SerializeField] private bool _isClimbing;
    [SerializeField] private bool _canClimb = true;
    [SerializeField] private bool _canClimbTimer = false;
    [SerializeField] private float _climbTimer;
    [SerializeField] private float _climbCooldownTimer;
    [SerializeField] private float _wallSlideTimer;
    [SerializeField] private float _climbMoMoveTimer;
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

    public bool OnGround { get; set; }

    public bool HitCeiling { get; set; }

    public bool AgainstWall { get; set; }

    /// <summary>
    /// 按键响应
    /// </summary>
    public bool Jump
    {
        get { return _jump; }
        set
        {
            // 按键释放
            if (_jump && !value)
            {
                _canJump = true;
                _jumpHeldDownTimer = 0.0f;
            }
            _jump = value;
            // 按键充能
            if (_jump)
            {
                _jumpHeldDownTimer += Time.deltaTime;
            }
        }
    }

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
            }
            _dash = value;
            if (!_isDashing)
            {
                _dashCooldownTimer += Time.deltaTime;
            }
        }
    }

    public bool IsDashing
    {
        get
        {
            if (_isDashing && !_canDashTimer && OnGround)
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
            if (_climb && !value && AgainstWall)
            {
                _canClimb = true;
                _climbTimer = 0.0f;
            }
            _climb = value;
            if (_climb && AgainstWall)
            {
                _climbTimer += Time.deltaTime;
            }
        }
    }

    public bool IsClimbing
    {
        get
        {
            if (_isClimbing && !_canClimbTimer && OnGround)
            {
                _isClimbing = false;
            }
            return _isClimbing;
        }

    }

    public bool IsFalling
    {
        get { return !OnGround && _speed.y < MinOffset; }
    }

    public void Move(float deltaTime)
    {
        ComputeRayOrigin();
        DetectGround(deltaTime);
        ApplyGravity(deltaTime);
        ApplyFacing();
        Moving(deltaTime);
        Jumping();
        MidAirJumping();
        UpdateJumpTimer(deltaTime);
        Dashing();
        UpdateDashTimer(deltaTime);
        Climbing();
        UpdateClimbTimer(deltaTime);
        //SnapToGround();
        //LimitLateralVelocity();
        //LimitVerticalVelocity();
        //PreventGroundPenetration();
        CorrectionAndMove(deltaTime);
    }

    private void DetectGround(float deltaTime)
    {
        OnGround = false;
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
                OnGround = true; break;
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
        if (!OnGround && !_isFreezing)
        {
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && (IsJumping || IsFalling) ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, _maxFall, Gravity * mult * deltaTime);
            if (Mathf.Abs(_speed.y) > MinOffset) Console.LogFormat("ApplyGravity after speed Y {0:F3}", _speed.y);
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
        float mult = OnGround ? 1.0f : 0.65f;
        if (Mathf.Abs(_speed.x) > MaxRun && Mathf.Sign(_speed.x) == MoveX)
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunReduce * mult * deltaTime);  //Reduce back from beyond the max speed
            //if (_speed.x != 0) Console.LogFormat("Move speed X Reduce {0:F3}", _speed.x);
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunAccel * mult * deltaTime);   //Approach the max speed
            //if (_speed.x != 0) Console.LogFormat("Move speed X Approach {0:F3}", _speed.x);
        }
        //Console.LogFormat("Move speed X {0:F3}", _speed.x);
    }

    private void Jumping()
    {
        if (!OnGround) return;
        if (!Jump || !_canJump) return;
        if (_jumpHeldDownTimer > JumpForceTime) return;
        _canJump = false;
        _isJumping = true;
        _canJumpTimer = true;
        _speed.x += JumpHBoost * MoveX;
        _speed.y = JumpSpeed;
        Console.LogFormat("Jumping {0}", _speed.y);
        // Apply jump impulse

    }

    private void SuperJumping()
    {
        _speed.x = SuperJumpH * Facing;
        _speed.y = JumpSpeed;
    }

    private void WallJumping(int dir)
    {
        _speed.x = WallJumpHSpeed * dir;
        _speed.y = JumpSpeed;
    }

    private void SuperWallJumping(int dir)
    {
        _speed.x = SuperWallJumpH * dir;
        _speed.y = SuperWallJumpSpeed;
    }

    private void MidAirJumping()
    {
        if (_midAirJump > 0 && OnGround)
            _midAirJump = 0;
        if (OnGround) return;
        if (!Jump || !_canJump) return;
        if (_midAirJump >= _midAirJumpCount) return;
        _midAirJump++;
        _canJump = false;
        _isJumping = true;
        _canJumpTimer = true;
        // Apply jump impulse

    }

    private void UpdateJumpTimer(float deltaTime)
    {
        if (!_canJumpTimer) return;
        // If jump button is held down and extra jump time is not exceeded...
        if (Jump && _jumpTimer < JumpVarTime)
        {
            _speed.y = JumpSpeed;
            //var jumpProcess = _jumpTimer / JumpVarTime;
            //_speed.y = Mathf.Lerp(JumpSpeed, 0.0f, jumpProcess);
            _jumpTimer = Mathf.Min(_jumpTimer + deltaTime, JumpVarTime);
            Console.LogFormat("Jumping Timer {0}", _speed.y);
        }
        else
        {
            // Button released or extra jump time ends, reset info
            _jumpTimer = 0.0f;
            _canJumpTimer = false;
        }
    }

    /// <summary>
    /// Idle状态: Dash方向---Facing
    /// MoveX输入状态(MoveY为0): Dash方向---MoveX
    /// MoveY输入状态(MoveX为0): Dash方向---MoveY 1 SuperJump, -1 FastMaxFall
    /// MoveX, MoveY同时输入状态: Dash方向: Angle(MoveX, MoveY)
    /// </summary>
    private void DashDir(ref Vector2 dashDir)
    {
        if (MoveX == 0 && MoveY == 0)
        {
            dashDir = new Vector2(Facing, 0);
        }
        else if (MoveX != 0 && MoveY == 0)
        {
            dashDir = new Vector2(MoveX, 0);
        }
        else if (MoveX == 0 && MoveY != 0)
        {
            dashDir = new Vector2(0, MoveY);
        }
        else
        {
            dashDir = new Vector2(MoveX, MoveY).normalized;
        }
    }

    private void Dashing()
    {
        if (!_dash || !_canDash) return;
        if (_dashCooldownTimer < DashCooldownTime) return;

        _canDash = false;
        _isDashing = true;
        _canDashTimer = true;
        _beforeDashSpeed = _speed;
        _speed = Vector2.zero;
        DashDir(ref _dashDir);
        Console.LogFormat("Dashing {0}", _speed);
    }

    private void UpdateDashTimer(float deltaTime)
    {
        if (!_canDashTimer) return;
        if (_dashTimer < DashTime)
        {
            if (_dashTimer > 0.05f)
            {
                _speed = DashSpeed * _dashDir;
                _isFreezing = false;
                Console.LogFormat("Dashing Timer Freeze {0}, Speed {1}", _isFreezing, _speed);
            }
            else
            {
                _isFreezing = true;
                Console.LogFormat("Freezing Timer Freeze {0}, Speed {1}", _isFreezing, _speed);
            }
            _dashTimer = Mathf.Min(_dashTimer + deltaTime, DashTime);
        }
        else
        {
            _dashTimer = 0.0f;
            _dashCooldownTimer = 0.0f;
            _canDashTimer = false;
            if (_dashDir.y < 0)
                _speed = EndDashSpeed * _dashDir;
            if (_speed.y > 0)
                _speed.y *= EndDashUpMult;
        }
    }

    /// <summary>
    /// 设计:
    /// @1撞到墙立即静止
    /// </summary>
    private void Climbing()
    {
        if (!_climb || !_canClimb) return;
        if (_climbMoMoveTimer < ClimbNoMoveTime) return;
        _canClimb = false;
        _isClimbing = true;
        _canClimbTimer = true;
        _speed.x = 0;
        _speed.y *= ClimbGrabYMult;
    }

    private void UpdateClimbTimer(float deltaTime)
    {
        if (!_canClimbTimer) return;
        if (_climbTimer > ClimbVarTime) return;
        if (MoveY > 0)
        {
            _speed.y = ClimbSlipSpeed;
        }
        else if (MoveY < 0)
        {

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

    private void FixedHorizontally(ref Vector2 deltaMovement)
    {
        AgainstWall = false;
        var isGoingRight = deltaMovement.x > MinOffset;
        var origin = isGoingRight ? _rayOrigin.bottomRight : _rayOrigin.bottomLeft;
        var direction = isGoingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        for (int i = 0; i < VerticalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _verticalRaysInterval * i);
            var _raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.blue);
            if (_raycastHit)
            {
                if (isGoingRight)
                {
                    AgainstWall = true;
                    deltaMovement.x = _raycastHit.distance - SkinWidth;   // 右方
                }
                else
                {
                    AgainstWall = true;
                    deltaMovement.x = SkinWidth - _raycastHit.distance;   // 左方
                }
                //if (Mathf.Abs(deltaMovement.x) < MinOffset) break;
            }
        }
    }

    private void FixedVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > MinOffset;
        var origin = isGoingUp ? _rayOrigin.topLeft : _rayOrigin.bottomLeft;
        var direction = isGoingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        for (int i = 0; i < HorizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y);
            var _raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (_raycastHit)
            {
                if (isGoingUp)
                {
                    //_onGround = false;
                    deltaMovement.y = _raycastHit.distance - SkinWidth;   // 上方
                }
                else
                {
                    //_onGround = true;
                    deltaMovement.y = SkinWidth - _raycastHit.distance;   // 下方
                }
                if (Mathf.Abs(deltaMovement.y) < MinOffset) return;
            }
        }
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
    }

    private void Awake()
    {
        Facing = 1;
        _groundMask = LayerMask.GetMask("Ground");
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        ComputeRayOrigin();
        ComputeRaysInterval();
    }
}
