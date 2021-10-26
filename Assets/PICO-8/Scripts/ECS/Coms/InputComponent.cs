
[System.Serializable]
public class InputComponent : IComponent
{
    public float _deltaTime;

    private bool _jump;                 // Press
    private bool _dash;
    private bool _climb;
    private float _moveX;
    private float _moveY;

    public bool CanJump;                // Release    
    public bool CanDash;
    public bool CanClimb;
    public float HeldDownTimer;         // HoldTime

    public bool Jump
    {
        get { return _jump; }
        set
        {
            if (_jump && !value)
            {
                CanJump = true;
                HeldDownTimer = 0.0f;
            }
            _jump = value;
            if (_jump)
            {
                HeldDownTimer += _deltaTime;
            }
        }
    }

    public bool Dash
    {
        get { return _dash; }
        set
        {
            if (_dash && !value)
            {
                CanDash = true;
                HeldDownTimer = 0.0f;
            }
            _dash = value;
            if (_dash)
            {
                HeldDownTimer += _deltaTime;
            }
        }
    }

    public bool Climb
    {
        get { return _climb; }
        set
        {
            if (_climb && !value)
            {
                CanClimb = true;
                HeldDownTimer = 0.0f;
            }
            _climb = value;
            if (_climb)
            {
                HeldDownTimer += _deltaTime;
            }
        }
    }

    public float MoveX
    {
        get => _moveX;
        set => _moveX = value;
    }

    public float MoveY
    {
        get => _moveY;
        set => _moveY = value;
    }
}
