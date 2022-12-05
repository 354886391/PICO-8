using Sirenix.OdinInspector;

[System.Serializable]
public class InputComponent : IComponent
{

    private bool _jump = true;                 // Press
    private bool _dash = true;
    private bool _climb = true;
    private float _moveX;
    private float _moveY;

    [ShowInInspector] public bool CanJump { get; set; }             // Release    
    [ShowInInspector] public bool CanDash { get; set; }
    [ShowInInspector] public bool CanClimb { get; set; }
    [ShowInInspector] public float DeltaTime { get; set; }
    [ShowInInspector] public float HeldDownTimer { get; set; }          // HoldTime

    [ShowInInspector]
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
                HeldDownTimer += DeltaTime;
            }
        }
    }

    [ShowInInspector]
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
                HeldDownTimer += DeltaTime;
            }
        }
    }

    [ShowInInspector]
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
                HeldDownTimer += DeltaTime;
            }
        }
    }

    [ShowInInspector]
    public float MoveX
    {
        get => _moveX;
        set => _moveX = value;
    }

    [ShowInInspector]
    public float MoveY
    {
        get => _moveY;
        set => _moveY = value;
    }
}
