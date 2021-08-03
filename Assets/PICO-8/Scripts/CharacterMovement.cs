using System.Collections;
using System.Collections.Generic;
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

    public const float ClimbSpeed = 4.8f;
    public const float ClimbSlipSpeed = -3.6f;
    public const float ClimbAccel = 90f + 50f;
    public const float ClimbGrabYMult = 0.2f;

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
    public const float DashSpeed = 24f;
    public const float EndDashSpeed = 16f;
    public const float EndDashUpMult = 0.75f;
    public const float DashTime = 0.15f;
    public const float DashCooldown = 0.2f;
    #endregion

    public const float SkinWidth = 0.02f;
    public const float MinOffset = 0.0001f;
    public const float VerticalRaysCount = 5;
    public const float HorizontalRaysCount = 5;
    #endregion

    #region Vars

    public bool _onGround;
    public bool _hitCeiling;
    public bool _againstWall;

    public float _maxFall;

    public Vector2 _facing;
    public Vector2 _speed;

    #region Jump
    public bool _jump;
    public bool _isJumping;
    public bool _canJump = true;
    public bool _canJumpTimer = false;
    public float _jumpTimer;
    public float _jumpHeldDownTimer;
    public int _midAirJump;
    public int _midAirJumpCount = 0;   // 0 or 1
    #endregion

    #region Dash
    public bool _dash;
    public bool _isDashing;
    public bool _canDash = true;
    public bool _canDashTimer = false;
    public float _dashTimer;
    public float _dashCooldownTimer;
    public Vector2 _dashDirection;
    public Vector2 _beforeDashSpeed;

    #endregion

    public float _verticalRaysInterval;
    public float _horizontalRaysInterval;

    public RayOrigin _rayOrigin;
    public LayerMask _groundMask;
    public Rigidbody2D _rigidbody;
    public BoxCollider2D _boxCollider;
    #endregion

    public float MoveX { get; set; }

    public float MoveY { get; set; }

    public bool Jump
    {
        get { return _jump; }
        set
        {
            // 按键释放
            if (_jump && value == false)
            {
                _canJump = true;
                _jumpHeldDownTimer = 0.0f;
            }
            // 按键充能
            _jump = value;
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

    public bool IsFalling
    {
        get { return !_onGround && _speed.y < MinOffset; }
    }

    public bool Dash
    {
        get { return _dash; }
        set
        {
            if (_dash && value == false)
            {
                _canDash = true;
                _dashCooldownTimer = 0.0f;
            }
            _dash = value;
            if (!_dash)
            {
                _dashCooldownTimer += Time.deltaTime;
            }
        }
    }

    public void Move(float deltaTime)
    {
        ComputeRayOrigin();
        DetectGround(deltaTime);
        ApplyGravity(deltaTime);
        Facing();
        Moving(deltaTime);
        Jumping();
        MidAirJumping();
        UpdateJumpTimer(deltaTime);
        Dashing();
        UpdateDashTimer(deltaTime);
        //SnapToGround();
        //LimitLateralVelocity();
        //LimitVerticalVelocity();
        //PreventGroundPenetration();
        CorrectionAndMove(deltaTime);
    }

    public void Facing()
    {
        _facing.y = MoveY;
        if (MoveX != 0) _facing.x = MoveX;
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
        _maxFall = Mathf.MoveTowards(_maxFall, MaxFall, FastMaxAccel * deltaTime);
        if (!_onGround)
        {
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && IsJumping ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, _maxFall, Gravity * mult * deltaTime);
            if (Mathf.Abs(_speed.y) > MinOffset) Console.LogFormat("ApplyGravity after speed Y {0:F3}", _speed.y);
        }
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
            if (_speed.x != 0) Console.LogFormat("Move speed X Reduce {0:F3}", _speed.x);
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunAccel * mult * deltaTime);   //Approach the max speed
            if (_speed.x != 0) Console.LogFormat("Move speed X Approach {0:F3}", _speed.x);
        }

        //if (MoveY != 0)
        //{
        //    _speed.x = Mathf.MoveTowards(_speed.x, MaxRun, RunAccel * mult * 0.5f * deltaTime);
        //}       

        //Console.LogFormat("Move speed X {0:F3}", _speed.x);
    }

    private void Jumping()
    {
        if (!_onGround) return;
        if (!Jump || !_canJump) return;
        // Is jump button pressed within jump tolerance?
        if (_jumpHeldDownTimer > JumpForceTime) return;
        _canJump = false;
        _isJumping = true;
        _canJumpTimer = true;
        _speed.x += JumpHBoost * MoveX;
        _speed.y = JumpSpeed;
        Console.Log("Jumping");
        // Apply jump impulse

    }

    private void SuperJumping()
    {

        _speed.x = SuperJumpH * _facing.x;
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
        if (_midAirJump > 0 && _onGround)
            _midAirJump = 0;
        if (_onGround) return;
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
            //var jumpProcess = _jumpTimer / JumpTime;
            //_speed.y = Mathf.Lerp(JumpSpeed, 0.0f, jumpProcess);
            _jumpTimer = Mathf.Min(_jumpTimer + deltaTime, JumpVarTime);
            //Console.Log("Jumping Timer");
        }
        else
        {
            // Button released or extra jump time ends, reset info
            _jumpTimer = 0.0f;
            _canJumpTimer = false;
        }
    }

    private void Dashing()
    {
        if (!_dash || !_canDash) return;
        _canDash = false;
        _isDashing = true;
        _canDashTimer = true;
        _beforeDashSpeed = _speed;
        _speed = Vector2.zero;
        _dashDirection = Vector2.zero;
    }

    /// <summary>
    /// Idle状态: Dash方向---Facing
    /// MoveX输入状态: Dash方向---MoveX
    /// MoveY输入状态(MoveX为0): Dash方向---MoveY 1 SuperJump, -1 FastMaxFall
    /// MoveX, MoveY同时输入状态: Dash方向: Angle(MoveX, MoveY)
    /// </summary>
    /// <param name="deltaTime"></param>
    private void UpdateDashTimer(float deltaTime)
    {
        if (!_canDashTimer) return;
        if (_dashTimer < DashTime)
        {
            if (MoveX == 0 && MoveY == 0)
            {
                _speed = DashSpeed * _facing;
            }
            else if (MoveX != 0 && MoveY == 0)
            {
                _speed.x = DashSpeed * MoveX;
            }
            else if (MoveX == 0 && MoveY != 0)
            {
                if (MoveY == 1)
                {
                    SuperJumping(1);
                }
                else
                {
                    //FastMaxFall;
                }
            }
            else
            {
                _speed = DashSpeed * new Vector2(MoveX, MoveY);
            }
            _dashTimer = Mathf.Min(_dashTimer + deltaTime, DashTime);
            Console.Log("Dashing Timer");
        }
        else
        {
            _dashTimer = 0.0f;
            _canDashTimer = false;

            //if (DashDir.Y <= 0)
            //    _speed = DashDir * EndDashSpeed;
            if (_speed.y > 0)
                _speed.y *= EndDashUpMult;
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
        _againstWall = false;
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
                    _againstWall = true;
                    deltaMovement.x = _raycastHit.distance - SkinWidth;   // 右方
                }
                else
                {
                    _againstWall = true;
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
    }

    private void Awake()
    {
        _facing = Vector2.right;
        _groundMask = LayerMask.GetMask("Ground");
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        ComputeRayOrigin();
        ComputeRaysInterval();
    }
}
