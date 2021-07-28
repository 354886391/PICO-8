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
    private const float EndDashUpMult = .75f;
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
    private const float ClimbGrabYMult = .2f;
    #endregion

    #region Vars

    [SerializeField] private bool _onGround; //当前帧在地面    

    [SerializeField] private int _facing = 1;
    [SerializeField] private int _jumpCount;
    [SerializeField] private float _maxFall;
    [SerializeField] private Vector2 _speed;

    [SerializeField] private float _skinWidth = 0.02f;
    [SerializeField] private float _verticalRaysCount;
    [SerializeField] private float _horizontalRaysCount;
    [SerializeField] private float _verticalBetweenRays;
    [SerializeField] private float _horizontalBetweenRays;

    private RayOrigin _rayOrigin;

    [SerializeField] private LayerMask _platformMask;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private RaycastHit2D _raycastHit;
    #endregion

    public void Move()
    {
        //ResetGroundInfo();
        DetectGround();
        //ApplyMovement();
        //SnapToGround();
        //LimitLateralVelocity();
        //LimitVerticalVelocity();
        //PreventGroundPenetration();
    }

    private void ResetGround()
    {

    }

    private void DetectGround()
    {

    }

    private void ApplyGravity(float deltaTime)
    {
        if (!_onGround)
        {
            float max = MaxFall;
            float mult = Mathf.Abs(_speed.y) < HalfGravThreshold && Input.Jump ? 0.5f : 1.0f;
            _speed.y = Mathf.MoveTowards(_speed.y, max, Gravity * mult * deltaTime);
        }
    }

    private void ApplyMovement(float moveX, float deltaTime)
    {
        float mult = _onGround ? 1.0f : AirMult;
        if (Mathf.Abs(_speed.x) >= MaxRun && Mathf.Sign(_speed.x) == moveX)
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * moveX, RunReduce * mult * deltaTime);  //Reduce back from beyond the max speed
        }
        else
        {
            _speed.x = Mathf.MoveTowards(_speed.x, MaxRun * moveX, RunAccel * mult * deltaTime);   //Approach the max speed          
        }
        Console.LogFormat("Move speed X {0:F3}", _speed.x);
    }
}
