using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    #region Structs

    private struct RayOrigin
    {
        public Vector2 topLeft;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }
    #endregion

    #region Constants

    private const float MaxRun = 9f;
    private const float MaxFall = -16f;
    private const float FastMaxFall = -24f;
    private const float FastMaxAccel = 30f;
    private const float RunAccel = 50f;
    private const float RunReduce = 40f;

    private const float Gravity = 90f;
    private const float HalfGravThreshold = 4f;

    private const float AirMult = 0.65f;

    private const float ClimbSpeed = 4.8f;
    private const float ClimbSlipSpeed = -3.6f;
    private const float ClimbAccel = Gravity + 50f;
    private const float ClimbGrabYMult = 0.2f;

    #region Jump
    private const float JumpBoost = 4f;
    private const float JumpPower = 10.5f;
    private const float JumpPower2 = 10f;
    private const float JumpTime = 0.2f;
    private const float JumpKeyTime = 0.15f;

    #endregion

    #region Dash   
    private const float DashPower = 30f;
    private const float DashTime = 0.15f;
    private const float DashCooldownTime = 0.2f;
    private const float DashRefillCooldown = 0.1f;
    #endregion

    private const float SkinWidth = 0.02f;
    private const float MinOffset = 0.0001f;
    private const float VerticalRaysCount = 5;
    private const float HorizontalRaysCount = 5;
    #endregion

    #region Vars

    private bool _onGround;
    private bool _hitCeiling;
    private bool _againstWall;

    private float _maxFall;

    private Vector2 _facing = Vector2.right;
    private Vector2 _speed;

    #region Jump
    private bool _jump;
    private bool _isJumping;
    private bool _canJump = true;
    private bool _canJumpTimer = false;
    private float _jumpTimer;
    private float _jumpHeldDownTimer;
    private int _midAirJump;
    private int _midAirJumpCount = 0;   // 0 or 1
    #endregion

    #region Dash
    private bool _dash;
    private bool _isDashing;
    private bool _canDash = true;
    private bool _canDashTimer = false;
    private float _dashTimer;
    private float _dashCooldownTimer;
    #endregion

    private float _verticalRaysInterval;
    private float _horizontalRaysInterval;

    private RayOrigin _rayOrigin;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;
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
        float mult = _onGround ? 1.0f : 1f;
        if (Mathf.Abs(_speed.x) >= MaxRun && Mathf.Sign(_speed.x) == MoveX)
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunReduce * mult * deltaTime);  //Reduce back from beyond the max speed
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunAccel * mult * deltaTime);   //Approach the max speed          
        }
        //Console.LogFormat("Move speed X {0:F3}", _speed.x);
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
        _speed.x = JumpBoost * MoveX;
        _speed.y = JumpPower;
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
            _speed.y = JumpPower;
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
    }
}
