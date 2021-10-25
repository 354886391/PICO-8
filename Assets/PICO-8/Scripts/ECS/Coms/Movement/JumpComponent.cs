using UnityEngine.Events;

public class JumpComponent : IComponent
{
    public readonly float HBoost = 4f;
    public readonly float WBoost = 6f;
    public readonly float Speed = 10.5f;
    public readonly float JumpTime = 0.2f;
    public readonly float ToleranceTime = 0.15f;
    public readonly int MaxMidAirJump = 1;   // 0 or 1

    public bool CanJump;
    public bool IsJumping;  
    public bool CanUpdate;

    public int MidAirJumps;
    public float JumpTimer;
    public float HeldDownTimer;

    public UnityAction BeginEvent;
    public UnityAction UpdateEvent;
    public UnityAction EndEvent;   
    public UnityAction LandEvent;
    public UnityAction MidAirBeginEvent;
}
