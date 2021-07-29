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

    private const float JumpHBoost = 7.2f;
    private const float JumpSpeed = 13.5f;
    private const float JumpTime = 0.2f;

    private const float Gravity = 90f;
    private const float HalfGravThreshold = 4f;

    private const float AirMult = 0.65f;

    private const float DashSpeed = 25.6f;
    private const float EndDashSpeed = 12.8f;
    private const float EndDashUpMult = 0.75f;
    private const float DashTime = 0.15f;
    private const float DashCooldown = 0.2f;
    private const float DashRefillCooldown = 0.1f;
    private const int DashHJumpThruNudge = 6;
    private const int DashCornerCorrection = 4;
    private const int DashVFloorSnapDist = 3;
    private const float DashAttackTime = 0.3f;

    private const float ClimbSpeed = 4.8f;
    private const float ClimbSlipSpeed = -3.6f;
    private const float ClimbAccel = Gravity + 50f;
    private const float ClimbGrabYMult = 0.2f;
    #endregion

    #region Vars

    private bool _onGround;
    private bool _againstWall;

    private int _facing = 1;
    private int _jumpCount;
    private float _maxFall;
    private Vector2 _speed;

    private bool _jump;
    private bool _canJump = true;
    private bool _canJumpTimer;
    private bool _isJumping;
    private float _jumpTimer;
    private float _jumpHeldDownTimer;
    private float _jumpToleranceTime = 0.15f;
    private float _extraJumpTime = 0.5f;
    private float _extraJumpPower = 13.5f;
    private int _midAirJump;
    private int _midAirJumpCount = 1;

    private float _skinWidth = 0.02f;
    private float _minOffset = 0.0001f;
    [SerializeField] private float _verticalRaysCount;
    [SerializeField] private float _horizontalRaysCount;
    [SerializeField] private float _verticalRaysInterval;
    [SerializeField] private float _horizontalRaysInterval;



    private RayOrigin _rayOrigin;

    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private RaycastHit2D _raycastHit;
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
            if (_isJumping && _speed.y < _minOffset)
            {
                _isJumping = false;
            }
            return _isJumping;
        }
    }

    public bool IsFalling
    {
        get { return !_onGround && _speed.y < _minOffset; }
    }

    public void Move(float deltaTime)
    {
        ComputeRayOrigin();
        DetectGround(deltaTime);
        ApplyGravity(deltaTime);
        Moving(deltaTime);
        Jumping(deltaTime);
        MidAirJumping(deltaTime);
        UpdateJumpTimer(deltaTime);
        //SnapToGround();
        //LimitLateralVelocity();
        //LimitVerticalVelocity();
        //PreventGroundPenetration();
        CorrectionAndMove(deltaTime);
    }

    private void DetectGround(float deltaTime)
    {
        _onGround = false;
        var origin = _rayOrigin.bottomLeft;
        var direction = Vector2.down;
        var distance = Mathf.Abs(_speed.y * deltaTime) + _skinWidth;
        for (int i = 0; i < _horizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y - _skinWidth);
            var raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (raycastHit /*&& raycastHit.distance < 0.001f + _skinWidth*/)
            {
                _onGround = true; break;
            }
        }
    }

    private void ApplyGravity(float deltaTime)
    {
        if (!_onGround)
        {
            float max = MaxFall;
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && Jump ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, max, Gravity * mult * deltaTime);
            Console.LogFormat("ApplyGravity after speed Y {0:F3}", _speed.y);
        }
    }

    private void Moving(float deltaTime)
    {
        float mult = _onGround ? 1.0f : AirMult;
        if (Mathf.Abs(_speed.x) >= MaxRun && Mathf.Sign(_speed.x) == MoveX)
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunReduce * mult * deltaTime);  //Reduce back from beyond the max speed
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * MoveX, RunAccel * mult * deltaTime);   //Approach the max speed          
        }
        Console.LogFormat("Move speed X {0:F3}", _speed.x);
    }

    private void Jumping(float deltaTime)
    {
        if (!_onGround) return;
        if (!Jump || !_canJump) return;
        // Is jump button pressed within jump tolerance?
        if (_jumpHeldDownTimer > _jumpToleranceTime) return;
        _canJump = false;
        _isJumping = true;
        _canJumpTimer = true;
        // Apply jump impulse

    }

    private void MidAirJumping(float deltaTime)
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
        if (Jump && _jumpTimer < _extraJumpTime)
        {
            var jumpProcess = _jumpTimer / _extraJumpTime;
            var proportionaljumpPower = Mathf.Lerp(_extraJumpPower, 0.0f, jumpProcess);
            _speed.y = proportionaljumpPower;
            //movement.ApplyForce(Vector3.up * proportionalJumpPower, ForceMode.Acceleration);
            _jumpTimer = Mathf.Min(_jumpTimer + deltaTime, _extraJumpTime);
        }
        else
        {
            // Button released or extra jump time ends, reset info
            _jumpTimer = 0.0f;
            _canJumpTimer = false;
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

    private void FixedVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var origin = isGoingUp ? _rayOrigin.topLeft : _rayOrigin.bottomLeft;
        var direction = isGoingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(deltaMovement.y) + _skinWidth;
        for (int i = 0; i < _horizontalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _horizontalRaysInterval * i, origin.y);
            var _raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (_raycastHit)
            {
                if (isGoingUp)
                {
                    //_onGround = false;
                    deltaMovement.y = _raycastHit.distance - _skinWidth;   // 上方
                }
                else
                {
                    //_onGround = true;
                    deltaMovement.y = _skinWidth - _raycastHit.distance;   // 下方
                }
                if (Mathf.Abs(deltaMovement.y) < _minOffset) return;
            }
        }
    }

    private void FixedHorizontally(ref Vector2 deltaMovement)
    {
        _againstWall = false;
        var isGoingRight = deltaMovement.x > 0;
        var origin = isGoingRight ? _rayOrigin.bottomRight : _rayOrigin.bottomLeft;
        var direction = isGoingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(deltaMovement.x) + _skinWidth;
        for (int i = 0; i < _verticalRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _verticalRaysInterval * i);
            var _raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, _groundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.blue);
            if (_raycastHit)
            {
                if (isGoingRight)
                {
                    _againstWall = true;
                    deltaMovement.x = _raycastHit.distance - _skinWidth;   // 右方
                }
                else
                {
                    _againstWall = true;
                    deltaMovement.x = _skinWidth - _raycastHit.distance;   // 左方
                }
                if (Mathf.Abs(deltaMovement.x) < _minOffset) break;
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
        bounds.Expand(-_skinWidth * 2f);
        _rayOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        _rayOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _rayOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    private void ComputeRaysInterval()
    {
        _horizontalRaysInterval = (_rayOrigin.bottomRight.x - _rayOrigin.bottomLeft.x) / (_horizontalRaysCount - 1);
        _verticalRaysInterval = (_rayOrigin.topLeft.y - _rayOrigin.bottomLeft.y) / (_verticalRaysCount - 1);
    }

    [ContextMenu("Rays Interval")]
    private void ComputerSomthing()
    {
        ComputeRayOrigin();
        ComputeRaysInterval();
    }

    public void UpdateInput()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        MoveY = Input.GetAxisRaw("Vertical");
        Jump = Input.GetKey(KeyCode.J);
    }

}
