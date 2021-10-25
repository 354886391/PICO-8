using UnityEngine;
using UnityEngine.Events;

public class DashComponent : IComponent
{

    public readonly float Speed = 24f;
    public readonly float DashTime = 0.15f;
    public readonly float ToleranceTime = 0.15f;
    public readonly float CooldownTime = 0.2f;

    public bool CanDash;   
    public bool IsDashing;
    public bool CanUpdate;
    public bool IsCooldown;
    public bool IsFreezing;

    public float DashTimer;
    public float HeldDownTimer;
    public float CooldownTimer;

    public Vector2 Direction;
    public Vector2 BeforeSpeed;

    public UnityAction BeginEvent;
    public UnityAction UpdateEvent;
    public UnityAction EndEvent;
    public UnityAction LandEvent;
}
