using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : IComponent
{

    #region Vars
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _onGround;
    [SerializeField] private bool _wasOnGround;
    [SerializeField] private bool _againstWall;
    [SerializeField] private bool _isFreezing;
    [SerializeField] private int _facing;
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
    #endregion

    public override void AddComponent()
    {
        base.AddComponent();
    }

    public override MovementComponent GetComponent<MovementComponent>()
    {
        return base.GetComponent<MovementComponent>();
    }

    public override bool HasComponent<MovementComponent>()
    {
        return base.HasComponent<MovementComponent>();
    }
}
