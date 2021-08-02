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

    public float MaxRun = 9f;
    public float MaxFall = -16f;
    public float FastMaxFall = -24f;
    public float FastMaxAccel = 30f;
    public float RunAccel = 75f;
    public float RunReduce = 30f;

    public float Gravity = 90f;
    public float HalfGravThreshold = 4f;

    public float AirMult = 0.65f;

    public float ClimbSpeed = 4.8f;
    public float ClimbSlipSpeed = -3.6f;
    public float ClimbAccel = 90f + 50f;
    public float ClimbGrabYMult = 0.2f;

    #region Jump
    public float JumpHBoost = 4f;
    public float JumpSpeed = 10.5f;
    public float JumpTime = 0.2f;
    public float JumpKeyTime = 0.15f;

    #endregion

    #region Dash   
    public float DashPower = 30f;
    public float DashTime = 0.15f;
    public float DashCooldownTime = 0.2f;
    public float DashRefillCooldown = 0.1f;
    #endregion

    public float SkinWidth = 0.02f;
    public float MinOffset = 0.0001f;
    public float VerticalRaysCount = 5;
    public float HorizontalRaysCount = 5;
    #endregion

    #region Vars

    public bool _onGround;
    public bool _hitCeiling;
    public bool _againstWall;

    public float _maxFall;

    public Vector2 _facing = Vector2.right;
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
            // If jump is released, allow to jump again
            if (_jump && value == false)
            {
                _canJump = true;
                _jumpHeldDownTimer = 0.0f;
            }
            // Update jump value; if pressed, update held down timer
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
        get
        {
            return _dash;
        }
        set
        {
            if (_dash && value == false)
            {
                _canDash = true;
                _dashCooldownTimer = 0.0f;
            }
            _dash = value;
            if (_dash)
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
        _facing = new Vector2(MoveX, MoveY);
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
        float mf = MaxFall;
        _maxFall = Mathf.MoveTowards(_maxFall, mf, FastMaxAccel * deltaTime);
        if (!_onGround)
        {
            float max = _maxFall;
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && IsJumping ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, max, Gravity * mult * deltaTime);
            //Console.LogFormat("ApplyGravity after speed Y {0:F3}", _speed.y);
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
            Console.LogFormat("Move speed X Reduce {0:F3}", _speed.x);
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunAccel * mult * deltaTime);   //Approach the max speed
            Console.LogFormat("Move speed X Approach {0:F3}", _speed.x);
        }

        //if (MoveY != 0)
        //{
        //    _speed.x = Mathf.MoveTowards(_speed.x, MaxRun, RunAccel * mult * 0.5f * deltaTime);
        //}       

        Console.LogFormat("Move speed X {0:F3}", _speed.x);
    }

    private void Jumping()
    {
        if (!_onGround) return;
        if (!Jump || !_canJump) return;
        // Is jump button pressed within jump tolerance?
        if (_jumpHeldDownTimer > JumpKeyTime) return;
        _canJump = false;
        _isJumping = true;
        _canJumpTimer = true;
        _speed.x += JumpHBoost * MoveX;
        _speed.y = JumpSpeed;
        Console.Log("Jumping");
        // Apply jump impulse

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
        if (Jump && _jumpTimer < JumpTime)
        {
            //var jumpProcess = _jumpTimer / JumpTime;
            _speed.y = JumpSpeed;
            //_speed.y = Mathf.Lerp(JumpPower, 0.0f, jumpProcess);
            //_speed.y = Mathf.Min(_speed.y, JumpPower);
            _jumpTimer = Mathf.Min(_jumpTimer + deltaTime, JumpTime);
            Console.Log("Jumping Timer");
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
    }

    private void UpdateDashTimer(float deltaTime)
    {
        if (!_canDashTimer) return;
        if (Dash && _dashTimer < DashTime)
        {
            var dashProcess = _dashTimer / DashTime;
            _speed = Mathf.Lerp(DashPower, 0.0f, dashProcess) * _facing;
            _dashTimer = Mathf.Min(_dashTimer + deltaTime, DashTime);
        }
        else
        {
            _dashTimer = 0.0f;
            _canDashTimer = false;
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
        Jump = Input.GetKey(KeyCode.J);
        Dash = Input.GetKey(KeyCode.K);
    }

    private void Awake()
    {
        ComputeRayOrigin();
        ComputeRaysInterval();
        _groundMask = LayerMask.GetMask("Ground");
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
}
