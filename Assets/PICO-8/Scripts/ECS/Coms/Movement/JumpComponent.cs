using UnityEngine.Events;

public class JumpComponent : IComponent
{
    public readonly float HBOOST = 4f;
    public readonly float WBOOST = 6f;
    public readonly float SPEED = 10.5f;
    public readonly float JUMPTIME = 0.2f;
    public readonly float TOLERANCETIME = 0.15f;
    public const int MAXMIDAIRJUMP = 0;   // 0 or 1

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
}
