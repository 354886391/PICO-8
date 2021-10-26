
[System.Serializable]
public class JumpComponent : IComponent
{
    public readonly float HBoost = 4f;
    public readonly float WBoost = 6f;
    public readonly float Speed = 10.5f;
    public readonly float JumpTime = 0.2f;
    public readonly float ToleranceTime = 0.15f;
    public readonly int MaxMidAirJump = 1;   // 0 or 1

    public bool IsJumping;
    public bool CanUpdate;
    public bool IsCooldown;

    public int MidAirJumps;
    public float JumpTimer;
    public float HeldDownTimer;

    public System.Action BeginEvent;
    public System.Action UpdateEvent;
    public System.Action EndEvent;
    public System.Action LandEvent;
    public System.Action MidAirBeginEvent;
}
