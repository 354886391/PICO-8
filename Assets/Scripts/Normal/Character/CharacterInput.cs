using Sirenix.OdinInspector;
using UnityEngine;

public enum PressState
{
    /// <summary>
    /// Press
    /// </summary>
    Enter,
    /// <summary>
    /// Stay
    /// </summary>
    Stay,
    /// <summary>
    /// Release
    /// </summary>
    Exit,
}

[System.Serializable]
public class CharacterInput : MonoBehaviour
{
    [SerializeField]
    private float _moveX;
    private float _moveXTimer;
    public System.Action<PressState> OnMoveXPressed;

    [SerializeField]
    private float _moveY;
    private float _moveYTimer;
    public System.Action<PressState> OnMoveYPressed;

    #region JUMP_PRESS
    [SerializeField]
    private bool _jumpPressed;
    private float _jumpTimer;
    public System.Action<PressState> OnJumpPressed;
    #endregion

    #region DASH_PRESS
    [SerializeField]
    private bool _dashPressed;
    private float _dashTimer;
    public System.Action<PressState> OnDashPressed;
    #endregion

    [ShowInInspector]
    public Vector2 move
    {
        get { return new Vector2(_moveX, _moveY); }
    }

    public float moveX
    {
        get { return _moveX; }
        private set
        {
            if (_moveX != 0 && value == 0)
            {
                _moveXTimer = 0;
                OnMoveXPressed?.Invoke(PressState.Exit);
            }
            else if (_moveX == 0 && value != 0)
            {
                _moveXTimer = 0;
                OnMoveXPressed?.Invoke(PressState.Enter);
            }
            _moveX = value;
            if (_moveX != 0)
            {
                _moveXTimer += Time.deltaTime;
                OnMoveXPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public float moveY
    {
        get { return _moveY; }
        private set
        {
            if (_moveY != 0 && value == 0)
            {
                _moveYTimer = 0;
                OnMoveYPressed?.Invoke(PressState.Exit);
            }
            else if (_moveY == 0 && value != 0)
            {
                _moveYTimer = 0;
                OnMoveYPressed?.Invoke(PressState.Enter);
            }
            _moveY = value;
            if (_moveY != 0)
            {
                _moveYTimer += Time.deltaTime;
                OnMoveYPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public bool jumpPressed
    {
        get { return _jumpPressed; }
        private set
        {
            if (_jumpPressed && !value)
            {
                _jumpTimer = 0;
                OnJumpPressed?.Invoke(PressState.Exit);
            }
            else if (!_jumpPressed && value)
            {
                _jumpTimer = 0;
                OnJumpPressed?.Invoke(PressState.Enter);
            }
            _jumpPressed = value;
            if (_jumpPressed)
            {
                _jumpTimer += Time.deltaTime;
                OnJumpPressed?.Invoke(PressState.Stay);
            }
        }
    }
    public float jumpPressTimer
    {
        get { return _jumpTimer; }
    }

    public bool dashPressed
    {
        get { return _dashPressed; }
        private set
        {
            if (_dashPressed && !value)
            {
                _dashTimer = 0;
                OnDashPressed?.Invoke(PressState.Exit);
            }
            else if (!_dashPressed && value)
            {
                _dashTimer = 0;
                OnDashPressed?.Invoke(PressState.Enter);
            }
            _dashPressed = value;
            if (_dashPressed)
            {
                _dashTimer += Time.deltaTime;
                OnDashPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public float dashPressTimer
    {
        get { return _dashTimer; }
    }

    public void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        jumpPressed = Input.GetKey(KeyCode.C);
        dashPressed = Input.GetKey(KeyCode.X);
    }
}