using UnityEngine;
using UnityEngine.Events;

public class DashComponent : IComponent
{

    public readonly float Speed = 24f;
    public readonly float DashTime = 0.15f;
    public readonly float ToleranceTime = 0.15f;
    public readonly float CooldownTime = 0.2f;

    public bool CanDash { get; set; }
    public bool IsDashing { get; set; }
    public bool CanUpdate { get; set; }
    public bool IsCooldown { get; set; }
    public bool IsFreezing { get; set; }

    public float DashTimer { get; set; }
    public float HeldDownTimer { get; set; }
    public float CooldownTimer { get; set; }

    public Vector2 Direction;
    public Vector2 BeforeSpeed;

    public UnityAction BeginEvent { get; set; }
    public UnityAction UpdateEvent { get; set; }
    public UnityAction EndEvent { get; set; }
    public UnityAction LandEvent { get; set; }
}
