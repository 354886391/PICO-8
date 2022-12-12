using Sirenix.OdinInspector;
using UnityEngine;

public enum PressState
{
    Enter,
    Stay,
    Exit,
}

[System.Serializable]
public class CharacterInput
{
    [SerializeField]
    private float _moveXPress;
    private float _moveXPressTimer;
    public System.Action<PressState> OnMoveXPressed;

    [SerializeField]
    private float _moveYPress;
    private float _moveYPressTimer;
    public System.Action<PressState> OnMoveYPressed;

    #region JUMP_PRESS
    [SerializeField]
    private bool _jumpPress;
    private float _jumpPressTimer;
    public System.Action<PressState> OnJumpPressed;
    #endregion

    #region DASH_PRESS
    [SerializeField]
    private bool _dashPress;
    private float _dashPressTimer;
    public System.Action<PressState> OnDashPressed;
    #endregion

    [ShowInInspector]
    public Vector2 move
    {
        get { return new Vector2(_moveXPress, _moveYPress); }
    }

    public float moveXPress
    {
        get { return _moveXPress; }
        private set
        {
            if (_moveXPress != 0 && value == 0)
            {
                _moveXPressTimer = 0;
                OnMoveXPressed?.Invoke(PressState.Exit);
            }
            else if (_moveXPress == 0 && value != 0)
            {
                _moveXPressTimer = 0;
                OnMoveXPressed?.Invoke(PressState.Enter);
            }
            _moveXPress = value;
            if (_moveXPress != 0)
            {
                _moveXPressTimer += Time.deltaTime;
                OnMoveXPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public float moveYPress
    {
        get { return _moveYPress; }
        private set
        {
            if (_moveYPress != 0 && value == 0)
            {
                _moveYPressTimer = 0;
                OnMoveYPressed?.Invoke(PressState.Exit);
            }
            else if (_moveYPress == 0 && value != 0)
            {
                _moveYPressTimer = 0;
                OnMoveYPressed?.Invoke(PressState.Enter);
            }
            _moveYPress = value;
            if (_moveYPress != 0)
            {
                _moveYPressTimer += Time.deltaTime;
                OnMoveYPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public bool jumpPress
    {
        get { return _jumpPress; }
        private set
        {
            if (_jumpPress && !value)
            {
                _jumpPressTimer = 0;
                OnJumpPressed?.Invoke(PressState.Exit);
            }
            else if (!_jumpPress && value)
            {
                _jumpPressTimer = 0;
                OnJumpPressed?.Invoke(PressState.Enter);
            }
            _jumpPress = value;
            if (_jumpPress)
            {
                _jumpPressTimer += Time.deltaTime;
                OnJumpPressed?.Invoke(PressState.Stay);
            }
        }
    }
    public float jumpPressTimer
    {
        get { return _jumpPressTimer; }
    }

    public bool dashPress
    {
        get { return _dashPress; }
        private set
        {
            if (_dashPress && !value)
            {
                _dashPressTimer = 0;
                OnDashPressed?.Invoke(PressState.Exit);
            }
            else if (!_dashPress && value)
            {
                _dashPressTimer = 0;
                OnDashPressed?.Invoke(PressState.Enter);
            }
            _dashPress = value;
            if (_dashPress)
            {
                _dashPressTimer += Time.deltaTime;
                OnDashPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public float dashPressTimer
    {
        get { return _dashPressTimer; }
    }

    public void Update()
    {
        moveXPress = Input.GetAxisRaw("Horizontal");
        moveYPress = Input.GetAxisRaw("Vertical");
        jumpPress = Input.GetKey(KeyCode.C);
        dashPress = Input.GetKey(KeyCode.X);
    }
}