
[System.Serializable]
public class DashComponent : IComponent
{

    public readonly float Speed = 24f;
    public readonly float DashTime = 0.15f;
    public readonly float ToleranceTime = 0.15f;
    public readonly float CooldownTime = 0.2f;

    public bool IsDashing;
    public bool CanUpdate;
    public bool IsCooldown;

    public bool IsFreezing;

    public float DashTimer;
    public float HeldDownTimer;
    public float CooldownTimer;

    public UnityEngine.Vector2 Direction;
    public UnityEngine.Vector2 BeforeSpeed;

    public System.Action BeginEvent;
    public System.Action UpdateEvent;
    public System.Action EndEvent;
    public System.Action LandEvent;
}
