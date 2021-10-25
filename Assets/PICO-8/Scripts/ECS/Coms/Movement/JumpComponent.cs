using UnityEngine.Events;

public class JumpComponent : IComponent
{
    public readonly float HBoost = 4f;
    public readonly float WBoost = 6f;
    public readonly float Speed = 10.5f;
    public readonly float JumpTime = 0.2f;
    public readonly float ToleranceTime = 0.15f;
    public readonly int MaxMidAirJump = 1;   // 0 or 1

    public bool CanJump { get; set; }
    public bool IsJumping { get; set; }
    public bool CanUpdate { get; set; }

    public int MidAirJumps { get; set; }
    public float JumpTimer { get; set; }
    public float HeldDownTimer { get; set; }

    public UnityAction BeginEvent { get; set; }
    public UnityAction UpdateEvent { get; set; }
    public UnityAction EndEvent { get; set; }
    public UnityAction LandEvent { get; set; }
    public UnityAction MidAirBeginEvent { get; set; }
}
